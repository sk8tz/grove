﻿namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Messages;
  using Zones;

  public class SearchLibraryPutToHand : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>, ICardValidator
  {
    private readonly bool _discardRandomCardAfterwards;
    private readonly int _maxCount;
    private readonly int _minCount;
    private readonly bool _revealCards;
    private readonly string _text;
    private readonly Func<Effect, Card, bool> _validator;

    private SearchLibraryPutToHand() {}

    public SearchLibraryPutToHand(
      Func<Card, bool> validator,
      int maxCount = 1,
      int minCount = 0,
      string text = null,
      bool discardRandomCardAfterwards = false,
      bool revealCards = true) : this(
        maxCount, minCount, (e, c) => validator(c), text, discardRandomCardAfterwards, revealCards) {}


    public SearchLibraryPutToHand(
      int maxCount = 1,
      int minCount = 0,
      Func<Effect, Card, bool> validator = null,
      string text = null,
      bool discardRandomCardAfterwards = false,
      bool revealCards = true)
    {
      _discardRandomCardAfterwards = discardRandomCardAfterwards;
      _validator = validator ?? delegate { return true; };
      _text = text ?? "Search your library for a card.";
      _revealCards = revealCards;
      _maxCount = maxCount;
      _minCount = minCount;
    }

    public bool IsValidCard(Card card)
    {
      return _validator(this, card);
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(x => -x.Score)
        .Take(_maxCount)
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        card.PutToHand();

        if (_revealCards)
        {
          Publish(new CardWasRevealed {Card = card});
        }
        else
        {
          card.ResetVisibility();
        }
      }

      if (_discardRandomCardAfterwards)
      {
        Controller.DiscardRandomCard();
      }

      Controller.ShuffleLibrary();
    }

    protected override void ResolveEffect()
    {
      Controller.RevealLibrary();

      Enqueue<SelectCards>(
        controller: Controller,
        init: p =>
          {
            p.MinCount = _minCount;
            p.MaxCount = _maxCount;
            p.Validator(this);
            p.Zone = Zone.Library;
            p.Text = FormatText(_text);
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
            p.OwningCard = Source.OwningCard;
          });
    }
  }
}