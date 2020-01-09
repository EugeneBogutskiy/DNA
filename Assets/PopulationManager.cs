using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PopulationManager : MonoBehaviour
{
    public GameObject personePrefab;
    public int populationSize = 10;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;
    int trialTime = 10;
    int generation = 1;

    GUIStyle guiStile = new GUIStyle();

    void OnGUI()
    {
        guiStile.fontSize = 12;
        guiStile.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 10, 10), "Generation" + generation, guiStile);
        GUI.Label(new Rect(10, 25, 10, 10), "Trial time" + (int)elapsed, guiStile);

    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(-1.4f, 1.4f), UnityEngine.Random.Range(-0.9f, 0.9f), 0);
            GameObject go = Instantiate(personePrefab, pos, Quaternion.identity);
            go.GetComponent<DNAscript>().r = UnityEngine.Random.Range(0.0f, 1.0f);
            go.GetComponent<DNAscript>().g = UnityEngine.Random.Range(0.0f, 1.0f);
            go.GetComponent<DNAscript>().b = UnityEngine.Random.Range(0.0f, 1.0f);
            population.Add(go);

        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }

    }

    private void BreedNewPopulation()
    {
        List<GameObject> newPopulation = new List<GameObject>();
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<DNAscript>().timeToDie).ToList();

        population.Clear();
        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

    private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 pos = new Vector3(UnityEngine.Random.Range(-1.4f, 1.4f), UnityEngine.Random.Range(-0.9f, 0.9f), 0);
        GameObject offspring = Instantiate(personePrefab, pos, Quaternion.identity);
        DNAscript dna1 = parent1.GetComponent<DNAscript>();
        DNAscript dna2 = parent2.GetComponent<DNAscript>();
        offspring.GetComponent<DNAscript>().r = UnityEngine.Random.Range(0, 10) < 5 ? dna1.r : dna2.r;
        offspring.GetComponent<DNAscript>().g = UnityEngine.Random.Range(0, 10) < 5 ? dna1.g : dna2.g;
        offspring.GetComponent<DNAscript>().b = UnityEngine.Random.Range(0, 10) < 5 ? dna1.b : dna2.b;
        return offspring;
    }
}
