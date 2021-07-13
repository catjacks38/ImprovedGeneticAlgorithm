# ImprovedGeneticAlgorithm
## Improved genetic algorithm made in Unity
This is an improved version of genetic algorithm I wrote here [GeneticAlgorithmPOC](https://github.com/catjacks38/GeneticAlgorithmPOC).

## Improvements I Made to the Original Algorithm
I got rid of the algorithm of taking the mean of all of the top two fittest parameters, and randomly adding offset variation to all of the new model parameters.

My new algorithm takes the top two fittest models and randomly swaps out parameters from eachother to create the new models. There is a small change that a couple of the parameters don't "swap correctly" and get set to random values. Like a point mutation.

This new algorithm is more similar to how organisms pass on there genes in biology. Due to the parameter swaping resembling chromosomal crossover, and the point mutation. Unlike my previous algorithm, which used the very biological method of taking the mean of the genes, and adding slight variation to the nucleobases that arent even numbers.

## Info on the Project
 - Made Using Unity Version 2020.3.13f1
 - No 3rd Party Assets were Used; They were all made by me, and are included in the project files.
