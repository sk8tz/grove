﻿namespace Grove.Gameplay.Characteristics
{
  using System;
  using Infrastructure;

  [Copyable, Serializable]
  public class Attachment : IHashable
  {
    public Attachment(Card card)
    {
      Card = card;
    }

    private Attachment() {}

    public Card Card { get; private set; }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(Card);
    }
  }
}