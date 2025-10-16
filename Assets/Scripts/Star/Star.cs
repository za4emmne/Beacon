using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Star : MonoBehaviour
{
    private Coroutine _coroutine;
    private StarGenerator _generator;
    private int _price;

    private void Start()
    {
        _price = 1;
    }

    public void Initialize(StarGenerator generator)
    {
        _generator = generator;
    }

    public void Add()
    {
        if (_coroutine == null && this.isActiveAndEnabled)
            _coroutine = StartCoroutine(MoveStar());
    }

    private IEnumerator MoveStar()
    {
        while (transform.position != Player.singleton.transform.position)
        {
            //transform.DOMoveY(transform.position.y + 0.3f, 0.3f)
            //.SetEase(Ease.Linear).From(0).SetLoops(1, LoopType.Yoyo).OnComplete(() =>
            transform.DOMove(Player.singleton.transform.position, 0.5f);

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerLevelManager>(out PlayerLevelManager player))
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            player.AddProgress(_price);
            _generator.PutObject(this);
        }
    }
}
