using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class BloodTrailEmitter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _minSpeedForTrail = 2f;
    [SerializeField] private float _dropsPerSecond = 8f;
    [SerializeField] private float _trailDuration = 3f;
    [SerializeField, Range(0.1f, 2f)] private float _intensity = 1f;
    [SerializeField] private bool _onlyOnGroundHit = false;

    [Header("Trail Appearance")]
    [SerializeField] private float _sizeMultiplier = 0.4f;
    [SerializeField] private float _positionRandomness = 0.1f;
    [SerializeField] [Range(0f, 180f)] private float _rotationRandomness = 30f;
    [SerializeField] private Vector3 _trailOffset;

    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundLayer = Physics2D.DefaultRaycastLayers;
    [SerializeField] private float _groundCheckDistance = 0.5f;

    [Header("Optimization")]
    [SerializeField] private bool _lowBloodMode = false;
    [SerializeField] private int _maxActiveTrails = 50;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private float _timeSinceLastDrop;
    private bool _isBleeding;
    private float _bleedTimeRemaining;
    private List<GameObject> _activeTrails = new List<GameObject>();
    private Transform _cachedTransform;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _cachedTransform = transform;
        
        // Auto-find collider if not on same object
        if (_collider == null)
        {
            _collider = GetComponentInChildren<Collider2D>();
        }
        
        if (_collider == null)
        {
            Debug.LogWarning($"BloodTrailEmitter on {name} requires a Collider2D component", this);
        }
    }

    private void Update()
    {
        if (!_isBleeding) return;

        _bleedTimeRemaining -= Time.deltaTime;
        if (_bleedTimeRemaining <= 0f)
        {
            StopBleeding();
            return;
        }

        // Check if we should emit based on speed
        float speed = _rigidbody != null ? _rigidbody.linearVelocity.magnitude : 0f;
        if (speed < _minSpeedForTrail) return;

        _timeSinceLastDrop += Time.deltaTime;
        float dropInterval = 1f / (_dropsPerSecond * _intensity);
        
        // Low blood mode doubles the interval
        if (_lowBloodMode) dropInterval *= 2f;
        
        // Check max trails limit
        int maxTrails = _lowBloodMode ? _maxActiveTrails / 2 : _maxActiveTrails;
        if (_activeTrails.Count >= maxTrails) return;

        if (_timeSinceLastDrop >= dropInterval)
        {
            _timeSinceLastDrop = 0f;
            EmitTrailDrop();
        }
    }

    private void EmitTrailDrop()
    {
        if (BloodSplatterManager.Instance == null) return;

        // Base emission point: object's position plus offset
        Vector2 emissionPoint = _cachedTransform.position + _trailOffset;
        Vector3 worldPoint = new Vector3(emissionPoint.x, emissionPoint.y, 0f);

        // Apply position randomness
        worldPoint += (Vector3)Random.insideUnitCircle * _positionRandomness;

        // Ground check if enabled (uses collider if present, otherwise skip)
        if (_onlyOnGroundHit && _collider != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.down, _groundCheckDistance, _groundLayer);
            if (!hit) return; // Don't emit if not hitting ground
            worldPoint = hit.point;
        }

        // Calculate size with randomness
        float minSize = BloodSplatterManager.Instance != null ? BloodSplatterManager.Instance.SplatterSizeMin : 0.3f;
        float maxSize = BloodSplatterManager.Instance != null ? BloodSplatterManager.Instance.SplatterSizeMax : 0.8f;
        float baseSize = Mathf.Lerp(minSize, maxSize, 0.5f);
        float size = baseSize * _sizeMultiplier * Random.Range(0.8f, 1.2f);

        // Calculate rotation with randomness
        float rotation = Random.Range(0f, 360f) + Random.Range(-_rotationRandomness, _rotationRandomness);

        // Create the trail splatter
        GameObject trailObj = BloodSplatterManager.Instance.CreateTrailSplatter(worldPoint, 
                                                                                Vector2.up, // Direction doesn't matter much for trails
                                                                                size);
        if (trailObj != null)
        {
            _activeTrails.Add(trailObj);
            
            // Setup auto-removal when object is destroyed
            Destroy destroyComponent = trailObj.AddComponent<Destroy>();
            destroyComponent.OnDestroyed += () => _activeTrails.Remove(trailObj);
        }
    }

    private Vector2 GetRandomPointInCollider()
    {
        if (_collider == null) return Vector2.zero;

        if (_collider is BoxCollider2D box)
        {
            Vector2 size = box.size;
            Vector2 offset = box.offset;
            return new Vector2(
                Random.Range(-size.x/2, size.x/2) + offset.x,
                Random.Range(-size.y/2, size.y/2) + offset.y
            );
        }
        else if (_collider is CircleCollider2D circle)
        {
            float radius = circle.radius;
            Vector2 offset = circle.offset;
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float dist = Random.Range(0f, radius);
            return offset + new Vector2(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist);
        }
        else if (_collider is PolygonCollider2D polygon)
        {
            // Simple approach: return a random point from the first path
            if (polygon.pathCount > 0 && polygon.GetPath(0).Length > 0)
            {
                Vector2[] points = polygon.GetPath(0);
                int index = Random.Range(0, points.Length);
                return points[index] + polygon.offset;
            }
            return Vector2.zero;
        }
        
        // Fallback to collider bounds
        Bounds bounds = _collider.bounds;
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        ) - (Vector2)_collider.offset;
    }

    /// <summary>
    /// Start the blood bleeding effect
    /// </summary>
    /// <param name="duration">How long the bleeding should last (seconds)</param>
    /// <param name="intensity">Intensity multiplier (0-2+)</param>
    public void StartBleeding(float duration, float intensity = 1f)
    {
        _isBleeding = true;
        _bleedTimeRemaining = duration;
        _intensity = Mathf.Clamp(intensity, 0.1f, 3f);
        _timeSinceLastDrop = 0f;
    }

    /// <summary>
    /// Stop the blood bleeding effect
    /// </summary>
    public void StopBleeding()
    {
        _isBleeding = false;
        _bleedTimeRemaining = 0f;
    }

    /// <summary>
    /// Check if currently bleeding
    /// </summary>
    public bool IsBleeding => _isBleeding;

    /// <summary>
    /// Get remaining bleed time
    /// </summary>
    public float BleedTimeRemaining => _bleedTimeRemaining;

    /// <summary>
    /// Clear all active trail effects from this emitter
    /// </summary>
    public void ClearActiveTrails()
    {
        foreach (GameObject trail in _activeTrails)
        {
            if (trail != null)
            {
                Destroy(trail);
            }
        }
        _activeTrails.Clear();
    }

    private void OnDisable()
    {
        ClearActiveTrails();
    }

    // Simple destroy callback component
    private class Destroy : MonoBehaviour
    {
        public System.Action OnDestroyed;
        
        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
        }
    }
}