using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    Transform playerTransform;
    public static EnemyManager instance = null;
    ScoreManager ScoreManager;
    ReferenceManager ReferenceManager;
    GameObject playArea;
    Transform enemyHolder;

    [SerializeField]
    List<EnemyType> enemyTypes;

    float waveTimer;
    public float currentSpawnRate;
    public float spawnrateMultiplier;

    int baseSpawnCap = 4;
    public int currentSpawnCap;
    public int spawnCapMultiplier;

    public int difficultyPeak;
    public LayerMask playerMask;

    public AnimationCurve spawnrateProgression;

    List<GameObject> enemiesOut = new List<GameObject>();

    public Action<Vector2> enemyKilledAction;
    int enemiesKilled;

    void Awake(){
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start(){
        ReferenceManager ReferenceManager = ReferenceManager.GetInstance();
        playerTransform = ReferenceManager.playerTransform;
        ScoreManager = ScoreManager.GetInstance();
        playArea = ReferenceManager.playArea;
    }

    public static EnemyManager GetInstance() {
        return instance;
    }

    void Update(){
        currentSpawnRate = Mathf.Min(5, (float)spawnrateProgression.Evaluate(ScoreManager.score / (float)difficultyPeak) * spawnrateMultiplier);
        if (enemiesOut.Count < currentSpawnCap) {
            waveTimer += Time.deltaTime;
            if (waveTimer >= currentSpawnRate){
                ChooseAnEnemyToSpawn();
                waveTimer = 0;
            }
        }
    }

    public void EnemyKilled(Enemy enemy, int score, Vector2 pos){
        if (enemiesOut.Contains(enemy.gameObject)){
            enemiesOut.Remove(enemy.gameObject);
        }
        ScoreManager.AddToScore(score);
        if (enemyKilledAction != null){
            enemyKilledAction(pos);
        }
        enemiesKilled ++;
        currentSpawnCap = (enemiesKilled % 15) + baseSpawnCap;
    }

    void ChooseAnEnemyToSpawn(){
        float totalProb = 0;
        foreach(EnemyType enemyType in enemyTypes){
            totalProb += enemyType.probability;
        }

        float r = UnityEngine.Random.Range(0, totalProb);
        float accumProb = 0;
        foreach(EnemyType enemyType in enemyTypes){
            accumProb += enemyType.probability;
            if (r <= accumProb){
                SpawnAVariantOfThisType(enemyType.variants);
            }
        }
    }

    void SpawnAVariantOfThisType(EnemyVariants variants){
        Vector2 min = playArea.GetComponent<Renderer>().bounds.min;
        Vector2 max = playArea.GetComponent<Renderer>().bounds.max;

        Vector2 spawnPos = new Vector2();
        spawnPos.x = UnityEngine.Random.Range(min.x, max.x);
        spawnPos.y = UnityEngine.Random.Range(min.y, max.y);

        Collider2D coll = Physics2D.OverlapCircle(spawnPos, 0.5f, playerMask);
        int escape = 100;
        while(coll != null && escape > 0){
            spawnPos.x = UnityEngine.Random.Range(min.x, max.x);
            spawnPos.y = UnityEngine.Random.Range(min.y, max.y);
            escape--;
        }

        List<EnemyVariant> availableVariants = variants.GetAvailableVariants(ScoreManager.score);
        int r = UnityEngine.Random.Range(0, availableVariants.Count);
        EnemyVariant enemyToSpawn = availableVariants[r];

        GameObject go = Instantiate(variants.templatePrefab, spawnPos, Quaternion.identity, enemyHolder);
        Enemy e = go.GetComponent<Enemy>();

        e.AbsorbVariantTraits(enemyToSpawn);
        enemiesOut.Add(go);
    }

}

[Serializable]
public struct EnemyType {
    [SerializeField]
    public float probability;
    [SerializeField]
    public EnemyVariants variants;
}
