﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class Sluggishness : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sluggishness")
        .ManaCost("{1}{R}")
        .Type("Enchantment Aura")
        .Text("Enchanted creature can't block.{EOL}When Sluggishness is put into a graveyard from the battlefield, return Sluggishness to its owner's hand.")
        .FlavorText("Vark decided to lie down and think of a good excuse to quit working.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddStaticAbility(Static.CannotBlock));
          

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCannotBlockAttack(blockOnly: true));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Sluggishness is put into a graveyard from the battlefield, return Sluggishness to its owner's hand.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}