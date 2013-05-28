﻿namespace Grove.Gameplay.Costs
{
  using System;
  using System.Collections.Generic;
  using ManaHandling;
  using Targeting;

  [Serializable]
  public class AggregateCost : Cost
  {
    private readonly List<Cost> _costs = new List<Cost>();

    private AggregateCost() {}

    public AggregateCost(params Cost[] costs)
    {
      _costs.AddRange(costs);
    }

    public override IManaAmount GetManaCost()
    {
      var manaAmount = Mana.Zero;

      foreach (var cost in _costs)
      {
        manaAmount = manaAmount.Add(cost.GetManaCost());
      }

      return manaAmount;
    }

    public override void Initialize(Card card, Game game, TargetValidator validator = null)
    {
      base.Initialize(card, game, validator);

      foreach (var cost in _costs)
      {
        cost.Initialize(card, game, validator);
      }
    }

    protected override void CanPay(CanPayResult result)
    {
      foreach (var cost in _costs)
      {
        var childResult = cost.CanPay();

        result.CanPay = childResult.CanPay;
        result.MaxX = result.MaxX ?? childResult.MaxX;
        result.MaxRepetitions = childResult.MaxRepetitions;

        if (!result.CanPay)
          return;
      }
    }

    public override void Pay(Targets targets, int? x, int repeat = 1)
    {
      foreach (var cost in _costs)
      {
        cost.Pay(targets, x, repeat);
      }
    }
  }
}