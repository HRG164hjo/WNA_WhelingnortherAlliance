using UnityEngine;
using Verse;

namespace WNA.ThingCompProp
{
    public class ShieldGenerator : CompProperties
    {
        public int ticksToReset = 120;

        public float minDrawSize = 1.4f;

        public float maxDrawSize = 1.6f;

        public float energyLossPerHit = 0.01f;

        public float energyGenPerTick = 1;

        public float energyMax = 10000;

        public string shieldTexPath;

        public Color shieldColor = Color.black;

        public bool shieldInstaReset = false;
        public ShieldGenerator()
        {
            compClass = typeof(CompShieldGenerator);
        }
    }
}
