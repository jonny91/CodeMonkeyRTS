using System;
using UnityEngine;

public class UnitSelectionManagerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform selectionAreaRectTransform;

    [SerializeField]
    private Canvas canvas;

    private void Start()
    {
        UnitSelectorManager.Instance.OnSelectionAreaStart += UnitSelectionManager_OnSelectionAreaStart;
        UnitSelectorManager.Instance.OnSelectionAreaEnd += UnitSelectionManager_OnSelectionAreaEnd;

        selectionAreaRectTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (selectionAreaRectTransform.gameObject.activeSelf)
        {
            UpdateVisual();
        }
    }

    private void UnitSelectionManager_OnSelectionAreaStart(object sender, EventArgs e)
    {
        selectionAreaRectTransform.gameObject.SetActive(true);

        UpdateVisual();
    }

    private void UnitSelectionManager_OnSelectionAreaEnd(object sender, EventArgs e)
    {
        selectionAreaRectTransform.gameObject.SetActive(false);
    }

    private void UpdateVisual()
    {
        var selectionAreaRect = UnitSelectorManager.Instance.GetSelectionAreaRect();
        var canvasScale = canvas.transform.localScale.x;
        selectionAreaRectTransform.anchoredPosition =
            new Vector2(selectionAreaRect.position.x, selectionAreaRect.position.y) / canvasScale;
        selectionAreaRectTransform.sizeDelta =
            new Vector2(selectionAreaRect.width, selectionAreaRect.height) / canvasScale;
    }
}