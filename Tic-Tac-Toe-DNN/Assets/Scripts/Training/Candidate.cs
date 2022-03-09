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

        public void Reset()
        {
            wins = 0;
            draws = 0;
            loses = 0;
        }

        public override string ToString()
        {
            return $"Wins: {wins}, Loses: {loses}, Draws: {draws}";
        }

        public static bool operator >(Score scoreA, Score scoreB)
        {
            if (scoreA.loses == scoreB.loses)
            {
                return scoreA.wins > scoreB.loses;
            }
            else
            {
                return scoreA.loses < scoreB.loses;
            }
        }

        public static bool operator <(Score scoreA, Score scoreB)
        {
            if (scoreA.loses == scoreB.loses)
            {
                return scoreA.wins < scoreB.loses;
            }
            else
            {
                return scoreA.loses > scoreB.loses;
            }
        }
    }
}
