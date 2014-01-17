﻿namespace Grove.Gameplay.Messages
{
  using System;
  using Zones;

  public class ZoneChanged
  {
    public Card Card { get; set; }
    public Player Controller { get { return Card.Controller; } }
    public Zone From { get; set; }
    public bool FromBattlefield { get { return From == Zone.Battlefield; } }
    public bool FromBattlefieldToGraveyard { get { return From == Zone.Battlefield && To == Zone.Graveyard; } }
    public Zone To { get; set; }
    public bool ToBattlefield { get { return To == Zone.Battlefield; } }
    public bool ToHand {get { return To == Zone.Hand; }}
    public bool FromHand { get { return From == Zone.Hand; } }    

    public bool DisplayInformationInUi()
    {
      return !(From.IsHiddenZone() && To.IsHiddenZone());
    }

    public override string ToString()
    {            
      if (To == Zone.Battlefield)                
        return String.Format("{0} entered the battlefield.", Card.Name);

      if (To == Zone.Graveyard)
      {
        if (From == Zone.Battlefield)
          return String.Format("{0} was destroyed.", Card.Name);
                
        if (From == Zone.Hand)
          return String.Format("{0} discarded {1}.", Controller.Name, Card.Name);
        
        return String.Format("{0} was put into graveyard.", Card.Name);
      }

      if (To == Zone.Hand)
      {
        if (From == Zone.Battlefield || From == Zone.Graveyard)
          return String.Format("{0} was returned to {1} hand.", Card.Name, Controller.Name);        
      }
                
      if (To == Zone.Exile)
      {
        return String.Format("{0} was exiled.", Card.Name);
      }      
            
      return String.Format("{0} was put to {1}.", Card.Name, To);
    }
  }
}