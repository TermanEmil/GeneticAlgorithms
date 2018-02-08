using System.Collections.Generic;

namespace GA.Gene
{
    public class Gene<T>
    {
        public int InnovNb { get; private set; }
        public T Val { get; set; }

        public Gene(int innovNb = 0)
        {
            InnovNb = innovNb;
        }

        public Gene(Gene<T> other)
        {
            InnovNb = other.InnovNb;
            Val = other.Val;
        }

        public override string ToString()
        {
            return string.Format("[{0}]", Val);
        }
    }

    public class GeneInovEq<T> : IEqualityComparer<Gene<T>>
    {
        public bool Equals(Gene<T> a, Gene<T> b)
        {
            return a.InnovNb == b.InnovNb;
        }

        public int GetHashCode(Gene<T> target)
        {
            return target.InnovNb.GetHashCode();
        }
    }
}