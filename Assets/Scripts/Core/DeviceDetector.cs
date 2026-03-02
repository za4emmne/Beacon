using UnityEngine;

public class DeviceDetector : MonoBehaviour
{
    public static DeviceDetector Instance { get; private set; }

    [Header("Editor/Test Settings")]
    [Tooltip("� ��������� ������������� ������� ���������� ���������.")]
    [SerializeField] private bool forceMobileInEditor = false;

    [Tooltip("���� ��� ���������� � YG2/������ SDK (���������� ��������� ����������).")]
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
        IsMobile = Application.isMobilePlatform || Input.touchSupported;
#else
        IsMobile = false;
#endif
        // � ��������� ��������� ������������� �������� ����� �������
#if UNITY_EDITOR
        if (forceMobileInEditor)
        {
            IsMobile = true;
        }
#endif
        // ����, ������� �� ������ ���������� �� YG2
        // (��������, ���� � ���������� ������� Mobile)
        if (simulatedMobileFromSdk)
        {
            IsMobile = true;
        }

        Debug.Log($"[DeviceDetector] Platform: {Platform}, IsMobile: {IsMobile}, IsEditor: {IsEditor}, " +
                  $"forceMobileInEditor: {forceMobileInEditor}, simulatedMobileFromSdk: {simulatedMobileFromSdk}, " +
                  $"touchSupported: {Input.touchSupported}");
    }
    public static void SetSimulatedMobile(bool isMobile)
    {
        if (Instance == null) return;

        Instance.simulatedMobileFromSdk = isMobile;
        Instance.DetectDevice();
    }

    public static bool IsTouchDevice()
    {
        // � ��������� ��� ����� ����� ������� true, ���� forceMobileInEditor ��� simulatedMobileFromSdk.
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
