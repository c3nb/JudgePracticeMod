using System;
using UnityEngine;
using System.Globalization;
using UnityModManagerNet;
using System.Reflection;
using HarmonyLib;
using static UnityModManagerNet.UnityModManager;

namespace JudgePracticeMod
{
    public delegate bool Draw(object instance, Type type, ModEntry mod, DrawFieldMask defaultMask, int unique);
    public static class Drawer
    {
        public static void IndentGUI(Action GUI, float indentSize = 20f)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(indentSize);
            GUILayout.BeginVertical();
            GUI();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        public static Draw Draw = (Draw)typeof(UI).GetMethod("Draw", AccessTools.all, null, new[] { typeof(object), typeof(Type), typeof(ModEntry), typeof(DrawFieldMask), typeof(int) }, null).CreateDelegate(typeof(Draw));
        public static bool DrawColor(ref Color vec, GUIStyle style = null, params GUILayoutOption[] option)
        {
            float[] array = new float[]
            {
        vec.r,
        vec.g,
        vec.b,
        vec.a
            };
            string[] labels = new string[]
            {
        "<color=#FF0000>R</color>",
        "<color=#00FF00>G</color>",
        "<color=#0000FF>B</color>",
        "A"
            };
            if (DrawFloatMultiField(ref array, labels, style, option))
            {
                vec = new Color(array[0], array[1], array[2], array[3]);
                return true;
            }
            return false;
        }
        public static bool DrawFloatMultiField(ref float[] values, string[] labels, GUIStyle style = null, params GUILayoutOption[] option)
        {
            if (values == null || values.Length == 0)
            {
                throw new ArgumentNullException("values");
            }
            if (labels == null || labels.Length == 0)
            {
                throw new ArgumentNullException("labels");
            }
            if (values.Length != labels.Length)
            {
                throw new ArgumentOutOfRangeException("labels");
            }
            bool result = false;
            float[] array = new float[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUILayout.Label(labels[i], new GUILayoutOption[]
                {
            GUILayout.ExpandWidth(false)
                });
                string text = GUILayout.TextField(values[i].ToString("f6"), style ?? GUI.skin.textField, option);
                GUILayout.EndHorizontal();
                float num;
                if (string.IsNullOrEmpty(text))
                {
                    array[i] = 0f;
                }
                else if (float.TryParse(text, NumberStyles.Any, NumberFormatInfo.CurrentInfo, out num))
                {
                    array[i] = num;
                }
                else
                {
                    array[i] = 0f;
                }
                if (array[i] != values[i])
                {
                    result = true;
                }
            }
            values = array;
            return result;
        }
        public static bool DrawTextArea(ref string value, string label, GUIStyle style = null, params GUILayoutOption[] option)
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label(label, new GUILayoutOption[]
            {
            GUILayout.ExpandWidth(false)
            });
            string text = GUILayout.TextArea(value, style ?? GUI.skin.textArea, option);
            GUILayout.EndHorizontal();
            if (text != value)
            {
                value = text;
                return true;
            }
            value = text;
            return false;
        }
    }
}
