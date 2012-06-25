﻿namespace Grove.Core
{
  using System.Collections.Generic;
  using System.Linq;
  using Controllers;
  using Infrastructure;
  using Messages;

  [Copyable]
  public class Combat : IHashable
  {
    private readonly IAttackerFactory _attackerFactory;
    private readonly TrackableList<Attacker> _attackers;
    private readonly IBlockerFactory _blockerFactory;
    private readonly TrackableList<Blocker> _blockers;
    private readonly Players _players;
    private readonly Publisher _publisher;

    private Combat() {}

    public Combat(ChangeTracker changeTracker, IAttackerFactory attackerFactory, IBlockerFactory blockerFactory,
                  Publisher publisher, Players players)
    {
      _attackerFactory = attackerFactory;
      _blockerFactory = blockerFactory;
      _publisher = publisher;
      _players = players;
      _attackers = new TrackableList<Attacker>(changeTracker);
      _blockers = new TrackableList<Blocker>(changeTracker);
    }

    public IEnumerable<Attacker> Attackers
    {
      get { return _attackers; }
    }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(_attackers),
        calc.Calculate(_blockers));
    }

    public void AssignCombatDamage(Decisions decisions, bool firstStrike = false)
    {
      var blockers = firstStrike
        ? _blockers.Where(x => x.Card.HasFirstStrike)
        : _blockers.Where(x => x.Card.HasNormalStrike);

      var attackers = firstStrike
        ? _attackers.Where(x => x.Card.HasFirstStrike)
        : _attackers.Where(x => x.Card.HasNormalStrike);

      foreach (var blocker in blockers)
      {
        blocker.DistributeDamageToAttacker();
      }

      foreach (var attacker in attackers)
      {
        attacker.DistributeDamageToBlockers(decisions);
      }
    }

    public bool CanAnyAttackerBeBlockedByAny(IEnumerable<Card> creatures)
    {
      foreach (var attacker in _attackers)
      {
        foreach (var creature in creatures)
        {
          if (attacker.CanBeBlockedBy(creature))
            return true;
        }
      }

      return false;
    }

    public void DealAssignedDamage()
    {
      foreach (var attacker in _attackers)
      {
        attacker.DealAssignedDamage();
      }

      foreach (var blocker in _blockers)
      {
        blocker.DealAssignedDamage();
      }

      _players.Defending.DealAssignedDamage();
    }

    public void JoinAttack(Card card, bool wasDeclared = false)
    {
      var attacker = _attackerFactory.Create(card);
      _attackers.Add(attacker);

      if (!card.Has().Vigilance)
        card.Tap();

      PublishMessage(new AttackerJoinedCombat
      {
        Attacker = attacker,
        WasDeclared = wasDeclared        
      });
    }

    public void DeclareAttacker(Card card)
    {
      if (!card.CanAttack)
        return;

      JoinAttack(card, wasDeclared: true);      
    }    

    public void DeclareBlocker(Card cardBlocker, Card cardAttacker)
    {
      var attacker = FindAttacker(cardAttacker);
      var blocker = _blockerFactory.Create(cardBlocker, attacker);

      attacker.AddBlocker(blocker);
      _blockers.Add(blocker);

      _publisher.Publish(new BlockerDeclared
        {
          Blocker = blocker,
          Attacker = attacker
        });
    }

    public bool IsAttacker(Card card)
    {
      return FindAttacker(card) != null;
    }

    public bool IsBlocker(Card card)
    {
      return FindBlocker(card) != null;
    }

    public bool IsBlockerThatWillBeDealtLeathalDamageAndWillNotKillAttacker(Card card)
    {
      var blocker = FindBlocker(card);

      if (blocker == null || blocker.Attacker == null)
        return false;

      return
        blocker.Attacker.TotalDamageThisCanDeal >= blocker.LifepointsLeft &&
          blocker.Attacker.LifepointsLeft > blocker.TotalDamageThisCanDeal;
    }

    public void Remove(Card card)
    {
      var attacker = FindAttacker(card);

      if (attacker != null)
      {
        _attackers.Remove(attacker);
        attacker.RemoveFromCombat();
        return;
      }

      var blocker = FindBlocker(card);

      if (blocker != null)
      {
        _blockers.Remove(blocker);
        blocker.RemoveFromCombat();
      }
    }

    public void RemoveAll()
    {
      foreach (var blocker in _blockers)
      {
        blocker.RemoveFromCombat();
      }

      foreach (var attacker in _attackers)
      {
        attacker.RemoveFromCombat();
      }

      _blockers.Clear();
      _attackers.Clear();
    }

    public void SetDamageAssignmentOrder(Decisions decisions)
    {
      foreach (var attacker in _attackers)
      {
        decisions.EnqueueSetDamageAssignmentOrder(
          attacker.Controller, attacker);
      }
    }

    private Attacker FindAttacker(Card cardAttacker)
    {
      return _attackers.FirstOrDefault(a => a.Card == cardAttacker);
    }

    private Blocker FindBlocker(Card cardBlocker)
    {
      return _blockers.FirstOrDefault(b => b.Card == cardBlocker);
    }

    private void PublishMessage<TMessage>(TMessage message)
    {
      _publisher.Publish(message);
    }

    public bool AnyCreaturesWithFirstStrike()
    {
      return _attackers.Any(x => x.Card.HasFirstStrike) ||
        _blockers.Any(x => x.Card.HasFirstStrike);
    }

    public bool AnyCreaturesWithNormalStrike()
    {
      return _attackers.Any(x => x.Card.HasNormalStrike) ||
        _blockers.Any(x => x.Card.HasNormalStrike);
    }
  }
}