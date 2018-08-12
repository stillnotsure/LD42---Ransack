using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
	Audio _audio;
	Enemy Enemy;
	public int damage;
	public List<AudioClip> spawnSounds;

	// Use this for initialization
	void Start () {
		Enemy = GetComponent<Enemy>();
		_audio = GetComponent<Audio>();


		if (_audio != null && spawnSounds != null){
			_audio.PlayRandomClip(spawnSounds);
		}
		
	}
	
	void OnTriggerEnter2D(Collider2D col) {

		if (col.gameObject.tag == "Player") {
			Enemy.DoDamage();
		}
	}

	// void OnCollisionEnter2D(Collision2D col) {
	// 	Debug.Log(col.transform.name);
	// 	if (col.gameObject.tag == "Player") {
	// 		Enemy.DoDamage(damage);
	// 	}
	// }

	// Update is called once per frame
	void Update () {
		Vector2 moveDir = (Enemy.target.position - transform.position).normalized;
		transform.Translate(moveDir * Enemy.speed * Time.deltaTime);
	}
}
