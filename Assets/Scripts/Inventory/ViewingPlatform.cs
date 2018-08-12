using UnityEngine;
using UnityEngine.EventSystems;

public class ViewingPlatform : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    InventoryInteraction InventoryInteraction;
    public Transform itemsHolder;
    public FloorLoot currentFloorloot;

    void Start(){
        InventoryInteraction = InventoryInteraction.GetInstance();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (InventoryInteraction.draggedItem != null){
            Debug.Log("Dragged item = " + InventoryInteraction.draggedItem);
            GameObject itemObject = InventoryInteraction.draggedItem.gameObject;
            currentFloorloot.itemObjects.Add(itemObject);
            itemObject.transform.SetParent(transform);
            itemObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (InventoryInteraction.draggedItem != null){
            GameObject itemObject = InventoryInteraction.draggedItem.gameObject;
            itemObject.transform.SetParent(itemsHolder);
            itemObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

}