using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBarrel : MonoBehaviour


{
    [SerializeField] float radius = 5f;
    [SerializeField] GameObject particle;
    // Start is called before the first frame update
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
    }
    public void Boom2()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody rb = colliders[i].attachedRigidbody;
            if (rb)
            {
                rb.AddExplosionForce(100, transform.position, radius);
            }
        }
    }
    


}
