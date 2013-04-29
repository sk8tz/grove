﻿namespace Grove.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Player;
  using Gameplay.Targeting;

  public class Opponent : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Player>()
        .Where(x => x == p.Controller.Opponent);

      return Group(candidates, 1);
    }
  }
}