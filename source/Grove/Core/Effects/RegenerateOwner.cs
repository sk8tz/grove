﻿namespace Grove.Effects
{
  using Grove.AI;

  public class RegenerateOwner : Effect
  {
    public RegenerateOwner()
    {
      SetTags(EffectTag.Regenerate);
    }

    protected override void ResolveEffect()
    {
      Source.OwningCard.HasRegenerationShield = true;
    }
  }
}