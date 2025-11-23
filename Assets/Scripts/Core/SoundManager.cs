using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour, ISoundManager
{
    [SerializeField] private AudioSource _effectSource;
    [SerializeField] private AudioClip _buttonSound;

    public void PlayClick()
    {
        if (_buttonSound != null)
            _effectSource.PlayOneShot(_buttonSound);
    }
}
