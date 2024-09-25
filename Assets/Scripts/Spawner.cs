using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject Prefab;
    [SerializeField] float radius;
    [SerializeField] float cooldown;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(test());
        for (int i = 0; i < 10; i+=2)
        {
            print(i);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    timer += Time.deltaTime;
    //    if (timer > cooldown)
    //    {
    //        timer = 0;
    //        spawn();
    //    }

    //}
    void spawn()
    {

       GameObject obj =  Instantiate(Prefab);
        float x = Random.Range  (- radius, radius);
        float z = Random.Range  (- radius, radius);
        obj.transform.position = new Vector3(x, 0, z);
    }
    IEnumerator test()
    {
        while (true)
            
        {
            print("через 1 секунду");
            yield return new WaitForSeconds(1);
            spawn();
            yield return new WaitForSeconds(cooldown);
        }
        for (int i = 0; i < 10; i++)
        {
            print(i);
        }
    }
}
