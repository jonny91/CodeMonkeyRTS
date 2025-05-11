using Unity.Entities;
using UnityEngine;

public class MoveSpeedAuthoring : MonoBehaviour
{
    public float Value;

    class Baker : Baker<MoveSpeedAuthoring>
    {
        public override void Bake(MoveSpeedAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MoveSpeed { Value = authoring.Value });
        }
    }
}

public struct MoveSpeed : IComponentData
{
    public float Value;
}