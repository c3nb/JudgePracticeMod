//#define Debug
using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;
using System.IO;
using System.Threading;
using UnityEngine.UI;
using SD;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;
using System.Reflection.Extensions;
using static UnityModManagerNet.UnityModManager;

namespace JudgePracticeMod
{
#if Debug
    [EnableReloading]
#endif
    public static partial class Main
    {
        public static bool NullableToBool(this bool? nullableBool, bool ifnull)
        {
            if (nullableBool == null)
                return ifnull;
            return (bool)nullableBool;
        }
        public static bool IsPlaying => scrController.instance.gameworld || ADOBase.sceneName.Contains("-X");
        public static void InitializeTagValues()
        {
            TagValues["{LHit}"] = "";
            TagValues["{NHit}"] = "";
            TagValues["{SHit}"] = "";

            TagValues["{LTE}"] = "0";
            TagValues["{LVE}"] = "0";
            TagValues["{LEP}"] = "0";
            TagValues["{LP}"] = "0";
            TagValues["{LLP}"] = "0";
            TagValues["{LVL}"] = "0";
            TagValues["{LTL}"] = "0";

            TagValues["{STE}"] = "0";
            TagValues["{SVE}"] = "0";
            TagValues["{SEP}"] = "0";
            TagValues["{SP}"] = "0";
            TagValues["{SLP}"] = "0";
            TagValues["{SVL}"] = "0";
            TagValues["{STL}"] = "0";

            TagValues["{NTE}"] = "0";
            TagValues["{NVE}"] = "0";
            TagValues["{NEP}"] = "0";
            TagValues["{NP}"] = "0";
            TagValues["{NLP}"] = "0";
            TagValues["{NVL}"] = "0";
            TagValues["{NTL}"] = "";

            TagValues["{CurTE}"] = "0";
            TagValues["{CurVE}"] = "0";
            TagValues["{CurEP}"] = "0";
            TagValues["{CurP}"] = "0";
            TagValues["{CurLP}"] = "0";
            TagValues["{CurVL}"] = "0";
            TagValues["{CurTL}"] = "0";

            TagValues["{Score}"] = "0";
            TagValues["{Combo}"] = "0";
            TagValues["{NScore}"] = "0";
            TagValues["{SScore}"] = "0";
            TagValues["{LScore}"] = "0";
            TagValues["{Accuracy}"] = "0";
            TagValues["{XAccuracy}"] = "0";
            TagValues["{Progress}"] = "0";
            TagValues["{CurDifficulty}"] = "";
            TagValues["{CurHit}"] = "";
            TagValues["{CheckPointCount}"] = "0";
            TagValues["{FailCount}"] = "0";
        }
        public static void UpdateTV()
        {
            TagValues["{LHit}"] = Patches.lmargin;
            TagValues["{NHit}"] = Patches.nmargin;
            TagValues["{SHit}"] = Patches.smargin;

            TagValues["{LTE}"] = lcounts[HitMargin.TooEarly].ToString();
            TagValues["{LVE}"] = lcounts[HitMargin.VeryEarly].ToString();
            TagValues["{LEP}"] = lcounts[HitMargin.EarlyPerfect].ToString();
            TagValues["{LP}"] = lcounts[HitMargin.Perfect].ToString();
            TagValues["{LLP}"] = lcounts[HitMargin.LatePerfect].ToString();
            TagValues["{LVL}"] = lcounts[HitMargin.VeryLate].ToString();
            TagValues["{LTL}"] = lcounts[HitMargin.TooLate].ToString();

            TagValues["{STE}"] = scounts[HitMargin.TooEarly].ToString();
            TagValues["{SVE}"] = scounts[HitMargin.VeryEarly].ToString();
            TagValues["{SEP}"] = scounts[HitMargin.EarlyPerfect].ToString();
            TagValues["{SP}"] = scounts[HitMargin.Perfect].ToString();
            TagValues["{SLP}"] = scounts[HitMargin.LatePerfect].ToString();
            TagValues["{SVL}"] = scounts[HitMargin.VeryLate].ToString();
            TagValues["{STL}"] = scounts[HitMargin.TooLate].ToString();

            TagValues["{NTE}"] = ncounts[HitMargin.TooEarly].ToString();
            TagValues["{NVE}"] = ncounts[HitMargin.VeryEarly].ToString();
            TagValues["{NEP}"] = ncounts[HitMargin.EarlyPerfect].ToString();
            TagValues["{NP}"] = ncounts[HitMargin.Perfect].ToString();
            TagValues["{NLP}"] = ncounts[HitMargin.LatePerfect].ToString();
            TagValues["{NVL}"] = ncounts[HitMargin.VeryLate].ToString();
            TagValues["{NTL}"] = ncounts[HitMargin.TooLate].ToString();

            TagValues["{CurTE}"] = CurMargin(HitMargin.TooEarly);
            TagValues["{CurVE}"] = CurMargin(HitMargin.VeryEarly);
            TagValues["{CurEP}"] = CurMargin(HitMargin.EarlyPerfect);
            TagValues["{CurP}"] = CurMargin(HitMargin.Perfect);
            TagValues["{CurLP}"] = CurMargin(HitMargin.LatePerfect);
            TagValues["{CurVL}"] = CurMargin(HitMargin.VeryLate);
            TagValues["{CurTL}"] = CurMargin(HitMargin.TooLate);

            TagValues["{Score}"] = $"{score}";
            TagValues["{Combo}"] = $"{combo}";
            TagValues["{NScore}"] = $"{nsco}";
            TagValues["{SScore}"] = $"{ssco}";
            TagValues["{LScore}"] = $"{lsco}";
            if (IsPlaying)
            {
                TagValues["{Accuracy}"] = $"{Math.Round(scrController.instance.mistakesManager.percentAcc * 100, Set.adecimals)}";
                TagValues["{XAccuracy}"] = $"{Math.Round(scrController.instance.mistakesManager.percentXAcc * 100, Set.adecimals)}".Replace("NaN", "100");
                TagValues["{Progress}"] = $"{Math.Round((!scrController.instance.lm ? 0 : scrController.instance.percentComplete) * 100f, Set.pdecimals)}";
                TagValues["{CurDifficulty}"] = RDString.Get("enum.Difficulty." + GCS.difficulty.ToString());
                TagValues["{CurHit}"] = RDString.Get("HitMargin." + CurHitMargin());
                TagValues["{CheckPointCount}"] = scrController.instance.customLevel?.checkpointsUsed.ToString();
                TagValues["{FailCount}"] = Patches.failCount.ToString();
            }
        }
        public static Dictionary<string, string> TagValues;

