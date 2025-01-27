using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private CameraShake _camera;

    private PlayerAnimation _animator;
    private PlayerHealth _health;

    public static Player singleton {  get; private set; }

    private void Awake()
    {
        _health = GetComponent<PlayerHealth>();
        _animator = GetComponent<PlayerAnimation>();
        singleton = this;
    }

    public void TakeDamage(float damage)
    {
        _camera.Shake();
        _animator.OnGetDamageAnimation();
        _health.TakeDamage(damage);
    }
}
