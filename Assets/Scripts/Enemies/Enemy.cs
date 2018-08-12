using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public EnemyVariant variant;
	AudioHost AudioHost;
	Audio _audio;
	ReferenceManager ReferenceManager;
	EnemyManager EnemyManager;

	public int health;
	public float speed;
	public int damage;

	public List<AudioClip> damagedSounds;
	Color origColor;
	SpriteRenderer spriteRenderer;

	public Transform target;

	void Start(){
		ReferenceManager = ReferenceManager.GetInstance();
		AudioHost = AudioHost.GetInstance();
		EnemyManager = EnemyManager.GetInstance();
		target = ReferenceManager.playerTransform;
		_audio = AudioHost.GetComponent<Audio>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		FadeIn();
	}
	
	public void TakeDamage(int damage){
		health -= damage;
		if (_audio != null){
			_audio.PlayRandomClip(damagedSounds);
		}
		
		if (health <= 0){
			Die();
		}
	}

	void Die(){
		EnemyManager.EnemyKilled(this, variant.score, transform.position);
		GameObject.Destroy(gameObject);
	}

	public void DoDamage(){
		target.GetComponent<HealthManager>().TakeDamage(damage);
	}

	public void AbsorbVariantTraits(EnemyVariant variant){
		this.variant = variant;

		this.health = variant.health;
		this.speed = variant.speed;
		this.damage = variant.damage;

		if (spriteRenderer == null){
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		}
		spriteRenderer.sprite = variant.sprite;
		spriteRenderer.color = variant.tint;
		origColor = variant.tint;
	}

	void FadeIn(){
		StartCoroutine(Fade(1));
	}

	IEnumerator Fade(float duration) {
		Color transparentColor = origColor;
		transparentColor.a = 0;
		spriteRenderer.color = transparentColor;
		float t = 0;
		
		while (t < 1) {
			spriteRenderer.color  = Color.Lerp(transparentColor, origColor, t);
			t += Time.deltaTime / duration;
			yield return new WaitForEndOfFrame();
		}
	}
}
