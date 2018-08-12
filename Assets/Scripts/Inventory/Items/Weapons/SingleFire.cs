using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class SingleFire : MonoBehaviour {

    Equippable Equippable;
    UIManager UIManager;
    public float shakeDuration;
    public float shakeMagnitude;

    Audio _audio;
    public List<AudioClip> fireAudioClips;

    public GameObject projectile;
    public int projectileCount = 1;

    public GameObject fireParticles;
    public float spread;

    bool ready = true;
    public float cooldownDelay;
    float cooldownTimer;

    public int clipCapacity;
    public int currentClip;
    public float reloadDelay;
    float reloadTimer;

    void Start(){
       Equippable = GetComponent<Equippable>();
       Equippable.UsedEquip += HandleFire;
       
       _audio = GetComponent<Audio>();
       currentClip = clipCapacity;
       UIManager = UIManager.GetInstance();
    }

    void Update(){
        // if (currentClip <= 0){
        //     // if (reloadTimer >= reloadDelay){
        //     //     FinishReload();
        //     // }
        //     // else {
        //     //     reloadTimer += Time.deltaTime;
        //     // }
        // }

        if (!ready){
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownDelay){
                ready = true;
                cooldownTimer = 0;
            }
        }
    }

    void HandleFire(Vector2 dir, Vector2 pos){    
        while (ready && currentClip > 0){
            for (int i = 0; i < projectileCount; i ++ ){
                float xSpread = UnityEngine.Random.Range(-spread, spread);
                float ySpread = UnityEngine.Random.Range(-spread, spread);

                Vector2 adjustedDir  = new Vector2 (dir.x + xSpread, dir.y + ySpread);
                Bullet bullet = Instantiate(projectile, pos, Quaternion.identity).GetComponent<Bullet>();
                bullet.direction = adjustedDir; 

                _audio.PlayRandomClip(fireAudioClips);
            }

            Equippable.BeUsed();
            UIManager.AddScreenshake(shakeDuration, shakeMagnitude);
            // currentClip -= 1;
            ready = false;
        } 
        
    }

    [ContextMenu("Reload")]
    void FinishReload(){
            reloadTimer = 0;
            currentClip = clipCapacity;
            //Play reload SFX
    }





}