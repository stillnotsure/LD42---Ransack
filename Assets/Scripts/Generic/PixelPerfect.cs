using UnityEngine;

public class PixelPerfect : MonoBehaviour {
    float pixelsPerUnit = 32f;

    void LateUpdate(){
        Vector3 pos = transform.localPosition;

        pos.x = (Mathf.Round(transform.parent.position.x * pixelsPerUnit) / pixelsPerUnit) - transform.parent.position.x;
        pos.y = (Mathf.Round(transform.parent.position.y * pixelsPerUnit) / pixelsPerUnit) - transform.parent.position.y;

    }

}