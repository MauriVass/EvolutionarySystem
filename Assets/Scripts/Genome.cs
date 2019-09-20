using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Genome : MonoBehaviour
{
    //This is a collection of genes
    //This can be thought as an 'organism'
    //It can move, mutate etc..
    private int numberInput, numberOutput;
    private int counterGenes;
    private List<Gene> genes;
    private int counterConnection;
    //private List<ConnectionGene> connections;
    private float fitness;
    private GameObject ball;
    private bool alive;

    //private NEAT neat;
    private Population population;

    private void Start()
    {
        ball = GameObject.FindWithTag("Ball");
        CreateGenome();
    }

    public void CreateGenome()
    {
        numberInput = 4;
        numberOutput = 2;
        genes = new List<Gene>();
        //connections = new List<ConnectionGene>();
        population = transform.parent.GetComponent<Population>();
        alive = true;

        for (int j = 0; j < (numberInput + numberOutput); j++)
        {
            AddGene();
        }

        int i, o;
        i = Random.Range(0, numberInput);
        o = Random.Range(0, numberOutput);
        float w;
        w = Random.Range(-0.5f, 0.5f);

        AddConnection(genes[i], genes[numberInput + o], w);
    }

    public void AddGene()
    {
        Gene g = population.GetGene(counterGenes++);
        genes.Add(g);

        if (counterGenes >= numberInput + numberOutput)
        {
            Gene i, o;
            float w = 1;
            //Select an input gene
            i = genes[Random.Range(0, numberInput)];
            //Select an output gene excluding the new gene
            o = genes[Random.Range(numberInput, counterGenes)];

            ConnectionGene c = population.GetConnectionGene(i, o);
            if (c != null)
            {
                c.GetWeight();
                c.SetEnabled(false);
            }
            AddConnection(i, g, w);
            AddConnection(g, o, 1);
        }
    }

    public void AddConnection(Gene from, Gene to, float weight)
    {
        population.AddConnection(from, to, weight);
        ConnectionGene c = population.GetConnectionGene(from, to);
        //connections.Add(c);
        from.AddOutputGene(to);
    }

    public void RemoveConnection(ConnectionGene connection)
    {
        //Connect the 2 extremes together
        Gene from = connection.GetFromGene();
        Gene to = connection.GetToGene();
        float weight = connection.GetWeight();
        AddConnection(from, to, weight);

        //Remove old connection
        population.RemoveConnection(connection);
    }

    private void Update()
    {
        if (alive)
        {
            PropagateInput();
            Movement();
            fitness+=Time.deltaTime;
        }

        if (ball.transform.position.x>transform.position.x)
        {
            alive = false;
        }
    }

    public float GetFitness()
    {
        return fitness;
    }
    
    void PropagateInput()
    {
        genes[0].PropagateInput(ball.transform.position.x);
        genes[1].PropagateInput(ball.transform.position.y);
        genes[2].PropagateInput(transform.position.x);
        genes[3].PropagateInput(transform.position.y);
    }

    void Movement()
    {
        float speed = 2f;
        if (genes[numberInput].GetValue() > genes[numberInput + 1].GetValue())
        {
            transform.Translate(0, speed, 0);
        }
        else
        {
            transform.Translate(0, -speed, 0);
        }
    }
}
