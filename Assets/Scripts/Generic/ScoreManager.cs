using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    public static ScoreManager instance = null;
    public int score;

    void Awake(){
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);
    }

    public static ScoreManager GetInstance() {
        return instance;
    }

    public void AddToScore(int score){
        this.score += score;
    }

    
}