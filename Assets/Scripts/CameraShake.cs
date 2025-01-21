using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float _shakeFrequency;
    [SerializeField] float _shakeAmplitude;
    [SerializeField] float _shakeTime;

    private CinemachineVirtualCamera _camera;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    private Coroutine _coroutine;

    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineBasicMultiChannelPerlin = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        _cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0;
    }

    public void Shake()
    {
        _coroutine = StartCoroutine(ShakeCamera());
    }

    private IEnumerator ShakeCamera()
    {
        WaitForSeconds wait = new WaitForSeconds(_shakeTime);

        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _shakeAmplitude;
        _cinemachineBasicMultiChannelPerlin.m_FrequencyGain = _shakeFrequency;
        yield return wait;
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        _cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0;

        StopCoroutine(_coroutine);
    }
}
