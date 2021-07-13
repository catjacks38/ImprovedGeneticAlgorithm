using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Optimizer;

public class AgentManager : MonoBehaviour
{
    public Transform agentPrefab;
    public Transform agentSpawn;
    public Transform cameraTransform;

    public Text fitnessDisp;
    public Toggle onlyShowFittestToggle;

    public float genTime = 15.0f;
    public float mutationRate = 0.1f;
    public float mutationVariation = 10.0f;
    public bool onlyShowFittest = false;

    private GeneticOptimizer optim;

    public Transform[] agents;

    private int gen = 0;
    private int fittestAgentIDX = 0;

    private IEnumerator TrainingLoop()
    {
        // Inits and instantiates agents with totally random params
        for (int i = 0; i < agents.Length; i++)
        {
            agents[i] = Instantiate(agentPrefab, agentSpawn.position, Quaternion.identity);
        }

        // Training loop
        while (true)
        {
            // Let the agents train for genTime every generation
            yield return new WaitForSeconds(genTime);

            // Get optimized agents
            List<(List<List<List<float>>>, List<List<float>>)> newAgents = optim.Evolve(agents);

            // Deletes all agent objects
            for (int i = 0; i < agents.Length; i++)
            {
                Destroy(agents[i].gameObject);
            }

            // Instantiates agents with optimized params
            for (int i = 0; i < agents.Length; i++)
            {
                agents[i] = Instantiate(agentPrefab, agentSpawn.position, Quaternion.identity);
                AgentNN agentNNComponent = agents[i].gameObject.GetComponent(typeof(AgentNN)) as AgentNN;

                agentNNComponent.model.SetParams(newAgents[i]);
            }

            gen++;
        }
    }

    private int FittestAgentIDX()
    {
        int bestAgentIDX = 0;

        for (int i = 0; i < agents.Length; i++)
        {
            if (optim.CalcFitness(agents[i]) > optim.CalcFitness(agents[bestAgentIDX]))
            {
                bestAgentIDX = i;
            }
        }

        return bestAgentIDX;
    }

    void Start()
    {
        optim = new GeneticOptimizer(mutationVariation, mutationRate, agentSpawn);

        // Starts training
        IEnumerator coroutine = TrainingLoop();
        StartCoroutine(coroutine);
    }

    private void Update()
    {
        float sum = 0.0f;
        onlyShowFittest = onlyShowFittestToggle.isOn;
        fittestAgentIDX = FittestAgentIDX();

        // Only shows fittest agent if onlyShowFittest is true
        for (int i = 0; i < agents.Length; i++)
        {
            if (i == fittestAgentIDX || !onlyShowFittest)
            {
                agents[i].gameObject.tag = "fittest";
            }
            else 
            {
                agents[i].gameObject.tag = "needsToGitGud";
            }
        }

        // Adds up the total fitness
        foreach (Transform agentTransform in agents)
        {
            sum += optim.CalcFitness(agentTransform);
        }

        // Displays stats
        fitnessDisp.text = "Average Fitness: " + sum / agents.Length + "\nBest Fitness: " + optim.CalcFitness(agents[fittestAgentIDX]) + "\nGeneration " + gen;

        // Moves camera to fittest agent
        cameraTransform.position = new Vector3(agents[fittestAgentIDX].GetChild(0).position.x, agents[fittestAgentIDX].GetChild(0).position.y, cameraTransform.position.z);
    }
}
