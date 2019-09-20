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
        genes.TryGetValue(ID, out g);
        if(g!=null)
            return g;
        else
            return AddGene();
    }

    public void AddConnection(Gene from, Gene to, float weight)
    {
        int index = HashFuction(from.GetID(), to.GetID());
        ConnectionGene c;
        c = GetConnectionGene(from,to);
        if (c == null)
        {
            print($"Added new connection ID {index} from {from.GetID()} to {to.GetID()}");
            c = new ConnectionGene(index, from, to, weight);
            connections.Add(index,c);
        }
        print($"Gotten connection ID {c.GetID()} from {from.GetID()} to {to.GetID()} with weight {c.GetWeight()}");
        c.SetEnabled(true);
    }


    public ConnectionGene GetConnectionGene(Gene from, Gene to)
    {
        ConnectionGene c;
        int index = HashFuction(from.GetID(), to.GetID());
        connections.TryGetValue(index, out c);
        return c;
    }
    public void RemoveConnection(ConnectionGene connection)
    {
        connections.Remove(connection.GetID());
        connections = null;//not best solution, can bring to memory leaks
    }

    int HashFuction(int from, int to)
    {
        return (from+1) * 100 + to;
    }
}
