using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    AudioHost AudioHost;
    PlayerInput PlayerInput;
    FadeInOut playerInvincibilityEffect;
    UIManager UIManager;
	ReferenceManager ReferenceManager;
    InventoryManager InventoryManager;
    public ItemInstance healthOrbInstance;
    Transform target;

    public float hitInvincibility = 1f;
    private float hitInvincibilityTimer;
    bool invincible;

    public AudioClip TakeDamageSound;

	void Start(){
        AudioHost = AudioHost.GetInstance();
        Debug.Log(AudioHost);
		ReferenceManager = ReferenceManager.GetInstance();
        InventoryManager = InventoryManager.GetInstance();
        UIManager = UIManager.GetInstance();
        PlayerInput = PlayerInput.GetInstance();
        playerInvincibilityEffect = PlayerInput.GetComponent<FadeInOut>();
		target = ReferenceManager.playerTransform;
	}
	
    [ContextMenu("TakeOneDamage")]
    public void TakeOneDamae(){
        TakeDamage(1);
    }

    void Update(){
        if (invincible){
            hitInvincibilityTimer += Time.deltaTime;
            if (hitInvincibilityTimer >= hitInvincibility) {
                hitInvincibilityTimer = 0;
                invincible = false;
                playerInvincibilityEffect.Stop();
            }
        }
    }

	public void TakeDamage(int damage){
        if (!invincible){
            Debug.Log(AudioHost._audio);
            Debug.Log(TakeDamageSound);
            AudioHost._audio.PlayClip(TakeDamageSound);
            playerInvincibilityEffect.Begin();
            for (int i = 0; i < damage; i ++ ){
                ItemInstance aRemainingOrb = InventoryManager.SearchForFirstItemOfType(healthOrbInstance);
                if (aRemainingOrb == null){
                    Die();
                } else {
                    InventoryManager.DestroyItemInInventory(aRemainingOrb);
                }
            }
            invincible = true;
        } 
	}

	void Die(){
		PlayerInput.Die();
        UIManager.TriggerGameOver();
	}

}
