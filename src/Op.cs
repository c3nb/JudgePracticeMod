using System;
using System.Runtime.CompilerServices;
namespace JudgePracticeMod
{
    [Flags]
    public enum Op
    {
        Eq = 1,
        Gt = 2,
        Lt = 4,
    }
    public static class OpToOp
    {
        public static string Operator(Op op)
        {
            switch (op)
            {
                case Op.Eq:
                    return "==";
                case Op.Gt:
                    return ">";
                case Op.Eq | Op.Gt:
                    return ">=";
                case Op.Lt:
                    return "<";
                case Op.Eq | Op.Lt:
                    return "<=";
                default:
                    throw new InvalidOperationException($"Invalid Operator {op.ToString().Replace(", ", " | ")}");
            }
        }
        public static Op Operator(string op)
        {
            switch (op)
            {
                case "==":
                    return Op.Eq;
                case ">":
                    return Op.Gt;
                case ">=":
                    return Op.Eq | Op.Gt;
                case "<":
                    return Op.Lt;
                case "<=":
                    return Op.Eq | Op.Lt;

                case "=>":
                    return Op.Eq | Op.Gt;
                case "=<":
                    return Op.Eq | Op.Lt;

                default:
                    throw new InvalidOperationException($"Invalid Operator {op}");
            }
        }
    }
}
