using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudgePracticeMod
{
    public class OList : IEnumerable<bool>
    {
        public List<bool> List = new List<bool>();
        public bool this[int index]
        {
            get => List[index];
            set
            {
                if (index >= List.Count)
                {
                    List.Add(value);
                    return;
                }
                List[index] = value;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => List.GetEnumerator();
        public IEnumerator<bool> GetEnumerator() => List.GetEnumerator();
    }
}
