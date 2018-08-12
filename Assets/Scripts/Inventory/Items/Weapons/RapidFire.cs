using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class RapidFire : MonoBehaviour {

    Equippable Equippable;
    UIManager UIManager;
    public float shakeDuration;
    public float shakeMagnitude;

    Audio _audio;
    public List<AudioClip> fireAudioClips;

    PlayerInput PlayerInput;

    public GameObject projectile;
    public float spread;
    public int projectileCount = 1;

    public GameObject fireParticles;
    public AudioClip fireSound;

    public bool triggerHeld;
    bool ready = true;

    public int clipCapacity;
    public int currentClip;
    public float reloadDelay;
    float reloadTimer;

    public float cooldownDelay;
    float cooldownTimer;

    void Start(){
       Equippable = GetComponent<Equippable>();
       PlayerInput = PlayerInput.GetInstance();
       Equippable.UsedEquip += HandleFire;
       Equippable.ReleasedEquip += HandleTriggerRelease;
       _audio = GetComponent<Audio>();
       currentClip = clipCapacity;
       UIManager = UIManager.GetInstance();
    }

    void Update(){
        if (triggerHeld){
            TryFire();
        }

        if (currentClip <= 0){
            triggerHeld = false;
            if (reloadTimer >= reloadDelay){
                FinishReload();
            }
            else {
                reloadTimer += Time.deltaTime;
            }
        }

        if (!ready){
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownDelay){
                ready = true;
                cooldownTimer = 0;
            }
        }
    }

    void TryFire(){
        Vector2 pos = PlayerInput.GetPlayerPos();
        Vector2 dir = PlayerInput.GetMouseDirection();

        if (ready && currentClip > 0){
            for (int i = 0; i < projectileCount; i ++ ){
                // currentClip -= 1;
                float xSpread = UnityEngine.Random.Range(-spread, spread);
                float ySpread = UnityEngine.Random.Range(-spread, spread);

                Vector2 adjustedDir  = new Vector2 (dir.x + xSpread, dir.y + ySpread);
                Bullet bullet = Instantiate(projectile, pos, Quaternion.identity).GetComponent<Bullet>();
                bullet.direction = adjustedDir; 
            }
            UIManager.AddScreenshake(shakeDuration, shakeMagnitude);
            _audio.PlayRandomClip(fireAudioClips);
            ready = false;
            Equippable.BeUsed();
        } 
    }

    void HandleFire(Vector2 dir, Vector2 pos){    
        triggerHeld = true; 
    }

    void HandleTriggerRelease(){
        Debug.Log("Trigger released");
        triggerHeld = false;
    }

    [ContextMenu("Reload")]
    void FinishReload(){
            reloadTimer = 0;
            currentClip = clipCapacity;
            //Play reload SFX
    }





}