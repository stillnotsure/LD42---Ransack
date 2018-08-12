using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Shotgun : MonoBehaviour {

    public GameObject projectile;
    public int projectileCount;

    public GameObject fireParticles;
    public AudioClip fireSound;

    public float spread;

    bool ready = true;
    public float cooldownDelay;
    float cooldownTimer;

    void Start(){
       GetComponent<Equippable>().UsedEquip += HandleFire;
    }

    void Update(){
        if (!ready){
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownDelay){
                ready = true;
                cooldownTimer = 0;
            }
        }
    }

    void HandleFire(Vector2 dir, Vector2 pos){    
        if (ready){
            for (int i = 0; i < projectileCount; i ++ ){
                float xSpread = UnityEngine.Random.Range(-spread, spread);
                float ySpread = UnityEngine.Random.Range(-spread, spread);

                Vector2 adjustedDir  = new Vector2 (dir.x + xSpread, dir.y + ySpread);
                Bullet bullet = Instantiate(projectile, pos, Quaternion.identity).GetComponent<Bullet>();
                bullet.direction = adjustedDir; 
            }
            ready = false;
        }    else {
            Debug.Log("Not ready");
        }
        
    }





}