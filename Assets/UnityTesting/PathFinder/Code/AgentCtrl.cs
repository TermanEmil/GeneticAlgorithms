using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GA.NeuralNet.NeuralGenome;

namespace GA_Tests.PathFinder
{
    public class AgentCtrl : MonoBehaviour
    {
        public INeuralGenome genome;

        public Transform target;
        public LayerMask rayLayermask;
        public Transform[] rayPoints;

        [Header("Movement params")]
        public double movementVel = 1d;
        public double rotationSpeed = 0.1d;
        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (genome != null)
                FeedMyNeuralNetwork(Time.fixedDeltaTime);
        }

        public void Init(INeuralGenome genome_, Transform target_)
        {
            genome = genome_;
            target = target_;
        }

        public void FeedMyNeuralNetwork(float deltaTime)
        {
            var inputs = new double[rayPoints.Length + 1];

            for (int i = 0; i < rayPoints.Length; i++)
                inputs[i] = DoRaycast(rayPoints[i].position);

            var heading = (target.position - transform.position);
            var direction = heading / heading.magnitude;
            double zDirection = Mathf.InverseLerp(-1, 1, direction.z);
            inputs[rayPoints.Length] = zDirection;

            ApplyNetworkOutputs(genome.FeedNetwork(inputs), deltaTime);
        }

        private void ApplyNetworkOutputs(
            IList<double> networkOutputs,
            float deltaTime)
        {
            var rotValue = Mathf.Lerp(-180, 180, (float)networkOutputs[0]);
            transform.rotation = Quaternion.Euler(Vector3.forward * rotValue);

            rb.AddForce(transform.up *
                        (float)(networkOutputs[1] * movementVel) * deltaTime);
        }

        private double DoRaycast(Vector3 targetPos)
        {
            var hit = Physics2D.Linecast(
                transform.position,
                targetPos,
                rayLayermask);

            if (hit.collider)
            {
                double result = Vector3.Distance(transform.position, hit.point);
                result /= Vector3.Distance(transform.position, targetPos);
                return result;
            }
            else
                return 1d;
            
        }
    }
}