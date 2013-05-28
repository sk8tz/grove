﻿namespace Grove.Artifical.TimingRules
{
  using System;

  [Serializable]
  public class GainHexproof : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return CanBeDestroyed(p, targetOnly: true, considerCombat: false);
    }
  }
}