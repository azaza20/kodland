using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{   
    [SerializeField] float radius = 5f;
    [SerializeField] GameObject particle;
    [SerializeField] AudioSource boomSound;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Boom", 3f);
    }
    
    public void Boom()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            player.ChangeHealth(-80);
        }
        GameObject boom = Instantiate(particle);
        boom.transform.position = transform.position;
        Boom2();
        Destroy(boom, 1);
        Destroy(gameObject);
        boomSound.Play();
    }
    public void Boom2()
    {
        Collider[] d = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < d.Length; i++)
        {
           Dest dest = d[i].gameObject.GetComponentInParent<Dest>(); 
            if (dest)
            {
                dest.DestroyObj();
                Debug.Log("boom");
            }
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("enemy_ragdoll"))
            {
                colliders[i].GetComponentInParent<Enemy>().Death(true); 
                Rigidbody rb = colliders[i].attachedRigidbody;
                if (rb)
                {
                    rb.AddExplosionForce(1000, transform.position, radius);
                }
            }
            else
            {
                Rigidbody rb = colliders[i].attachedRigidbody;
                if (rb)
                {
                    rb.AddExplosionForce(500, transform.position, radius);
                }
            }
        }
            
    }



}
