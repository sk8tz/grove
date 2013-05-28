﻿namespace Grove.Artifical
{
  using System;
  using Gameplay;
  using Gameplay.Misc;

  [Serializable]
  public abstract class MachinePlayRule : GameObject
  {
    public abstract void Process(ActivationContext c);

    public virtual void Initialize(Game game)
    {
      Game = game;
    }
  }
}