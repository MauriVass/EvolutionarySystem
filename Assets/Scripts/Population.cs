using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Population : MonoBehaviour
{
    //Keep the collection of all the genomes and
    //Keep information of all genes and connections
    //If you want to add a gene to a connection you can do it 
    //only if that gene is already present in the genes dictionary


    public int populationSize;
    private List<Genome> genomes;

    private int counterGenes;
    private Dictionary<int, Gene> genes;
    private int counterConnection;
    private Dictionary<int, ConnectionGene> connections;
    
    [SerializeField]
    private GameObject genomePrefab;
    
    [SerializeField]
    private GameObject food, badFood, foodContainer;
    private int nFood, nBadFood;
    public float platformSize;
    private void Start()
    {
        platformSize *= 0.95f;
        nFood = 10;
        nBadFood = 7;
        CreateFood(nFood,food,"Food");
        CreateFood(nBadFood,badFood, "BadFood");

        genes = new Dictionary<int, Gene>();
        connections = new Dictionary<int, ConnectionGene>();
        CreatePopulation();
    }

    /// <summary>
    /// Create genomes forming a population
    /// </summary>
    public void CreatePopulation()
    {
        genomes = new List<Genome>();

        for (int i = 0; i < populationSize; i++)
        {
            Genome g = CreateGenome();
            g.InitializeGenome();
        }
    }

    public Genome CreateGenome()
    {
        float size = platformSize / 2;
        Vector3 pos = new Vector3(Random.Range(-size, size), 0, Random.Range(-size, size));
        GameObject tmp = Instantiate(genomePrefab, pos * 2, Quaternion.identity);
        tmp.transform.SetParent(transform);
        tmp.name = "Genome " + (genomes.Count + 1);
        Genome g = tmp.GetComponent<Genome>();
        g.SetPopulation(this);
        genomes.Add(g);
        return g;
    }

    Gene AddGene()
    {
        Gene g = new Gene(counterGenes);
        genes.Add(counterGenes,g);
        counterGenes++;
        return g;
    }
    public Gene GetGene(int ID)
    {
        Gene g;
        if(genes.TryGetValue(ID, out g))
            return g;
        else
            return AddGene();
    }
    
    /// <summary>
    /// Get connection if it exists otherwise it is created a new Connection
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="weight"></param>
    /// <returns></returns>
    public ConnectionGene GetConnection(Gene from, Gene to, float weight = 0)
    {
        int index = HashFuction(from.GetID(), to.GetID());
        ConnectionGene c;
        connections.TryGetValue(index, out c);
        if (c==null)
        {
            c = new ConnectionGene(index, from, to, weight);
            connections.Add(index, c);
            c.SetEnabled(true);
        }
        return c;
    }
    public void RemoveConnection(ConnectionGene connection)
    {
        connections.Remove(connection.GetID());
        connection = null;//not best solution, can bring to memory leaks
    }

    private void CreateFood(int size, GameObject prefab, string name)
    {
        for (int i = 0; i < size; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-platformSize, platformSize), 0, Random.Range(-platformSize, platformSize));
            GameObject tmp = Instantiate(prefab, pos, Quaternion.identity);
            tmp.name = $"{name}: {i + 1}";
            tmp.transform.SetParent(foodContainer.transform);
        }
    }

    void Selection(int n)
    {
        genomes.Sort((g1, g2) => g2.GetFitness().CompareTo(g1.GetFitness()));
        //Remove worst Genomes
        for (int i = n - 1;  i < genomes.Count;  i++)
        {
            Genome g = genomes[i];
            genomes.RemoveAt(i);
            Destroy(g.gameObject);
        }
    }
    void Reproduction(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Genome parent1 = genomes[Random.Range(0,genomes.Count)];
            Genome parent2 = genomes[Random.Range(0, genomes.Count)];
            Genome g = parent1.Reproduce(parent2);
            genomes.Add(g);
        }
    }

    int HashFuction(int from, int to)
    {
        return (from + 1) * 100 + to;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Selection(populationSize/2);
            foreach (Genome i in genomes)
            {
                print(i.GetFitness());
            }
            Reproduction(populationSize/2);
        }
    }
}
