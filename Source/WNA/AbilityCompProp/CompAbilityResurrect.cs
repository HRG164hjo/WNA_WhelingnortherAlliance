﻿using RimWorld;
using UnityEngine;
using Verse;

namespace WNA.AbilityCompProp
{
    public class CompAbilityResurrect : CompAbilityEffect
    {
        public new AbilityResurrect Props => (AbilityResurrect)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn innerPawn = ((Corpse)target.Thing).InnerPawn;
            if (ResurrectionUtility.TryResurrect(innerPawn))
            {
                Messages.Message("MessagePawnResurrected".Translate(innerPawn), innerPawn, MessageTypeDefOf.PositiveEvent);
                MoteMaker.MakeAttachedOverlay(innerPawn, ThingDefOf.Mote_ResurrectFlash, Vector3.zero);
            }
        }
    }
}
