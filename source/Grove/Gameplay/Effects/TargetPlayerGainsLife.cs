﻿namespace Grove.Gameplay.Effects
{
  using System;
  using Targeting;

  [Serializable]
  public class TargetPlayerGainsLife : Effect
  {
    private readonly int _amount;

    private TargetPlayerGainsLife() {}

    public TargetPlayerGainsLife(int amount)
    {
      _amount = amount;
    }

    protected override void ResolveEffect()
    {
      Target.Player().Life += _amount;
    }
  }
}