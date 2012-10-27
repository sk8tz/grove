﻿namespace Grove.Core.Details.Cards.Triggers
{
  using Infrastructure;
  using Messages;

  public class OpponentWasNotDealtDamageThisTurn : Trigger, IReceive<StepStarted>, IReceive<DamageHasBeenDealt>
  {
    private Trackable<bool> _wasDealtDamage;

    public void Receive(DamageHasBeenDealt message)
    {
      if (message.Receiver == Players.Passive)
      {
        _wasDealtDamage.Value = true;
      }
    }

    public void Receive(StepStarted message)
    {
      if (message.Step == Step.EndOfTurn)
      {
        if (_wasDealtDamage == false)
          Set();

        _wasDealtDamage.Value = false;
      }
    }

    protected override void Initialize()
    {
      _wasDealtDamage = new Trackable<bool>(Game.ChangeTracker);
    }
  }
}