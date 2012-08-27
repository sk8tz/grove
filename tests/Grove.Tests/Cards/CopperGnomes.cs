﻿namespace Grove.Tests.Cards
{
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class CopperGnomes
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutEngineIntoPlay()
      {
        var engine = C("Wurmcoil Engine");
        
        Hand(P2, engine);
        Battlefield(P2, "Forest", "Forest", "Forest", "Forest", "Copper Gnomes");

        RunGame(1);

        Equal(Zone.Battlefield, C(engine).Zone);
      }
    }
  }
}