using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EquipManager : MonoBehaviour { 
    public static EquipManager instance = null;

    public Equippable currentEquippedItem;
    InventoryManager InventoryManager;
    AudioHost AudioHost;

    void Awake() {
		if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);
    }

    public static EquipManager GetInstance() {
        return instance;
    }

    void Start(){
        InventoryManager = InventoryManager.GetInstance();
        AudioHost = AudioHost.GetInstance();

        InventoryManager.OnItemEquipped += HandleItemEquipped;
        InventoryManager.OnItemUnequipped += HandleItemUnequipped;
    }

    void HandleItemEquipped(ItemInstance item) {
        if (currentEquippedItem != null){
            currentEquippedItem.equipped = false;
            if (!currentEquippedItem.GetComponent<Equippable>().defunct){
                currentEquippedItem.gameObject.GetComponent<FadeInOut>().Stop();
            }
        }

        currentEquippedItem = item.GetComponent<Equippable>();
        currentEquippedItem.equipped = true;
        currentEquippedItem.gameObject.GetComponent<FadeInOut>().Begin();
        AudioHost._audio.PlayClip(AudioHost.equipItemSound);
    }

    void HandleItemUnequipped(ItemInstance item){
        if (currentEquippedItem == item.GetComponent<Equippable>()){

            currentEquippedItem.equipped = false;
            currentEquippedItem.gameObject.GetComponent<FadeInOut>().Stop();
        }
    }

    public void UnequipItem(ItemInstance item){
        InventoryManager.UnequipItem(item);
    }

}
