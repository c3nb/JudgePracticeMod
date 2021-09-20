using System;
using System.Collections.Generic;
using System.Reflection;
using ExtLib;
using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;
using System.IO;
using System.Threading;
using UnityEngine.UI;
using SD;
using System.Xml.Serialization;

namespace JudgePracticeMod
{

    public struct TextSetting
    {
        public static implicit operator (float, float, float[], int, string, string)(TextSetting TS) => (TS.X, TS.Y, TS.Color, TS.Size, TS.Form, TS.NotForm);
        public static implicit operator TextSetting((float, float, float[], int, string, string) tuple) => new TextSetting(tuple);
        public TextSetting((float, float, float[], int, string, string) tuple)
        {
            X = tuple.Item1;
            Y = tuple.Item2;
            Color = tuple.Item3;
            Size = tuple.Item4;
            Form = tuple.Item5;
            NotForm = tuple.Item6;
        }
        public float X;
        public float Y;
        public float[] Color;
        public int Size;
        public string Form;
        public string NotForm;
    }
    public static class Main
    {
        public static readonly Dictionary<string, string> TagExplanation = new Dictionary<string, string>()
        {
            {"{LHit}", "HitMargin in Lenient Difficulty"},
            {"{NHit}", "HitMargin in Normal Difficulty"},
            {"{SHit}", "HitMargin in Strict Difficulty"},

            {"{LTE}", "TooEarly in Lenient Difficulty"},
            {"{LVE}", "VeryEarly in Lenient Difficulty"},
            {"{LEP}", "EarlyPerfect in Lenient Difficulty"},
            {"{LP}", "Perfect in Lenient Difficulty"},
            {"{LLP}", "LatePerfect in Lenient Difficulty"},
            {"{LVL}", "VeryLate in Lenient Difficulty"},
            {"{LTL}", "TooLate in Lenient Difficulty"},

            {"{NTE}", "TooEarly in Normal Difficulty"},
            {"{NVE}", "VeryEarly in Normal Difficulty"},
            {"{NEP}", "EarlyPerfect in Normal Difficulty"},
            {"{NP}", "Perfect in Normal Difficulty"},
            {"{NLP}", "LatePerfect in Normal Difficulty"},
            {"{NVL}", "VeryLate in Normal Difficulty"},
            {"{NTL}", "TooLate in Normal Difficulty"},

            {"{STE}", "TooEarly in Strict Difficulty"},
            {"{SVE}", "VeryEarly in Strict Difficulty"},
            {"{SEP}", "EarlyPerfect in Strict Difficulty"},
            {"{SP}", "Perfect in Strict Difficulty"},
            {"{SLP}", "LatePerfect in Strict Difficulty"},
            {"{SVL}", "VeryLate in Strict Difficulty"},
            {"{STL}", "TooLate in Strict Difficulty"},

            {"{Score}", "Score in Current Difficulty"},
            {"{Combo}", "Combo"},
            {"{LScore}", "Score in Lenient Difficulty"},
            {"{NScore}", "Score in Normal Difficulty"},
            {"{SScore}", "Score in Strict Difficulty"},

            {"{CurTE}", "TooEarly in Current Difficulty"},
            {"{CurVE}", "VeryEarly in Current Difficulty"},
            {"{CurEP}", "EarlyPerfect in Current Difficulty"},
            {"{CurP}", "Perfect in Current Difficulty"},
            {"{CurLP}", "LatePerfect in Current Difficulty"},
            {"{CurVL}", "VeryLate in Current Difficulty"},
            {"{CurTL}", "TooLate in Current Difficulty"},
            {"{CurHit}", "HitMargin in Current Difficulty"},
            {"{CurDifficulty}", "Current Difficulty"},

            {"{Accuracy}", "Accuracy"},
            {"{Progress}", "Progress"},
            {"{CheckPointCount}", "Check Point Used Count"}
        };
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
            if (scrController.instance.gameworld)
            {
                TagValues["{Accuracy}"] = $"{Math.Round(scrController.instance.mistakesManager.percentAcc * 100, Set.adecimals)}";
                TagValues["{Progress}"] = $"{Math.Round(scrController.instance.percentComplete * 100f, Set.pdecimals)}";
                TagValues["{CurDifficulty}"] = RDString.Get("enum.Difficulty." + GCS.difficulty.ToString());
                TagValues["{CurHit}"] = RDString.Get("HitMargin." + CurHitMargin());
                TagValues["{CheckPointCount}"] = scrController.instance.customLevel.checkpointsUsed.ToString();
            }
        }
        public static Dictionary<string, string> TagValues;

