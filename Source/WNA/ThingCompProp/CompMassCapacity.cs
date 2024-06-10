using HarmonyLib;
using RimWorld;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using Verse;
using WNA.WNADefOf;

namespace WNA.ThingCompProp
{
    public class CompMassCapacity : ThingComp
    {
        public MassCapacity Props => (MassCapacity)props;

        [HarmonyPatch(typeof(MassUtility), nameof(MassUtility.Capacity))]
        public static float PostFix(Pawn p, StringBuilder explanation = null)
        {
            float num2 = p.BodySize;
            if (p.kindDef.race == WNAMainDefOf.WNA_WNThan)
            {
                num2 = p.BodySize * 1000000f;
            }
            else if (p.kindDef.race == WNAMainDefOf.WNA_Human)
            {
                num2 = p.BodySize * 1000f;
            }
            return num2;
        }
    }
}
