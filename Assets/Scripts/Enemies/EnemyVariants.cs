using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//Keep a list of all the types of difficulty one enemy goes through
[CreateAssetMenu]
public class EnemyVariants : ScriptableObject {

    public GameObject templatePrefab;
    [SerializeField]
    public List<VariantThreshold> progression = new List<VariantThreshold>();

    public List<EnemyVariant>  GetAvailableVariants(int score){
        List<EnemyVariant> variants = new List<EnemyVariant>();
        foreach (VariantThreshold x in progression){
            if (score >= x.scoreThreshold){
                variants.Add(x.variant);
            }
            else {
                break;
            }
        }
        return variants;
    }

}

[Serializable]
public struct VariantThreshold {
    public int scoreThreshold;
    public EnemyVariant variant;   
}

