using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    [SerializeField] List<Transform> checkPointPositions; //все точки спавна чекпоинтов
    [SerializeField] Transform carPosition; //текущее положение машины    
    [SerializeField] GameObject checkPointVisual; //префаб ParticleSystem
    GameObject target; //инициированый префаб
    int countCheckPoint; //номер текущего чекпоинта из списка
    // Start is called before the first frame update
    void Start()
    {
        target = Instantiate(checkPointVisual);
        ChangeCheckPoint();
    }
    private void ChangeCheckPoint()
    {
        target.transform.position = checkPointPositions[countCheckPoint].position;
        target.transform.rotation = checkPointPositions[countCheckPoint].rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.transform.position, carPosition.position)<5f)
        {
            if (countCheckPoint < checkPointPositions.Count - 1)
            {
                countCheckPoint += 1;
                ChangeCheckPoint();
            }
            else Destroy(target);
        }
    }
}
