using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Star : MonoBehaviour
{
    private Coroutine _coroutine;
    private StarsSpawner _generator;
    private int _price;

    private void Start()
    {
        _price = 1;
    }

    public void Initialize(StarsSpawner generator)
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
        Vector2 bounceDir = Random.insideUnitCircle.normalized * 1f;
        Vector3 bounceTarget = transform.position + new Vector3(bounceDir.x, bounceDir.y, 0);

        // Отскок
        yield return transform.DOMove(bounceTarget, 0.2f).SetEase(Ease.InCubic).WaitForCompletion();
        yield return new WaitForSeconds(0.1f);

        // Магнит: всегда летим к актуальной позиции игрока
        while (Vector3.Distance(transform.position, Player.singleton.transform.position) > 0.1f)
        {
            transform.DOMove(Player.singleton.transform.position, 0.1f).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(0.01f); // Короткая анимация и пауза
        }

        _coroutine = null;
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
