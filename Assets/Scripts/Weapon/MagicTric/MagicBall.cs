using System.Collections;
using UnityEngine;

public class MagicBall : Weapon
{
    [Header("Orbit Settings")]
    [SerializeField] private float orbitRadius = 2f;
    [SerializeField] private float orbitSpeed = 90f; // градусы в секунду
    [SerializeField] private bool clockwise = true;

    [Header("Starting Position")]
    [SerializeField] private float startAngle = 0f;

    private float currentAngle;
    private Coroutine _coroutine;

    private void Update()
    {
    }

    public override void Initialize()
    {
        base.Initialize();

        if (_target == null)
        {
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(MainMoving());
            }
        }
        else
        {
            _coroutine = StartCoroutine(MainMoving());
        }

    }

    private void StopCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private IEnumerator MainMoving()
    {
        while (enabled)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator FirstMoving()
    {
        while (enabled)
        {
            Vector3 offset = new Vector3(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitRadius,
                Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitRadius,
                0f
            );

            // ”станавливаем позицию относительно игрока
            transform.position = player.transform.position + offset;

            // ќбновл€ем угол дл€ следующего кадра
            float direction = clockwise ? -1f : 1f;
            currentAngle += orbitSpeed * direction * Time.deltaTime;

            // Ќормализуем угол в пределах 0-360
            if (currentAngle >= 360f) currentAngle -= 360f;
            if (currentAngle < 0f) currentAngle += 360f;
            yield return null;
        }
    }
}
