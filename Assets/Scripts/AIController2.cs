using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class AIController2 : AIController
{
    [SerializeField][Range(0, 360)] private float ViewAngle = 90f;
    [SerializeField] private float ViewDistance = 15f;
    [SerializeField] private Transform Target;
    [SerializeField] float attackDistance;
    [SerializeField] int damage;
    [SerializeField] float cooldown;
    [SerializeField] Image healthBar;
    protected float timer;
    protected Animator  animator; 
    protected Rigidbody rb;
    bool dead = false;
    // Start is called before the first frame update
    bool FieldOfView()
    {
        float currentAngle = Vector3.Angle(transform.forward, Target.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Target.position - transform.position, out hit, ViewDistance))
        {
            if (currentAngle < ViewAngle / 2f && Vector3.Distance(transform.position, Target.position) <= ViewDistance && hit.transform == Target.transform)
            {
                return true;
            }
        }
        return false;
    }


    // Update is called once per frame
    public void ChangeHealth(int count)
    {
        float fillPercent = health / 100f;
        healthBar.fillAmount = fillPercent;
        health -= count;
        if (health <= 0)
        {
            dead = true;
            GetComponent<Collider>().enabled = false;
            anim.enabled = true;
            anim.SetBool("Die", true);
        }
    }
    private void MoveToTarget()
    {
        agent.isStopped = false;
        agent.speed = 3.5f;
        agent.SetDestination(Target.position);
    }
    protected override void Update()
    {
        timer += Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(Target.transform.position, agent.transform.position);
        if (FieldOfView())
        {
            if (distanceToPlayer >= 2f)
                MoveToTarget();

            else
            {
                if (timer > cooldown)
            { 
                    timer = 0;
                Target.gameObject.GetComponent<PlayerController>().ChangeHealth(-5);
                    anim.SetBool("idle", true);
                }
                agent.isStopped = true;
               
            }
        }
        else
        {
            agent.isStopped = false;
            base.Update();
        }
        DrawView();
    }

 

    private void DrawView()
    {
        Vector3 left = transform.position + Quaternion.Euler(new Vector3(0, ViewAngle / 2f, 0)) * (transform.forward * ViewDistance);
        Vector3 right = transform.position + Quaternion.Euler(-new Vector3(0, ViewAngle / 2f, 0)) * (transform.forward * ViewDistance);
        Debug.DrawLine(transform.position, left, Color.yellow);
        Debug.DrawLine(transform.position, right, Color.yellow);
    }
}