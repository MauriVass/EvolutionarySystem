using System.Collections.Generic;
using UnityEngine;

public class NEAT : MonoBehaviour
{
    //NeuroEvolution of Augmenting Topologies
    //Keep information of all genes and connections
    //If you want to add a gene to a connection you can do it 
    //only if that gene is already present in the genes dictionary
    private int numberGenes;
    private Dictionary<int, Gene> genes;
    private int numberConnection;
    private Dictionary<int, ConnectionGene> connections;

    private void Start()
    {
        genes = new Dictionary<int, Gene>();
        connections = new Dictionary<int, ConnectionGene>();
    }

    public void AddGene(Gene gene)
    {
        genes.Add(numberGenes++, gene);
    }
    public Gene GetGene(int ID)
    {
        if (ID < 0 && ID > numberGenes) Debug.LogError("Wrong ID");
        Gene g;
        genes.TryGetValue(ID, out g);
        return g;
    }

    public void AddConnection(Gene from, Gene to)
    {
        ConnectionGene c = new ConnectionGene(from, to, 1);
        //Given the IDs of the 'from' gene and 'to' gene returns a unique number (a sort of hash function)
        int index = HashFuction(from.GetID(),to.GetID());
        connections.Add(index, c);
        numberConnection++;
    }
    public ConnectionGene GetConnection(Gene from, Gene to)
    {
        ConnectionGene c;
        int index = HashFuction(from.GetID(), to.GetID());
        connections.TryGetValue(index, out c);
        return c;
    }

    int HashFuction(int from, int to)
    {
        return from * 100 + to;
    }

}
