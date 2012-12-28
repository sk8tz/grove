﻿namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Dsl;

  public class GoblinRaider : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Goblin Raider")
        .ManaCost("{1}{R}")
        .Type("Creature Goblin Warrior")
        .Text("Goblin Raider can't block.")
        .FlavorText("He was proud to wear the lizard skin around his waist, just for the fun of annoying the enemy.")
        .Power(2)
        .Toughness(2)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.CannotBlock
        );
    }
  }
}