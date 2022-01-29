using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;
using static JudgePracticeMod.Main;
using SD;
using System.Text.RegularExpressions;
using System.IO;

namespace JudgePracticeMod
{
    public class TEXT : IText
    {
        public GameObject gameObject;
        public string Text
        {
            get => form;
            set => form = value;
        }
        public bool IsError = false;
        public G g;
        public string form = @"엄격 : <b>{SHit}</b> | <color=#ED3E3E>{STE}</color> <color=#EB9A46>{SVE}</color> <color=#E3E370>{SEP}</color> <color=#86E370>{SP}</color> <color=#E3E370>{SLP}</color> <color=#EB9A46>{SVL}</color> <color=#ED3E3E>{STL}</color>
보통 : <b>{NHit}</b> | <color=#ED3E3E>{NTE}</color> <color=#EB9A46>{NVE}</color> <color=#E3E370>{NEP}</color> <color=#86E370>{NP}</color> <color=#E3E370>{NLP}</color> <color=#EB9A46>{NVL}</color> <color=#ED3E3E>{NTL}</color>
느슨 : <b>{LHit}</b> | <color=#ED3E3E>{LTE}</color> <color=#EB9A46>{LVE}</color> <color=#E3E370>{LEP}</color> <color=#86E370>{LP}</color> <color=#E3E370>{LLP}</color> <color=#EB9A46>{LVL}</color> <color=#ED3E3E>{LTL}</color>";
        public string notplayingform = "Not Playing";
        public int num;
        public float X = 0.01f;
        public float Y = 1f;
        public Color color = new Color(1, 1, 1, 1);
        public int size = 40;
        public bool Destroyed = false;
        private string title;
        public static void AddSetting(int key, TextSetting value)
        {
            if (!TextSettings.ContainsKey(key))
            {
                TextSettings.Add(key, value);
            }
        }
        public static void Serialize()
        {
            TextSettings.SerializeJson(Path.Combine("Mods", "JudgePracticeMod", "TextSettings.jpm"));
        }
        public TEXT()
        {
            num = count++;
        }
        public void UpdateValue()
        {
            if (TextSettings.Remove(num))
                TextSettings.Add(num, (X, Y, new float[] { color.r, color.g, color.b, color.a }, size, form, notplayingform));
            else Main.Logger.Log("Can't Update Value!");
        }
        public void InitValue()
        {
            try
            {
                AddSetting(num, (X, Y, new float[] { color.r, color.g, color.b, color.a }, size, form, notplayingform));
                X = TextSettings[num].X;
                Y = TextSettings[num].Y;
                color.r = TextSettings[num].Color[0];
                color.g = TextSettings[num].Color[1];
                color.b = TextSettings[num].Color[2];
                color.a = TextSettings[num].Color[3];
                size = TextSettings[num].Size;
                form = TextSettings[num].Form;
                notplayingform = TextSettings[num].NotForm;
            }
            catch (Exception e)
            {
                mod.Logger.LogException(e);
            }
        }
        public void Init(bool em = false)
        {
            InitValue();
            IsError = em;
            if (em) notplayingform = string.Empty;
            title = $"Text {num}";
            gameObject = new GameObject(num.ToString());
            g = gameObject.AddComponent<G>(); 
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            g.Color = color;
            g.Center = new Vector2(X, Y);
            g.Position = new Vector2(X, Y);
            g.FontSize = size;

            if (em)
            {
                title = "ErrorMeter Text";
            }
        }
        public bool IsExpanded = false;
        public void GUI()
        {
            if (Destroyed) return;
            if (IsExpanded = GUILayout.Toggle(IsExpanded, title))
            {
                Drawer.IndentGUI(() =>
                {
                    if (!IsError)
                    {
                        GUILayout.BeginVertical();
                        GUILayout.BeginHorizontal();
                        if (UnityModManager.UI.DrawFloatField(ref X, "텍스트 X좌표 Text X-Position")) Do();
                        X = GUILayout.HorizontalSlider(X, 0, 1);
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        if (UnityModManager.UI.DrawFloatField(ref Y, "텍스트 Y좌표 Text Y-Position")) Do();
                        Y = GUILayout.HorizontalSlider(Y, 0, 1);
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        if (UnityModManager.UI.DrawIntField(ref size, "텍스트 크기 Text Size")) Do();
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("텍스트 색상 Text Color");
                        GUILayout.Space(1);
                        if (Drawer.DrawColor(ref color)) Do();
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        if (Drawer.DrawTextArea(ref form, "텍스트 형식 Text Form")) Do();
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        if (Drawer.DrawTextArea(ref notplayingform, "플레이 중이 아닐때 표시되는 텍스트 Text displayed when not playing")) Do();
                        //GUILayout.Label("플레이 중이 아닐때 표시되는 텍스트 Text displayed when not playing");
                        //notplayingform = GUILayout.TextArea(notplayingform);
                        //GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Destroy"))
                    {
                        if (num != 1 || IsError)
                        {
                            UnityEngine.Object.DestroyImmediate(gameObject);
                            Destroyed = true;
                            texts.Remove(this);
                            TextSettings.Remove(num);
                            if (IsError)
                            {
                                Set.Error = false;
                            }
                            else
                            {
                                Set.CompCount--;
                                count--;
                            }
                        }
                        else mod.Logger.Log("Can't Destroy First Text!");
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                });
            }
            void Do()
            {
                if (Destroyed) return;
                g.Color = color;
                g.Center = new Vector2(X, Y);
                g.Position = new Vector2(X, Y);
                g.FontSize = size;
                UpdateValue();
                Serialize();
                Set.Save(mod);
            }
        }
        public void Update()
        {
            if (Destroyed) return;
            if (IsPlaying)
            {
                g.Text = TagValues.Aggregate(form, (current, value) => current.Replace(value.Key, value.Value));
                g.Center = new Vector2(X, Y);
                g.Position = new Vector2(X, Y);
            }
            else
            {
                g.Text = notplayingform;
            }
        }
        public void Term()
        {
            if (Destroyed) return;
            UnityEngine.Object.DestroyImmediate(g);
            UpdateValue();
            Serialize();
            Set.Save(mod);
        }
    }
}