        public static UnityModManager.ModEntry mod;
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
        public static UnityModManager.ModEntry.ModLogger Logger { get; private set; }
        public static bool IsEnabled { get; private set; }
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Set = UnityModManager.ModSettings.Load<Settings>(modEntry);
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnUpdate = OnUpdate;
            return true;
        }
        public static bool isFailActioned;
        public static void OnUpdate(UnityModManager.ModEntry modEntry, float deltaTime)
        {
            UpdateTV();
            foreach (Overload ol in Overload.overloads)
            {
                ol.Run();
            }
            for (int i = 0; i < texts.Count; i++)
            {
                texts[i].Update();
            }
            /*if (scrController.instance.gameworld)
            {
                if (Set.Shadow)
                {
                    foreach (KeyValuePair<string, string> kvp in uistrings)
                    {
                        g.Text = Set.form.Replace(kvp.Key, kvp.Value);
                    }
                }
                if (!Set.Shadow)
                {
                    foreach (KeyValuePair<string, string> kvp in uistrings)
                    {
                        nG.text = Set.form.Replace(kvp.Key, kvp.Value);
                    }
                }
            }
            else
            {
                if (Set.Shadow)
                {
                    g.Text = Set.notPlaying;
                }
                else
                {
                    nG.text = Set.notPlaying;
                }
            }*/
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
        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            mod = modEntry;
            IsEnabled = value;
            if (value)
            {
                if (File.Exists(Path.Combine("Mods", "JudgePracticeMod", "TextSettings.jpm")))
                {
                    TextSettings = Path.Combine("Mods", "JudgePracticeMod", "TextSettings.jpm").DeserializeJson<Dictionary<int, TextSetting>>();
                }
                if (Set.save)
                {
                    if (File.Exists(Path.Combine("Mods", "JudgePracticeMod", "Lenient.jpm")) && File.Exists(Path.Combine("Mods", "JudgePracticeMod", "Strict.jpm")) && File.Exists(Path.Combine("Mods", "JudgePracticeMod", "Normal.jpm")))
                    {
                        lcounts = Exts.BinDeserialize<Dictionary<HitMargin, int>>(Path.Combine("Mods", "JudgePracticeMod", "Lenient.jpm"));
                        scounts = Exts.BinDeserialize<Dictionary<HitMargin, int>>(Path.Combine("Mods", "JudgePracticeMod", "Strict.jpm"));
                        ncounts = Exts.BinDeserialize<Dictionary<HitMargin, int>>(Path.Combine("Mods", "JudgePracticeMod", "Normal.jpm"));
                    }
                }
                harmony = new Harmony(mod.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                TagValues = new Dictionary<string, string>();
                if (Set.Error) ErrorMeter.Do();
                Create(Set.CompCount);
            }
            else
            {
                TEXT.Serialize();
                harmony.UnpatchAll(mod.Info.Id);
                harmony = null;
                for(int i = 0; i < texts.Count; i++)
                {
                    texts[i].Term();
                }
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
        public static string num = "0";
        public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Set.Save(modEntry);
            TEXT.Serialize();
            if (Main.Set.save)
            {
                Main.lcounts.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Lenient.jpm"));
                Main.ncounts.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Normal.jpm"));
                Main.scounts.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Strict.jpm"));
            }
            if (Overload.overloads.Count > 0)
            Overload.overloads.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Overloads.jpm"));
        }
        public static bool IsExpandedO;
        public static void OnGUI(UnityModManager.ModEntry modEntry) => imsigui(modEntry);
        public static void GUICreate()
        {
            Create();
            Set.CompCount++;
            Set.Save(mod);
        }
        public static void imsigui(UnityModManager.ModEntry modEntry)
        {
            Set.Draw(modEntry);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("텍스트 추가 Add Text"))
            {
                GUICreate();
            }
            if (!Set.Error)
            {
                if (GUILayout.Button("판정선 텍스트 추가 ErrorMeter Add Text (Beta)"))
                {
                    ErrorMeter.Do();
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            for(int i = 0; i < texts.Count; i++)
            {
                texts[i].GUI();
            }
            if (IsExpandedO = GUILayout.Toggle(IsExpandedO, "태그 제한 설정 Tag Limit Setting"))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("태그 ");
                OTag = GUILayout.TextField(OTag);
                GUILayout.Label("(을)를");
                double.TryParse(GUILayout.TextField(OCount.ToString()), out OCount);
                GUILayout.Label(" (으)로 제한");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                Set.IHA = GUILayout.Toggle(Set.IHA, "이하");
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("추가"))
                {
                    Set.restartreset = true;
                    Set.DeadReset = false;
                    Set.MenuReset = true;
                    Overload.overloads.Add(new Overload(OTag, OCount, Set.IHA));
                    Overload.overloads.SerializeJson(Path.Combine("Mods", "JudgePracticeMod", "Overloads.jpm"));
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                Set.OVERLOADS = GUILayout.Toggle(Set.OVERLOADS, "제한된 목록 보기");
                if (Set.OVERLOADS)
                {
                    for (int i = 0; i < Overload.overloads.Count; i++)
                    {
                        GUILayout.Label($"{i}. 태그: {Overload.overloads[i].Tag}, 값: {Overload.overloads[i].Count}, 이하 여부: {Overload.overloads[i].IHA}");
                    }
                    /*foreach (Overload ol in Overload.overloads)
                    {
                        GUILayout.Label($"태그 {ol.Tag}, 횟수 {ol.Count}");
                    }*/
                    GUILayout.BeginHorizontal();
                    num = GUILayout.TextField(num);
                    if (GUILayout.Button("번 삭제"))
                    {
                        Overload.overloads.RemoveAt(num.ToInt());
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.BeginHorizontal();
            if(TAG = GUILayout.Toggle(TAG, "Tags"))
            {
                var tags = TagExplanation.Keys.ToList();
                var exs = TagExplanation.Values.ToList();
                GUILayout.BeginVertical();
                for(int i = 0; i < TagExplanation.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(1);
                    GUILayout.Label($"{tags[i]}: {exs[i]}");
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        public static HitMargin[] hitmargins = new HitMargin[7] { HitMargin.Perfect, HitMargin.LatePerfect, HitMargin.EarlyPerfect, HitMargin.VeryEarly, HitMargin.VeryLate, HitMargin.TooEarly, HitMargin.TooLate };
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
            return (HitMargin)Exts.GetField(Patches.instance, GCS.difficulty.ToString());
        }
        public const string Too = "FF0000";
        public const string Very = "FF6F4D";
        public const string EL = "A0FF4D";
        public const string Perfect = "00FF00";
        public const string MultiPress = "0000FF";
        public static double OCount;
        public static string OTag;
    }
    
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);
        public void OnChange()
        {
            Save(this, Main.mod);

        }
        [Draw("횟수 저장 (Save Counts)")]
        public bool save = false;
        [Draw("보일 소수점 (정확도) Showing Decimals (Accuracy)")]
        public int adecimals = 2;
        [Draw("보일 소수점 (진행도) Showing Decimals (Progress)")]
        public int pdecimals = 2;
        [Draw("죽으면 횟수 초기화")]
        public bool DeadReset = false;
        [Draw("메뉴로 나가면 초기화")]
        public bool MenuReset = false;
        [Draw("맵이 재시작하면 초기화")]
        public bool restartreset = true;
        /*[Draw("느슨 난이도의 점수와 정확도 계산 Calcultate Score and Accuracy from Lenient")]
        public bool lCalculateAnS = false;
        [Draw("보통 난이도의 점수와 정확도 계산 Calcultate Score and Accuracy from Normal")]
        public bool nCalculateAnS = false;
        [Draw("엄격 난이도의 점수와 정확도 계산 Calcultate Score and Accuracy from Strict")]
        public bool sCalculateAnS = false;*/
        [Draw("그림자 Shadow")]
        public bool Shadow = true;
        public bool OVERLOADS = true;
        public bool IHA = false;
        public int CompCount = 1;
        public bool Error = false;
        //public List<float> X = new List<float>();
        //public List<float> Y = new List<float>();
        //public List<Color> color = new List<Color>();
        //public List<int> size = new List<int>();
        //public List<string> form = new List<string>();
        //public List<string> notform = new List<string>();
    }
    public class Patches
    {
        public static Patches instance;
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
        [HarmonyPatch(typeof(scrUIController), "WipeFromBlack")]
        public class Patch9
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
        [HarmonyPatch(typeof(scrController), "FailAction")]
        public class Patch7
        {
            public static void Postfix()
            {
                if (Main.Set.DeadReset)
                {
                    Main.Reset();
                }
                Main.isFailActioned = false;
            }
        }
        [HarmonyPatch(typeof(scrController), "QuitToMainMenu")]
        public class Patch8
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
        public class Patch4
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
        public class Patch3
        {
            public static void Postfix()
            {
                if (Main.Set.save)
                {
                    Main.lcounts.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Lenient.jpm"));
                    Main.ncounts.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Normal.jpm"));
                    Main.scounts.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Strict.jpm"));
                }
                Main.isFailActioned = false;
            }
        }
        [HarmonyPatch(typeof(scrMisc), "GetHitMargin")]
        public class Patch2
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
        public static class Patch5
        {
            public static void Postfix()
            {
                if (Main.Set.save)
                {
                    Main.lcounts.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Lenient.jpm"));
                    Main.ncounts.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Normal.jpm"));
                    Main.scounts.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Strict.jpm"));
                }
                if (Overload.overloads.Count > 0)
                    Overload.overloads.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Overloads.jpm"));
            }
        }
        [HarmonyPatch(typeof(scnEditor), "ResetScene")]
        public static class Patch6
        {
            public static void Postfix()
            {
                if (Main.Set.save)
                {
                    Main.lcounts.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Lenient.jpm"));
                    Main.ncounts.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Normal.jpm"));
                    Main.scounts.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Strict.jpm"));
                }
                if (Overload.overloads.Count > 0)
                    Overload.overloads.BinSerialize(Path.Combine("Mods", "JudgePracticeMod", "Overloads.jpm"));
                Main.isFailActioned = false;
            }
        }
    }
    public static class Extsss
    {
        public static List<TKey> ToList<TKey, TValue>(this Dictionary<TKey, TValue>.KeyCollection Keys)
        {
            var list = new List<TKey>();
            foreach(TKey tk in Keys)
            {
                list.Add(tk);
            }
            return list;
        }
        public static List<TValue> ToList<TKey, TValue>(this Dictionary<TKey, TValue>.ValueCollection Values)
        {
            var list = new List<TValue>();
            foreach (TValue tk in Values)
            {
                list.Add(tk);
            }
            return list;
        }
        public static double ToDouble(this object obj)
        {
            return Convert.ToDouble(obj);
        }
        public static int ToInt(this object obj)
        {
            return Convert.ToInt32(obj);
        }
    }
    [Serializable]
    public class Overload
    {
        public static List<Overload> overloads = new List<Overload>();
        public double Count;
        public string Tag;
        public bool IHA;
        public Overload(string Tag, double Count, bool IHA)
        {
            this.Tag = Tag;
            this.Count = Count;
            this.IHA = IHA;
        }
        public void Run()
        {
            if (Main.TagValues[Tag].ToDouble() >= Count && !IHA)
            {
                if (!Main.isFailActioned) scrController.instance.FailAction(true, false);
                Main.isFailActioned = true;
            }
            if (Main.TagValues[Tag].ToDouble() <= Count && IHA)
            {
                if (!Main.isFailActioned) scrController.instance.FailAction(true, false);
                Main.isFailActioned = true;
            }
        }
    }
}
