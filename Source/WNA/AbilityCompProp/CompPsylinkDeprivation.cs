using RimWorld;
using Verse;
using WNA.WNADefOf;
using System;

namespace WNA.AbilityCompProp
{
    public class CompPsylinkDeprivation : CompAbilityEffect
    {
        public new PsylinkDeprivation Props => (PsylinkDeprivation)props;
        private void PsylinkDeprive(Pawn pawn)
        {
            Hediff hedifftoremove = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier);
            Hediff hedifftrauma = HediffMaker.MakeHediff(WNAMainDefOf.WNA_PsychicTrauma, pawn);
            Hediff hedifftoadd = HediffMaker.MakeHediff(WNAMainDefOf.WNA_Disabled, pawn);
            float sens = pawn.GetStatValue(StatDefOf.PsychicSensitivity);
            if (pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicAmplifier))
            {
                pawn.health.RemoveHediff(hedifftoremove);
                pawn.health.AddHediff(hedifftrauma);
            }
            else
            {
                pawn.health.AddHediff(hedifftoadd);
                hedifftoadd.Severity = 2 * sens ;
            }
        }
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            if (target.Pawn != null && target.Pawn != parent.pawn)
            {
                PsylinkDeprive(target.Pawn);
            }
        }
    }
}
