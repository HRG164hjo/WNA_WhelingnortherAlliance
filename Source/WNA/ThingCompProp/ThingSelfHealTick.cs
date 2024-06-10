using RimWorld;
using Verse;

namespace WNA.ThingCompProp
{
    public class ThingSelfHealTick : CompProperties
    {
        public int ticksPerHeal;

        public int wna_wallHealDenominator = 10;

        public ThingSelfHealTick()
        {
            compClass = typeof(CompThingSelfHealTick);
        }
    }
}
