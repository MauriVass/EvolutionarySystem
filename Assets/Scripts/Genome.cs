using System;
using System.Collections.Generic;
using UnityEngine;

public class Genome : MonoBehaviour
{
    //This is a collection of genes
    //This can be thought as an 'organism'
    //It can move, mutate etc..
    private int numberInput, numberOutput;
    private int counterGenes;
    private List<Gene> genes;
    private int counterConnection;
    private List<ConnectionGene> connections;
    private float fitness;

    private NEAT neat;

    private void Start()
    {
        CreateGenome();
    }

    public void CreateGenome()
    {
        numberInput = 4;
        numberOutput = 2;
        genes = new List<Gene>();
        connections = new List<ConnectionGene>();
        neat = GameObject.FindGameObjectWithTag("NEAT").GetComponent<NEAT>();

        for (int i = 0; i < (numberInput+numberOutput); i++)
        {
            AddGene();
        }
    }

    public float GetFitness()
    {
        return fitness;
    }

    public void AddGene()
    {
        Gene g = neat.GetGene(counterGenes++);
        genes.Add(g);
    }

    public void AddConnection()
    {
        //TO DO
        //ConnectionGene c = neat.AddConnection(coun)
    }
}
