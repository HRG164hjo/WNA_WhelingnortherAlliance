using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;
using WNA.WNAMiscs;

namespace WNA.ThingCompProp
{
    public class CompShieldGenerator : ThingComp
    {
        protected float energy;

        protected int ticksToReset = -1;

        protected int lastKeepDisplayTick = -9999;

        private Vector3 impactAngleVect;

        private int lastAbsorbDamageTick = -9999;

        protected Material bubbleMat;

        private Material BubbleMat
        {
            get
            {
                if ((object)bubbleMat == null)
                {
                    if (Props.shieldTexPath.NullOrEmpty())
                    {
                        bubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent, Props.shieldColor);
                    }
                    else
                    {
                        bubbleMat = MaterialPool.MatFrom(Props.shieldTexPath, ShaderDatabase.Transparent, Props.shieldColor);
                    }
                }
                return bubbleMat;
            }
        }

        public ShieldGenerator Props => (ShieldGenerator)props;

        public float energyMax => Props.energyMax;
        /*{
            get
            {
                float num = Props.energyMax;
                return num;
            }
        } => Props.energyMax;*/
        public float EnergyMax => energyMax;
        public float Energy => energy;

        public ShieldState ShieldState
        {
            get
            {
                if (parent is Pawn p && (p.IsCharging() || p.IsSelfShutdown()))
                {
                    return ShieldState.Disabled;
                }
                CompCanBeDormant comp = parent.GetComp<CompCanBeDormant>();
                if (comp != null && !comp.Awake)
                {
                    return ShieldState.Disabled;
                }
                if (ticksToReset <= 0)
                {
                    return ShieldState.Active;
                }
                return ShieldState.Resetting;
            }
        }

        protected bool ShouldDisplay = true;

        protected Pawn PawnOwner
        {
            get
            {
                if (parent is Apparel apparel)
                {
                    return apparel.Wearer;
                }
                if (parent is Pawn result)
                {
                    return result;
                }
                return null;
            }
        }

        public bool IsApparel => parent is Apparel;

        private bool IsBuiltIn => !IsApparel;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref energy, "energy", 0f);
            Scribe_Values.Look(ref ticksToReset, "ticksToReset", -1);
            Scribe_Values.Look(ref lastKeepDisplayTick, "lastKeepDisplayTick", 0);
        }

        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            foreach (Gizmo item in base.CompGetWornGizmosExtra())
            {
                yield return item;
            }
            if (IsApparel)
            {
                foreach (Gizmo gizmo in GetGizmos())
                {
                    yield return gizmo;
                }
            }
            if (!DebugSettings.ShowDevGizmos)
            {
                yield break;
            }
            Command_Action command_Action = new Command_Action();
            command_Action.defaultLabel = "DEV: Break";
            command_Action.action = Break;
            yield return command_Action;
            if (ticksToReset > 0)
            {
                Command_Action command_Action2 = new Command_Action();
                command_Action2.defaultLabel = "DEV: Clear reset";
                command_Action2.action = delegate
                {
                    ticksToReset = 0;
                };
                yield return command_Action2;
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo item in base.CompGetGizmosExtra())
            {
                yield return item;
            }
            if (!IsBuiltIn)
            {
                yield break;
            }
            foreach (Gizmo gizmo in GetGizmos())
            {
                yield return gizmo;
            }
        }

        private IEnumerable<Gizmo> GetGizmos()
        {
            if ((PawnOwner.Faction == Faction.OfPlayer || (parent is Pawn pawn && pawn.RaceProps.IsMechanoid)) && Find.Selector.SingleSelectedThing == PawnOwner)
            {
                Gizmo_ShieldStatus gizmo_ShieldStatus = new Gizmo_ShieldStatus
                {
                    shield = this
                };
                yield return gizmo_ShieldStatus;
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (PawnOwner == null)
            {
                energy = 0f;
            }
            else if (ShieldState == ShieldState.Resetting)
            {
                ticksToReset--;
                if (ticksToReset <= 0)
                {
                    Reset();
                }
            }
            else if (ShieldState == ShieldState.Active)
            {
                energy = energyMax;

                if (energy < energyMax)
                {
                    energy += Props.energyGenPerTick;
                }
            }
        }

        public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            absorbed = false;
            if (ShieldState != 0 || PawnOwner == null)
            {
                return;
            }
            else
            {
                energy -= Props.energyLossPerHit;
                if (energy < 0f)
                {
                    if (Props.shieldInstaReset)
                    {
                        Reset();
                    }
                    else
                    {
                        Break();
                    }
                        
                }
                else
                {
                    AbsorbedDamage(dinfo);
                }
                absorbed = true;
            }
        }

        public void KeepDisplaying()
        {
            lastKeepDisplayTick = Find.TickManager.TicksGame;
        }

        private void AbsorbedDamage(DamageInfo dinfo)
        {
            SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(PawnOwner.Position, PawnOwner.Map));
            impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
            Vector3 loc = PawnOwner.TrueCenter() + impactAngleVect.RotatedBy(180f) * 0.5f;
            float num = Mathf.Min(10f, 2f + dinfo.Amount / 10f);
            FleckMaker.Static(loc, PawnOwner.Map, FleckDefOf.ExplosionFlash, num);
            int num2 = (int)num;
            for (int i = 0; i < num2; i++)
            {
                FleckMaker.ThrowDustPuff(loc, PawnOwner.Map, Rand.Range(0.8f, 1.2f));
            }
            lastAbsorbDamageTick = Find.TickManager.TicksGame;
            KeepDisplaying();
        }

        private void Break()
        {
            float scale = Mathf.Lerp(Props.minDrawSize, Props.maxDrawSize, energy);
            EffecterDefOf.Shield_Break.SpawnAttached(parent, parent.MapHeld, scale);
            FleckMaker.Static(PawnOwner.TrueCenter(), PawnOwner.Map, FleckDefOf.ExplosionFlash, 12f);
            for (int i = 0; i < 6; i++)
            {
                FleckMaker.ThrowDustPuff(PawnOwner.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle(Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f), PawnOwner.Map, Rand.Range(0.8f, 1.2f));
            }
            energy = 0f;
            ticksToReset = Props.ticksToReset;
        }

        private void Reset()
        {
            if (PawnOwner.Spawned)
            {
                SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(PawnOwner.Position, PawnOwner.Map));
                FleckMaker.ThrowLightningGlow(PawnOwner.TrueCenter(), PawnOwner.Map, 3f);
            }
            ticksToReset = -1;
            energy = energyMax;
        }

        public override void CompDrawWornExtras()
        {
            base.CompDrawWornExtras();
            if (IsApparel)
            {
                Draw();
            }
        }

        public override void PostDraw()
        {
            base.PostDraw();
            if (IsBuiltIn)
            {
                Draw();
            }
        }

        private void Draw()
        {
            if (ShieldState == ShieldState.Active && ShouldDisplay)
            {
                float num = Mathf.Lerp(Props.minDrawSize, Props.maxDrawSize, energy);
                Vector3 drawPos = PawnOwner.Drawer.DrawPos;
                drawPos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
                int num2 = Find.TickManager.TicksGame - lastAbsorbDamageTick;
                if (num2 < 8)
                {
                    float num3 = (float)(8 - num2) / 8f * 0.05f;
                    drawPos += impactAngleVect * num3;
                    num -= num3;
                }
                float angle = Rand.Range(0, 360);
                Vector3 s = new Vector3(num, 1f, num);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(drawPos, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, BubbleMat, 0);
            }
        }
    }

}
