using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using HarmonyLib;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace JudgePracticeMod
{
    public static class ErrorMeter
    {
        public static readonly Vector2 Perfect = new Vector2(0.5f, 0.119f);
        public static readonly Vector2 EarlyPerfect = new Vector2(0.45f, 0.095f);
        public static readonly Vector2 LatePerfect = new Vector2(0.55f, 0.095f);
        public static readonly Vector2 VeryEarly = new Vector2(0.435f, 0.075f);
        public static readonly Vector2 VeryLate = new Vector2(0.565f, 0.095f);
        public static readonly Vector2 TooEarly = new Vector2(0.427f, 0.035f);
        public static readonly Vector2 TooLate = new Vector2(0.573f, 0.035f);
        public static TEXT txt;
        public static void Do()
        {
            txt = new TEXT();
            Main.texts.Add(txt);
            txt.Init(true);
            Main.Set.Error = true;
		}
        [HarmonyPatch(typeof(scrHitErrorMeter), "DrawCurvedTick")]
        public static class Follow
        {
            public static bool Prefix(scrHitErrorMeter __instance, float angle, ref string[] ___cachedTweenIds, ref Image[] ___cachedTickImages, ref int ___tickIndex)
            {
                if (!Main.Set.Error) return true;
                Color color = (Color)typeof(scrHitErrorMeter).GetMethod("CalculateTickColor", AccessTools.all).Invoke(__instance, new object[] { angle });
                Image tickImage = ___cachedTickImages[___tickIndex];
                string text = ___cachedTweenIds[___tickIndex];
                tickImage.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
                DOTween.Kill(text, false);

                tickImage.color = color;
                tickImage.DOColor(color.WithAlpha(0f), __instance.tickLife).SetEase(Ease.InQuad).SetId(text).OnKill(delegate
                {
                    tickImage.color = Color.clear;
                });

                txt.g.mainText.color = color;
                txt.g.mainText.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
                txt.g.shadowText.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
                ___tickIndex = (___tickIndex + 1) % __instance.tickCacheSize;
                return false;
            }
        }

    }
}
