using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NN
{
    public class EvolutionaryNeuralNetwork : DeepNeuralNetwork
    {
        public static System.Random random = new System.Random();

        public EvolutionaryNeuralNetwork(int numInput, int[] numHidden, int numOutput) : base(numInput, numHidden, numOutput)
        {

        }

        /// <summary>
        /// Recommended mutationChance: 0.05 mutationStrength: 0.20
        /// </summary>
        public void Mutate(float mutationChance, float mutationStrength)
        {
            // Gets weights and biases
            double[] weights = this.GetWeights();
            double[] biases = this.GetBiases();

            // Debug section start
            // Debug.Log($"mutationChance = {mutationChance}, mutationStrength = {mutationStrength}"); 
            // Debug section end

            // Mutates weights
            for (int i = 0; i < weights.Length; i++)
            {
                if (random.NextDouble() <= mutationChance)
                {
                    // Random.Range(-mutationStrength, mutationStrength);
                    weights[i] += random.NextDouble() * (mutationStrength + mutationStrength) - mutationStrength;
                }
            }

            // Mutates biases
            for (int i = 0; i < biases.Length; i++)
            {
                if (random.NextDouble() <= mutationChance)
                {
                    // Random.Range(-mutationStrength, mutationStrength);
                    biases[i] += random.NextDouble() * (mutationStrength + mutationStrength) - mutationStrength;
                }
            }

            // Updates mutated weights and biases
            this.SetWeights(weights);
            this.SetBiases(biases);
        }

        /// <summary>
        /// Returns a deep copy of the EvolutionaryNeuralNetwork
        /// </summary>
        public static EvolutionaryNeuralNetwork Coppy(EvolutionaryNeuralNetwork evolutionaryNeuralNetwork)
        {
            // Copies weights
            double[] wheightsToCoppy = evolutionaryNeuralNetwork.GetWeights();
            double[] wheights = new double[wheightsToCoppy.Length];
            for (int i = 0; i < wheights.Length; i++)
            {
                wheights[i] = wheightsToCoppy[i];
            }

            // Copies biases
            double[] biasesToCoppy = evolutionaryNeuralNetwork.GetBiases();
            double[] biases = new double[biasesToCoppy.Length];
            for (int i = 0; i < biases.Length; i++)
            {
                biases[i] = biasesToCoppy[i];
            }

            // Create a new copy of the network
            EvolutionaryNeuralNetwork newEvolutionaryNeuralNetwork = new EvolutionaryNeuralNetwork(evolutionaryNeuralNetwork.nInput, evolutionaryNeuralNetwork.nHidden, evolutionaryNeuralNetwork.nOutput);
            newEvolutionaryNeuralNetwork.SetWeights(wheights);
            newEvolutionaryNeuralNetwork.SetBiases(biases);

            // Return the new network
            return newEvolutionaryNeuralNetwork;
        }
    }
}