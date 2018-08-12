using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    One instance of an item.
    Handles being dragged etc, used etc.
    Gets its info from an ItemDetail
 */
public class ItemInstance : MonoBehaviour {
    private Image image;
    public ItemDetail ItemDetail;
    public GameObject prefab;

    //Positional Stuff
    public Rect rect {get;set;}
    public Vector2Int gridPos;
    public List<Vector2Int> points;

    public bool beingDragged;

    void Start(){
        CalculatePointsAndRect();
        image = GetComponent<Image>();
        image.sprite = ItemDetail.sprite;
    }

    //Creates a List of points and their positions in the world
    void CalculatePointsAndRect() {
        List<Vector2Int> newPoints = new List<Vector2Int>();
        int width = ItemDetail.width; int height = ItemDetail.height;

        for(int x = 0; x < width; x ++) {
            for (int y = 0; y < height; y++) {
                if (ItemDetail.shape[x,y] == true){
                    Vector2Int pointPos = new Vector2Int(gridPos.x + (x), gridPos.y + (y));
                    newPoints.Add(pointPos);
                }
            }
        }

        points = newPoints;
        rect = new Rect(gridPos, new Vector2(width, height));
    }

    public bool Overlaps(ItemInstance otherItem){
        if (rect.Overlaps(otherItem.rect)){
            //Then we have to check closer
            for (int i = 0; i < points.Count; i++){
                if (otherItem.points.Contains(points[i])){
                    return true;
                }
            }

        }
        return false;
    }

    public void GetNewGridPos(Vector2Int gridPos){
        this.gridPos = gridPos;
        Debug.Log(gridPos);
        CalculatePointsAndRect();
    }

}
