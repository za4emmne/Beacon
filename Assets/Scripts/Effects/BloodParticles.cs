using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ParticleSystem))]
public class BloodParticles : MonoBehaviour
{
    [Header("Hit Effect Settings")]
    [SerializeField] private int _hitParticleCount = 30;
    [SerializeField] private float _hitParticleLifetime = 0.4f;
    [SerializeField] private float _hitParticleSize = 0.08f;
    [SerializeField] private Color _hitColor = Color.red;
    [SerializeField] private float _hitSpeedMin = 2f;
    [SerializeField] private float _hitSpeedMax = 4f;

    [Header("Death Effect Settings")]
    [SerializeField] private int _deathParticleCount = 50;
    [SerializeField] private float _deathParticleLifetime = 0.6f;
    [SerializeField] private float _deathParticleSize = 0.12f;
    [SerializeField] private Color _deathColor = new Color(0.8f, 0f, 0f);
    [SerializeField] private float _deathSpeedMin = 3f;
    [SerializeField] private float _deathSpeedMax = 6f;

    [Header("Trail Settings")]
    [SerializeField] private bool _leaveTrail = true;
    [SerializeField] private float _trailInterval = 0.05f;

    private ParticleSystem _particleSystem;
    private List<BloodParticle> _customParticles = new List<BloodParticle>();
    private float _timeSinceLastTrail;

    private class BloodParticle
    {
        public Vector3 position;
        public Vector3 velocity;
        public float lifetime;
        public float age;
        public float size;
        public Color color;
        public bool isHit;
    }

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        if (_particleSystem == null)
            _particleSystem = gameObject.AddComponent<ParticleSystem>();

        ConfigureParticleSystem();
    }

    private void ConfigureParticleSystem()
    {
        var main = _particleSystem.main;
        main.startSpeed = 0;
        main.startLifetime = 0.5f;
        main.startSize = 0.1f;
        main.startColor = Color.red;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = _particleSystem.emission;
        emission.rateOverTime = 0;

        var shape = _particleSystem.shape;
        shape.enabled = false;

        var renderer = _particleSystem.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.sortMode = ParticleSystemSortMode.Distance;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        _timeSinceLastTrail += deltaTime;

        for (int i = _customParticles.Count - 1; i >= 0; i--)
        {
            BloodParticle p = _customParticles[i];
            p.age += deltaTime;

            if (p.age >= p.lifetime)
            {
                _customParticles.RemoveAt(i);
                continue;
            }

            p.velocity.y -= 15f * deltaTime;
            Vector3 newPos = p.position + p.velocity * deltaTime;

            if (_leaveTrail && _timeSinceLastTrail >= _trailInterval && newPos.y <= p.position.y)
            {
                _timeSinceLastTrail = 0;
                if (BloodSplatterManager.Instance != null)
                {
                    BloodSplatterManager.Instance.SpawnSplatter(newPos, p.velocity.normalized);
                }
            }

            p.position = newPos;
        }

        if (_customParticles.Count == 0 && _particleSystem.isPlaying == false)
        {
            Destroy(gameObject, 0.1f);
        }
    }

    public void PlayHitEffect(Vector3 position, Vector3 direction)
    {
        for (int i = 0; i < _hitParticleCount; i++)
        {
            BloodParticle p = new BloodParticle();
            p.position = position;
            p.lifetime = _hitParticleLifetime;
            p.age = 0;
            p.size = _hitParticleSize * Random.Range(0.8f, 1.2f);
            p.color = _hitColor;
            p.isHit = true;

            float speed = Random.Range(_hitSpeedMin, _hitSpeedMax);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += Random.Range(-45f, 45f);
            float rad = angle * Mathf.Deg2Rad;

            p.velocity = new Vector3(Mathf.Cos(rad) * speed, Mathf.Sin(rad) * speed, 0);

            _customParticles.Add(p);
        }

        if (_leaveTrail && BloodSplatterManager.Instance != null)
        {
            BloodSplatterManager.Instance.SpawnSplatter(position, direction);
        }
    }

    public void PlayDeathEffect(Vector3 position)
    {
        for (int i = 0; i < _deathParticleCount; i++)
        {
            BloodParticle p = new BloodParticle();
            p.position = position;
            p.lifetime = _deathParticleLifetime;
            p.age = 0;
            p.size = _deathParticleSize * Random.Range(0.8f, 1.2f);
            p.color = _deathColor;
            p.isHit = false;

            float speed = Random.Range(_deathSpeedMin, _deathSpeedMax);
            float angle = Random.Range(0f, 360f);
            float rad = angle * Mathf.Deg2Rad;

            p.velocity = new Vector3(Mathf.Cos(rad) * speed, Mathf.Sin(rad) * speed, 0);

            _customParticles.Add(p);
        }

        if (_leaveTrail && BloodSplatterManager.Instance != null)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 offset = Random.insideUnitCircle * 0.3f;
                BloodSplatterManager.Instance.SpawnSplatterAtDeath(position + offset);
            }
        }
    }

    public static void CreateBloodBurst(Vector3 position, Vector3 direction, bool isDeath = false)
    {
        GameObject bloodObj = new GameObject("BloodBurst");
        bloodObj.transform.position = position;

        BloodParticles particles = bloodObj.AddComponent<BloodParticles>();
        
        if (isDeath)
            particles.PlayDeathEffect(position);
        else
            particles.PlayHitEffect(position, direction);
    }
}
