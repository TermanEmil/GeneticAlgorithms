using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GA_Tests.Shakespeare;
using System.Linq;

public class ShakespearMono : MonoBehaviour
{
    public ShakespeareSolver shakespear;

    public string targetStr = "To be or not to be";
    public int populationLen = 50;
    public double partToReinsert = 0.1d;
    public double geneMutChance = 0.1f;
    public int geneMutationRange = 50;

    public int maxNbOfGenerations = 100;
    public bool autostart = true;
    public bool seedRandom = false;

    public double maxFitness = 0;
    public string bestStr = "";

    private void Start()
    {
        InitGA();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GAIter();
            Debug.Log(shakespear.Population.Generation);
        }
        if (Input.GetKeyDown(KeyCode.R))
            InitGA();
    }

    private void InitGA()
    {
        System.Random rand;

        rand = (seedRandom) ? new System.Random(1) : new System.Random();
        shakespear = new ShakespeareSolver(
            targetStr,
            rand,
            partToReinsert,
            populationLen,
            geneMutChance,
            geneMutationRange
        );

        if (autostart)
        {
            for (int i = 0; i < maxNbOfGenerations && !shakespear.MaxReached; i++)
                GAIter();
            Debug.Log(shakespear.Population.Generation);
        }
    }

    private void GAIter()
    {
        shakespear.EvaluateGenomes();
        maxFitness = shakespear.BestFitness;
        bestStr = shakespear.BestStr;

        shakespear.PassGeneration();

        // Once again, because I'm too lazy...
        shakespear.EvaluateGenomes();
    }
}
