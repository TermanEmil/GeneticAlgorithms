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
        public float smallestDistanceToTarget = Mathf.Infinity;
        public LayerMask rayLayermask;
        public Transform[] rayPoints;

        [Header("Movement params")]
        public double movementVel = 1d;
        public double rotationSpeed = 0.1d;
        private Rigidbody2D rb;

        [Header("Fitness")]
        public bool inTargetArea = false;

        private PathFinderCtrl pathFinderCtrl;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            pathFinderCtrl = FindObjectOfType<PathFinderCtrl>();
        }

        private void Update()
        {
            if (genome == null || target == null)
                return;

            FeedMyNeuralNetwork(Time.fixedDeltaTime);

            var currentDist = Vector3.Distance(target.position,
                                               transform.position);

            //var addFitness = 1 / Mathf.Max(currentDist, 0.001f);
            //if (inTargetArea)
            //    addFitness *= pathFinderCtrl.fitnessMultInArea;
            //genome.Fitness += addFitness;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            gameObject.SetActive(false);
        }

        public void Init(INeuralGenome genome_, Transform target_)
        {
            genome = genome_;
            target = target_;
        }

        public void FeedMyNeuralNetwork(float deltaTime)
        {
            var inputs = new double[rayPoints.Length + 2];

            for (int i = 0; i < rayPoints.Length; i++)
                inputs[i] = DoRaycast(rayPoints[i].position);

            var heading = (target.position - transform.position);
            var direction = heading / heading.magnitude;

            inputs[rayPoints.Length] = direction.x;
            inputs[rayPoints.Length + 1] = direction.y;

            ApplyNetworkOutputs(genome.FeedNetwork(inputs), deltaTime);
        }

        private void ApplyNetworkOutputs(
            IList<double> networkOutputs,
            float deltaTime)
        {
            var rotVal = Mathf.Lerp(-1, 1, (float)networkOutputs[0]);
            rotVal = (float)(rotVal * rotationSpeed * deltaTime);
            transform.Rotate(Vector3.forward * rotVal);

            var movementVal = Mathf.Lerp(-1, 1, (float)networkOutputs[1]);
            movementVal *= (float)movementVel;
            rb.velocity = transform.up * (float)(movementVal * movementVel);
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