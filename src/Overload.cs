using System;
using UnityEngine;
using UnityModManagerNet;
using System.Linq;
using System.Collections.Generic;
using SD;

namespace JudgePracticeMod
{
    [Serializable]
    public class Overload
    {
        public static List<Overload> overloads = new List<Overload>();
        public static Dictionary<string, bool> TagChanged = new Dictionary<string, bool>();
        public static Dictionary<string, string> RichTxt = new Dictionary<string, string>();
        public static Dictionary<string, OList> CurState = new Dictionary<string, OList>();
        public static Dictionary<string, int> Counts = new Dictionary<string, int>();
        public static void ChangeColor(string Tag, string hex, bool recover = false)
        {
            if (recover)
            {
                if (!CurState[Tag].All(b => b == false))
                {
                    foreach (var txt in Main.texts)
                        txt.Text = txt.Text.Replace(RichTxt[Tag], $"{{{Tag}}}");
                    TagChanged[Tag] = false;
                }
                return;
            }
            var richTxt = $"<color={hex}>{{{Tag}}}</color>";
            if (TagChanged[Tag])
            {
                foreach (var txt in Main.texts)
                    txt.Text = txt.Text.Replace(RichTxt[Tag], richTxt);
                RichTxt[Tag] = richTxt;
                return;
            }
            foreach (var txt in Main.texts)
                txt.Text = txt.Text.Replace($"{{{Tag}}}", richTxt);
            TagChanged[Tag] = true;
            RichTxt[Tag] = richTxt;
        }
        public double Count;
        public string Tag;
        public Op Op;
        public Actions Action;
        public float[] TextColor;
        public int index;
        
        public Overload(string Tag, double Count, Op Op, Actions Action, float[] TextColor)
        {
            this.Tag = Tag;
            this.Count = Count;
            this.Op = Op;
            this.Action = Action;
            this.TextColor = TextColor;
            if (!Counts.ContainsKey(Tag))
                Counts.Add(Tag, 0);
            index = Counts[Tag]++;
            if (!TagChanged.ContainsKey(Tag))
                TagChanged.Add(Tag, false);
            if (!RichTxt.ContainsKey(Tag))
                RichTxt.Add(Tag, "");
            if (!CurState.ContainsKey(Tag))
                CurState.Add(Tag, new OList());
            CurState[Tag][index] = false;
        }
        void ChangeCol()
        {
            var color = new Color(TextColor[0], TextColor[1], TextColor[2], TextColor[3]);
            var hex = color.ToHex();
            ChangeColor(Tag, hex);
        }
        void RecoverCol()
        {
            CurState[Tag][index] = false;
            var color = new Color(TextColor[0], TextColor[1], TextColor[2], TextColor[3]);
            var hex = color.ToHex();
            ChangeColor(Tag, hex, true);
        }
        public void Run()
        {
            switch (Op)
            {
                case Op.Eq:
                    if (Main.TagValues[$"{{{Tag}}}"].ToDouble() == Count)
                    {
                        CurState[Tag][index] = true;
                        if (!Main.isFailActioned && Action == Actions.Kill)
                        {
                            scrController.instance.FailAction(true, false);
                            Main.isFailActioned = true;
                        }
                        else if (Action == Actions.ChangeColor) ChangeCol();
                    }
                    else if (Action == Actions.ChangeColor) RecoverCol();
                    break;
                case Op.Gt:
                    if (Main.TagValues[$"{{{Tag}}}"].ToDouble() > Count)
                    {
                        CurState[Tag][index] = true;
                        if (!Main.isFailActioned && Action == Actions.Kill)
                        {
                            scrController.instance.FailAction(true, false);
                            Main.isFailActioned = true;
                        }
                        else if (Action == Actions.ChangeColor) ChangeCol();
                    }
                    else if (Action == Actions.ChangeColor) RecoverCol();
                    break;
                case Op.Lt:
                    if (Main.TagValues[$"{{{Tag}}}"].ToDouble() < Count)
                    {
                        CurState[Tag][index] = true;
                        if (!Main.isFailActioned && Action == Actions.Kill)
                        {
                            scrController.instance.FailAction(true, false);
                            Main.isFailActioned = true;
                        }
                        else if (Action == Actions.ChangeColor) ChangeCol();
                    }
                    else if (Action == Actions.ChangeColor) RecoverCol();
                    break;
                case Op.Eq | Op.Gt:
                    if (Main.TagValues[$"{{{Tag}}}"].ToDouble() >= Count)
                    {
                        CurState[Tag][index] = true;
                        if (!Main.isFailActioned && Action == Actions.Kill)
                        {
                            scrController.instance.FailAction(true, false);
                            Main.isFailActioned = true;
                        }
                        else if (Action == Actions.ChangeColor) ChangeCol();
                    }
                    else if (Action == Actions.ChangeColor) RecoverCol();
                    break;
                case Op.Eq | Op.Lt:
                    if (Main.TagValues[$"{{{Tag}}}"].ToDouble() <= Count)
                    {
                        CurState[Tag][index] = true;
                        if (!Main.isFailActioned && Action == Actions.Kill)
                        {
                            scrController.instance.FailAction(true, false);
                            Main.isFailActioned = true;
                        }
                        else if (Action == Actions.ChangeColor) ChangeCol();
                    }
                    else if (Action == Actions.ChangeColor) RecoverCol();
                    break;
            }
        }
    }
}
