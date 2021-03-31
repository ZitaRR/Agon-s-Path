﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class Dragable : MonoBehaviour, IDragHandler, IDropHandler, IBeginDragHandler
{
    private Transform parent;
    private Image image;
    private Slot slot;
    private GraphicRaycaster raycaster;

    private void Start()
    {
        parent = transform.parent;
        image = GetComponent<Image>();
        slot = GetComponentInParent<Slot>();
        raycaster = GameManager.UI.GetComponent<GraphicRaycaster>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slot.IsEmpty)
            return;

        GameManager.UI.Disable("SlotDescription");
        transform.SetParent(GameManager.UI.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (slot.IsEmpty)
            return;

        image.transform.position = Input.mousePosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        transform.SetParent(parent);
        image.transform.position = parent.position;

        var pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        raycaster.Raycast(pointer, results);

        if (results.Count <= 0)
            return;

        var slot = results[0].gameObject.GetComponentInParent<Slot>();

        if (!slot || slot.Type != Item.ItemType.None && slot.Type != this.slot.Item.Type)
            return;
        else if (!slot.Add(this.slot.Item))
            return;

        this.slot.Remove();
    }
}
