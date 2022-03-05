using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NN.Training
{
    public struct Candidate
    {
        public EvolutionaryNeuralNetwork brain;
        public Score score;

        public Candidate(EvolutionaryNeuralNetwork brain)
        {
            this.brain = brain;
            score = new Score();
        }
    }

    public struct Score
    {
        public int wins;
        public int loses;
        public int draws;
    }
}
