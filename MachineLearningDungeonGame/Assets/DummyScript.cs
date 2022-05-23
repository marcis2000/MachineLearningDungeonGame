using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    [SerializeField] Material dissolveMaterial;
    private Animator _animator;
    private CapsuleCollider dummyCapsuleCollider;
    private BoxCollider dummyBoxCollider;
    private int hitCount;
    private bool isDestroyed = false;
    private float dissolveAmount;

    private Transform dummyGFX;
    private SkinnedMeshRenderer renderer;
    [SerializeField] private float disolveSpeed = 0.001f;

    public event EventHandler addToCount;

    private void Start()
    {
        _animator = this.GetComponent<Animator>();
        dummyCapsuleCollider = this.GetComponent<CapsuleCollider>();
        dummyGFX = this.transform.GetChild(1);
        renderer = dummyGFX.GetComponent<SkinnedMeshRenderer>();
        renderer.enabled = true;
        hitCount = 0;
        dissolveAmount = -1.25f;
        Debug.Log(dummyGFX);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isDestroyed)
        {
            Vector3 collisionToDummy = (collision.transform.position - this.transform.position).normalized;
            if (collision.collider.name == "SwordPolyart" && Vector3.Dot(this.transform.forward, collisionToDummy) >= 0)
            {
                hitCount++;
                if (hitCount < 3)
                {
                    _animator.SetBool("Pushed", true);
                    StartCoroutine(PushedCoroutine());
                }
                else
                {
                    renderer.material = dissolveMaterial;               
                    //_animator.SetBool("Destroyed", true);
                    StartCoroutine(DeadCoroutine());
                    isDestroyed = true;
                    addToCount?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
    IEnumerator PushedCoroutine()
    {
        //animation is read only
        yield return new WaitForSeconds(0.1f);
        _animator.SetBool("Pushed", false);

    }
    IEnumerator DeadCoroutine()
    {
        yield return new WaitForSeconds(2f);
        //this.gameObject.SetActive(false);
        _animator.SetBool("Dead", false);
        if(dissolveAmount < 1) 
        {
            dissolveAmount = dissolveAmount + Time.deltaTime * disolveSpeed;
            dissolveMaterial.SetFloat("Dissolve intensity", dissolveAmount);
        }

    }
}