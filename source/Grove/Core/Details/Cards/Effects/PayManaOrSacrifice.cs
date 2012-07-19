﻿namespace Grove.Core.Details.Cards.Effects
{
  using System.Windows;
  using Controllers;
  using Controllers.Results;
  using Mana;
  using Ui;

  public class PayManaOrSacrifice : Effect
  {
    public IManaAmount Amount { get; set; }

    protected override void ResolveEffect()
    {
      if (Controller.HasMana(Amount) == false)
      {
        Source.OwningCard.Sacrifice();
        return;
      }

      Decisions.Enqueue<AdHocDecision<BooleanResult>>(
        controller: Controller,
        init: p =>
          {
            p.Param("card", Source.OwningCard);
            p.QueryAi = self => { return true; };
            p.QueryUi = self =>
              {
                var result = self.Shell.ShowMessageBox(
                  message: string.Format("Pay {0}?", Amount),
                  buttons: MessageBoxButton.YesNo,
                  type: DialogType.Small);

                return result == MessageBoxResult.Yes;
              };

            p.Process = self =>
              {
                if (self.Result.IsTrue)
                {
                  self.Controller.Consume(Amount);
                  return;
                }

                self.Param<Card>("card").Sacrifice();
              };
          });
    }
  }
}