using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FloorLoot : MonoBehaviour {

	ReferenceManager ReferenceManager;
	private ViewingPlatform platform;

	float timer;

	public bool permanent;
	int timeBeforeDespawn = 6;
	float timeBeforeFlashing = 4;
	bool flashing;
	bool unattended = true;
	
	public List<ItemInstance> items = new List<ItemInstance>();
	public List<GameObject> itemObjects = new List<GameObject>();
	bool shownContents;

	public Action bagWasOpened;

	// Use this for initialization
	void Start () {
		ReferenceManager = ReferenceManager.GetInstance();
		platform = ReferenceManager.platform.GetComponent<ViewingPlatform>();
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			unattended = false;
			ShowContents();
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			unattended = true;
			HideContents();
		}
	}

	// Update is called once per frame
	void Update () {
		if(unattended && !permanent){
			timer += Time.deltaTime;
        	if (timer >= timeBeforeFlashing && !flashing){
        	    GetComponent<FadeInOut>().Begin();
				flashing = true;
        	}
			if (timer >= timeBeforeDespawn){
				foreach (GameObject item in itemObjects){
					if (item != null &&  item.transform.parent == platform.transform){
						Destroy(item);
					}
				}
				Destroy(gameObject);
			}
		}
	}

	//Places 
	void ShowContents(){
		if (bagWasOpened != null){
			bagWasOpened();
		}
		platform.gameObject.SetActive(true);
		platform.currentFloorloot = this;
		if (!shownContents){
			CreateItemGameObjects();
			shownContents = true;
		} else {
			foreach (GameObject item in itemObjects){
				if (item != null && item.transform.parent == platform.transform){
					item.SetActive(true);
				}
			}
		}
	}

	void HideContents(){
		foreach (GameObject item in itemObjects){
			Debug.Log("Hiding " + item.GetInstanceID());
			if (item != null && item.transform.parent == platform.transform){
				item.SetActive(false);
			} else {
				Debug.Log("Did not hide ");
			}
		}
		platform.gameObject.SetActive(false);
	}

	void CreateItemGameObjects(){
		float xSpacing = platform.GetComponent<RectTransform>().sizeDelta.x /( items.Count +1);
		Debug.Log(xSpacing);
		int i = 1;
		foreach (ItemInstance item in items) {
			Vector2 pos = platform.transform.position;
			pos.x += i * xSpacing;
			GameObject itemObject = Instantiate(item.prefab, pos, Quaternion.identity);
			itemObject.transform.SetParent(platform.transform);
			itemObject.SetActive(true);
			// itemObject.GetComponent<RectTransform>().anchorMin = new Vector2( 0,0.5f);
			// itemObject.GetComponent<RectTransform>().anchorMax = new Vector2( 0,0.5f);

			itemObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
			itemObjects.Add(itemObject);
			i ++;
		}
	}
}
