using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NN
{
    public class EvolutionaryNeuralNetwork : DeepNeuralNetwork
    {
        public EvolutionaryNeuralNetwork(int numInput, int[] numHidden, int numOutput) : base(numInput, numHidden, numOutput)
        {

        }

        public void Mutate(float mutationChance, float mutationStrength)
        {
            // Gets weights and biases
            double[] weights = this.getWeights();
            double[] biases = this.getBiases();

            // Debug section start
            //Debug.Log($"mutationChance = {mutationChance}, mutationStrength = {mutationStrength}");
            // Debug section end

            // Mutates weights
            for (int i = 0; i < weights.Length; i++)
            {
                if (Random.value <= mutationChance)
                {
                    weights[i] += Random.Range(-mutationStrength, mutationStrength);
                }
            }

            // Mutates biases
            for (int i = 0; i < biases.Length; i++)
            {
                if (Random.value <= mutationChance)
                {
                    biases[i] += Random.Range(-mutationStrength, mutationStrength);
                }
            }

            // Updates mutated weights and biases
            this.SetWeights(weights);
            this.SetBiases(biases);
        }
    }
}