        public static ModEntry mod;
        public static Harmony harmony;
        public static Settings Set { get; set; } = new Settings();
        public static Dictionary<HitMargin, int> ncounts = new Dictionary<HitMargin, int>() {
            { HitMargin.Perfect, 0 },
            { HitMargin.EarlyPerfect, 0 },
            { HitMargin.LatePerfect, 0 },
            { HitMargin.VeryEarly, 0 },
            { HitMargin.VeryLate, 0 },
            { HitMargin.TooEarly, 0 },
            { HitMargin.TooLate, 0 }
        };
        public static Dictionary<HitMargin, int> scounts = new Dictionary<HitMargin, int>() {
            { HitMargin.Perfect, 0 },
            { HitMargin.EarlyPerfect, 0 },
            { HitMargin.LatePerfect, 0 },
            { HitMargin.VeryEarly, 0 },
            { HitMargin.VeryLate, 0 },
            { HitMargin.TooEarly, 0 },
            { HitMargin.TooLate, 0 }
        };
        public static Dictionary<HitMargin, int> lcounts = new Dictionary<HitMargin, int>() {
            { HitMargin.Perfect, 0 },
            { HitMargin.EarlyPerfect, 0 },
            { HitMargin.LatePerfect, 0 },
            { HitMargin.VeryEarly, 0 },
            { HitMargin.VeryLate, 0 },
            { HitMargin.TooEarly, 0 },
            { HitMargin.TooLate, 0 }
        };
        public static Dictionary<int, TextSetting> TextSettings = new Dictionary<int, TextSetting>();
        public static ModEntry.ModLogger Logger { get; private set; }
        public static bool IsEnabled { get; private set; }
        public static bool Load(ModEntry modEntry)
        {
            Set = ModSettings.Load<Settings>(modEntry);
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnUpdate = OnUpdate;
            return true;
        }
        public static bool isFailActioned;
        public static void OnUpdate(ModEntry modEntry, float deltaTime)
        {
            UpdateTV();
            if (IsPlaying)
                foreach (Overload ol in Overload.overloads)
                    ol.Run();
            for (int i = 0; i < texts.Count; i++)
                texts[i].Update();
        }
        public static int combo;
        public static int score;
        public static void CalculateScore()
        {
            switch (Patches.GetHitMarginForDifficulty(Patches.angle, Difficulty.Normal))
            {
                case HitMargin.VeryEarly:
                case HitMargin.VeryLate:
                    Main.nsco += 91;
                    break;
                case HitMargin.EarlyPerfect:
                case HitMargin.LatePerfect:
                    Main.nsco += 150;
                    break;
                case HitMargin.Perfect:
                    Main.nsco += 300;
                    break;
            }

            switch (Patches.GetHitMarginForDifficulty(Patches.angle, Difficulty.Strict))
            {
                case HitMargin.VeryEarly:
                case HitMargin.VeryLate:
                    Main.ssco += 91;
                    break;
                case HitMargin.EarlyPerfect:
                case HitMargin.LatePerfect:
                    Main.ssco += 150;
                    break;
                case HitMargin.Perfect:
                    Main.ssco += 300;
                    break;
            }

            switch (Patches.GetHitMarginForDifficulty(Patches.angle, Difficulty.Lenient))
            {
                case HitMargin.VeryEarly:
                case HitMargin.VeryLate:
                    Main.lsco += 91;
                    break;
                case HitMargin.EarlyPerfect:
                case HitMargin.LatePerfect:
                    Main.lsco += 150;
                    break;
                case HitMargin.Perfect:
                    Main.lsco += 300;
                    break;
            }

        }
        public static int nsco;
        public static int lsco;
        public static int ssco;
        public static List<IText> texts = new List<IText>();
        public static bool OnToggle(ModEntry modEntry, bool value)
        {
            mod = modEntry;
            IsEnabled = value;
            if (value)
            {
                TagValues = new Dictionary<string, string>();
                InitializeTagValues();
                if (File.Exists(Path.Combine("Mods", "JudgePracticeMod", "TextSettings.jpm")))
                    TextSettings = Path.Combine("Mods", "JudgePracticeMod", "TextSettings.jpm").DeserializeJson<Dictionary<int, TextSetting>>();
                if (File.Exists(Path.Combine("Mods", "JudgePracticeMod", "Overloads.jpm")))
                    Overload.overloads = Path.Combine("Mods", "JudgePracticeMod", "Overloads.jpm").DeserializeJson<List<Overload>>();
                if (File.Exists(Path.Combine("Mods", "JudgePracticeMod", "OverloadsTagChanged.jpm")))
                    Overload.TagChanged = Path.Combine("Mods", "JudgePracticeMod", "OverloadsTagChanged.jpm").DeserializeJson<Dictionary<string, bool>>();
                if (File.Exists(Path.Combine("Mods", "JudgePracticeMod", "OverloadsRichTxt.jpm")))
                    Overload.RichTxt = Path.Combine("Mods", "JudgePracticeMod", "OverloadsRichTxt.jpm").DeserializeJson<Dictionary<string, string>>();
                harmony = new Harmony(mod.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                harmony.Patch(typeof(scrController).GetMethod("FailAction"), postfix:new HarmonyMethod(((Action<scrController>)((__instance) =>
                {
                    if (__instance.noFail)
                        Patches.failCount++;
                    if (Set.DeadReset && !__instance.noFail)
                        Reset();
                })).ToDynamicMethod()));

                if (Set.Error) ErrorMeter.Do();
                Create(Set.CompCount);
            }
            else
            {
                TEXT.Serialize();
                harmony.UnpatchAll(mod.Info.Id);
                harmony = null;
                for(int i = 0; i < texts.Count; i++)
                    texts[i].Term();
                OnSaveGUI(modEntry);
            }
            return true;
        }
        public static int count = 1;
        public static void Create(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                TEXT t = new TEXT();
                texts.Add(t);
                t.Init();
            }
        }
        public static bool TAG = false;
        public static string Operator = "<=";
        public static string num = "0";
        public static void OnSaveGUI(ModEntry modEntry)
        {
            Set.Save(modEntry);
            TEXT.Serialize();
            if (Overload.overloads.Count > 0)
            {
                Overload.overloads.SerializeJson(Path.Combine("Mods", "JudgePracticeMod", "Overloads.jpm"));
                Overload.TagChanged.SerializeJson(Path.Combine("Mods", "JudgePracticeMod", "OverloadsTagChanged.jpm"));
                Overload.RichTxt.SerializeJson(Path.Combine("Mods", "JudgePracticeMod", "OverloadsRichTxt.jpm"));
            }
        }
        public static bool IsExpandedO;

