using System;
using SD;
using HarmonyLib;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Extensions;
using System.Collections.Generic;

namespace JudgePracticeMod
{
    public class Patches
    {
        [HarmonyPatch(typeof(scrPlanet), "SwitchChosen")]
        public static class TimingP
        {
            public static void Postfix(scrPlanet __instance)
            {
                if (__instance.controller.gameworld)
                {
                    Main.TagValues["{Timing}"] = Math.Round((__instance.angle - __instance.targetExitAngle) * (__instance.controller.isCW ? 1.0 : -1.0) * 60000.0 / (3.1415926535897931 * (double)__instance.conductor.bpm * __instance.controller.speed * (double)__instance.conductor.song.pitch), Main.Set.tdecimals).ToString();
                }
                else
                {
                    Main.TagValues["{Timing}"] = 0.ToString();
                }
            }
        }
        public static HitMargin GetHitMargin(float angle)
        {
            double bpmTimesSpeed = scrConductor.instance.bpm * scrController.instance.speed;
            double conductorPitch = scrConductor.instance.song.pitch;
            double counted =
                scrMisc.GetAdjustedAngleBoundaryInDeg(
                    HitMarginGeneral.Counted, bpmTimesSpeed, conductorPitch);
            double perfect =
                scrMisc.GetAdjustedAngleBoundaryInDeg(
                    HitMarginGeneral.Perfect, bpmTimesSpeed, conductorPitch);
            double pure =
                scrMisc.GetAdjustedAngleBoundaryInDeg(
                    HitMarginGeneral.Pure, bpmTimesSpeed, conductorPitch);
            if (angle < -counted)
            {
                return HitMargin.TooEarly;
            }
            else if (angle < -perfect)
            {
                return HitMargin.VeryEarly;
            }
            else if (angle < -pure)
            {
                return HitMargin.EarlyPerfect;
            }
            else if (angle <= pure)
            {
                return HitMargin.Perfect;
            }
            else if (angle <= perfect)
            {
                return HitMargin.LatePerfect;
            }
            else if (angle <= counted)
            {
                return HitMargin.VeryLate;
            }
            else
            {
                return HitMargin.TooLate;
            }
        }
        public static HitMargin GetHitMarginForDifficulty(float angle, Difficulty difficulty)
        {
            Difficulty temp = GCS.difficulty;
            GCS.difficulty = difficulty;
            HitMargin margin = GetHitMargin(angle);
            GCS.difficulty = temp;
            return margin;
        }
        public static string nmargin;
        public static string smargin;
        public static string lmargin;
        public static float angle;
        public static HitMargin Lenient;
        public static HitMargin Normal;
        public static HitMargin Strict;
        public static int failCount;
        [HarmonyPatch(typeof(scrUIController), "WipeFromBlack")]
        public class Patch1
        {
            public static void Postfix()
            {
                if (Main.Set.restartreset)
                {
                    Main.Reset();
                }
                Main.isFailActioned = false;
            }
        }
        [HarmonyPatch(typeof(scrController), "QuitToMainMenu")]
        public class Patch2
        {
            public static void Postfix()
            {
                if (Main.Set.MenuReset)
                {
                    Main.Reset();
                }
                Main.isFailActioned = false;
            }
        }
        [HarmonyPatch(typeof(scrMistakesManager), "AddHit")]
        public class Patch3
        {
            public static void Postfix(HitMargin hit)
            {
                if (Main.Set.Error)
                {
                    switch (hit)
                    {
                        case HitMargin.TooEarly: ErrorMeter.txt.form = "{CurTE}"; ErrorMeter.txt.X = ErrorMeter.TooEarly.x; ErrorMeter.txt.Y = ErrorMeter.TooEarly.y; break;
                        case HitMargin.VeryEarly: ErrorMeter.txt.form = "{CurVE}"; ErrorMeter.txt.X = ErrorMeter.VeryEarly.x; ErrorMeter.txt.Y = ErrorMeter.VeryEarly.y; break;
                        case HitMargin.EarlyPerfect: ErrorMeter.txt.form = "{CurEP}"; ErrorMeter.txt.X = ErrorMeter.EarlyPerfect.x; ErrorMeter.txt.Y = ErrorMeter.EarlyPerfect.y; break;
                        case HitMargin.Perfect: ErrorMeter.txt.form = "{CurP}"; ErrorMeter.txt.X = ErrorMeter.Perfect.x; ErrorMeter.txt.Y = ErrorMeter.Perfect.y; break;
                        case HitMargin.LatePerfect: ErrorMeter.txt.form = "{CurLP}"; ErrorMeter.txt.X = ErrorMeter.LatePerfect.x; ErrorMeter.txt.Y = ErrorMeter.LatePerfect.y; break;
                        case HitMargin.VeryLate: ErrorMeter.txt.form = "{CurVL}"; ErrorMeter.txt.X = ErrorMeter.VeryLate.x; ErrorMeter.txt.Y = ErrorMeter.VeryLate.y; break;
                        case HitMargin.TooLate: ErrorMeter.txt.form = "{CurTL}"; ErrorMeter.txt.X = ErrorMeter.TooLate.x; ErrorMeter.txt.Y = ErrorMeter.TooLate.y; break;
                    }
                }
                if (hit == HitMargin.Perfect)
                {
                    Main.combo++;
                }
                else
                {
                    Main.combo = 0;
                }
                switch (hit)
                {
                    case HitMargin.VeryEarly:
                    case HitMargin.VeryLate:
                        Main.score += 91;
                        break;
                    case HitMargin.EarlyPerfect:
                    case HitMargin.LatePerfect:
                        Main.score += 150;
                        break;
                    case HitMargin.Perfect:
                        Main.score += 300;
                        break;
                }
                Main.CalculateScore();
            }
        }
        [HarmonyPatch(typeof(scrController), "OnLandOnPortal")]
        public class Patch4
        {
            public static void Postfix()
            {
                Main.isFailActioned = false;
            }
        }
        [HarmonyPatch(typeof(scrMisc), "GetHitMargin")]
        public class Patch5
        {
            public static bool Prefix(float hitangle, float refangle, bool isCW, float bpmTimesSpeed, float conductorPitch, ref HitMargin __result)
            {
                float num = (hitangle - refangle) * (float)(isCW ? 1 : -1);
                HitMargin result = HitMargin.TooEarly;
                float num2 = num;
                num2 = 57.29578f * num2;
                double adjustedAngleBoundaryInDeg = scrMisc.GetAdjustedAngleBoundaryInDeg(HitMarginGeneral.Counted, (double)bpmTimesSpeed, (double)conductorPitch);
                double adjustedAngleBoundaryInDeg2 = scrMisc.GetAdjustedAngleBoundaryInDeg(HitMarginGeneral.Perfect, (double)bpmTimesSpeed, (double)conductorPitch);
                double adjustedAngleBoundaryInDeg3 = scrMisc.GetAdjustedAngleBoundaryInDeg(HitMarginGeneral.Pure, (double)bpmTimesSpeed, (double)conductorPitch);
                if ((double)num2 > -adjustedAngleBoundaryInDeg)
                {
                    result = HitMargin.VeryEarly;
                }
                if ((double)num2 > -adjustedAngleBoundaryInDeg2)
                {
                    result = HitMargin.EarlyPerfect;
                }
                if ((double)num2 > -adjustedAngleBoundaryInDeg3)
                {
                    result = HitMargin.Perfect;
                }
                if ((double)num2 > adjustedAngleBoundaryInDeg3)
                {
                    result = HitMargin.LatePerfect;
                }
                if ((double)num2 > adjustedAngleBoundaryInDeg2)
                {
                    result = HitMargin.VeryLate;
                }
                if ((double)num2 > adjustedAngleBoundaryInDeg)
                {
                    result = HitMargin.TooLate;
                }
                Lenient = GetHitMarginForDifficulty(num2, Difficulty.Lenient);
                Normal = GetHitMarginForDifficulty(num2, Difficulty.Normal);
                Strict = GetHitMarginForDifficulty(num2, Difficulty.Strict);
                lmargin = RDString.Get("HitMargin." + Lenient.ToString());
                nmargin = RDString.Get("HitMargin." + Normal.ToString());
                smargin = RDString.Get("HitMargin." + Strict.ToString());
                Main.lcounts[Lenient]++;
                Main.ncounts[Normal]++;
                Main.scounts[Strict]++;
                angle = num2;
                __result = result;
                return false;
            }
        }
        [HarmonyPatch(typeof(scrController), "ResetCustomLevel")]
        public static class Patch6
        {
            public static void Postfix()
            {
                if (Overload.overloads.Count > 0)
                    Overload.overloads.SerializeJson(Path.Combine("Mods", "JudgePracticeMod", "Overloads.jpm"));
            }
        }
        [HarmonyPatch(typeof(scnEditor), "ResetScene")]
        public static class Patch7
        {
            public static void Postfix()
            {
                if (Overload.overloads.Count > 0)
                    Overload.overloads.SerializeJson(Path.Combine("Mods", "JudgePracticeMod", "Overloads.jpm"));
                Main.isFailActioned = false;
            }
        }
    }
}
