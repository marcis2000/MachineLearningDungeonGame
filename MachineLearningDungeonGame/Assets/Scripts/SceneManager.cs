using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    public List<SpawnPoint> dummySpawnPoints;
    public GameObject dummy;
    public int requiredDummies;
    private int randomVal;
    private Vector3 position, rotation;
    System.Random rnd = new System.Random();
    private void Awake()
    {
        if (requiredDummies > dummySpawnPoints.Count) return;
        for(int i = 0; i<requiredDummies; i++)
        {
            randomVal = rnd.Next(0, dummySpawnPoints.Count);
            rotation = dummySpawnPoints[randomVal].GetComponent<SpawnPoint>().rotation;
            Instantiate(dummy, dummySpawnPoints[randomVal].transform.position, Quaternion.Euler(rotation));
            dummySpawnPoints.RemoveAt(randomVal);
        }
        Destroy(dummy);
    }
    private void Update()
    {
        
    }

}
