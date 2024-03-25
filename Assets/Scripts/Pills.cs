using UnityEngine;

public class Pills : MonoBehaviour
{
    [SerializeField] private int _countPills;

    private int _minCountPills = 5;
    private int _maxCountPills = 30;


    private void Start()
    {
        _countPills = Random.Range(_minCountPills, _maxCountPills);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.GetComponent<CharactersHealth>().TakeHealth(_countPills);
            Destroy(this.gameObject);
        }
    }

}
