using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float _shakeFrequency = 1f;
    [SerializeField] private float _shakeAmplitude = 1f;
    [SerializeField] private float _shakeTime = 0.2f;

    private CinemachineCamera _camera;
    private CinemachineBasicMultiChannelPerlin _noise;
    private Coroutine _coroutine;

    private void Awake()
    {
        _camera = GetComponent<CinemachineCamera>();

        // В Cinemachine 3 GetCinemachineComponent принимает Stage, а не generic-параметр
        if (_camera != null)
        {
            _noise = _camera.GetCinemachineComponent(CinemachineCore.Stage.Noise)
                     as CinemachineBasicMultiChannelPerlin;
        }

        if (_noise == null)
            Debug.LogError("[CameraShake] Noise component (CinemachineBasicMultiChannelPerlin) not found");
    }

    private void Start()
    {
        if (_noise == null) return;
        _noise.AmplitudeGain = 0f;
        _noise.FrequencyGain = 0f;
    }

    public void Shake()
    {
        if (_noise == null) return;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ShakeCamera());
    }

    private IEnumerator ShakeCamera()
    {
        var wait = new WaitForSeconds(_shakeTime);

        _noise.AmplitudeGain = _shakeAmplitude;
        _noise.FrequencyGain = _shakeFrequency;

        yield return wait;

        _noise.AmplitudeGain = 0f;
        _noise.FrequencyGain = 0f;
        _coroutine = null;
    }
}