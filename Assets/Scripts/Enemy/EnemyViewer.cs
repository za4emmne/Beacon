 using UnityEngine;

public class EnemyViewer : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Sprite _image;

    private void Start()
    {
        _image = _enemy.Sprite;
    }
}
