using GA.Gene;

namespace GA.Gene.GeneChoice
{
    /// <summary>
    /// Choose between 2 Genes.
    /// </summary>
    public interface IGeneChoice<T>
    {
        Gene<T> Choice(Gene<T> gene1, Gene<T> gene2);
    }
}