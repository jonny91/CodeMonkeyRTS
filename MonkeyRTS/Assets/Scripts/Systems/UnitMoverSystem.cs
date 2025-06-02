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
        var unitMoverJob = new UnitMoverJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };
        unitMoverJob.ScheduleParallel();
        /*foreach (var (localTransform, unitMover, physicsVelocity) in
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
        }*/
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
}

[BurstCompile]
public partial struct UnitMoverJob : IJobEntity
{
    public float deltaTime;

    public void Execute(ref LocalTransform localTransform, in UnitMover unitMover, ref PhysicsVelocity physicsVelocity)
    {
        var targetPosition = unitMover.TargetPosition;
        var moveDirection = targetPosition - localTransform.Position;
        // 到达这个距离的平方 就认为单位到了指定位置
        var reachedTargetDistanceSq = 2f;
        if (math.lengthsq(moveDirection) < reachedTargetDistanceSq)
        {
            physicsVelocity.Linear = 0;
            physicsVelocity.Angular = 0;
            return;
        }

        moveDirection = math.normalize(targetPosition - localTransform.Position);
        localTransform.Rotation = math.slerp(localTransform.Rotation,
            quaternion.LookRotationSafe(moveDirection, math.up()),
            unitMover.RotationSpeed * deltaTime);
        physicsVelocity.Linear = moveDirection * unitMover.MoveSpeed;
        physicsVelocity.Angular = 0;
    }
}