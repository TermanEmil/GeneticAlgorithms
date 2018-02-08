using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GA_Tests.EvolveSum;
using System.Linq;

public class EvolveSumMonobehav : MonoBehaviour
{
    public GAEvolveSum evolveSum;
    public int targetSum = 256;
    public int populationLen = 50;
    public int genomeGenesLen = 5;
    public int geneValRange = 50;
    public double partToKeepFromLastGeneration = 0.1d;
    public double geneMutChance = 0.1f;
    public int geneMutationRange = 50;

    public int maxNbOfGenerations = 100;
    public bool autostart = true;
    public bool seedRandom = false;

    public double maxFitness = 0;
    public int bestSum = 0;

    private void Start()
    {
        InitGA();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GAIter();
            Debug.Log(evolveSum.Population.Generation);
        }
        if (Input.GetKeyDown(KeyCode.R))
            InitGA();
    }

    private void InitGA()
    {
        System.Random rand;

        rand = (seedRandom) ? new System.Random(1) : new System.Random();
        evolveSum = new GAEvolveSum(
            rand, targetSum, populationLen, genomeGenesLen, geneValRange);

        ((EvolveSumGenerationGenerator)evolveSum.Population.GenerationGenerator)
            .GeneMutationChance = geneMutChance;

        ((EvolveSumGenerationGenerator)evolveSum.Population.GenerationGenerator)
            .GeneMutationRange = geneMutationRange;

        ((EvolveSumGenerationGenerator)evolveSum.Population.GenerationGenerator)
            .GenomesToKeep = partToKeepFromLastGeneration;

        if (autostart)
        {
            for (int i = 0; i < maxNbOfGenerations && !evolveSum.MaxReached; i++)
                GAIter();
            Debug.Log(evolveSum.Population.Generation);
        }
    }

    private void GAIter()
    {
        maxFitness = evolveSum.BestFitness;
        bestSum = evolveSum.BestSum;

        //Debug.Log(evolveSum.Population.Generation);
        //Debug.Log(evolveSum.Population);

        evolveSum.PassGeneration();

        maxFitness = evolveSum.BestFitness;
        bestSum = evolveSum.BestSum;
    }
}
