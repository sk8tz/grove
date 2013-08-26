﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class Exhaustion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Exhaustion")
        .ManaCost("{2}{U}")
        .Type("Sorcery")
        .Text("Creatures and lands target opponent controls don't untap during his or her next untap step.")
        .FlavorText(
          "The mage felt as though he'd been in the stasis suit for days. Upon his return, he found it was months.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPlayer(
              selector: e => e.Controller.Opponent,
              modifiers: () =>
                {
                  var cp = new ContinuousEffectParameters
                    {
                      Modifier = () => new AddStaticAbility(Static.DoesNotUntap),
                      CardFilter = (card, effect) =>
                        card.Controller == effect.SourceEffect.Controller.Opponent &&
                          (card.Is().Creature || card.Is().Land)
                    };

                  var modifier = new AddContiniousEffect(new ContinuousEffect(cp));

                  modifier.AddLifetime(new EndOfUntapStep(l => l.Modifier.SourceCard.Controller.IsActive));
                  return modifier;
                });

            p.TimingRule(new OnSecondMain());
          });
    }
  }
}