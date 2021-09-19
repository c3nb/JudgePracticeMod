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
using System.Text.RegularExpressions;

namespace JudgePracticeMod
{
    public class G : MonoBehaviour
    {
        private Text mainText;
        private Text shadowText;

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
                shadowText.fontSize = value;
                shadowText.rectTransform.anchoredPosition =
                    Position + new Vector2(value / 20f, -value / 20f);
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
                shadowText.rectTransform.anchorMin = value;
                shadowText.rectTransform.anchorMax = value;
                shadowText.rectTransform.pivot = value;
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
                shadowText.rectTransform.anchoredPosition =
                    value + new Vector2(FontSize / 20f, -FontSize / 20f);
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

            GameObject shadowObject = new GameObject();
            fitter = shadowObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            shadowObject.transform.SetParent(transform);
            shadowText = shadowObject.AddComponent<Text>();
            shadowText.font = RDString.GetFontDataForLanguage(RDString.language).font;
            shadowText.color = Color.black.WithAlpha(0.4f);

            GameObject mainObject = new GameObject();
            mainObject.transform.SetParent(transform);
            fitter = mainObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            mainText = mainObject.AddComponent<Text>();
            mainText.font = RDString.GetFontDataForLanguage(RDString.language).font;
        }
    }
    public class nG : MonoBehaviour
    {
        void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.font = RDString.GetFontDataForLanguage(RDString.language).font;
            style.normal.textColor = color;
            style.fontSize = size;
            style.richText = true;
            GUI.Label(new Rect(X, Y, Screen.width, Screen.height), text, style);
        }
        public float X = 0;
        public float Y = 0;
        public Color color = new Color(1, 1, 1, 1);
        public int size = 40;
        public string text = "";
    }
    public class TEXT : IText
    {
        public GameObject gameObject;
        public nG ng;
        public G g;
        public string form = "";
        public string notplayingform = "Not Playing";
        public int num;
        public float X = 0.01f;
        public float Y = 1f;
        public Color color = new Color(1, 1, 1, 1);
        public int size = 40;
        public bool Destroyed = false;

        public TEXT()
        {
            num = count++;
        }
        public void UpdateValue()
        {
            if (!((Set.X.Count < 1) && (Set.Y.Count < 1) && (Set.size.Count < 1) && (Set.form.Count < 1) && (Set.notform.Count < 1) && (Set.color.Count < 1)))
            {
                Set.color[num - 1] = color;
                Set.X[num - 1] = X;
                Set.Y[num - 1] = Y;
                Set.size[num - 1] = size;
                Set.form[num - 1] = form;
                Set.notform[num - 1] = notplayingform;
            }
        }
        public void InitValue()
        {
            try
            {
                X = Set.X[num - 1];
                Y = Set.Y[num - 1];
                size = Set.size[num - 1];
                form = Set.form[num - 1];
                notplayingform = Set.notform[num - 1];
                color = Set.color[num - 1];
            }
            catch (Exception e)
            {
                mod.Logger.LogException(e);
                Set.X.Add(X);
                Set.Y.Add(Y);
                Set.size.Add(size);
                Set.form.Add(form);
                Set.notform.Add(notplayingform);
                Set.color.Add(color);

                X = Set.X[num - 1];
                Y = Set.Y[num - 1];
                size = Set.size[num - 1];
                form = Set.form[num - 1];
                notplayingform = Set.notform[num - 1];
                color = Set.color[num - 1];
            }
            /*if ((Set.X.Count + 1 <= Set.CompCount) && (Set.Y.Count + 1 <= Set.CompCount) && (Set.size.Count + 1 <= Set.CompCount) && (Set.form.Count + 1 <= Set.CompCount) && (Set.notform.Count + 1 <= Set.CompCount) && (Set.color.Count + 1 <= Set.CompCount))
            {
                Set.X.Add(X);
                Set.Y.Add(Y);
                Set.size.Add(size);
                Set.form.Add(form);
                Set.notform.Add(notplayingform);
                Set.color.Add(color);
            }*/
        }
        public void Init()
        {
            InitValue();
            /*if (!((Set.X.Count < 1) && (Set.Y.Count < 1) && (Set.size.Count < 1) && (Set.form.Count < 1) && (Set.notform.Count < 1) && (Set.color.Count < 1)))
            {
                X = Set.X[num - 1];
                Y = Set.Y[num - 1];
                size = Set.size[num - 1];
                form = Set.form[num - 1];
                notplayingform = Set.notform[num - 1];
                color = Set.color[num - 1];
            }*/
            gameObject = new GameObject(num.ToString());
            if (Set.Shadow)
            {
                g = gameObject.AddComponent<G>(); UnityEngine.Object.DontDestroyOnLoad(g);
                g.Color = color;
                g.Center = new Vector2(X, Y);
                g.Position = new Vector2(X, Y);
                g.FontSize = size;
            }
            else { ng = gameObject.AddComponent<nG>(); UnityEngine.Object.DontDestroyOnLoad(ng); };
        }
        public bool IsExpanded = false;
        public void GUI()
        {
            if (Destroyed) return;
            if (IsExpanded = GUILayout.Toggle(IsExpanded, $"Text {num}"))
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
                GUILayout.Label("텍스트 형식 Text Form");
                form = GUILayout.TextArea(form);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("플레이 중이 아닐때 표시되는 텍스트 Text displayed when not playing");
                notplayingform = GUILayout.TextArea(notplayingform);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Destroy"))
                {
                    if (num != 1)
                    {
                        UnityEngine.Object.Destroy(gameObject);
                        Destroyed = true;
                        Set.CompCount--;
                        count--;
                        texts.RemoveAt(num - 1);
                        Set.X.RemoveAt(num - 1);
                        Set.Y.RemoveAt(num - 1);
                        Set.size.RemoveAt(num - 1);
                        Set.form.RemoveAt(num - 1);
                        Set.notform.RemoveAt(num - 1);
                        Set.color.RemoveAt(num - 1);
                    }
                    else mod.Logger.Log("Can't Destroy First Text!");
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            void Do()
            {
                if (Destroyed) return;
                if (Set.Shadow)
                {
                    g.Color = color;
                    g.Center = new Vector2(X, Y);
                    g.Position = new Vector2(X, Y);
                    g.FontSize = size;
                }
                else
                {
                    ng.color = color;
                    ng.X = X;
                    ng.Y = Y;
                    ng.size = size;
                }
                UpdateValue();
                Set.Save(mod);
            }
        }
        public void Update()
        {
            if (Destroyed) return;
            if (scrController.instance.gameworld)
            {
                if (Set.Shadow)
                {
                    g.Text = TagValues.Aggregate(form, (current, value) => current.Replace(value.Key, value.Value));
                }
                else
                {
                    ng.text = TagValues.Aggregate(form, (current, value) => current.Replace(value.Key, value.Value));
                }
            }
            else
            {
                if (Set.Shadow)
                {
                    g.Text = notplayingform;
                }
                else
                {
                    ng.text = notplayingform;
                }
            }
        }
        public void Term()
        {
            if (Destroyed) return;
            if (Set.Shadow)
            {
                UnityEngine.Object.Destroy(g);
            }
            else
            {
                UnityEngine.Object.Destroy(ng);
            }
            UpdateValue();
            Set.Save(mod);
        }
    }
    public interface IText
    {
        void Init();
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
    }
}
