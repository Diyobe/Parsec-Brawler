using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField]
    ParticleSystem hitSpeedline;
    [SerializeField]
    Animator backgroundFlash;
    [SerializeField]
    Animator cameraZoom;

    // =====================================
    // J'ai un peu mal
    public static FeedbackManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 60;
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    // =====================================

    public void BackgroundFlash()
    {
        backgroundFlash.SetTrigger("Feedback");
    }

    public void HitSpeedline()
    {
        hitSpeedline.Play();
    }

    public void CameraZoomDeSesMorts()
    {
        cameraZoom.SetTrigger("Feedback");
    }

}
