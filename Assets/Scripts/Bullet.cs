using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10;
    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    void FixedUpdate()
    {
        transform.position += direction * speed * Time.deltaTime;
        speed += 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().ChangeHealth(-20);
        }
        if (other.tag == "RedBarrel")
        {
            other.GetComponent<RedBarrel>().Boom();
        }
        if (other.CompareTag("enemy_ragdoll"))
        {
            
            other.gameObject.GetComponentInParent<Enemy>().Death(true);

            other.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * 100, ForceMode.Impulse);
        }
        Destroy(gameObject);
        
    }
}