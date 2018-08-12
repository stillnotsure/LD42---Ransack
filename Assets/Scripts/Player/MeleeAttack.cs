using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {

	public Vector2 playerPos;

	

	public float force;
	public int damage;

	
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Enemy") {
			Rigidbody2D enemyBody = col.GetComponent<Rigidbody2D>();
			if (enemyBody != null) {
				Vector2 direction = (Vector2) enemyBody.transform.position - playerPos;
				Debug.Log(direction);
				enemyBody.AddForce(force* direction.normalized);
			}
			Enemy enemyScript = col.GetComponent<Enemy>();
			if (enemyScript != null){
				enemyScript.TakeDamage(damage);
			}
		}
	}

	public void Destroy(){
		Destroy(transform.parent.gameObject);
	}
}
