using System;
using System.Collections.Generic;

namespace NN
{
    public class LinearLayer
    {
        public List<List<float>> m = new List<List<float>>();
        public List<float> b = new List<float>();

        private int InputShape = 0;
        private int OutputShape = 0;

        public LinearLayer(int inputShape, int outputShape, float startingVariation, Random rng)
        {
            InputShape = inputShape;
            OutputShape = outputShape;

            // Adds RNG to weights
            for (int y = 0; y < InputShape; y++)
            {
                List<float> tempM = new List<float>();

                for (int x = 0; x < OutputShape; x++)
                {
                    tempM.Add((float) rng.NextDouble() * (startingVariation + startingVariation) - startingVariation);
                }

                m.Add(tempM);
            }

            // Adds RNG to biases
            for (int i = 0; i < OutputShape; i++)
            {
                b.Add((float) rng.NextDouble() * (startingVariation + startingVariation) - startingVariation);
            }
        }

        public List<float> forward(List<float> input)
        {
            // Inits output list
            List<float> output = new List<float>();

            for (int i = 0; i < OutputShape; i++)
            {
                output.Add(0.0f);
            }

            // Adds the inputs times the weights to the output
            for (int y = 0; y < InputShape; y++)
            {
                for (int x = 0; x < OutputShape; x++)
                {
                    output[x] += input[y] * m[y][x];
                }
            }

            // Adds the biases to the output
            for (int i = 0; i < OutputShape; i++)
            {
                output[i] += b[i];
            }

            return output;
        }
    }
    class ActivationFunction
    {
        // Sigmoid(x) = 1/(1 + e^-x)
        static public List<float> Sigmoid(List<float> input)
        {
            List<float> output = new List<float>();

            foreach(float value in input)
            {
                output.Add((float) (1.0 / (1.0 + Math.Exp(-value))));
            }

            return output;
        }

        // Tanh(x) = (e^x - e^-x)/(e^x + e^-x)
        static public List<float> Tanh(List<float> input)
        {
            List<float> output = new List<float>();

            foreach (float value in input)
            {
                output.Add((float) ((Math.Exp(value) - Math.Exp(-value)) / (Math.Exp(value) + Math.Exp(-value))));
            }

            return output;
        }

        // ReLU(x) = Max(0, x)
        static public List<float> ReLU(List<float> input)
        {
            List<float> output = new List<float>();

            foreach (float value in input)
            {
                output.Add(Math.Max(0, value));
            }

            return output;
        }

    }
}
