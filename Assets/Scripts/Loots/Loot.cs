using UnityEngine;
using System.Collections;

public class Loot : MonoBehaviour
{
    private SpawnerCoins _spawnerCoins;
    private PillsGenerator _spawnerPills;
    private PlayerHealth _playerHealth;
    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private Coroutine _coroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _transform = transform;
    }

    public void Initialized(SpawnerCoins spawnerCoins, PillsGenerator spawnerPills)
    {
        _spawnerCoins = spawnerCoins;
        _spawnerPills = spawnerPills;

        if (Player.singleton != null)
            _playerHealth = Player.singleton.GetComponent<PlayerHealth>();

    }

    public void Disapear()
    {
        _coroutine = StartCoroutine(IAnimateHit());
    }

    private IEnumerator IAnimateHit()
    {
        for (int i = 0; i < 4; i++)
        {
            //_spriteRenderer.color = Color.red;
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(.1f);

            _spriteRenderer.color = _originalColor;
            yield return new WaitForSeconds(.1f);
        }

        if (_playerHealth != null)
        {
            if (_playerHealth.Current > _playerHealth.MaxCurrent / 2)
            {
                var loot = _spawnerCoins.GetObject();

                if (loot != null)
                    loot.transform.position = _transform.position;
            }
            else
            {
                var loot = _spawnerPills.GetObject();

                if (loot != null)
                    loot.transform.position = _transform.position;
            }

        }
        gameObject.SetActive(false);
    }
}
