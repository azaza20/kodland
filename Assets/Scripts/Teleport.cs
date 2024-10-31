using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Teleport : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject canvas;
    // Start is called before the first frame update

    public void StartRace()
    {
        SceneManager.LoadScene(1);

    }
    private void OnTriggerEnter(Collider other)
    {
        canvas.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        canvas.SetActive(false);
    }

}
