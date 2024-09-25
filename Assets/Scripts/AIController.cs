using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIController : MonoBehaviour
{
     protected NavMeshAgent agent;
    [SerializeField] List<Transform>  points = new();
    protected Animator anim;
    bool isPanic;
    [SerializeField] protected int health;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetDestination();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    virtual protected void Update()
    {
       
        if(agent.remainingDistance < .25f)
        {
            StartCoroutine("Idle");
        }    

    }
    public void SetDestination()
    {
        Vector3 NewTarget = points[Random.Range(0, points.Count)].position;
        agent.SetDestination(NewTarget);

    }

    IEnumerator Idle()
    {
        agent.speed = 0;
        SetDestination();
        anim.SetBool("idle", true);
        yield return new WaitForSeconds(Random.Range(5f, 10f) );
        agent.speed = 3.5f;
        anim.SetBool("idle", false);

    }
    IEnumerator Panic1()
    {
        isPanic = true;
        agent.speed = 5f;
        SetDestination();
        anim.SetBool("Panic", true);
        yield return new WaitForSeconds(Random.Range(5f, 10f));
        agent.speed = 3.5f;
        anim.SetBool("Panic", false);
        isPanic = false;

    }
    public void Panic()
    {
      
        if(!isPanic)
        {
            StartCoroutine("Panic1");
        }
     
    }
}
