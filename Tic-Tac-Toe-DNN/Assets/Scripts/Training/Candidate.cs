using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NN.Training
{
    public struct Candidate
    {
        public AI AI;
        public Score score;

        public Candidate(EvolutionaryNeuralNetwork brain)
        {
            this.AI = new AI(brain);
            score = new Score();
        }
    }

    public struct Score
    {
        public int wins;
        public int loses;
        public int draws;

        public override string ToString()
        {
            return $"Wins: {wins}, Loses: {loses}, Draws: {draws}";
        }
    }
}
