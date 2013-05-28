﻿namespace Grove.Gameplay.Characteristics
{
  using System;
  using Infrastructure;
  using Modifiers;

  [Serializable]
  public class LandLimit : Characteristic<int?>, IModifiable
  {
    private LandLimit() {}

    public LandLimit(int value)
      : base(value) {}

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}