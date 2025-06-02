using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct SelectedVisualSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var selected in SystemAPI.Query<RefRO<Selected>>().WithDisabled<Selected>())
        {
            var visualLocalTrans = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualEntity);
            visualLocalTrans.ValueRW.Scale = 0f;
        }

        foreach (var selected in SystemAPI.Query<RefRO<Selected>>())
        {
            var visualLocalTrans = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualEntity);
            visualLocalTrans.ValueRW.Scale = selected.ValueRO.showScale;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
}