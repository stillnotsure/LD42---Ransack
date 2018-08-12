using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float force;
	public Vector2 direction;
	public float speed;
	public int damage;
	
	void Update () {
		transform.Translate(direction.normalized * speed * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Enemy") {
			Rigidbody2D enemyBody = col.GetComponent<Rigidbody2D>();
			if (enemyBody != null) {
				enemyBody.AddForce(force* direction.normalized);
			}
			Enemy enemyScript = col.GetComponent<Enemy>();
			if (enemyScript != null){
				enemyScript.TakeDamage(damage);
			}

			Destroy(gameObject);
		}
		if (col.gameObject.tag == "Barrier") {
			Destroy(gameObject);
		}
	}
}
