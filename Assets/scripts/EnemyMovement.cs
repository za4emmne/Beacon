using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    private const string AnimationNameRun = "Run";

    [SerializeField] private float _speed = 0.1f;

    private Animator _animator;
    private Player _player;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            _player.transform.position, _speed * Time.deltaTime);
        AnimationRun();
    }

    private void AnimationRun()
    {
        _animator.SetTrigger(AnimationNameRun);
    }
}