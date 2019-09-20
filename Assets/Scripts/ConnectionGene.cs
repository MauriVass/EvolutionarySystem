public class ConnectionGene
{
    //Create a connection between two Genes
    private int ID;
    private Gene from, to;
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
    public bool GetEnable()
    {
        return enabled;
    }
}
