using HarmonyLib;
using Verse;

namespace WNA
{
    [StaticConstructorOnStartup]
    internal static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            new Harmony("hrg164hjo.whelingnorther.alliance").PatchAll();
        }
    }
}
