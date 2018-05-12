using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public bool isDead = false;

        private float lastActivation = 0;
        private Transform groundPoint;

        private Animator myAnimator;
        private Rigidbody2D rb;

        [SerializeField] private Text scoreText = null;

        private void Start()
        {
            myAnimator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            groundPoint = GameObject.FindWithTag("GroundPoint").transform;
        }

        private void Update()
        {
            if (genome == null)
                return;
            
            BirdPopulation.instance.ComputeFitness(this);
            scoreText.text = string.Format("{0:0.0}",
                                           genome.Fitness.ToString());
        }

        public override void Init(INeuralGenome targetGenome)
        {
            base.Init(targetGenome);
            lastActivation = 0;
            columns = 0;
            distToNextColOnDeath = Vector2.zero;
            isDead = false;
            foreach (var neuron in genome.Neurons)
                neuron.Value.Val = 0;
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
            if (isDead)
                return;
            
            var nxtCol = ColumnPool.Instance.ClossestColumn(transform.position);
            if (nxtCol != null)
            {
                distToNextColOnDeath.x = Mathf.Abs(nxtCol.position.x -
                                                   transform.position.x);
                distToNextColOnDeath.y = Mathf.Abs(nxtCol.position.y -
                                                   transform.position.y);
            }
            gameObject.SetActive(false);
            isDead = true;
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
            //rb.AddForce(Vector2.up * flapVel * Time.fixedDeltaTime);
            rb.velocity = Vector2.up * flapVel;
            myAnimator.SetTrigger("flap");
        }

        private void ActivateNeuralNetwork()
        {
            var inputs = GetNeuralNetInputs();
            var output = genome.FeedNetwork(inputs)[0];

            if (output > 0.5)
                Flap();
        }

        private double[] GetNeuralNetInputs()
        {
            float deltaX, columnY;

            var clossestColumn = ColumnPool.Instance
                                           .ClossestColumn(transform.position);

            if (clossestColumn == null)
            {
                ColumnPool.Instance.UpdateColumns();
                return GetNeuralNetInputs();
            }

            var maxDeltaX = (ColumnPool.Instance.visibilityPoint.position.x) +
                (ColumnPool.Instance.distBetweenColumns);
            var maxDeltaY = 2 * Mathf.Abs(groundPoint.position.y);

            deltaX = clossestColumn.position.x - transform.position.x;
            columnY = clossestColumn.position.y - groundPoint.position.y;

            var distToGround = transform.position.y - groundPoint.position.y;

            var results = new double[]
            {
                deltaX / maxDeltaX,
                columnY / maxDeltaY,
                distToGround / maxDeltaY
            };

            return results;
        }
    }
}