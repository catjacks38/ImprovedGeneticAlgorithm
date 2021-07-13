using UnityEngine;
using System.Collections.Generic;

namespace Optimizer
{
    public class GeneticOptimizer
    {
        private float AgentSpawnX;

        private float MutationRate;
        private float MaxMutationVariation;

        public GeneticOptimizer(float mutationVariation, float mutationRate, Transform agentSpawn)
        {
            AgentSpawnX = agentSpawn.position.x;

            MutationRate = mutationRate;
            MaxMutationVariation = mutationVariation;
        }

        public float CalcFitness(Transform agent)
        {
            return agent.GetChild(0).position.x - AgentSpawnX;
        }

        public List<(List<List<List<float>>>, List<List<float>>)> Evolve(Transform[] agents)
        {
            (List<List<List<float>>>, List<List<float>>)[] agentParams = new (List<List<List<float>>>, List<List<float>>)[2];
            List<(List<List<List<float>>>, List<List<float>>)> newAgentParams = new List<(List<List<List<float>>>, List<List<float>>)>();

            // Gets top 2 best agents
            (int, int) bestAgentsIDX = (0, 0);

            for(int i = 0; i < agents.Length; i++)
            {
                if (CalcFitness(agents[i]) > CalcFitness(agents[bestAgentsIDX.Item1]))
                {
                    bestAgentsIDX.Item1 = i;
                }
            }

            for(int i = 0; i < agents.Length; i++)
            {
                if (i == bestAgentsIDX.Item1)
                {
                    continue;
                }

                if (CalcFitness(agents[i]) > CalcFitness(agents[bestAgentsIDX.Item2]))
                {
                    bestAgentsIDX.Item2 = i;
                }
            }

            // Extracts agent parameters of top 2 fittest agents
            agentParams[0] = (agents[bestAgentsIDX.Item1].gameObject.GetComponent(typeof(AgentNN)) as AgentNN).model.GetParams();
            agentParams[1] = (agents[bestAgentsIDX.Item2].gameObject.GetComponent(typeof(AgentNN)) as AgentNN).model.GetParams();

            // Optimizes new agents with top 2 RNG crossover
            for (int i = 0; i < agents.Length; i++)
            {
                (List<List<List<float>>>, List<List<float>>) newAgent = (new List<List<List<float>>>(), new List<List<float>>());

                // Optimizes weights
                for (int a = 0; a < agentParams[0].Item1.Count; a++)
                {
                    List<List<float>> newWeights2D = new List<List<float>>();

                    for (int b = 0; b < agentParams[0].Item1[a].Count; b++)
                    {
                        List<float> newWeights1D = new List<float>();

                        for (int c = 0; c < agentParams[0].Item1[a][b].Count; c++)
                        {
                            // Picks mutationRNG
                            // Picks one of the top two agents randomly
                            float mutationRNG = Random.Range(0.0f, 1.0f);
                            int fittestAgentsIDX = Random.Range(0, 2);

                            // If mutationRNG was less than mutation rate, add a point mutation
                            // Else do the normal parameter cross over
                            if (mutationRNG <= MutationRate)
                            {
                                newWeights1D.Add(Random.Range(-MaxMutationVariation, MaxMutationVariation));
                            }
                            else
                            {
                                newWeights1D.Add(agentParams[fittestAgentsIDX].Item1[a][b][c]);
                            }
                        }

                        newWeights2D.Add(newWeights1D);
                    }

                    newAgent.Item1.Add(newWeights2D);
                }

                // Optimizes biases
                for (int a = 0; a < agentParams[0].Item2.Count; a++)
                {
                    List<float> newBiases1D = new List<float>();

                    for (int b = 0; b < agentParams[0].Item2[a].Count; b++)
                    {
                        // Picks mutationRNG
                        // Picks one of the top two agents randomly
                        float mutationRNG = Random.Range(0.0f, 1.0f);
                        int fittestAgentsIDX = Random.Range(0, 2);

                        // If mutationRNG was less than mutation rate, add a point mutation
                        // Else do the normal parameter cross over
                        if (mutationRNG <= MutationRate)
                        {
                            newBiases1D.Add(Random.Range(-MaxMutationVariation, MaxMutationVariation));
                        }
                        else
                        {
                            newBiases1D.Add(agentParams[fittestAgentsIDX].Item2[a][b]);
                        }
                    }

                    newAgent.Item2.Add(newBiases1D);
                }

                newAgentParams.Add(newAgent);
            }

            return newAgentParams;
        }
    }
}
