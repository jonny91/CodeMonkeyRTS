using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class UnitSelectorManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var mouseWorldPos = MouseWorldPosition.Instance.GetPosition();
            var em = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMover>().Build(em);
            // var entities = entityQuery.ToEntityArray(Allocator.Temp);
            var unitMoverArray = entityQuery.ToComponentDataArray<UnitMover>(Allocator.Temp);
            for (int i = 0; i < unitMoverArray.Length; i++)
            {
                var unitMover = unitMoverArray[i];
                unitMover.TargetPosition = mouseWorldPos;
                // em.SetComponentData(entities[i], unitMover);
                unitMoverArray[i] = unitMover;
            }
            entityQuery.CopyFromComponentDataArray(unitMoverArray);
        }
    }
}