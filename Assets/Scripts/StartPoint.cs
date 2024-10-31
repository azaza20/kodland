
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPoint : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    // Start is called before the first frame update
   
    public void StartRace()
    {
        SceneManager.LoadScene(2);

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
