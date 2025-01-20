using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Cam_Manager : MonoBehaviour
{
    public static Cam_Manager instance;

    [SerializeField] CinemachineVirtualCamera virtualCameraOne;
    [SerializeField] CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        noise = virtualCameraOne.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public IEnumerator ShakeCamera(float shakeTime = .3f, float shakeAmplitud = 2.5f)
    {
        noise.m_AmplitudeGain = shakeAmplitud;
        yield return new WaitForSeconds(shakeTime);
        noise.m_AmplitudeGain = 0;
    }
}
