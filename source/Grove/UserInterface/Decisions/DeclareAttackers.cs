﻿namespace Grove.UserInterface.Decisions
{
  using System.Linq;
  using Gameplay.Targeting;
  using Messages;
  using SelectTarget;
  using Shell;

  public class DeclareAttackers : Gameplay.Decisions.DeclareAttackers
  {
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var tp = new TargetValidatorParameters {MinCount = 0, MaxCount = null, Message = "Select attackers."}
        .Is.Card(c => c.CanAttack && c.Controller == Controller)
        .On.Battlefield();


      tp.MustBeTargetable = false;

      var validator = new TargetValidator(tp);
      validator.Initialize(Game, Controller);

      var selectParameters = new SelectTargetParameters
        {
          Validator = validator,
          CanCancel = false,          
          TargetSelected = target => Shell.Publish(
            new AttackerSelected
              {
                Attacker = target.Card()
              }),
          TargetUnselected = target => Shell.Publish(
            new AttackerUnselected
              {
                Attacker = target.Card()
              })
        };

      var dialog = DialogFactory.Create(selectParameters);
      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);
      Result = dialog.Selection.ToList();
    }
  }
}