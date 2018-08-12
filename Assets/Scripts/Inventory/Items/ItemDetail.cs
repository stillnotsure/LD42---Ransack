using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class ItemDetail : SerializedScriptableObject {

	[SerializeField]
	private Sprite Sprite;

	[BoxGroup("Shape")]
    [TableMatrix(HorizontalTitle = "Starts at Top Left")]
	public bool[,] shape = new bool[5,5];
	
	[SerializeField]
	private string Name;
	[SerializeField]
	private string Description;

	public Sprite sprite {get {return Sprite; }}
	[SerializeField]
	public string name {get {return Name; }}
	[SerializeField]
	public string description {get {return Description; }}
	public int width;
	public int height;

	public bool equippable;
	//Equip Vars
	public bool melee;
	public int damage;

}
