using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryInteraction : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerDownHandler {
	public static InventoryInteraction instance = null;
	public InventoryGrid grid;
	public InventoryManager manager;

	public ItemInstance draggedItem;

	public Transform platform;
	public Transform inventoryHolder;

	void Awake() {
		if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);
	}

	public static InventoryInteraction GetInstance() {
        return instance;
    }
	
	public void OnPointerDown(PointerEventData eventData) {
		Vector2Int gridPos = getGridPosFromScreen(eventData.position);
		if (eventData.button == PointerEventData.InputButton.Right){
			manager.TryEquipCell(gridPos);
		}
	}

	//Triggered from ItemDragHandler
	public void ContinueDrag(ItemInstance item, Vector2 mousePos){
		bool cellsUnavailable = manager.CanAddAtCell(item, item.gridPos);
		grid.SelectItemOutline(draggedItem, cellsUnavailable);
	}

	public void OnBeginDrag(PointerEventData eventData) {
		
	}

	public void OnEndDrag(PointerEventData eventData) {
		
	}

	// public void OnPointerEnter(PointerEventData eventData){
    //     if (draggedItem != null){
    //        
    //     }
    // }

	public Vector2Int getGridPosFromScreen(Vector2 screenPos){
		Vector2 position;

		RectTransformUtility.ScreenPointToLocalPointInRectangle( grid.rectTransform, screenPos, null, out position);
		position.x += grid.rectTransform.sizeDelta.x / 2;
		position.y += grid.rectTransform.sizeDelta.y / 2;

		int convertedX = Mathf.FloorToInt(position.x / grid.resolution);
		int convertedY = Mathf.FloorToInt(position.y / grid.resolution);
		return new Vector2Int(convertedX, convertedY);
	}

	// public void ClickedOnGridPos(Vector2Int gridPos){
	// 	manager.ToggleCell(gridPos);
	// }

	public bool TryPlaceItem(ItemInstance item, Vector2Int cell){
		return manager.TryPlaceItem(item, cell);
	}

	public void DragItem(ItemInstance draggedItem){
		//manager.RemoveItem(draggedItem);
		this.draggedItem = draggedItem;
	}

	public void StopDragging(){
		this.draggedItem.beingDragged = false;
		this.draggedItem = null;
		grid.ClearSelectedCells();
	}

}
