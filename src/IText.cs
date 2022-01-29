namespace JudgePracticeMod
{
    public interface IText
    {
        string Text { get; set; }
        void Init(bool ErrorMeter);
        void GUI();
        void Update();
        void Term();
    }
}
