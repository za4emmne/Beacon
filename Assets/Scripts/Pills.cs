using UnityEngine;

public class Pills : MonoBehaviour
{
    [SerializeField] private int _countPills;

    private int _minCountPills = 5;
    private int _maxCountPills = 30;

    public int Count => _countPills;

    private void Start()
    {
        _countPills = Random.Range(_minCountPills, _maxCountPills);
    }
}
