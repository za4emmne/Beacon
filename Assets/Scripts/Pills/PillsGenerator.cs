using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillsGenerator : Spawner<Pills>
{
    [SerializeField] private AudioClip _pillsAudio;
    [SerializeField] private ParticleSystem _hillEffect;

    private PlayerHealth _player;
    private AudioSource _audioSource;

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Init(ParticleSystem effect, PlayerHealth player)
    {
        _hillEffect = effect;
        _player = player;
    }

    //private void OnEnable()
    //{
    //    _player = Player.singleton?.GetComponent<PlayerHealth>();

    //    if (_player != null)
    //        _player.CriticalHealth += OneSpawn;
    //}

    //private void OnDisable()
    //{
    //    if (_player != null)
    //        _player.CriticalHealth -= OneSpawn;
    //}

    public override Pills GetObject()
    {
        var pill = base.GetObject();
        pill.Initialize(this, _player.MaxCurrent);
        pill.gameObject.SetActive(true);

        return pill;
    }

    public override void PutObject(Pills obj)
    {
        _hillEffect.Play();
        _audioSource.PlayOneShot(_pillsAudio);
        base.PutObject(obj);
    }

    //public override void OnStartGenerator()
    //{
    //    base.OnStartGenerator();
    //}
}
