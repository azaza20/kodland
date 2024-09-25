using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dest : MonoBehaviour
{
    [SerializeField] GameObject Main;
    [SerializeField] GameObject Cell;
    public void DestroyObj()
    {
        Destroy(Main);
        Cell.SetActive(true);
        Destroy(Cell, 5f);
    }
}
