using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;


public class Enemy : MonoBehaviour
{
    Animator anim;
    Rigidbody[] childrenRb;
    [SerializeField] protected int health = 100;
    void Start()
    {
        anim = GetComponent<Animator>();
        childrenRb = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in childrenRb)
        {
            rb.isKinematic = true;
            rb.tag = "enemy_ragdoll";
        }
    }
    public void ChangeHealth(int count)
    {
        float fillPercent = health / 100f;
        health -= count;
        if (health <= 0)
        {

            Death(true);
        }
    }
    public virtual void Death(bool gravity)
    {
        foreach (Rigidbody rb in childrenRb)
        {
            rb.isKinematic = false;
            rb.useGravity = gravity;
        }
        anim.enabled = false;
    }
    public void OffTelekinesis()
    {
        foreach (Rigidbody rb in childrenRb)
        {
            rb.useGravity = true;
        }
    }
}