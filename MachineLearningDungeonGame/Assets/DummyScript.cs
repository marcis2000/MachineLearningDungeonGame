using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    private Animator _animator;
    private CapsuleCollider dummyCapsuleCollider;
    private BoxCollider dummyBoxCollider;
    private int hitCount;
    private bool isDestroyed = false;

    public event EventHandler addToCount;

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        dummyCapsuleCollider = this.GetComponent<CapsuleCollider>();
        hitCount = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isDestroyed)
        {
            Debug.Log(collision.collider.name);
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

                    _animator.SetBool("Destroyed", true);
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
        this.gameObject.SetActive(false);
        _animator.SetBool("Dead", false);

    }
}