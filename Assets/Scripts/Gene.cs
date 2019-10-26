using System;
using Unity;
using System.Collections.Generic;
using UnityEngine;

public class Gene
{
    //This is the most elementary part
    //This can thought as a neuron in a neural network

    //Identifier for single gene
    private int ID;
    //The total number of input for this gene
    private int nInput;
    //The actuall number of inputs the have already arrived.
    private int counterInput;
    //The actuall value of the gene: f(SUM(input*weight)) where f() is the ActivationFunction
    private float value;
    //ActivationFunction: this can be Sigmoid, Tanh, ReLU, etc..
    private Func<float, float> ActivationFunction;
    //The list of all the connections from this gene to the next genes
    private List<ConnectionGene> output;


    /// <summary>
    /// Create a new Gene object
    /// </summary>
    /// <param name="ID"></param>
    public Gene(int ID)
    {
        this.ID = ID;
        //this.activationFunction = activationFunction;
        output = new List<ConnectionGene>();
        SetActivationFunction(0);
    }
    
    public int GetID()
    {
        return ID;
    }

    /// <summary>
    /// Set the activation function by index:
    /// 0) Sigmoid Function
    /// 1) Linear
    /// </summary>
    /// <param name="func"></param>
    public void SetActivationFunction(int func)
    {
        switch (func)
        {
            case 0:
                ActivationFunction = SigmoidFunction;
                    break;
            case 1:
                ActivationFunction = Linear;
                break;

            default:
                break;
        }
    }

    public float GetValue()
    {
        return value;
    }

    public void AddConnection(ConnectionGene connection)
    {
        output.Add(connection);
    }
    public void RemoveConnection()
    {
        for (int i = 0; i < output.Count; i++)
        {
            output.RemoveAt(i);
        }
    }
    public bool HasConnection(ConnectionGene connection)
    {
        return output.Contains(connection);
    }
    public void AddInput()
    {
        nInput++;
    }

    public void PropagateInput(float input)
    {
        value += input;
        counterInput++;

        //When counterInput>nInput the value can be propagated to the next genes
        if (counterInput>nInput)
        {
            value = ActivationFunction(value);
            counterInput = 0;
            for (int i = 0; i < output.Count; i++)
            {
                output[i].PropagateInput(value);
            }
            value = 0;
        }
        else if (output.Count==0)
        {
            value = ActivationFunction(value);
        }
    }

    float SigmoidFunction(float input)
    {
        return 1.0f / (1.0f + Mathf.Exp(-input));
    }
    float Linear(float input)
    {
        return input;
    }

    public void Print(string genome)
    {
        if (output.Count>0)
        {
            string s = genome + " Gene: " + GetID() + " -> ";

            for (int i = 0; i < output.Count; i++)
            {
                s += ", Connection to: " + output[i].GetToGene().GetID() + " with weight: " + output[i].GetWeight();
            }
            Debug.Log(s);
        }
    }
}
