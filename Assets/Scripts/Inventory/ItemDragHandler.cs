using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler {
    public InventoryInteraction InventoryInteraction;
    private AudioHost AudioHost;
    private ItemInstance ItemInstance;

    public Vector2 positionAtStart;
    public Transform parentAtStart;
    public Vector3 scaleAtStart;

    public Transform inventoryHolder;
    public Transform platform;

    public Vector2 lastSuccesfulPlace;

    public AudioClip grabSound;
    public AudioClip dropSound;

    void Start(){
        InventoryInteraction = InventoryInteraction.GetInstance();
        
        ItemInstance = GetComponent<ItemInstance>();
        inventoryHolder = InventoryInteraction.inventoryHolder;
        platform = InventoryInteraction.platform;
        AudioHost = AudioHost.GetInstance();
    }


    public void OnDrag(PointerEventData eventData) {
        //Todo - Add mouse smoothing and inertia
        if (!ItemInstance.beingDragged){
            SetStartVars();
            ItemInstance.gameObject.GetComponent<Image>().raycastTarget = false;
            ItemInstance.beingDragged = true;
            InventoryInteraction.DragItem(ItemInstance);
            AudioHost._audio.PlayClip(AudioHost.grabItemSound);
        }
        InventoryInteraction.ContinueDrag(ItemInstance, Input.mousePosition);
        transform.position = Input.mousePosition;

        float heightOFfset = ItemInstance.GetComponent<RectTransform>().rect.height / 2;
        float widthOFfset = ItemInstance.GetComponent<RectTransform>().rect.width / 2;
        Vector3 adjustedMousePos = Input.mousePosition;
        adjustedMousePos.y -= heightOFfset;
        adjustedMousePos.x -= widthOFfset;
        ItemInstance.GetNewGridPos(InventoryInteraction.getGridPosFromScreen(adjustedMousePos));
    }

    void SetStartVars(){
        positionAtStart = transform.position;
        parentAtStart = transform.parent;
        scaleAtStart = transform.localScale;
    }

    void ReturnToStartVars(){
        transform.position = positionAtStart;
        transform.SetParent(parentAtStart);
        transform.localScale = scaleAtStart;
    }


    public void OnEndDrag(PointerEventData eventData) {
        ItemInstance.beingDragged = false;
        InventoryInteraction.StopDragging();
        ItemInstance.gameObject.GetComponent<Image>().raycastTarget = true;
        AudioHost._audio.PlayClip(AudioHost.dropItemSound);
        
        float heightOFfset = ItemInstance.GetComponent<RectTransform>().rect.height / 2;
        float widthOFfset = ItemInstance.GetComponent<RectTransform>().rect.width / 2;
        Vector3 adjustedMousePos = Input.mousePosition;
        adjustedMousePos.y -= heightOFfset;
        adjustedMousePos.x -= widthOFfset;
        ItemInstance.GetNewGridPos(InventoryInteraction.getGridPosFromScreen(adjustedMousePos));
        
        //If not being placed back in a bag
        if ( !(parentAtStart == inventoryHolder && gameObject.transform.parent == platform)){
            if (!InventoryInteraction.TryPlaceItem(ItemInstance, InventoryInteraction.getGridPosFromScreen(eventData.position))){
                ReturnToStartVars();
                if (lastSuccesfulPlace != null) {
                    ItemInstance.GetNewGridPos(InventoryInteraction.getGridPosFromScreen(lastSuccesfulPlace));
                    InventoryInteraction.TryPlaceItem(ItemInstance, InventoryInteraction.getGridPosFromScreen(lastSuccesfulPlace));
                }
            } else {
                lastSuccesfulPlace = eventData.position;
            }
        } 

    }

}