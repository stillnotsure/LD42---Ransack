using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Audio : MonoBehaviour {

	AudioSource audioSource;

	// Use this for initialization
	void Awake () {
		audioSource = GetComponent<AudioSource>();
	}
	
	public void PlayClip(AudioClip clip){
		audioSource.PlayOneShot(clip);
	}

	public void PlayRandomClip(List<AudioClip> clips) {
		int r = UnityEngine.Random.Range(0, clips.Count);
		audioSource.PlayOneShot(clips[r]);
	}
}
