using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
	public static InventoryManager instance = null;
	InventoryInteraction InventoryInteraction;

	private Rect rect;
	public int width;
	public int height;

	public InventoryGrid inventoryGrid {get; private set;}
	private List<ItemInstance> items = new List<ItemInstance>();
	public ItemInstance equippedItem;

	public Action<ItemInstance> OnItemAdded;
	public Action<ItemInstance> OnItemEquipped;
	public Action<ItemInstance> OnItemUnequipped;

	void Awake() {
		if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);

		rect.Set(0,0, width + 1, height + 1);
		inventoryGrid = GetComponent<InventoryGrid>();
	}

	void Start(){
		InventoryInteraction = InventoryInteraction.GetInstance();
	}

	public static InventoryManager GetInstance() {
        return instance;
    }

	public ItemInstance SearchForFirstItemOfType(ItemInstance itemType){
		foreach (ItemInstance item in items) {
			if (item.ItemDetail == itemType.ItemDetail){
				return item;
			}
		}
		return null;
	}

	public void DestroyItemInInventory(ItemInstance item){
		if (items.Contains(item)){
			RemoveItem(item);

			DestroyHandler DestroyHandler = item.GetComponent<DestroyHandler>();
			if (DestroyHandler != null) {
				DestroyHandler.DestroySelf();
			} else {
				GameObject.Destroy(item.gameObject);
			}
		}
	}

	public bool CanAddAtCell(ItemInstance placedItem, Vector2Int coords) {
		//TODO - Remember the previous spot placedItem was in
		Vector2Int origPosition = placedItem.gridPos;
		//If not even with the inventory

		if (!rect.Contains(placedItem.rect.min) || !rect.Contains(placedItem.rect.max)) {
			Debug.Log("Doesn't fit");
			placedItem.gridPos = origPosition;
			return false;
		}

		foreach(ItemInstance item in items) {
			if (item != placedItem && item.Overlaps(placedItem)){
				Debug.Log("Ocerlaps " + item.transform.name);
				return false;
			}
		}
		return true;
	}

	public void RemoveItem(ItemInstance item){
		if (items.Contains(item)){
			Debug.Log(item.transform.name);
			if (item.ItemDetail.equippable){
				UnequipItem(item);
			}
			items.Remove(item);
		}
	}

	public void ToggleCell(Vector2Int cell){
		inventoryGrid.ToggleCell(cell);
	}

	public bool TryPlaceItem(ItemInstance placedItem, Vector2Int coord){
		if (CanAddAtCell(placedItem, coord)){
			items.Add(placedItem);
			Debug.Log("Placed " + placedItem.transform.name);
			placedItem.gridPos = coord;
			OnItemAdded(placedItem);
			return true;
		} else {
			Debug.Log("Couldn't place " + placedItem.transform.name + " at " + coord);
			return false;
		}
	}

	public void TryEquipCell(Vector2Int coord){
		ItemInstance itemAtCell = GetItemAtCell(coord);

		if (itemAtCell != null) {
			ItemDetail ItemDetail = itemAtCell.ItemDetail;
			if (ItemDetail.equippable && !itemAtCell.GetComponent<Equippable>().defunct){
				EquipItem(itemAtCell);
			}
		} else {
			foreach( Vector2 point in items[0].points){
			Debug.Log(point);

			}
			Debug.Log("Nothing to equip at cell + " + coord.x + " : " + coord.y);
		}
	}

	public void EquipItem(ItemInstance item) {
		OnItemEquipped(item);
	}

	public void UnequipItem(ItemInstance item) {
		OnItemUnequipped(item);
	}

	private ItemInstance GetItemAtCell(Vector2Int coord) {
		foreach (ItemInstance item in items ) {
			if (item.points.Contains(coord)){
				return item;
			}
		}
		return null;
	}
}
