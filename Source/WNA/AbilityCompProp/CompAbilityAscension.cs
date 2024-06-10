using RimWorld;
using Verse;
using WNA.WNADefOf;

namespace WNA.AbilityCompProp
{
    public class CompAbilityAscension : CompAbilityEffect
    {

        public new AbilityAscension Props => (AbilityAscension)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Pawn pawn = target.Pawn;
            base.Apply(target, dest);
            pawn.Kill(new DamageInfo(WNAMainDefOf.WNA_CastRange, 1000000f, 0f, -1f, parent.pawn));
            pawn.Corpse?.Destroy();
        }

        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return false;
        }

        public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
        {
            return Valid(target);
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            Pawn pawn = target.Pawn;
            if (pawn == null)
            {
                return false;
            }
            return true;
        }
    }

}
