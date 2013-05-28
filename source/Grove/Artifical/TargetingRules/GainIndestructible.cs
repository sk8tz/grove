﻿namespace Grove.Artifical.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Targeting;

  [Serializable]
  public class GainIndestructible : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = GetCandidatesThatCanBeDestroyed(p)
        .OrderByDescending(x => x.Card().Score);

      return Group(candidates, p.MinTargetCount());
    }
  }
}