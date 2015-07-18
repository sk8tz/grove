﻿namespace Grove.UserInterface.PlayerBox
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using Infrastructure;
  using Messages;

  public class ViewModel : ViewModelBase, IDisposable, IReceive<UiInteractionChanged>
  {
    private readonly Timer _timer;

    public ViewModel(Player player)
    {      
      Player = player;

      IncreaseLifeAnimation = Animation.Create();
      DecreaseLifeAnimation = Animation.Create();
      
      Update(enableAnimations: false);

      _timer = new Timer(delegate { Update(); }, null,
        TimeSpan.FromMilliseconds(20),
        TimeSpan.FromMilliseconds(20));      
      
    }

    public Player Player { get; private set; }

    public virtual int HandCount { get; protected set; }
    public virtual int LibraryCount { get; protected set; }
    public virtual int GraveyardCount { get; protected set; }
    public virtual int EmblemsCount { get; protected set; }
    public virtual int Life { get; protected set; }
    public virtual bool IsActive { get; protected set; }
    public virtual bool CanChangeSelection { get; protected set; }
    public virtual bool CanShowEmblems { get; protected set; }
    public virtual int LifeChange { get; protected set; }
    public virtual bool IsEmblemsDisplayed { get; set; }

    public Animation IncreaseLifeAnimation { get; private set; }
    public Animation DecreaseLifeAnimation { get; private set; }

    private List<string> _emblems = new List<string>();
    public IEnumerable<string> Emblems { get { return _emblems.AsEnumerable(); } }
    public virtual string EmblemString { get; set; }

    public void Dispose()
    {
      _timer.Dispose();
    }

    public void Receive(UiInteractionChanged message)
    {
      CanChangeSelection = message.State == InteractionState.SelectTarget;
      CanShowEmblems = message.State != InteractionState.SelectTarget;
    }

    private void Update(bool enableAnimations = true)
    {
      Update(() => HandCount != Player.Hand.Count, () => HandCount = Player.Hand.Count);
      Update(() => LibraryCount != Player.Library.Count, () => LibraryCount = Player.Library.Count);
      Update(() => GraveyardCount != Player.Graveyard.Count, () => GraveyardCount = Player.Graveyard.Count);
      
      Update(() => EmblemsCount != Player.Emblems.Count(), () =>EmblemsCount = Player.Emblems.Count());
      
      Update(() => Life != Player.Life, () =>
        {
          var oldLife = Life;                                        
          Life = Player.Life;

          LifeChange = Math.Abs(oldLife - Life);
          
          // at the time ctor is called
          // animations are not yet ready 
          // to be executed so disable them
          if (enableAnimations)
            AnimateLifeChange(oldLife, Player.Life);
        });

      Update(() => IsActive = Player.IsActive, () => IsActive = Player.IsActive);
    }

    private void AnimateLifeChange(int oldLife, int newLife)
    {
      if (oldLife > newLife)
      {            
        DecreaseLifeAnimation.Start();
      }
      else
      {            
        IncreaseLifeAnimation.Start();
      }
    }

    private static void Update(Func<bool> condition, Action update)
    {
      if (condition()) update();
    }

    public void ChangeSelection()
    {
      Publisher.Publish(
        new SelectionChanged {Selection = Player});
    }

    [Updates("Emblems")]
    public void ShowEmblems()
    {
      IsEmblemsDisplayed = true;

      _emblems = Player.Emblems.Select(x => x.Text).ToList();
      EmblemString = String.Join("\n\n\n", _emblems);
    }
    public void HideEmblems()
    {
      IsEmblemsDisplayed = false;
    }

    public interface IFactory
    {
      ViewModel Create(Player player);
      void Release(ViewModel viewModel);
    }
  }
}