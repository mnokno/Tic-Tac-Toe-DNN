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

        public static bool operator >(Score scoreA, Score scoreB)
        {
            if (scoreA.wins == scoreB.wins)
            {
                if (scoreA.loses == scoreB.loses)
                {
                    return scoreA.draws > scoreB.draws;
                }
                else
                {
                    return scoreA.loses < scoreB.loses;
                }
            }
            else
            {
                return scoreA.wins > scoreB.wins;
            }
        }

        public static bool operator <(Score scoreA, Score scoreB)
        {
            if (scoreA.wins == scoreB.wins)
            {
                if (scoreA.loses == scoreB.loses)
                {
                    return scoreA.draws < scoreB.draws;
                }
                else
                {
                    return scoreA.loses > scoreB.loses;
                }
            }
            else
            {
                return scoreA.wins < scoreB.wins;
            }
        }
    }
}
