using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxTest : MonoBehaviour

{
    public Material boxMaterial;
    // Start is called before the first frame update
  void OnEnable()
    {
        Dialog._color += ChangeLight;
    }
    void OnDisable()
    {
        Dialog._color -= ChangeLight;
    }
    // Update is called once per frame
    void ChangeLight()
    {
        boxMaterial.EnableKeyword("_EMISSION");
    }
}
