using System;

public class ConnectionGene{
  //Create a connection between two Genes
  private Gene from, to;
  private float weight;
  private bool enabled;

  public ConnectionGene(Gene from, Gene to, float weight, bool enabled = true){
    this.from = from;
    this.to = to;
    this.weight = weight;
    this.enabled = enabled;
  }

  public void SetWeight(float newWeight){
    weight = newWeight;
  }
  public float GetWeight(){
    return weight;
  }

  public Gene GetFromGene(){
    return from;
  }
  public Gene GetToGene(){
    return to;
  }

  public void SetEnabled(bool value){
    enabled = value;
  }
  public bool GetEnable(){
    return enabled;
  }

}
