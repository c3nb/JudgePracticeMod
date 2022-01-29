using UnityModManagerNet;

namespace JudgePracticeMod
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);
        public void OnChange()
        {
            Save(this, Main.mod);
        }
        [Draw("보일 소수점 (정확도) Showing Decimals (Accuracy)")]
        public int adecimals = 2;
        [Draw("보일 소수점 (진행도) Showing Decimals (Progress)")]
        public int pdecimals = 2;
        [Draw("보일 소수점 (판정 오차) Showing Decimals (Hit Timing)")]
        public int tdecimals = 2;
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
        public bool OVERLOADS = false;
        public int CompCount = 1;
        public bool Error = false;
        //public List<float> X = new List<float>();
        //public List<float> Y = new List<float>();
        //public List<Color> color = new List<Color>();
        //public List<int> size = new List<int>();
        //public List<string> form = new List<string>();
        //public List<string> notform = new List<string>();
    }
}
