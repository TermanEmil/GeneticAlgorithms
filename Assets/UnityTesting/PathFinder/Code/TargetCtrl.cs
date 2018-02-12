using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA_Tests.PathFinder
{
    public class TargetCtrl : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var agent = other.GetComponent<AgentCtrl>();
            if (agent == null)
                return;
            agent.inTargetArea = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var agent = other.GetComponent<AgentCtrl>();
            if (agent == null)
                return;
            agent.inTargetArea = false;
        }
    }
}