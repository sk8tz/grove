﻿namespace Grove.Modifiers
{
  public class DisableAllAbilities : Modifier, ICardModifier
  {
    private ActivatedAbilities _activatedAbilities;
    private SimpleAbilities _simpleAbilties;
    private TriggeredAbilities _triggeredAbilities;

    public override void Apply(ActivatedAbilities abilities)
    {
      abilities.DisableAll();
      _activatedAbilities = abilities;
    }

    public override void Apply(SimpleAbilities abilities)
    {
      abilities.Disable();
      _simpleAbilties = abilities;
    }

    public override void Apply(TriggeredAbilities abilities)
    {
      abilities.DisableAll();
      _triggeredAbilities = abilities;
    }

    protected override void Unapply()
    {
      _activatedAbilities.EnableAll();
      _simpleAbilties.Enable();
      _triggeredAbilities.EnableAll();
    }
  }
}