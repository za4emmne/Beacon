using UnityEngine;
using System.Collections;
using System;

public class VampireSkill : MonoBehaviour
{
    [SerializeField] private float _vampireDamageIndex = 1;
    [SerializeField] private CharactersHealth _health;
    //[SerializeField] private InputManager _inputManager;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private GameObject _vampireArea;

    public event Action IsReady;

    private Collider2D[] hitEnemy;
    //private CharactersHealth _enemyHealth;
    private int _vampireSkillDelay = 2;
    private Coroutine _coroutine;
    private bool _isCoroutineStart;
    private int _vampireSkillCount = 6;
    //private int _currentEnemy = 0;

    private void OnEnable()
    {
        IsReady += OnVampiredInput;
    }

    private void OnDisable()
    {
        IsReady -= OnVampiredInput;
    }
    private void Start()
    {
        _vampireArea.SetActive(false);
        _isCoroutineStart = false;
        //_enemyHealth = hitEnemy[_currentEnemy].GetComponent<CharactersHealth>();
    }

    private void Update()
    {
        ActivateVampireSkillHelp();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    private void ActivateVampireSkillHelp()
    {
        hitEnemy = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);

        if (hitEnemy.Length != 0)
        {
            _vampireArea.SetActive(true);

            if (Input.GetKey(KeyCode.Z) && _isCoroutineStart == false)
                IsReady?.Invoke();
            else
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);

                _isCoroutineStart = false;
            }
        }
        else
            _vampireArea.SetActive(false);
    }

    private void OnVampiredInput()
    {
        _coroutine = StartCoroutine(Vampired(hitEnemy[0]));

    }

    private IEnumerator Vampired(Collider2D enemy)
    {
        var waitForAnySecond = new WaitForSeconds(_vampireSkillDelay);
        _isCoroutineStart = true;

        for (int i = 0; i < _vampireSkillCount; i++)
        {
            enemy.GetComponent<CharactersHealth>().TakeDamage(_vampireDamageIndex);
            _health.TakePills(_vampireDamageIndex);
            yield return waitForAnySecond;
        }
    }
}