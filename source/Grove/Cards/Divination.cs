﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Divination : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Divination")
        .ManaCost("{2}{U}")
        .Type("Sorcery")
        .Text("Draw two cards.")
        .FlavorText(
          "Even the House of Galan, who takes the most scholarly approach to the mystic traditions, has resorted to exploring more primitive methods in Avacyn's absence.")
        .Cast(p =>
          {
            p.Effect = () => new DrawCards(2);
            p.TimingRule(new OnFirstMain());
          });
    }
  }
}