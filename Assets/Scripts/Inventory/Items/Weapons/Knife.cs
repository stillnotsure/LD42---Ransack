using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Knife : MonoBehaviour {
    PlayerInput PlayerInput;
    public GameObject attack;

    Audio _audio;
    public List<AudioClip> swingAudioClips;

    bool ready = true;
    public float cooldownDelay;
    float cooldownTimer;

    void Start(){
        PlayerInput = PlayerInput.GetInstance();
        GetComponent<Equippable>().UsedEquip += HandleFire;
        _audio = GetComponent<Audio>();
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
            float rotation=Mathf.Atan2(dir.x, dir.y)*Mathf.Rad2Deg;
            GameObject knifeSwing = Instantiate(attack, pos, Quaternion.Euler(0, 0, -rotation), PlayerInput.transform);
            knifeSwing.GetComponentInChildren<MeleeAttack>().playerPos = pos;
            ready = false;

            if (_audio != null && swingAudioClips != null){
				_audio.PlayRandomClip(swingAudioClips);
			}

       }
    }





}