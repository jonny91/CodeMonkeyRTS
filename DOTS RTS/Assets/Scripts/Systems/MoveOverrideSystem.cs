using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct MoveOverrideSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        foreach ((RefRO<LocalTransform> localTransform,
            RefRW<UnitMover> unitMover,
            RefRO<MoveOverride> moveOverride,
            EnabledRefRW<MoveOverride> moveOverrideEnabled
            )
            in SystemAPI.Query<RefRO<LocalTransform>, RefRW<UnitMover>,
            RefRO<MoveOverride>, EnabledRefRW<MoveOverride>>())
        {
 
            if(math.distancesq(localTransform.ValueRO.Position, moveOverride.ValueRO.targetPosition)
                > UnitMoverSystem.REACHED_TARGET_POSITION_DISTANCE_SQ){
                //move closer
                unitMover.ValueRW.targetPosition = moveOverride.ValueRO.targetPosition;
            }
            else
            {
                //reached the move override position
                moveOverrideEnabled.ValueRW = false;
            }

        }

    }
}
