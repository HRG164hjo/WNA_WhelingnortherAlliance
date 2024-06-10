using RimWorld;
using Verse;

namespace WNA.ThingCompProp
{
    public class CompThingSelfHealTick : ThingComp
    {
        public int ticksPassedSinceLastHeal;

        public ThingSelfHealTick Props => (ThingSelfHealTick)props;

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref ticksPassedSinceLastHeal, "ticksPassedSinceLastHeal", 0);
        }

        public override void CompTick()
        {
            Tick(1);
        }

        public override void CompTickRare()
        {
            Tick(250);
        }

        public override void CompTickLong()
        {
            Tick(2000);
        }

        private void Tick(int ticks)
        {
            ticksPassedSinceLastHeal += ticks;
            if (ticksPassedSinceLastHeal >= Props.ticksPerHeal)
            {
                ticksPassedSinceLastHeal = 0;
                if (parent.HitPoints < parent.MaxHitPoints)
                {
                    parent.HitPoints += parent.MaxHitPoints / Props.wna_wallHealDenominator;
                };
                if (parent.HitPoints > parent.MaxHitPoints)
                {
                    parent.HitPoints = parent.MaxHitPoints;
                }
            }
        }
    }
}
