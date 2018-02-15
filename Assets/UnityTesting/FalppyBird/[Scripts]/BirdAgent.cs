using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GA.UnityProxy.Genome;
using GA.Population;
using GA.NeuralNet.SynapseStruct;
using GA.NeuralNet.NeuronClass;
using GA.NeuralNet.NeuralGenome;
using GA.NeuralNet.RandomGenomeGenerator;
using GA.NeuralNet.Activation;
using GA.NeuralNet.Mutation;
using GA.GenerationGenerator;
using GA.GenerationGenerator.GenomeProducer.Reinsertion;
using GA.GenerationGenerator.GenomeProducer.Breeding;
using GA.GenerationGenerator.GenomeProducer.Breeding.Selection;
using GA.GenerationGenerator.GenomeProducer.Breeding.Crossover;
using GA.GenerationGenerator.GenomeProducer.Breeding.Mutation;

namespace GA_Tests.FlappyBird
{
    public class BirdAgent : GenomeProxy
    {
        [SerializeField] private bool godMode = false;
        [SerializeField] private float flapVel = 10;
        [SerializeField] private float activationDelaTime = 0.1f;

        public int columns = 0;
        [HideInInspector] public Vector2 distToNextColOnDeath;

        private float lastActivation = 0;
        private Transform groundPoint;

        private Animator myAnimator;
        private Rigidbody2D rb;

        private Dictionary<double, double[]> inputsOutputs = new Dictionary<double, double[]>();

        private void Start()
        {
            myAnimator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            groundPoint = GameObject.FindWithTag("GroundPoint").transform;
        }

        public override void Init(INeuralGenome targetGenome)
        {
            base.Init(targetGenome);
            lastActivation = 0;
            columns = 0;
            distToNextColOnDeath = Vector2.zero;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (godMode || genome == null)
                return;
            
            Die();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            columns++;
        }

        public void Die()
        {
            var nxtCol = ColumnPool.Instance.ClossestColumn(transform.position);
            if (nxtCol != null)
            {
                distToNextColOnDeath.x = Mathf.Abs(nxtCol.position.x -
                                                   transform.position.x);
                distToNextColOnDeath.y = Mathf.Abs(nxtCol.position.y -
                                                   transform.position.y);
            }
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (genome == null)
                return;
            
            if (lastActivation + activationDelaTime < Time.time)
            {
                ActivateNeuralNetwork();
                lastActivation = Time.time;
            }
        }

        public void Flap()
        {
            rb.AddForce(Vector2.up * flapVel * Time.fixedDeltaTime);
            myAnimator.SetTrigger("flap");
        }

        private void ActivateNeuralNetwork()
        {
            var inputs = GetNeuralNetInputs();
            var output = genome.FeedNetwork(inputs)[0];

            if (inputsOutputs.ContainsKey(output))
            {
                for (int i = 0; i < inputs.Length; i++)
                    if (inputs[i] != inputsOutputs[output][i])
                        throw new System.Exception("qweqweqweqwe");
                inputsOutputs[output] = inputs;
            }
            else
                inputsOutputs.Add(output, inputs);

            if (output > 0.5)
                Flap();
        }

        private double[] GetNeuralNetInputs()
        {
            float deltaX, columnY;

            var clossestColumn = ColumnPool.Instance
                                           .ClossestColumn(transform.position);

            var maxDeltaX = ColumnPool.Instance.distBetweenColumns;
            var maxDeltaY = Mathf.Abs(groundPoint.position.y);

            if (clossestColumn == null)
            {
                deltaX = maxDeltaX;
                columnY = maxDeltaY;
            }
            else
            {
                deltaX = clossestColumn.position.x - transform.position.x;
                columnY = clossestColumn.position.y - groundPoint.position.y;
            }

            var distToGround = transform.position.y - groundPoint.position.y;

            return new double[3]
            {
                deltaX / ColumnPool.Instance.distBetweenColumns,
                columnY / maxDeltaY,
                distToGround / maxDeltaY
            };
        }

    }
}