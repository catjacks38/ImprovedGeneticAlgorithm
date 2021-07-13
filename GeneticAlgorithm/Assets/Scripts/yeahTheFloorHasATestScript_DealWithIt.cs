using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class yeahTheFloorHasATestScript_DealWithIt : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            System.Random rng = new System.Random();
            Debug.Log((float) rng.NextDouble() * (10 + 10) - 10);
        }
    }

    void Update()
    {
        
    }
}
