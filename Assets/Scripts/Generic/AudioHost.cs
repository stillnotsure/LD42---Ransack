using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHost : MonoBehaviour {
	public Audio _audio;
	public AudioClip grabItemSound;
	public AudioClip dropItemSound;
	public AudioClip equipItemSound;

	public AudioClip playerDeathSound;


	public static AudioHost instance = null;

	void Awake() {
		if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);

		_audio = GetComponent<Audio>();
	}

	public static AudioHost GetInstance() {
        return instance;
    }
	
}
