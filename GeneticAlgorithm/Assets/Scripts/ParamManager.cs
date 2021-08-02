using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ParamManager : MonoBehaviour
{
    private InputField genTimeInput;
    private InputField genSizeInput;
    private InputField mutationRateInput;

    private float genTime = 15.0f;
    private int genSize = 100;
    private float mutationRate = 0.005f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        genTimeInput = GameObject.Find("GenTimeParam").GetComponent(typeof(InputField)) as InputField;
        genSizeInput = GameObject.Find("GenSizeParam").GetComponent(typeof(InputField)) as InputField;
        mutationRateInput = GameObject.Find("MutationRateParam").GetComponent(typeof(InputField)) as InputField;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            AgentManager agentManager = GameObject.Find("AgentManager").GetComponent(typeof(AgentManager)) as AgentManager;

            agentManager.genTime = genTime;
            agentManager.genSize = genSize;
            agentManager.mutationRate = mutationRate;

            agentManager.StartTraining();

            Destroy(gameObject);
        }
    }

    public void SwitchScene()
    {
        try
        {
            genTime = float.Parse(genTimeInput.text);
            genSize = int.Parse(genSizeInput.text);
            mutationRate = float.Parse(mutationRateInput.text);

            SceneManager.LoadScene("training");
        }
        catch
        {
            return;
        }
    }
}
