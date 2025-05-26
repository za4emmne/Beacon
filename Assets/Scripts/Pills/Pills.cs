using UnityEngine;

public class Pills : MonoBehaviour
{
    private PillsGenerator _generator;
    private int _countPills;
    private int _minCountPills = 1;
    private int _maxCountPills = 5;

    public int Count => _countPills;

    public void Initialize(PillsGenerator generator)
    {
        _generator = generator;
        _countPills = Random.Range(_minCountPills, _maxCountPills);
    }

    public void PutPills()
    {
        _generator.PutObject(this);
    }
}
