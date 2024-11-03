using UnityEngine;
using System.Collections;
using System;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;

public class VampireSkill : MonoBehaviour
{
    [SerializeField] private float _vampireDamageIndex = 1;
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private GameObject _vampireArea;
    [SerializeField] private Text _vampireHelp;

    public event Action IsReady;
    public event Action EndVampired;

    private CharactersHealth _enemy;
    private Collider2D[] hitEnemy;
    private Coroutine _coroutineVampired;
    private int _delay = 1;
    private int _vampireSkillCount = 6;
    private WaitForSeconds _vampireSkillBusy;

    private void Awake()
    {
        _vampireSkillBusy = new WaitForSeconds(_delay);
    }
    private void Start()
    {
        _vampireArea.SetActive(false);
        _vampireHelp.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        IsReady += OnVampiredButtomInput;
        EndVampired += OnVampireEnd;
    }

    private void OnDisable()
    {
        IsReady -= OnVampiredButtomInput;
        EndVampired -= OnVampireEnd;
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
            _vampireHelp.gameObject.SetActive(true);

            if (Input.GetKey(KeyCode.Z) && (_coroutineVampired == null))
                OnVampiredButtomInput();
        }
        else
            OnVampireEnd();
    }

    private void OnVampireEnd()
    {
        if (_coroutineVampired != null)
            StopCoroutine(_coroutineVampired);

        _vampireHelp.text = "Нажмите   Z, чтобы активировать вампиризм";
        _coroutineVampired = null;
        _vampireHelp.gameObject.SetActive(false);
        _vampireArea.SetActive(false);
    }

    private void OnVampiredButtomInput()
    {
        _vampireArea.SetActive(true);
        _enemy = hitEnemy[0].GetComponent<CharactersHealth>();
        _coroutineVampired = StartCoroutine(Vampired(_enemy));
    }

    private IEnumerator Vampired(CharactersHealth enemy)
    {
        int timer = _vampireSkillCount + 1;

        for (int i = 0; i < _vampireSkillCount; i++)
        {
            enemy.TakeDamage(_vampireDamageIndex);
            timer -= _delay;
            _vampireHelp.text = timer.ToString();
            _health.TakePills(_vampireDamageIndex);
            yield return _vampireSkillBusy;
        }

        EndVampired?.Invoke();
    }
}