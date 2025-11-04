using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Coroutine _coroutine;
    private SpawnerCoins _spawner;
    private int _amount;

    private void Start()
    {
        _amount = 1;
    }

    public void Initialize(SpawnerCoins generator)
    {
        _spawner = generator;
    }

    public void Add()
    {
        if (_coroutine == null && this.isActiveAndEnabled)
            _coroutine = StartCoroutine(MoveCoin());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            GameDataManager.Instance.AddCoins(_amount);
            _spawner.PutObject(this);
        }
    }

    private IEnumerator MoveCoin()
    {
        while (transform.position != Player.singleton.transform.position)
        {
            //transform.DOMoveY(transform.position.y + 0.3f, 0.3f)
            //.SetEase(Ease.Linear).From(0).SetLoops(1, LoopType.Yoyo).OnComplete(() =>
            transform.DOMove(Player.singleton.transform.position, 0.5f);

            yield return null;
        }
    }
}
