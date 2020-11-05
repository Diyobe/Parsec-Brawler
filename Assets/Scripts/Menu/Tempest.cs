using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tempest : MonoBehaviour
{
    [SerializeField]
    Material newSkybox;
    public void EventTempest()
    {
        RenderSettings.skybox = newSkybox;
    }
}
