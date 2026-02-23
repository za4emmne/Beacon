using UnityEngine;

public class DeviceDetector : MonoBehaviour
{
    public static DeviceDetector Instance { get; private set; }

    [Header("Editor/Test Settings")]
    [Tooltip("¬ редакторе принудительно считать устройство мобильным.")]
    [SerializeField] private bool forceMobileInEditor = false;

    [Tooltip("‘лаг дл€ интеграции с YG2/другим SDK (симулируем мобильное устройство).")]
    [SerializeField] private bool simulatedMobileFromSdk = false;

    public bool IsMobile { get; private set; }
    public bool IsEditor { get; private set; }
    public RuntimePlatform Platform { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DetectDevice();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void DetectDevice()
    {
        Platform = Application.platform;
        IsEditor = Application.isEditor;

#if UNITY_ANDROID || UNITY_IOS
        IsMobile = true;
#elif UNITY_WEBGL
        IsMobile = Application.isMobilePlatform;
#else
        IsMobile = false;
#endif
        // ¬ редакторе разрешаем принудительно включать режим мобилки
#if UNITY_EDITOR
        if (forceMobileInEditor)
        {
            IsMobile = true;
        }
#endif
        // ‘лаг, который ты можешь выставл€ть из YG2
        // (например, если в симул€торе выбрана Mobile)
        if (simulatedMobileFromSdk)
        {
            IsMobile = true;
        }

        IsMobile = IsMobile;

        Debug.Log($"[DeviceDetector] Platform: {Platform}, IsMobile: {IsMobile}, IsEditor: {IsEditor}, " +
                  $"forceMobileInEditor: {forceMobileInEditor}, simulatedMobileFromSdk: {simulatedMobileFromSdk}");
    }
    public static void SetSimulatedMobile(bool isMobile)
    {
        if (Instance == null) return;

        Instance.simulatedMobileFromSdk = isMobile;
        Instance.DetectDevice();
    }

    public static bool IsTouchDevice()
    {
        // ¬ редакторе дл€ теста можно вернуть true, если forceMobileInEditor или simulatedMobileFromSdk.
        if (Instance != null)
        {
#if UNITY_EDITOR
            if (Instance.forceMobileInEditor || Instance.simulatedMobileFromSdk)
                return true;
#endif
        }

        return Input.touchSupported;
    }

    public static bool IsWebGL()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
}
