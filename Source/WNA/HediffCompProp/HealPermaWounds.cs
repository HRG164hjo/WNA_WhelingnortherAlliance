using Verse;
using WNA.HediffCompProp;

namespace WNA.HediffCompProp
{
    public class HealPermaWounds : HediffCompProperties
    {
        public int wna_healTickerMin = 600;

        public int wna_healTickerMax = 900;
        public HealPermaWounds()
        {
            compClass = typeof(CompHealPermaWounds);
        }
    }
}
