using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerScript : MonoBehaviour
{
    public List<GameObject> dummies = new List<GameObject>();
    public int dummiesCount;
    // Start is called before the first frame update
    void Start()
    {
        dummiesCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToCount()
    {
        dummiesCount++;
        if (dummiesCount >= 2)
        {
            Debug.Log("You won");
        }
    }
}
