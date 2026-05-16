using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Text _statusText;
    [SerializeField] private Slider _progressSlider;
    
    [Header("Animation Settings")]
    [SerializeField] private float _fadeInDuration = 0.3f;
    [SerializeField] private float _fadeOutDuration = 0.5f;
    [SerializeField] private Ease _fadeOutEase = Ease.InQuad;

    private CanvasGroup _canvasGroupInstance;

    private void Awake()
    {
        if (_canvasGroup != null)
            _canvasGroupInstance = _canvasGroup;
        else
            _canvasGroupInstance = GetComponent<CanvasGroup>();
        
        if (_canvasGroupInstance != null)
            _canvasGroupInstance.alpha = 0f;
    }

    public void Show(float fadeDuration = -1f)
    {
        if (_canvasGroupInstance == null) return;
        
        float duration = fadeDuration > 0 ? fadeDuration : _fadeInDuration;
        
        gameObject.SetActive(true);
        _canvasGroupInstance.blocksRaycasts = true;
        _canvasGroupInstance.DOFade(1f, duration);
        
        SetProgress(0f, "Загрузка...");
    }


    public void Hide(float fadeDuration = -1f, System.Action onComplete = null)
    {
        if (_canvasGroupInstance == null)
        {
            gameObject.SetActive(false);
            onComplete?.Invoke();
            return;
        }
        
        float duration = fadeDuration > 0 ? fadeDuration : _fadeOutDuration;
        
        _canvasGroupInstance.blocksRaycasts = false;
        _canvasGroupInstance.DOFade(0f, duration)
            .SetEase(_fadeOutEase)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            });
    }

    public void SetProgress(float value, string status = "")
    {
        if (_progressSlider != null)
            _progressSlider.value = Mathf.Clamp01(value);
        
        if (!string.IsNullOrEmpty(status) && _statusText != null)
            _statusText.text = status;
    }

    public void SetStatus(string status)
    {
        if (_statusText != null)
            _statusText.text = status;
    }
}