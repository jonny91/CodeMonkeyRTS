using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public partial struct TestingSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        /*var count = 0;
        foreach (var (localTransform,
                     unitMover,
                     physicsVelocity) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<UnitMover>, RefRW<PhysicsVelocity>>()
                     .WithDisabled<Selected>())
        {
            count++;
        }

        Debug.Log($"entity count: {count}");*/
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
}