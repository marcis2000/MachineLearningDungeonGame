using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    Cinemachine.CinemachineImpulseSource source;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        source = GetComponent < Cinemachine.CinemachineImpulseSource>();

        source.GenerateImpulse(Camera.main.transform.forward);
    }
}
