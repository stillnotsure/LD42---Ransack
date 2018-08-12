using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour{
	public static ReferenceManager instance = null;
    public GameObject playArea;
    public GameObject platform;

    void Awake() {
		if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);
	}

    public static ReferenceManager GetInstance() {
        return instance;
    }

    public Transform playerTransform;
    public Transform enemyHolder;
    public EnemyManager EnemyManager;

    public void StartGame(){
        EnemyManager.enabled = true;
    }

}

	
