using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Optimizer;
using UnityEngine.SceneManagement;

public class AgentManager : MonoBehaviour
{
    public Transform agentPrefab;
    public Transform agentSpawn;
    public Transform cameraTransform;

    public Text fitnessDisp;
    public Toggle onlyShowFittestToggle;

    public int genSize = 50;
    public float genTime = 15.0f;
    public float mutationRate = 0.1f;
    public float mutationVariation = 10.0f;
    public bool onlyShowFittest = false;

    private GeneticOptimizer optim;

    private List<Transform> agents = new List<Transform>();

    private int gen = 0;
    private int fittestAgentIDX = 0;
    private bool training = false;

    private IEnumerator TrainingLoop()
    {
        // Inits and instantiates agents with totally random params
        for (int i = 0; i < genSize; i++)
        {
            agents.Add(Instantiate(agentPrefab, agentSpawn.position, Quaternion.identity));
        }

        // Training loop
        while (true)
        {
            // Let the agents train for genTime every generation
            yield return new WaitForSeconds(genTime);

            // Get optimized agents
            List<(List<List<List<float>>>, List<List<float>>)> newAgents = optim.Evolve(agents);

            // Deletes all agent objects
            for (int i = 0; i < agents.Count; i++)
            {
                Destroy(agents[i].gameObject);
            }

            // Instantiates agents with optimized params
            agents.Clear();

            for (int i = 0; i < genSize; i++)
            {
                agents.Add(Instantiate(agentPrefab, agentSpawn.position, Quaternion.identity));
                AgentNN agentNNComponent = agents[i].gameObject.GetComponent(typeof(AgentNN)) as AgentNN;

                agentNNComponent.model.SetParams(newAgents[i]);
            }

            gen++;
        }
    }

    private int FittestAgentIDX()
    {
        int bestAgentIDX = 0;

        for (int i = 0; i < agents.Count; i++)
        {
            if (optim.CalcFitness(agents[i]) > optim.CalcFitness(agents[bestAgentIDX]))
            {
                bestAgentIDX = i;
            }
        }

        return bestAgentIDX;
    }

    private void Update()
    {
        if (training)
        {
            float sum = 0.0f;
            onlyShowFittest = onlyShowFittestToggle.isOn;
            fittestAgentIDX = FittestAgentIDX();

            // Only shows fittest agent if onlyShowFittest is true
            for (int i = 0; i < agents.Count; i++)
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
            fitnessDisp.text = "Average Fitness: " + sum / agents.Count + "\nBest Fitness: " + optim.CalcFitness(agents[fittestAgentIDX]) + "\nGeneration " + gen;

            // Moves camera to fittest agent
            cameraTransform.position = new Vector3(agents[fittestAgentIDX].GetChild(0).position.x, agents[fittestAgentIDX].GetChild(0).position.y, cameraTransform.position.z);
        }
    }

    public void StartTraining()
    {
        optim = new GeneticOptimizer(mutationVariation, mutationRate, agentSpawn);

        // Starts training
        IEnumerator coroutine = TrainingLoop();
        StartCoroutine(coroutine);

        training = true;
    }

    public void BackToStart()
    {
        SceneManager.LoadScene("start");
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
