using UnityEngine;

public class Pills : MonoBehaviour
{
    private PillsGenerator _generator; //изменить на максимальный запас от здоровья
    private float _countPills;
    private float _minCountPills = 15;
    private float _maxCountPills = 20;

    public float Count => _countPills;

    public void Initialize(PillsGenerator generator, float maxPlayerHealth)
    {
        _generator = generator;
        _countPills = Random.Range(maxPlayerHealth / 100 * _minCountPills, maxPlayerHealth / 100 * _maxCountPills);

    }

    public void PutPills()
    {
        _generator.PutObject(this);
    }
}
