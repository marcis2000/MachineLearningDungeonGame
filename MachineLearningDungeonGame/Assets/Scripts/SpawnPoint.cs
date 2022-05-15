using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnPoint : MonoBehaviour
{
    public Vector3 rotation;
    public bool isInUse;

    public void Awake()
    {
        rotation = transform.rotation.eulerAngles;
    }
}