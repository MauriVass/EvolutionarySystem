using System;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour
{
    //Keep the collection of all the genomes and
    //Keep information of all genes and connections
    //If you want to add a gene to a connection you can do it 
    //only if that gene is already present in the genes dictionary
    private int populationSize;
    private List<Genome> population;

    private int counterGenes;
    private Dictionary<int, Gene> genes;
    private int counterConnection;
    private Dictionary<int, ConnectionGene> connections;

    [SerializeField]
    private GameObject genomePrefab;

    private void Start()
    {
        genes = new Dictionary<int, Gene>();
        connections = new Dictionary<int, ConnectionGene>();
        CreatePopulation();
    }

    public void CreatePopulation()
    {
        populationSize = 5;
        population = new List<Genome>();

        for (int i = 0; i < populationSize; i++)
        {
            GameObject tmp = Instantiate(genomePrefab);
            tmp.transform.SetParent(transform);
            tmp.name = "Genome " + (i + 1);
            Genome g = tmp.GetComponent<Genome>();
            AddGenome(g);
        }
    }

    public void AddGenome(Genome genome)
    {
        population.Add(genome);
    }

    Gene AddGene()
    {
        Gene g = new Gene(counterGenes++);
        genes.Add(counterGenes,g);
        return g;
    }
    public Gene GetGene(int ID)
    {
        Gene g;
        if(!genes.TryGetValue(ID,out g))
            return g;
        else
            return AddGene();
    }

    ConnectionGene AddConnection(Gene from, Gene to, float weight)
    {
        int index = HashFuction(from.GetID(), to.GetID());
        ConnectionGene c = new ConnectionGene(from,to,weight);
        connections.Add(index,c);
        return c;
    }
    public ConnectionGene GetConnectionGene(Gene from, Gene to, float weight)
    {
        ConnectionGene c = new ConnectionGene(from, to, 1);
        int index = HashFuction(from.GetID(), to.GetID());
        if (!connections.TryGetValue(index, out c))
            return c;
        else
            return AddConnection(from,to,weight);
    }

    int HashFuction(int from, int to)
    {
        return from * 100 + to;
    }
}
