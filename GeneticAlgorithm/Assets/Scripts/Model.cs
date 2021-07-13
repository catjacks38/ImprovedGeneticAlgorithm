using System;
using System.Collections.Generic;
using NN;

namespace Model
{
    public class AgentModel
    {
        private const float MaxVariation = 5.0f;
        private static Random rng = new Random();
        private LinearLayer[] Sequential = { new LinearLayer(6, 10, MaxVariation, rng), new LinearLayer(10, 15, MaxVariation, rng), new LinearLayer(15, 10, MaxVariation, rng), new LinearLayer(10, 4, MaxVariation, rng) };

        public List<float> forward(List<float> input)
        {
            List<float> output = new List<float>();

            // Passes input into first layer
            output = Sequential[0].forward(input);

            // Passes the output of the last layer into the next layer
            for (int i = 1; i < Sequential.Length - 1; i++)
            {
                output = Sequential[i].forward(output);
            }

            // Final output layer
            output = Sequential[Sequential.Length - 1].forward(output);

            return output;
        }

        public (List<List<List<float>>>, List<List<float>>) GetParams()
        {
            (List<List<List<float>>>, List<List<float>>) parameters = (new List<List<List<float>>>(), new List<List<float>>());

            foreach (LinearLayer fc in Sequential)
            {
                parameters.Item1.Add(fc.m);
                parameters.Item2.Add(fc.b);
            }

            return parameters;
        }

        public void SetParams((List<List<List<float>>>, List<List<float>>) parameters)
        {
            for (int i = 0; i < parameters.Item1.Count; i++)
            {
                Sequential[i].m = parameters.Item1[i];
            }
            
            for (int i = 0; i < parameters.Item2.Count; i++)
            {
                Sequential[i].b = parameters.Item2[i];
            }

            return;
        }
    }
}
