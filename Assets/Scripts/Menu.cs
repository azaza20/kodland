using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject options;
    [SerializeField] GameObject menu;
    windows myWindows;
    [SerializeField] GameObject pos1;
    [SerializeField] GameObject pos2;
    [SerializeField] GameObject startpos;
    GameObject camera;
    bool isoptions;
    enum windows { options, menu }
    // Start is called before the first frame update
    void Start()
    {
        myWindows = windows.menu;
        changeWindows();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    private void Update()
    {
        if (!isoptions)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, pos1.transform.position, 0.05f);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation,pos1.transform.rotation, 0.05f);
        }
        else
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, pos2.transform.position, 0.05f);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, pos2.transform.rotation, 0.05f);
        }
    }
    // Update is called once per frame
    public void openOptions()
    {
        myWindows = windows.options;
        changeWindows();
        isoptions = true;
    }
    public void closeOptions()
    {
        myWindows = windows.menu;
        changeWindows();
        isoptions = false;
    }
    void changeWindows()
    {
        switch (myWindows)
        {
            case windows.options:
                options.SetActive(true);
                menu.SetActive(false);
                break;

            case windows.menu:
                options.SetActive(false);
                menu.SetActive(true);
                break;
        }
    }
    public void openGame()
    {
        SceneManager.LoadScene(1);
    }
    public void closeGame()
    {
        Application.Quit();
    }
}
