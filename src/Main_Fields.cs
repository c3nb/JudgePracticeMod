using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudgePracticeMod
{
    public static partial class Main
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
            {"{CheckPointCount}", "Check Point Used Count"},
            {"{Timing}", "Hit Timing"},
            {"{XAccuracy}", "XAccuracy" },
            {"{FailCount}", "Fail Count" },
        };
    }
}
