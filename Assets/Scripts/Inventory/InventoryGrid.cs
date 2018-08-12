using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGrid : MonoBehaviour {

	public int resolution = 32;

    public RectTransform rectTransform {get; private set;}

	public Sprite spriteOpen;
	public Sprite spriteSelected;
	public Sprite spriteFull;

	public Transform cellsHolder;
	public Transform itemsHolder;

	public InventoryManager inventoryManager;
	public Image[,] imageGrid;

	void Start(){
		rectTransform = GetComponent<RectTransform>();

		inventoryManager.OnItemAdded += ItemAdded;
		CreateGrid();
	}

	void CreateGrid(){
		int width = inventoryManager.width;
		int height = inventoryManager.height;
		imageGrid = new Image[width, height];

		for (int x = 0; x < width; x++){
			for (int y = 0; y < height; y++){
				Image img = new GameObject("InventoryCell " + x + " : " + y).AddComponent<Image>();
				img.transform.SetParent(cellsHolder);

				img.sprite = spriteOpen;
				img.transform.localScale = Vector3.one;

				img.rectTransform.anchorMin = new Vector2(0,0);
				img.rectTransform.anchorMax = new Vector2(0,0);

				img.rectTransform.anchoredPosition = new Vector3(x * resolution + (resolution/2), y * resolution + (resolution/2), 0);

				img.rectTransform.sizeDelta = new Vector2(resolution, resolution);

				imageGrid[x,y] = img;
			}
		}
	}

	Image GetImageAtCell(Vector2Int cell){
		return imageGrid[cell.x,cell.y];
	}

	public void ToggleCell(Vector2Int cell){
		Image img = GetImageAtCell(cell);
		
		//This should be checking against manager rather than the sprite itself
		if (img.sprite == spriteOpen){
			img.sprite = spriteSelected;
		} else {
			img.sprite = spriteOpen;
		}
	}

	public void ClearSelectedCells(){
		int width = inventoryManager.width;
		int height = inventoryManager.height;
		for (int x = 0; x < width; x++){
			for (int y = 0; y < height; y++){
				imageGrid[x,y].sprite = spriteOpen;
			}
		}
	}

	//Gets availablity from management, just uses item to get the outline
	public void SelectItemOutline(ItemInstance item, bool available){
		ClearSelectedCells();
		foreach(Vector2Int point in item.points){
			if (point.x >= 0 && point.y >= 0 && point.x < inventoryManager.width && point.y < inventoryManager.height){
				//Get the grid value from the point
				int x = (point.x);
				int y = (point.y);

				if (available){
					imageGrid[x,y].sprite = spriteSelected;
				} else {
					imageGrid[x,y].sprite = spriteFull;
				}
			}
		}
	}

	private void ItemAdded(ItemInstance item){
		RecenterDroppedItem(item);
	}

	public void RecenterDroppedItem(ItemInstance item) {
		//Debug.Log("X: " + item.gridPos.x + ", Y: " + item.gridPos.y);
		item.transform.SetParent(itemsHolder);

		item.GetComponent<Image>().rectTransform.anchorMin = new Vector2(0,0);
		item.GetComponent<Image>().rectTransform.anchorMax = new Vector2(0,0);
		
		int multipliedX = (16 * item.ItemDetail.width);
		int multipliedY = (16 * item.ItemDetail.height);

		item.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(  multipliedX + (item.gridPos.x * 32), multipliedY + (item.gridPos.y * 32));
		
		// Vector2 itemPositionRelative = transform.InverseTransformPoint(item.transform.position);
		// Debug.Log(itemPositionRelative);

		// int dividedX = (int)(itemPositionRelative.x / 32) * 10;
		// int dividedy = (int)(itemPositionRelative.x / 32) * 10;
		// Vector2 roundedPos = new Vector2(dividedX, dividedy);
		// Vector2 roundedPosNonRelative = transform.TransformPoint(roundedPos);

		// item.transform.position = roundedPosNonRelative;

	}
	

}
