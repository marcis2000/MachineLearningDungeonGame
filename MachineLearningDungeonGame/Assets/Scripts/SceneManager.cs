using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneManager : MonoBehaviour
{
    private DummyScript dummyScript;
   // private List<DummyScript> dummyScripts = new List<DummyScript>();
    public List<SpawnPoint> dummySpawnPoints;
    public GameObject dummy;
    public GameObject newDummy;

    public int requiredDummies;
    private int randomVal;
    private int currentPoints;
    private Vector3 position, rotation;
    [SerializeField] TextMeshProUGUI currentDummies;
    [SerializeField] TextMeshProUGUI allDummies;


    System.Random rnd = new System.Random();
    private void Awake()
    {
        if (requiredDummies > dummySpawnPoints.Count) return;
        for(int i = 0; i<requiredDummies; i++)
        {
            randomVal = rnd.Next(0, dummySpawnPoints.Count);
            rotation = dummySpawnPoints[randomVal].GetComponent<SpawnPoint>().rotation;
            newDummy = Instantiate(dummy, dummySpawnPoints[randomVal].transform.position, Quaternion.Euler(rotation));
            dummyScript = newDummy.GetComponent<DummyScript>();
            //dummyScripts.Add(dummyScript);
            dummyScript.OnAddToCount += AddToCount;
            dummySpawnPoints.RemoveAt(randomVal);
            allDummies.text = requiredDummies.ToString();
        }
        Destroy(dummy);
    }

    private void AddToCount(object sender, EventArgs e)
    {
        currentPoints++;
        currentDummies.text = currentPoints.ToString();
    }
}
