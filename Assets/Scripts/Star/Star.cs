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
        if (_coroutine == null)
            _coroutine = StartCoroutine(MoveStar());
    }

    private IEnumerator MoveStar()
    {
        while (transform.position != Player.singleton.transform.position)
        {
            transform.DOMove(Player.singleton.transform.position, 0.5f);
            yield return null;
        }
        
        _coroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerProgress>(out PlayerProgress player))
        {
            _generator.OnRelease(this);
            player.AddProgress(_price);
        }
    }
}
