﻿namespace Grove.Artifical.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  [Serializable]
  public class LandEnchantment : TargetingRule
  {
    private readonly ControlledBy _controlledBy;

    public LandEnchantment(ControlledBy controlledBy)
    {
      _controlledBy = controlledBy;
    }

    private LandEnchantment() {}

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(_controlledBy);
      return Group(candidates, 1);
    }
  }
}