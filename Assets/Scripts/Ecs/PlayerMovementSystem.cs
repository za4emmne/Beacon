using Leopotam.EcsLite;
using UnityEngine;

public class PlayerMovementSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world
            .Filter<PlayerTag>()
            .Inc<PlayerInputComponent>()
            .Inc<PlayerSpeedComponent>()
            .Inc<Rigidbody2DReference>()
            .End();

        var inputPool = world.GetPool<PlayerInputComponent>();
        var speedPool = world.GetPool<PlayerSpeedComponent>();
        var rbPool = world.GetPool<Rigidbody2DReference>();
    }
}
