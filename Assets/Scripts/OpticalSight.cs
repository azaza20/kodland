using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OpticalSight : MonoBehaviour
{
    [SerializeField] Camera cameraMain;
    [SerializeField] Camera opticalCamera;
    [SerializeField] Slider slider;
    [SerializeField] GameObject optic;
    float mouse;
    PlayerLook playerLook;

    float mouseMax = 0.5f;
    float maxFOV = 60;

    bool isOptic;
    // Start is called before the first frame update
    void Start()
    {
        mouse = mouseMax;
        isOptic = false;
        playerLook = GetComponent<PlayerLook>();
    }
    public void OpticOnOff()
    {
        if (!isOptic)
        {
            isOptic = true;
        }
        else isOptic = false;
    }
    public void OnScopeChanged(float value)
    {
        playerLook.ChangeMouseSensivity(mouse);
    }
    // Update is called once per frame
    void Update()
    {
        if (isOptic)
        {
            mouse = slider.value / maxFOV * mouseMax;
            opticalCamera.fieldOfView = Mathf.Lerp(opticalCamera.fieldOfView, slider.value, 10 * Time.deltaTime);
            playerLook.ChangeMouseSensivity(mouse);
            cameraMain.enabled = false;
            opticalCamera.enabled = true;
            optic.SetActive(true);
        }
        else
        {
            slider.value = maxFOV;
            mouse = mouseMax;
            cameraMain.enabled = true;
            opticalCamera.enabled = false;
            optic.SetActive(false);
        }
    }
}
