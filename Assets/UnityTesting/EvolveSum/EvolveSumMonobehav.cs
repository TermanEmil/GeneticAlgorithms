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
    public double partToReinsert = 0.1d;
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
            rand,
            partToReinsert,
            targetSum,
            populationLen,
            genomeGenesLen,
            geneValRange,
            geneMutChance,
            geneMutationRange);

        if (autostart)
        {
            for (int i = 0; i < maxNbOfGenerations && !evolveSum.MaxReached; i++)
                GAIter();
            Debug.Log(evolveSum.Population.Generation);
        }
    }

    private void GAIter()
    {
        evolveSum.EvaluateGenomes();
        maxFitness = evolveSum.BestFitness;
        bestSum = evolveSum.BestSum;

        evolveSum.PassGeneration();

        // Once again, because I'm too lazy...
        evolveSum.EvaluateGenomes();
    }
}
