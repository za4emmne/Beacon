using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningGenerator : Spawner<LightningWeapon>
{
    [Header("Auto Start")]
    [SerializeField] private bool autoStart = true;
    [SerializeField] private float startDelay = 0.5f;

    [Header("Lightning Specific Settings")]
    [SerializeField] private int maxActiveLightnings = 1;

    [Header("Monitoring")]
    [SerializeField] private int activeCount;

    private List<LightningWeapon> activeLightnings = new List<LightningWeapon>();
    private Transform player;
    private bool hasStarted = false;
    private Coroutine lightningCoroutine;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (autoStart)
        {
            StartCoroutine(AutoStartWithDelay());
        }
    }

    private IEnumerator AutoStartWithDelay()
    {
        while (Player.singleton == null)
        {
            yield return null;
        }

        player = Player.singleton.transform;
        yield return new WaitForSeconds(startDelay);

        if (!hasStarted)
        {
            OnStartGenerator(); // Запускаем свой генератор
            hasStarted = true;
        }
    }

    // ПЕРЕОПРЕДЕЛЯЕМ МЕТОД — не используем базовый Spawn()
    public override void OnStartGenerator()
    {
        // НЕ вызываем base.OnStartGenerator() !!!
        // base.OnStartGenerator(); ← УБРАЛИ ЭТО

        // Запускаем только НАШУ корутину
        if (lightningCoroutine == null)
        {
            lightningCoroutine = StartCoroutine(LightningSpawn());
            Debug.Log("Lightning spawn coroutine started");
        }
    }

    // СОБСТВЕННАЯ КОРУТИНА без обращения к базовому Spawn()
    private IEnumerator LightningSpawn()
    {
        while (true)
        {
            float spawnTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(spawnTime);

            // Создаём молнию только если есть свободный слот
            if (activeLightnings.Count < maxActiveLightnings)
            {
                LightningWeapon obj = GetObject();

                if (obj != null)
                {
                    obj.gameObject.SetActive(true);
                    obj.transform.position = PositionGeneraton();
                }
            }
        }
    }

    // Остановка корутины
    public override void OnStop()
    {
        if (lightningCoroutine != null)
        {
            StopCoroutine(lightningCoroutine);
            lightningCoroutine = null;
        }
    }

    public override LightningWeapon GetObject()
    {
        if (activeLightnings.Count >= maxActiveLightnings)
        {
            return null;
        }

        LightningWeapon lightning = base.GetObject();

        if (lightning != null && player != null)
        {
            lightning.transform.position = player.position;
            lightning.gameObject.SetActive(true);
            lightning.InitGenerator(this);
            lightning.Initialize();

            if (!activeLightnings.Contains(lightning))
            {
                activeLightnings.Add(lightning);
            }

            activeCount = activeLightnings.Count;
        }

        return lightning;
    }

    public override void PutObject(LightningWeapon obj)
    {
        if (activeLightnings.Contains(obj))
        {
            activeLightnings.Remove(obj);
        }

        activeCount = activeLightnings.Count;
        base.PutObject(obj);
    }

    protected override Vector3 PositionGeneraton()
    {
        if (player != null)
        {
            return player.position;
        }
        return transform.position;
    }

    public void UpgradeSpawnRate(int level)
    {
        minDelay /= (1f + level * 0.2f);
        maxDelay /= (1f + level * 0.2f);
        minDelay = Mathf.Max(minDelay, 0.5f);
        maxDelay = Mathf.Max(maxDelay, 1f);
    }

    public void UpgradeMaxLightnings(int additionalCount)
    {
        maxActiveLightnings += additionalCount;
    }

    private void OnDisable()
    {
        OnStop();
    }
}