        public static Actions Action = Actions.Kill;
        public static float[] Color = new float[4];
        public static Color Prev => new Color(Color[0], Color[1], Color[2], Color[3]);
        public static void GUICreate()
        {
            Create();
            Set.CompCount++;
            Set.Save(mod);
        }
        public static void OnGUI(ModEntry modEntry)
        {
            Set.Draw(modEntry);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("텍스트 추가 Add Text"))
                GUICreate();
            if (!Set.Error)
                if (GUILayout.Button("판정선 텍스트 추가 ErrorMeter Add Text (Beta)"))
                    ErrorMeter.Do();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            for (int i = 0; i < texts.Count; i++)
                texts[i].GUI();
            if (IsExpandedO = GUILayout.Toggle(IsExpandedO, "태그 제한 설정 Tag Limit Setting"))
            {
                Drawer.IndentGUI(() =>
                {
                    GUILayout.BeginHorizontal();
                    OTag = GUILayout.TextField(OTag);
                    Operator = GUILayout.TextField(Operator);
                    double.TryParse(GUILayout.TextField(OCount.ToString()), out OCount);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"Current: {(Action == Actions.Kill ? "죽이기 (Kill)" : "텍스트 색 변경 (Change Text Color)")}");
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("죽이기 (Kill)")) Action = Actions.Kill;
                    if (GUILayout.Button("색 변경 (Change Text Color)")) Action = Actions.ChangeColor;
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    if (Action == Actions.ChangeColor)
                    {
                        GUILayout.BeginHorizontal();
                        Drawer.DrawFloatMultiField(ref Color, new[] { "<color=#FF0000>R</color>", "<color=#00FF00>G</color>", "<color=#0000FF>B</color>", "A" });
                        GUILayout.Label($"<color={Prev.ToHex()}>Preview</color>");
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("추가"))
                    {
                        Set.restartreset = true;
                        Set.DeadReset = false;
                        Set.MenuReset = true;
                        Overload.overloads.Add(new Overload(OTag, OCount, OpToOp.Operator(Operator), Action, Color));
                        Overload.overloads.SerializeJson(Path.Combine("Mods", "JudgePracticeMod", "Overloads.jpm"));
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    if (Set.OVERLOADS = GUILayout.Toggle(Set.OVERLOADS, "제한된 목록 보기 (Limited Tag List)"))
                    {
                        Drawer.IndentGUI(() =>
                        {
                            for (int i = 0; i < Overload.overloads.Count; i++)
                            {
                                Overload ov = Overload.overloads[i];
                                string preview = "";
                                if (ov.Action == Actions.ChangeColor)
                                {
                                    preview += " | To Change: ";
                                    preview += $"<color={new Color(ov.TextColor[0], ov.TextColor[1], ov.TextColor[2], ov.TextColor[3]).ToHex()}>{{{ov.Tag}}}</color>";
                                }
                                GUILayout.Label($"{i}. {ov.Tag} {OpToOp.Operator(ov.Op)} {ov.Count} | Mode: {ov.Action.ToString().Replace("ChangeColor", "Change Text Color")}{preview}");
                            }
                            GUILayout.BeginHorizontal();
                            num = GUILayout.TextField(num);
                            if (GUILayout.Button("번 삭제 (Index Remove)"))
                            {
                                Overload.overloads.RemoveAt(num.ToInt());
                            }
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                        });
                    }
                });
            }
            GUILayout.BeginHorizontal();
            if(TAG = GUILayout.Toggle(TAG, "Tags"))
            {
                Drawer.IndentGUI(() =>
                {
                    var tags = TagExplanation.Keys.ToList();
                    var exs = TagExplanation.Values.ToList();
                    GUILayout.BeginVertical();
                    for (int i = 0; i < TagExplanation.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(1);
                        GUILayout.Label($"{tags[i]}: {exs[i]}");
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                });
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        public static readonly HitMargin[] hitmargins = new HitMargin[7] { HitMargin.Perfect, HitMargin.LatePerfect, HitMargin.EarlyPerfect, HitMargin.VeryEarly, HitMargin.VeryLate, HitMargin.TooEarly, HitMargin.TooLate };
        public static void Reset()
        {
            foreach (HitMargin h in hitmargins)
            {
                ncounts[h] = 0;
                lcounts[h] = 0;
                scounts[h] = 0;
            }
            nsco = 0;
            lsco = 0;
            ssco = 0;
            score = 0;
            combo = 0;
            Patches.failCount = 0;
        }
        public static string CurMargin(HitMargin hit)
        {
            string result = string.Empty;
            if (GCS.difficulty == Difficulty.Lenient)
            {
                result = lcounts[hit].ToString();
            }
            if (GCS.difficulty == Difficulty.Normal)
            {
                result = ncounts[hit].ToString();
            }
            if (GCS.difficulty == Difficulty.Strict)
            {
                result = scounts[hit].ToString();
            }
            return result;
        }
        public static HitMargin CurHitMargin()
        {
            return (HitMargin)typeof(Patches).GetField(GCS.difficulty.ToString()).GetValue(null);
        }
        public const string Too = "FF0000";
        public const string Very = "FF6F4D";
        public const string EL = "A0FF4D";
        public const string Perfect = "00FF00";
        public const string MultiPress = "0000FF";
        public static double OCount;
        public static string OTag;
    }
}
