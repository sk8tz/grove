﻿namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Targeting;

  public class EffectBounce : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = GetBounceCandidates(p);
      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }

    protected override IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .OrderBy(c => c.Score);

      return Group(candidates, p.MinTargetCount());
    }
  }
}