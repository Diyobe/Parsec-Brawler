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

    public void FinalFeedback(GameObject hitAnimation, Vector3 position)
    {
        cameraZoom.SetTrigger("FinalFeedback");
        StartCoroutine(FinalFeedbackCoroutine(hitAnimation, position));
    }

    private IEnumerator FinalFeedbackCoroutine(GameObject hitAnimation, Vector3 position)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(Instantiate(hitAnimation, position, Quaternion.identity), 5f);
    }

}
