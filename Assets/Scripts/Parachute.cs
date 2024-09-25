using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Parachute : MonoBehaviour
{
    bool deployed; // сработал ли парашут
    Animator animator;
    Rigidbody rb;
    [SerializeField] float airResistance;
    [SerializeField] float deploymentHeight;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent <Animator>();
        rb = GetComponent <Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit; //объект, в который попал луч
        Ray ray = new Ray(transform.position, Vector3.down); // направление луча вниз
        Debug.DrawRay(transform.position, -transform.up, Color.red); // рисуем луч для проверки
        if (!deployed) //если парашут не раскрывался
        {
            if (Physics.Raycast(ray, out hit, deploymentHeight)) // если луч задел коллайдер на расстоянии deploymentHeight
            {
            OpenParachute(); // открываем парашют
                
            }
        }
    }
    void OpenParachute() 
    {
        deployed = true;
        rb.drag = airResistance;
        animator.SetTrigger("open"); // включаем анимацию раскрытия парашюта
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        animator.SetTrigger("close"); // включим анимацию закрытия парашюта
    }
}


