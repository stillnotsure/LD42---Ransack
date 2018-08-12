using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour {

	public float inSecs;
	public float outSecs;

    public bool sprite;


	public void Begin () {
		StartCoroutine(FadeOut(GetComponent<Image>()));
	}
	
	public FadeInOut(float inSecs, float outSecs){
		this.inSecs = inSecs;
		this.outSecs = outSecs;
	}

    public void Stop(){
        Color newColor = new Color(1, 1, 1);
        if (sprite){
            GetComponent<SpriteRenderer>().color = newColor;
        } else {
            GetComponent<Image>().color = newColor;
        }

        StopAllCoroutines();
    }

	IEnumerator FadeIn(Image image) {
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / inSecs) {
                Color oldColor;
                if (sprite){
                    oldColor = GetComponent<SpriteRenderer>().color;
                } else {
                    oldColor = image.color;
                }

                Color newColor = new Color(1, 1, 1, Mathf.Lerp(oldColor.a, 1.00f, t));

                if (sprite){
                    GetComponent<SpriteRenderer>().color = newColor;
                } else {
                    image.color = newColor;
                }
                yield return null;
            }
            yield return null;
            StartCoroutine(FadeOut(image));
    }

	IEnumerator FadeOut(Image image) {
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / outSecs) {
                Color oldColor;
                if (sprite){
                    oldColor = GetComponent<SpriteRenderer>().color;
                } else {
                    oldColor = image.color;
                }

                Color newColor = new Color(1, 1, 1, Mathf.Lerp(oldColor.a, 0.00f, t));
                if (sprite){
                    GetComponent<SpriteRenderer>().color = newColor;
                } else {
                    image.color = newColor;
                }
                yield return null;
            }
            yield return null;
            StartCoroutine(FadeIn(image));
        }
}
