using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour {
	[SerializeField]
    public LootTable LootTable;
	public GameObject lootBagPrefab;

	public static LootManager instance = null;
	ScoreManager ScoreManager;
	public EnemyManager EnemyManager;

	public float baseDropRate;
	public float currentDropRate;
	public AnimationCurve dropRateOverScore;


	void Awake(){
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);
    }

	void Start(){
		ScoreManager = ScoreManager.GetInstance();
		Debug.Log(ScoreManager);
		EnemyManager.enemyKilledAction += MaybeDropLootAtPosition;
	}

	public void MaybeDropLootAtPosition(Vector2 pos) {
		int r = UnityEngine.Random.Range(0, 100);

		if (r < currentDropRate){
			DropLootAtPosition(pos);
		}
	}
	
	public void DropLootAtPosition (Vector2 pos) {
		GameObject lootBag = Instantiate(lootBagPrefab, pos, Quaternion.identity);
		List<ItemInstance> itemsInBag = GetItemsInBag();

		lootBag.GetComponent<FloorLoot>().items = itemsInBag;
	}

	List<ItemInstance> GetItemsInBag(){
		int score = ScoreManager.score;

		List<ItemDropRate> availableItems = LootTable.GetAvailableItems(score);
		List<ItemInstance> itemsInBag = new List<ItemInstance>();

		int numberOfItems = ChooseNumberOfItems();
		for (int i = 0; i < numberOfItems; i++){
			itemsInBag.Add(PickRandomItem(availableItems));
		}

		return itemsInBag;
	}

	ItemInstance PickRandomItem(List<ItemDropRate> availableItems){
		int totalChance = 0;
		foreach(ItemDropRate itemRate in availableItems){
			totalChance += itemRate.chances;
		}

		int r = UnityEngine.Random.Range(0, totalChance);
		int cumulativeChance = 0;
		foreach(ItemDropRate itemRate in availableItems){
			cumulativeChance += itemRate.chances;
			if (r <= cumulativeChance){
				return itemRate.item;
			}
		}
		return null;
	}

	int ChooseNumberOfItems(){
		int r = UnityEngine.Random.Range(0, 5);

		if (r >= 0 && r < 1){
			return 1;
		} else if (r >= 1 && r < 3){
			return 2;
		} else if (r >= 3 && r < 4){
			return 3;
		}

		return 2;
	}
	
}
