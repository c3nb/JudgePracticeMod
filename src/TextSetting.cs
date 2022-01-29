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
}
