﻿namespace Grove.Gameplay.Decisions
{
  using System;
  using System.Collections.Generic;
  using Results;

  [Serializable]
  public abstract class OrderCards : Decision<Ordering>
  {
    public List<Card> Cards;
    public IChooseDecisionResults<List<Card>, Ordering> ChooseDecisionResults;
    public string Title;
    public IProcessDecisionResults<Ordering> ProcessDecisionResults;

    protected override bool ShouldExecuteQuery { get { return Cards.Count > 1; } }

    public override void ProcessResults()
    {
      switch (Cards.Count)
      {
        case 0:
          Result = new Ordering();
          break;
        case 1:
          Result = new Ordering(0);
          break;
      }

      ProcessDecisionResults.ProcessResults(Result);
    }
  }
}