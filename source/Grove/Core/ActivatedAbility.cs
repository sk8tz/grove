﻿namespace Grove.Core
{
  using System.Linq;
  using Costs;
  using Infrastructure;
  using Mana;
  using Messages;
  using Zones;

  public class ActivatedAbility : Ability
  {
    private readonly bool _activateAsSorcery;
    private readonly Zone _activationZone;
    private readonly Cost _cost;
    private readonly bool _activateOnlyOnceEachTurn;
    private readonly Trackable<int> _lastActivation = new Trackable<int>();

    protected ActivatedAbility() {}
    
    public ActivatedAbility(ActivatedAbilityParameters parameters) : base(parameters)
    {
      _cost = parameters.Cost;
      _activationZone = parameters.ActivationZone;
      _activateAsSorcery = parameters.ActivateAsSorcery;
      _activateOnlyOnceEachTurn = parameters.ActivateOnlyOnceEachTurn;
    }

    public void Activate(ActivationParameters p)
    {
      _lastActivation.Value = Turn.TurnCount;
      
      var effectParameters = new EffectParameters
        {
          Source = this,
          Targets = p.Targets,
          X = p.X
        };
      
      // create effect first, since some cost e.g Sacrifice can
      // put owning card to graveyard which will alter some card
      // properties e.g counters, power, toughness ...     
      var effect = EffectFactory().Initialize(effectParameters, Game);
      
      Pay(p);      
      Resolve(effect, p.SkipStack);

      Publish(new PlayerHasActivatedAbility(this, p.Targets));
    }

    protected void Pay(ActivationParameters p = null)
    {      
      p = p ?? new ActivationParameters();
      
      if (p.PayCost)
      {
        _cost.Pay(p.Targets, p.X);
      }
    }

    protected bool CanPay(ref int? maxX)
    {
      return _cost.CanPay(ref maxX);
    }

    public IManaAmount GetManaCost()
    {
      return _cost.GetManaCost();
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        base.CalculateHash(calc),
        _activateAsSorcery.GetHashCode(),
        calc.Calculate(_cost));
    }

    public virtual bool CanActivate(out ActivationPrerequisites activationPrerequisites)
    {
      int? maxX = null;
      activationPrerequisites = null;

      if (CanBeActivated(ref maxX) == false)
        return false;

      activationPrerequisites = new ActivationPrerequisites
        {
          Card = OwningCard,
          Description = Text,
          Selector = TargetSelector,
          DistributeAmount = DistributeAmount,
          MaxX = maxX,
          Rules = Rules,
        };

      return true;
    }

    public override void Initialize(Card owner, Game game)
    {
      base.Initialize(owner, game);
      _cost.Initialize(owner, game, TargetSelector.Cost.FirstOrDefault());
      _lastActivation.Initialize(ChangeTracker);
    }

    private bool CanBeActivated(ref int? maxX)
    {
      return
        IsEnabled &&          
          CanBeActivatedAtThisTime() &&
            _cost.CanPay(ref maxX);
    }

    private bool CanBeActivatedAtThisTime()
    {
      if (OwningCard.Zone != _activationZone)
        return false;

      if (_activateOnlyOnceEachTurn && _lastActivation.Value == Turn.TurnCount)
      {
        return false;
      }
      
      if (_activateAsSorcery)
      {
        return Turn.Step.IsMain() &&
          OwningCard.Controller.IsActive &&
            Stack.IsEmpty;
      }

      return true;
    }
  }
}