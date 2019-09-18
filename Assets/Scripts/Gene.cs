using System;
using Unity;
using System.Collections.Generic;

public class Gene
{
    //This is the most elementary part
    //This can thought as a neuron in a neural network
    private int ID;
    private float value;
    private List<ConnectionGene> connections;
    private Func<float, float> activationFunction;

    public Gene(int ID)
    {
        this.ID = ID;
        //this.activationFunction = activationFunction;
        connections = new List<ConnectionGene>();
    }
    public Gene(int ID, Func<float, float> activationFunction)
    {
        this.ID = ID;
        this.activationFunction = activationFunction;
        connections = new List<ConnectionGene>();
    }

    public void AddConnection(Gene from = null, Gene to = null)
    {
        ConnectionGene c;
        if (from!=null)
            c = new ConnectionGene(from, this, 1);
        else if(to!=null)
            c = new ConnectionGene(this, to, 1);
    }

    public int GetID()
    {
        return ID;
    }

    public void SetActivationFunction()
    {
        //TO DO
    }
}
