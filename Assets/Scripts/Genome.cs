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
    private Dictionary<int,Gene> genes;
    private Dictionary<int, ConnectionGene> connections;

    private float fitness, mutationFactor;
    private bool alive;
    private GameObject food;

    private Vector3[] sensorsDirection;
    private float[] sensorsValue;
    
    private Population population;
    
    private void Awake()
    {
        food = GameObject.FindWithTag("Food");
        genes = new Dictionary<int, Gene>();
        connections = new Dictionary<int, ConnectionGene>();
    }

    /// <summary>
    /// Create a new Genome object and add all genes needed: input and output genes
    /// </summary>
    public void InitializeGenome()
    {
        sensorsDirection = new[] {
            new Vector3(0, 0, 1),                 //Up
            new Vector3(1, 0, 1)/Mathf.Sqrt(2),   //Up-Right
            new Vector3(1, 0, 0),                 //Right
            new Vector3(1, 0, -1)/Mathf.Sqrt(2),  //Right-Down
            new Vector3(0, 0, -1),                //Down
            new Vector3(-1, 0, -1)/Mathf.Sqrt(2), //Down-Left
            new Vector3(-1, 0, 0),                //Left
            new Vector3(-1, 0, 1)/Mathf.Sqrt(2)   //Left-Up
        };
        sensorsValue = new float[sensorsDirection.Length];

        mutationFactor = 0.85f;

        numberInput = sensorsDirection.Length;
        numberOutput = 4;
        alive = true;

        for (int j = 0; j < (numberInput + numberOutput); j++)
        {
            AddGene();
        }

        //When create a new genome, create also a first connection between inputs and outputs
        int nofConnections = (numberInput + numberOutput) / 2 ;
        nofConnections = 1;
        for (int j = 0; j < nofConnections; j++)
        {
            int i, o;
            i = Random.Range(0, numberInput);
            o = Random.Range(numberInput, counterGenes);
            float w;
            w = Random.Range(-1f, 1f);

            AddConnection(genes[i], genes[o], w);
        }

        //Add a new gene after the input and output have been created
        //AddGene();
        Mutate();
    }

    /// <summary>
    /// Add a new Gene to the list of genes
    /// </summary>
    public void AddGene()
    {
        Gene g = population.GetGene(counterGenes);
        genes.Add(counterGenes,g);
        counterGenes++;

        //All input and output genes have been created
        //So the next ones will be in the middle and they will be used for inter-connections
        if (counterGenes > numberInput + numberOutput)
        {
            Gene i, o;
            float w = 1;
            //Select a gene from input genes 
            i = genes[Random.Range(0, numberInput)];
            //Select the next gene (output or middle one)
            //(The last gene created is excluded since it make no sense to be connected to itself)
            o = genes[Random.Range(numberInput, counterGenes)];

            ConnectionGene c = population.GetConnection(i, o);
            if (c != null)
            {
                w = c.GetWeight();
                c.SetEnabled(false);
            }
            AddConnection(i, g, w);
            AddConnection(g, o, 1);
        }
    }
    
    /// <summary>
    /// Add a specific connection between from Gene  and to Gene with a given weight
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="weight"></param>
    public void AddConnection(Gene from, Gene to, float weight)
    {
        ConnectionGene c = population.GetConnection(from, to, weight);
        //if (!from.HasConnection(c))
        {
            from.AddConnection(c);
            to.AddInput();
            connections.Add(c.GetID(),c);
        }
    }
 
    public void RemoveConnection(ConnectionGene connection)
    {
        ////Connect the 2 extremes together
        //Gene from = connection.GetFromGene();
        //Gene to = connection.GetToGene();
        //float weight = connection.GetWeight();
        //AddConnection(from, to, weight);

        ////Remove old connection
        //population.RemoveConnection(connection);
        connection.SetEnabled(false);
    }

    void Mutate()
    {
        int n = Random.Range(0, 6);
        switch (n)
        {
            case 0:
                //Mutation: Add a Gene and its connections
                AddGene();
                break;

            //case 0:
            //    //Mutation: Remove Gene and its connections
            //    break;

            case 1:
                //Mutation: Add simple Connection
                Gene i, o;
                float w = Random.Range(-1f,1f);
                //Select a gene from input genes 
                i = genes[Random.Range(0, numberInput)];
                //Select the next gene (output or middle one)
                //(The last gene created is excluded since it make no sense to be connected to itself)
                o = genes[Random.Range(numberInput, counterGenes)];

                ConnectionGene c = population.GetConnection(i, o, w);
                connections.Add(c.GetID(),c);
                break;

            case 2:
                //RemoveConnection(connections[Random.Range(0,connections.Count)]);
                break;

            case 3:
                //Mutation: Change weigths
                for (int j = 0; j < connections.Count/2; j++)
                {
                    w = connections[j].GetWeight();
                    connections[j].SetWeight(w + mutationFactor * Random.Range(-1,-1));
                }
                break;

            case 4:
                //Mutation: toogle enabled
                //connections[Random.Range(0, connections.Count)].ChangeEnabled();
                break;
                
            case 5:
                //Mutation: Change activation function
                int func = Random.Range(0,1);
                genes[Random.Range(0, genes.Count)].SetActivationFunction(func);
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        if (alive)
        {
            GetSensorsValue();
            PropagateInput();
            Movement();
        }
    }


    void GetSensorsValue()
    {
        RaycastHit hit;
        for (int i = 0; i < sensorsDirection.Length; i++)
        {
            sensorsValue[i] = 0.5f;
            int distance = 5;
            if (Physics.Raycast(transform.position, sensorsDirection[i], out hit, distance))
            {
                if (hit.collider.tag=="Food")
                {
                    sensorsValue[i] = 1;
                    fitness += Time.deltaTime;
                }
                else if (hit.collider.tag == "Wall")
                {
                    sensorsValue[i] = 0;
                }
            }

            Color color = new Color();
            if (sensorsValue[i] == 0)
                color = Color.red;
            else if (sensorsValue[i] == 0.5f)
                color = Color.black;
            else if (sensorsValue[i] == 1)
                color = Color.green;


            Debug.DrawRay(transform.position, sensorsDirection[i] * distance, color);
        }
    }
    void PropagateInput()
    {
        //Get the inputs. In this case there are 4 inputs so na array of 4 elements
        //0 -> x cordinate of the ball position
        //1 -> y cordinate of the ball 
        //2 -> x cordinate of the genome position
        //3 -> y cordinate of the genome position
        for (int i = 0; i < numberInput; i++)
        {
            genes[i].PropagateInput(sensorsValue[i]);
        }
    }
    public float[] o = new float[4];
    void Movement()
    {
        //Debug
        //for (int i = 0; i < 4; i++)
        //{
        //    o[i] = genes[numberInput + i].GetValue();
        //}

        float speed = 2f;
        //In this case there are only four possible outputs and four actions: go up and go down, go right and left.
        //The action is chosen according with greatest output value
        //Go Up od Down
        if (genes[numberInput].GetValue() > genes[numberInput + 1].GetValue())
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(0, 0, -speed * Time.deltaTime);
        }
        //Go Right or Left
        if (genes[numberInput+2].GetValue() > genes[numberInput + 3].GetValue())
        {
            transform.Translate(speed * Time.deltaTime,0, 0);
        }
        else
        {
            transform.Translate(-speed * Time.deltaTime,0, 0);
        }
    }
    
    public GameObject neuron;    
    void Visualize()
    {
        for (int i = 0; i < genes.Count; i++)
        {
            genes[i].Print(name);
        }
    }

    public Genome Reproduce(Genome parentB)
    {
        //Create a reference for a new Genome child
        Genome child = population.CreateGenome();

        //Create a dict of genes picking them from the 2 parents
        Dictionary<int, Gene> g = new Dictionary<int, Gene>();
        g = GetGenes();
        foreach (KeyValuePair<int,Gene> i in parentB.GetGenes())
        {
            if (g.ContainsKey(i.Key))
            {
                //Choose a random number between 0 and 1
                int n = Random.Range(0,2);
                // If it is 0 select the gene from the first parent -> do nothing
                // If it is 1 select the gene from the second parent -> overwrite the current value in the dict
                if (n==1)
                {
                    g[i.Key] = i.Value;
                }
            }
        }
        child.SetGenes(g);

        // Create a dict of ConnectionGene picking them from the 2 parents
        Dictionary<int, ConnectionGene> c = new Dictionary<int, ConnectionGene>();
        c = GetConnectionGenes();
        foreach (KeyValuePair<int, ConnectionGene> i in parentB.GetConnectionGenes())
        {
            if (c.ContainsKey(i.Key))
            {
                //Choose a random number between 0 and 1
                int n = Random.Range(0, 2);
                // If it is 0 select the gene from the first parent -> do nothing
                // If it is 1 select the gene from the second parent -> overwrite the current value in the dict
                if (n == 1)
                {
                    c[i.Key] = i.Value;
                }
        
    }
        }
        foreach (ConnectionGene i in parentB.GetConnectionGenes().Values)
        {
            child.AddConnection(i.GetFromGene(),i.GetToGene(),i.GetWeight());
        }

        return child;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Food"))
        {
            fitness += 1;
            other.transform.position = new Vector3(Random.Range(-population.platformSize, population.platformSize), 0, Random.Range(-population.platformSize, population.platformSize));
        }
        else if(other.tag.Equals("Wall"))
        {
            fitness -= 1;
        }
    }


    public float GetFitness()
    {
        return fitness;
    }
    public void SetGenes(Dictionary<int, Gene> newGenes)
    {
        genes = newGenes;
    }
    public Dictionary<int, Gene> GetGenes()
    {
        //Create a copy of the genes and return it
        //This avoid changes in a Genome object
        Dictionary<int, Gene> g = new Dictionary<int, Gene>();
        foreach (KeyValuePair<int, Gene> i in genes)
        {
            g.Add(i.Key, i.Value);
        }
        return g;
    }
    public Dictionary<int, ConnectionGene> GetConnectionGenes()
    {
        //Create a copy of the ConnectionGene and return it
        //This avoid changes in a Genome object
        Dictionary<int, ConnectionGene> c = new Dictionary<int, ConnectionGene>();
        foreach (KeyValuePair<int, ConnectionGene> i in connections)
        {
            c.Add(i.Key, i.Value);
        }
        return c;
    }
    public void SetPopulation(Population pop)
    {
        population = pop;
    }
}
