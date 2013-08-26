﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Catastrophe : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Catastrophe")
        .ManaCost("{4}{W}{W}")
        .Type("Sorcery")
        .Text("Destroy all lands or all creatures. Creatures destroyed this way can't be regenerated.")
        .FlavorText(
          "Radiant's eyes flashed. 'Go, then,' the angel spat at Serra, 'and leave this world to those who truly care.'")
        .Cast(p =>
          {
            p.Effect = () => new DestroyAllLandsOrCreatures();
            p.TimingRule(new OnSecondMain());
          });
    }
  }
}