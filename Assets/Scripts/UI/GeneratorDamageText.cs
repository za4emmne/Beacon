using UnityEngine;

public class GeneratorDamageText : Spawner<DamageText>
{
    private static GeneratorDamageText _instance;

    protected override void Awake()
    {
        base.Awake();

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        FillPool(30);
    }

    public static GeneratorDamageText Instance => _instance;

    private void FillPool(int size)
    {
        while (pool.Count < size)
        {
            if (_objects.Length == 0 || _objects[0] == null)
            {
                Debug.LogError("DamageText prefab not assigned!");
                return;
            }

            DamageText text = Instantiate(_objects[0], transform);
            PutObject(text);
            text.InitGenerator(this);
        }
    }

    public void ShowDamageText(Vector3 position, float damage)
    {
        if (pool.Count == 0)
        {
            Debug.LogError("pool damageTxt is empty");
            return;
        }

        var damageText = base.GetObject();
        damageText.transform.position = position + Vector3.up * 0.3f;
        damageText.gameObject.SetActive(true);
        damageText.SetDamage(damage);
    }
}
