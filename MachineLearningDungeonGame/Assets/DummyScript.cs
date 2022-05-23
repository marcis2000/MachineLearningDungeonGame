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
    private float disolveSpeed = 1.5f;

    public event EventHandler addToCount;

    private void Start()
    {
        _animator = this.GetComponent<Animator>();
        dummyCapsuleCollider = this.GetComponent<CapsuleCollider>();
        dummyGFX = this.transform.GetChild(1);
        renderer = dummyGFX.GetComponent<SkinnedMeshRenderer>();
        renderer.enabled = true;
        hitCount = 0;
        dissolveAmount =  -1.85f;
    }

    private void FixedUpdate()
    {
        if (isDestroyed)
        {
            if (dissolveAmount < 0.5f)
            {
                dissolveAmount = dissolveAmount + Time.deltaTime * disolveSpeed;
                dissolveMaterial.SetFloat("Dissolve_intensity", dissolveAmount);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
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
                    addToCount?.Invoke(this, EventArgs.Empty);
                    isDestroyed = true;

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

}