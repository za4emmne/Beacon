using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BloodDecal : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _lifetime = 15f;
    [SerializeField] private float _fadeStartTime = 12f;
    [SerializeField] private float _fadeDuration = 3f;
    [SerializeField] private float _spawnDuration = 0.3f;

    private SpriteRenderer _spriteRenderer;
    private float _timeAlive;
    private bool _isFading;
    private Vector3 _originalScale;

    public System.Action OnDecalExpired;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    public void Initialize(float lifetime, float fadeStart, Sprite sprite = null)
    {
        _lifetime = lifetime;
        _fadeStartTime = Mathf.Clamp(fadeStart, 0, _lifetime - 1f);
        _fadeDuration = _lifetime - _fadeStartTime;
        _timeAlive = 0f;
        _isFading = false;

        if (sprite != null && _spriteRenderer != null)
            _spriteRenderer.sprite = sprite;
    }

    private void Update()
    {
        _timeAlive += Time.deltaTime;

        if (_timeAlive < _spawnDuration)
        {
            float t = _timeAlive / _spawnDuration;
            transform.localScale = Vector3.Lerp(Vector3.zero, _originalScale, t);
        }
        else if (!_isFading && _timeAlive >= _fadeStartTime)
        {
            _isFading = true;
        }
        else if (_isFading)
        {
            float fadeProgress = (_timeAlive - _fadeStartTime) / _fadeDuration;
            fadeProgress = Mathf.Clamp01(fadeProgress);

            float scale = Mathf.Lerp(_originalScale.x, 0f, fadeProgress);
            transform.localScale = new Vector3(scale, scale, 1f);

            if (_spriteRenderer != null)
            {
                Color color = _spriteRenderer.color;
                color.a = Mathf.Lerp(1f, 0f, fadeProgress);
                _spriteRenderer.color = color;
            }
        }

        if (_timeAlive >= _lifetime)
        {
            Expire();
        }
    }

    private void Expire()
    {
        OnDecalExpired?.Invoke();
        Destroy(gameObject);
    }

    public void SetSprite(Sprite sprite)
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (_spriteRenderer != null)
            _spriteRenderer.sprite = sprite;
    }

    public void SetColor(Color color)
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (_spriteRenderer != null)
            _spriteRenderer.color = color;
    }

    public void SetSize(float size)
    {
        _originalScale = new Vector3(size, size, 1f);
    }

    public void SetRotation(float rotation)
    {
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
