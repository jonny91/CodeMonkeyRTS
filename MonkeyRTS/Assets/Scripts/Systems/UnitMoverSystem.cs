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
        foreach (var (localTransform, unitMover, physicsVelocity) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<UnitMover>, RefRW<PhysicsVelocity>>())
        {
            var targetPosition = unitMover.ValueRO.TargetPosition;
            var moveDirection = math.normalize(targetPosition - localTransform.ValueRO.Position);
            // localTransform.ValueRW.Rotation = quaternion.LookRotationSafe(moveDirection, math.up());
            localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRW.Rotation,
                quaternion.LookRotationSafe(moveDirection, math.up()),
                unitMover.ValueRO.RotationSpeed * SystemAPI.Time.DeltaTime);
            physicsVelocity.ValueRW.Linear = moveDirection * unitMover.ValueRO.MoveSpeed;
            physicsVelocity.ValueRW.Angular = 0;
            // localTransform.ValueRW.Position += moveDirection * moveSpeed.ValueRO.Value * SystemAPI.Time.DeltaTime;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
}