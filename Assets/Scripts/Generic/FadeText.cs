using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour {
	Text text;

	void Awake(){
		text = GetComponent<Text>();
	}

	public void FadeIn(float duration){
		StopAllCoroutines();
		StartCoroutine(UpdateTextColor(duration, new Color(1.0f,1.0f,1.0f,1.0f)));
	}

	public void FadeOut(float duration){
		StopAllCoroutines();
		StartCoroutine(UpdateTextColor(duration, new Color(0f,0f,0f,0f)));
	}

	IEnumerator UpdateTextColor(float duration, Color newColor) {
		float t = 0;
		Color origColor = text.color;
		while (t < 1) {
			text.color = Color.Lerp(origColor, newColor, t);
			t += Time.deltaTime / duration;
			yield return new WaitForEndOfFrame();
		}
	}
}
