using RimWorld;
using Verse;

namespace WNA.WNADefOf
{
    [DefOf]
    public class WNAMainDefOf
    {
        public static DamageDef WNA_CastMelee;
        public static DamageDef WNA_CastRange;
        public static DamageDef WNA_CastBomb;
        public static DamageDef WNA_ChimingBlaze;
        public static DamageDef WNA_KineCut;
        public static DamageDef WNA_KinePoint;
        public static DamageDef WNA_KineBomb;
        public static DamageDef WNA_DemoCut;

        public static HediffDef WNA_Corrosion;
        public static HediffDef sWNA_DeathRefusal;
        public static HediffDef WNA_PsychicTrauma;
        public static HediffDef WNA_Disabled;

        [MayRequireAnomaly]
        public static MutantDef WNA_Halfbeing;

        public static PawnKindDef WNA_Saint;
        public static PawnKindDef WNA_Initiate;

        public static ThingDef WNA_WNThan;
        public static ThingDef WNA_Human;
    }
}
