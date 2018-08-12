using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour {

    public static PlayerInput instance = null;
    SpriteRenderer sprite;

    public Sprite upSprite;
    public Sprite downSprite;

    public GameObject floor;

    public float speed = 1f;
    public float sensitivity = 0.1f;
    //public Rigidbody2D rigidbody;
    private float turnSmoothing = 5f;

    public Action<Vector2, Vector2> UsedEquip;
    public Action ReleasedTrigger;

    // Use this for initialization
    void Awake () {
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);

        sprite = GetComponent<SpriteRenderer>();
	}

	public static PlayerInput GetInstance() {
        return instance;
    }

	void OnTriggerEnter2D(Collider2D col) {
		
	}

    void OnCollisionEnter2D(Collision2D col) {
		
	}

	// Update is called once per frame
	void Update () {
        float xMovement = Input.GetAxisRaw("Horizontal") * speed;
        float YMovement = Input.GetAxisRaw("Vertical") * speed;
        Vector2 movement = new Vector2(xMovement, YMovement);

        if (YMovement > 0){
            sprite.sprite = upSprite;
        } else if (YMovement < 0) {
            sprite.sprite = downSprite;
        }

        transform.position += Vector3.ClampMagnitude(movement, speed) * Time.deltaTime;  
        Vector2 movedPos = transform.position;

        Bounds playArea = floor.GetComponent<Renderer>().bounds;
        if (movedPos.x >= playArea.max.x - 0.65f){
            movedPos.x = playArea.max.x - 0.66f;
        }
        else if (movedPos.x <= playArea.min.x + 0.65f){
            movedPos.x = playArea.min.x + 0.66f;
        }
        if (movedPos.y >= playArea.max.y){
            movedPos.y = playArea.max.y - 0.1f;
        }
        else if (movedPos.y <= playArea.min.y + 0.65f){
            movedPos.y = playArea.min.y + 0.66f;
        }

        transform.position = movedPos;

        float activePortionOfScreen = (float)0.7 * Screen.width ;

        if (Input.GetButtonDown("Fire1") && Input.mousePosition.x < activePortionOfScreen) {
            PressedFire();
        }

        if (Input.GetButtonUp("Fire1")) {
            ReleasedFire();
        }
    }

    public void Die(){
        gameObject.SetActive(false);
    }


    void PressedFire() {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 dir = (Vector2)Input.mousePosition - pos;

        if (UsedEquip != null){
            UsedEquip(dir, transform.position);
        }
    }

    void ReleasedFire() {
        if (ReleasedTrigger != null) {
            ReleasedTrigger();
        }
    }

    public Vector2 GetPlayerPos(){
        return transform.position;
    }

    public Vector2 GetMouseDirection(){
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 dir = (Vector2)Input.mousePosition - pos;

        return dir;
    }
}
