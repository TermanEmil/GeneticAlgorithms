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
    }
}