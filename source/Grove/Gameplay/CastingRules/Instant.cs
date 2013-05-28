﻿namespace Grove.Gameplay.CastingRules
{
  using System;
  using Effects;

  [Serializable]
  public class Instant : CastingRule
  {
    private readonly Action<Card> _afterResolvePutToZone;

    private Instant() {}

    public Instant(Action<Card> afterResolvePutToZone = null)
    {
      _afterResolvePutToZone = afterResolvePutToZone ?? (card => card.PutToGraveyard());
    }

    public override bool CanCast()
    {
      return true;
    }

    public override void Cast(Effect effect)
    {
      Game.Stack.Push(effect);
    }

    public override void AfterResolve()
    {
      _afterResolvePutToZone(Card);
    }
  }
}