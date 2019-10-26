using System;

public class ConnectionGene
{
    //Create a connection between two Genes

    //Identifier for a single connection
    private int ID;
    //The two genes the connection links together
    private Gene from, to;
    //The connection's weight
    private float weight;
    private bool enabled;

    public ConnectionGene(int ID, Gene from, Gene to, float weight)
    {
        this.ID=ID;
        this.from = from;
        this.to = to;
        this.weight = weight;
    }

    public int GetID()
    {
        return ID;
    }
    public Gene GetFromGene()
    {
        return from;
    }
    public Gene GetToGene()
    {
        return to;
    }

    public void SetWeight(float newWeight)
    {
        weight = newWeight;
    }
    public float GetWeight()
    {
        return weight;
    }

    public void SetEnabled(bool value)
    {
        enabled = value;
    }
    public void ChangeEnabled()
    {
        enabled = !enabled;
    }
    public bool GetEnable()
    {
        return enabled;
    }

    public void PropagateInput(float value)
    {
        if (enabled)
            to.PropagateInput(value * weight);
        else
            to.PropagateInput(0);
    }
}
