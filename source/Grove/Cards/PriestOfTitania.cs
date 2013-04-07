﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Dsl;
  using Core.Mana;

  public class PriestOfTitania : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Priest of Titania")
        .ManaCost("{1}{G}")
        .Type("Creature Elf Druid")
        .Text("{T}: Add {G} to your mana pool for each Elf on the battlefield.")
        .FlavorText("Titania rewards all who honor the forest by making them a living part of it.")
        .Power(1)
        .Toughness(1)
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} to your mana pool for each Elf on the battlefield.";
            p.ManaAmount((ability, game) => ManaAmount.OfSingleColor(ManaColors.Green,
              game.Players.Permanents().Count(x => x.Is("elf"))));
          });
    }
  }
}