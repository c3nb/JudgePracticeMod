using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityModManagerNet;
using static JudgePracticeMod.Main;
using System.Globalization;
using SD;
using System.Text.RegularExpressions;
using System.IO;

namespace JudgePracticeMod
{
    public class G : MonoBehaviour
    {
        public Text mainText;
        public Text shadowText;

        /// <summary>
        /// The text that this object displays.
        /// </summary>
        public string Text
        {
            get
            {
                return mainText.text;
            }
            set
            {
                mainText.text = value;
                if (Set.Shadow)
                shadowText.text = value;
            }
        }

        /// <summary>
        /// The alignment of the text.
        /// </summary>
        public TextAnchor Alignment
        {
            get
            {
                return mainText.alignment;
            }
            set
            {
                mainText.alignment = value;
                if (Set.Shadow)
                    shadowText.alignment = value;
            }
        }

        /// <summary>
        /// The font size of the text.
        /// </summary>
        public int FontSize
        {
            get
            {
                return mainText.fontSize;
            }
            set
            {
                mainText.fontSize = value;
                if (Set.Shadow)
                {
                    shadowText.fontSize = value;
                    shadowText.rectTransform.anchoredPosition =
                        Position + new Vector2(value / 20f, -value / 20f);
                }  
            }
        }

        /// <summary>
        /// The color of the text.
        /// </summary>
        public Color Color
        {
            get
            {
                return mainText.color;
            }
            set
            {
                mainText.color = value;
            }
        }

        /// <summary>
        /// The normalized center position within the bounding box of the text.
        /// </summary>
        public Vector2 Center
        {
            get
            {
                return mainText.rectTransform.anchorMin;
            }
            set
            {
                mainText.rectTransform.anchorMin = value;
                mainText.rectTransform.anchorMax = value;
                mainText.rectTransform.pivot = value;
                if (Set.Shadow)
                {
                    shadowText.rectTransform.anchorMin = value;
                    shadowText.rectTransform.anchorMax = value;
                    shadowText.rectTransform.pivot = value;
                }
            }
        }

        /// <summary>
        /// The position within the <see cref="Canvas"/> that the text is at.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return mainText.rectTransform.anchoredPosition;
            }
            set
            {
                mainText.rectTransform.anchoredPosition = value;
                if (Set.Shadow)
                {
                    shadowText.rectTransform.anchoredPosition =
                                       value + new Vector2(FontSize / 20f, -FontSize / 20f);
                }
               
            }
        }

        /// <summary>
        /// Unity's Awake lifecycle event handler. Creates the text with a drop
        /// shadow.
        /// </summary>
        protected void Awake()
        {
            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            ContentSizeFitter fitter;

            if (Set.Shadow)
            {
                GameObject shadowObject = new GameObject();
                fitter = shadowObject.AddComponent<ContentSizeFitter>();
                fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                shadowObject.transform.SetParent(transform);
                shadowText = shadowObject.AddComponent<Text>();
                shadowText.font = RDString.GetFontDataForLanguage(RDString.language).font;
                shadowText.color = Color.black.WithAlpha(0.4f);
            }

            GameObject mainObject = new GameObject();
            mainObject.transform.SetParent(transform);
            fitter = mainObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            mainText = mainObject.AddComponent<Text>();
            mainText.font = RDString.GetFontDataForLanguage(RDString.language).font;
        }
    }
    public class TEXT : IText
    {
        public GameObject gameObject;
        public bool IsError = false;
        public G g;
        public string form = "";
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
            g = gameObject.AddComponent<G>(); UnityEngine.Object.DontDestroyOnLoad(g);
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
                    //GUILayout.Label("텍스트 형식 Text Form");
                    //form = GUILayout.TextArea(form);
                    //GUILayout.FlexibleSpace();
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
                        UnityEngine.Object.Destroy(gameObject);
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
            if (scrController.instance.gameworld)
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
            UnityEngine.Object.Destroy(g);
            UpdateValue();
            Serialize();
            Set.Save(mod);
        }
    }
    public interface IText
    {
        void Init(bool ErrorMeter);
        void GUI();
        void Update();
        void Term();
    }
    public static class Drawer
    {
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
        "<color=#0000BB>B</color>",
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
