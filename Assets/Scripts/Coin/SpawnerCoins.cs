using UnityEngine;

public class SpawnerCoins : Spawner<Coin>
{
    [SerializeField] private AudioClip _addCoin;

    private AudioSource _audioSourse;

    protected override void Awake()
    {
        base.Awake();
        _audioSourse = GetComponent<AudioSource>();
    }

    public override Coin GetObject()
    {
        var coin = base.GetObject();
        coin.Initialize(this);
        coin.gameObject.SetActive(true);

        return coin;
    }

    public override void PutObject(Coin coin)
    {
        _audioSourse.PlayOneShot(_addCoin);
        base.PutObject(coin);
    }
}
