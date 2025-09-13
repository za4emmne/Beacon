using UnityEngine;
using Leopotam.EcsLite;

public class EcsStartup : MonoBehaviour
{
    private EcsWorld _world;
    private IEcsSystems _systems;
    private int _playerEntity;

    [SerializeField] private Rigidbody2D _rigidbodyPlayer;
    [SerializeField] private float playerSpeed = 3f;

    private void Start()
    {
        _world = new EcsWorld();
        //_systems = new EcsSystems(_world)
        //    .Add()
    }
}
