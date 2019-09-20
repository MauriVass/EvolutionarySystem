using System;
using Unity;
using System.Collections.Generic;
using UnityEngine;

public class Gene
{
    //This is the most elementary part
    //This can thought as a neuron in a neural network
    private int ID;
    private float value;
    private List<Gene> output;
    private Func<float, float> activationFunction;

    public Gene(int ID)
    {
        this.ID = ID;
        //this.activationFunction = activationFunction;
        output = new List<Gene>();
    }

    //public void AddConnection(Gene from = null, Gene to = null)
    //{
    //    ConnectionGene c;
    //    if (from!=null)
    //        c = new ConnectionGene(from, this, 1);
    //    else if(to!=null)
    //        c = new ConnectionGene(this, to, 1);
    //}

    public int GetID()
    {
        return ID;
    }

    public void SetActivationFunction()
    {
        //TO DO
    }

    public float GetValue()
    {
        return value;
    }
    public void AddOutputGene(Gene gene)
    {
        output.Add(gene);
    }

    public void PropagateInput(float input)
    {
        value = activationFunction(input);

        for (int i = 0; i < output.Count; i++)
        {
            output[i].PropagateInput(value);
        }
    }
}
