﻿namespace Grove.Core.Controllers
{
  public abstract class SelectCardsToSacrifice : SelectCards
  {
    protected override void ProcessCard(Card chosenCard)
    {
      chosenCard.Sacrifice();
    }
  }
}