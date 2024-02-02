using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text _healthText;
    [SerializeField] private PlayerChangeHealth _player;
    [SerializeField] private int _pill;
    [SerializeField] private int _damage;

    private void Update()
    {
        _healthText.text = _player.GetHealth().ToString() + "/" + _player.GetMaxHealth().ToString();
    }

    public void AddHP()
    {
        _player.TakeHealth(_pill);
    }

    public void TakeDamage()
    {
        _player.TakeDamage(_damage);
    }
}
