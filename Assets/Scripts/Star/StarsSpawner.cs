using UnityEngine;

public class StarsSpawner : Spawner<Star>
{
    [SerializeField] private EnemiesGenerator _enemyManager;
    [SerializeField] private AudioClip _addStar;

    private AudioSource _audioSourse;

    protected override void Awake()
    {
        base.Awake();
        _audioSourse = GetComponent<AudioSource>();
    }

    public override Star GetObject()
    {
        var star = base.GetObject();
        star.Initialize(this);
        star.gameObject.SetActive(true);

        return star;
    }

    public override void PutObject(Star star)
    {
        _audioSourse.PlayOneShot(_addStar);
        base.PutObject(star);
    }
}
