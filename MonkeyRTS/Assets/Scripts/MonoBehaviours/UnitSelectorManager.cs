using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class UnitSelectorManager : MonoBehaviour
{
    public static UnitSelectorManager Instance { get; private set; }

    public event EventHandler OnSelectionAreaStart;
    public event EventHandler OnSelectionAreaEnd;
    private Vector2 selectionStartMousePosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionStartMousePosition = Input.mousePosition;
            OnSelectionAreaStart?.Invoke(this, EventArgs.Empty);
        }

        if (Input.GetMouseButtonUp(0))
        {
            var selectionEndMousePosition = Input.mousePosition;

            var em = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entityQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<LocalTransform, Unit, Selected>()
                .Build(em);
            var entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < entityArray.Length; i++)
            {
                em.SetComponentEnabled<Selected>(entityArray[i], false);
            }

            entityQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<LocalTransform, Unit>()
                .WithPresent<Selected>()
                .Build(em);
            var selectionAreaRect = GetSelectionAreaRect();
            entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            var localTransformArray = entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
            for (int i = 0; i < localTransformArray.Length; i++)
            {
                var unitLocalTransform = localTransformArray[i];
                var unitScreenPosition = Camera.main.WorldToScreenPoint(unitLocalTransform.Position);
                if (selectionAreaRect.Contains(unitScreenPosition))
                {
                    // unit is inside the selection area
                    em.SetComponentEnabled<Selected>(entityArray[i], true);
                }
            }


            OnSelectionAreaEnd?.Invoke(this, EventArgs.Empty);
        }

        if (Input.GetMouseButtonDown(1))
        {
            var mouseWorldPos = MouseWorldPosition.Instance.GetPosition();
            var em = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMover, Selected>().Build(em);
            var unitMoverArray = entityQuery.ToComponentDataArray<UnitMover>(Allocator.Temp);
            for (int i = 0; i < unitMoverArray.Length; i++)
            {
                var unitMover = unitMoverArray[i];
                unitMover.TargetPosition = mouseWorldPos;
                unitMoverArray[i] = unitMover;
            }

            entityQuery.CopyFromComponentDataArray(unitMoverArray);
        }
    }

    public Rect GetSelectionAreaRect()
    {
        var selectionEndMousePosition = Input.mousePosition;
        var lowerLeftCorner = new Vector2(
            Mathf.Min(selectionStartMousePosition.x, selectionEndMousePosition.x),
            Mathf.Min(selectionStartMousePosition.y, selectionEndMousePosition.y));
        var upperRightCorner = new Vector2(
            Mathf.Max(selectionStartMousePosition.x, selectionEndMousePosition.x),
            Mathf.Max(selectionStartMousePosition.y, selectionEndMousePosition.y));
        return new Rect(
            lowerLeftCorner.x,
            lowerLeftCorner.y,
            upperRightCorner.x - lowerLeftCorner.x,
            upperRightCorner.y - lowerLeftCorner.y
        );
    }
}