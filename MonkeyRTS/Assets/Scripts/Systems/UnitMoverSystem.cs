using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public partial struct UnitMoverSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (localTransform, moveSpeed, physicsVelocity) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeed>, RefRW<PhysicsVelocity>>())
        {
            var targetPosition = (float3)MouseWorldPosition.Instance.GetPosition();
            var moveDirection = math.normalize(targetPosition - localTransform.ValueRO.Position);
            // localTransform.ValueRW.Rotation = quaternion.LookRotationSafe(moveDirection, math.up());
            var rotationSpeed = 10f;
            localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRW.Rotation,
                quaternion.LookRotationSafe(moveDirection, math.up()), rotationSpeed * SystemAPI.Time.DeltaTime);
            physicsVelocity.ValueRW.Linear = moveDirection * moveSpeed.ValueRO.Value;
            physicsVelocity.ValueRW.Angular = 0;
            // localTransform.ValueRW.Position += moveDirection * moveSpeed.ValueRO.Value * SystemAPI.Time.DeltaTime;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
}