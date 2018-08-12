using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Equippable : MonoBehaviour {
    public Color defunctColor;
    EquipManager EquipManager;
    public bool usesAmmo;
    public int uses;
    public bool defunct;

    PlayerInput PlayerInput;
    public bool equipped;

    public Action<Vector2, Vector2> UsedEquip;
    public Action ReleasedEquip;

    void Start(){
        EquipManager = EquipManager.GetInstance();
        PlayerInput = PlayerInput.GetInstance();
        PlayerInput.UsedEquip += HandleUse; 
        PlayerInput.ReleasedTrigger += ReleasedTrigger; 
    }

    void HandleUse(Vector2 dir, Vector2 pos){        
        if (equipped){
            if (UsedEquip != null){
                UsedEquip(dir, pos);
            }
        }
    }

    void ReleasedTrigger(){

        if (equipped){
            if (ReleasedEquip != null){
                ReleasedEquip();
            }
        }
    }

    void BeEquipped(){
        equipped = true;
    }

    void BeUnequipped(){
        equipped = false;
    }

    public void BeUsed(){
        uses--;
        if (uses < 0){
            SetAsDefunct();
        }
    }

    void SetAsDefunct(){
        defunct = true;
        ReleasedTrigger();
        EquipManager.UnequipItem(GetComponent<ItemInstance>());
        GetComponent<Image>().color = defunctColor;
    }

}