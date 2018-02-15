using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA_Tests.PathFinder
{
    public class Checkpoint : MonoBehaviour
    {
        public double fitness = 100;
        public List<AgentCtrl> alreadyPassedHere = new List<AgentCtrl>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var agent = collision.GetComponent<AgentCtrl>();
            if (agent == null || alreadyPassedHere.Contains(agent))
                return;

            alreadyPassedHere.Add(agent);
            agent.genome.Fitness += fitness;
        }
    }
}