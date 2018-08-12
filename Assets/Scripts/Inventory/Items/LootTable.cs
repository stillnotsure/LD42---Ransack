using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//Keep a list of all the types f difficulty one enemy goes through
[CreateAssetMenu]
public class LootTable : ScriptableObject {

    [SerializeField]
    public List<ItemDropRate> itemDropRates = new List<ItemDropRate>();

    public List<ItemDropRate> GetAvailableItems(int score){
		List<ItemDropRate> availableItems = new List<ItemDropRate>();
        
        foreach (ItemDropRate x in itemDropRates){
            if (score >= x.scoreThreshold){
                availableItems.Add(x);
            }
            else {
                break;
            }
        }
        return availableItems;
    }

}

[Serializable]
public struct ItemDropRate {
    public ItemInstance item;
    public int scoreThreshold;
	public int chances;
}		

