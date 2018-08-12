using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour {
	public static UIManager instance = null;
    public ScoreManager ScoreManager;
    InventoryManager InventoryManager;
    ReferenceManager ReferenceManager;

    public enum gameOverStates {None, ToFreeze, SlideTextUp, ToCountdown, CountingDown, WaitToRetry, Retry};
    public gameOverStates state;

    public ScreenShake screenshake;
    float timer;

    bool gameOver;
    
    public float gameOverFreezeDelay;
    public float scoreCountdownDelay;

    //Game Over Screen
    public Vector3 targetPosition;
    public GameObject gameOverSlider;
    public GameObject gameOverText;
    private Vector3 gameOverVelocity = Vector3.zero;
    public float smoothTime;
    public GameObject scoreObject;
    public GameObject RetryButton;


    bool finishedCounting;
    int internalScore = 0;
    int targetScore;
    public Text scoreCounter;

    //Tutorial stuff
    bool tutorial = true;
    public bool tutStateDone;
    public GameObject tutorialText1;
    public GameObject tutorialText2;
    public GameObject tutorialText3;
    public GameObject tutorialFloorLoot;
    public GameObject beginButton;

    public enum tutorialStates {ApproachBag, DragIntoInventory, WaitingToBegin};
    public tutorialStates tutState;
    bool bagWasOpened;
    bool itemWasDragged;
    bool buttonWasClicked;
    
    bool coroutineActive;

    void Awake() {
		if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);
	}

    public static UIManager GetInstance() {
        return instance;
    }

    [ContextMenu("TriggerGameOver")]
    public void TriggerGameOver(){
        gameOver = true;
        gameOverSlider.SetActive(true);
        state = gameOverStates.ToFreeze;
        targetScore = ScoreManager.score;
    }

    void Start(){
        screenshake = GetComponent<ScreenShake>();
        tutorialFloorLoot.GetComponent<FloorLoot>().bagWasOpened += TutorialBagWasOpened;
        ReferenceManager = ReferenceManager.GetInstance();
        InventoryManager = InventoryManager.GetInstance();
        InventoryManager.OnItemAdded += ItemWasAdded;
    }

    void Update(){
        if (tutorial){
            tutorialRoutine();
        }
        if (gameOver){
            GameOverRoutine();
        }
    }

    public void AddScreenshake(float duration, float magnitude){
        screenshake.AddScreenshake(duration, magnitude);
    }


    public void AddTinyScreenshake(){
        screenshake.shakeDuration += 0.1f;
        screenshake.shakeMagnitude += 0.025f;
    }

    public void AddSmallScreenshake(){
        screenshake.shakeDuration += 0.25f;
        screenshake.shakeMagnitude += 0.05f;
    }

    public void AddBigScreenShake(){
        screenshake.shakeDuration += 0.4f;
        screenshake.shakeMagnitude += 0.2f;
    }

    void tutorialRoutine(){
        if (tutState == tutorialStates.ApproachBag){
            ApproachBag();
        } else if (tutState == tutorialStates.DragIntoInventory){
            WaitForDrag();
        } 
        else if (tutState == tutorialStates.WaitingToBegin){
            WaitingToBegin();
        }
    }

    void ApproachBag(){
        if (!tutStateDone){
            tutorialText1.SetActive(true);
            tutorialText1.GetComponent<FadeText>().FadeIn(2);
            tutStateDone = true;
        }
        
        if (bagWasOpened){
            tutorialText1.GetComponent<FadeText>().FadeOut(2);
            tutState = tutorialStates.DragIntoInventory;
            tutStateDone = false;
        }
    }

    void TutorialBagWasOpened(){
        bagWasOpened = true;
    }


    void WaitForDrag(){
        if (!tutStateDone){
            tutorialText2.SetActive(true);
            tutorialText2.GetComponent<FadeText>().FadeIn(2);
            tutStateDone = true;
        }
        if (itemWasDragged){
            tutorialText2.GetComponent<FadeText>().FadeOut(2);
            tutState = tutorialStates.WaitingToBegin;
            tutStateDone = false;
        }
    }

    void WaitingToBegin(){
        if (!tutStateDone){
            tutorialText3.SetActive(true);
            tutorialText3.GetComponent<FadeText>().FadeIn(2);
            tutStateDone = true;
        }
    }

    public void BeginButtonClicked(){
        ClearDownTutorialText();
        tutorial = false;
        ReferenceManager.StartGame();
        tutorialFloorLoot.GetComponent<FloorLoot>().permanent = false;
        beginButton.SetActive(false);
        GetComponent<AudioSource>().Play();
    }

    void ClearDownTutorialText(){
        if(tutorialText1.activeSelf){
            tutorialText1.GetComponent<FadeText>().FadeOut(2);
        }
        if(tutorialText2.activeSelf){
            tutorialText2.GetComponent<FadeText>().FadeOut(2);
        }
        if(tutorialText3.activeSelf){
            tutorialText3.GetComponent<FadeText>().FadeOut(2);
        }
    }

    void ItemWasAdded(ItemInstance item){
        itemWasDragged = true;
    }

    void GameOverRoutine(){
        if (state == gameOverStates.ToFreeze){
            ToFreezeSlider();
        } else if (state == gameOverStates.SlideTextUp){
            SlideTextUp();
        }else if (state == gameOverStates.ToCountdown){
            ToBeginCountdown();
        } else if (state == gameOverStates.CountingDown){
            PerformCountdown();
        } else if (state == gameOverStates.WaitToRetry){
            WaitToRevealRetryButton();
        } else if (state == gameOverStates.Retry){
            ShowRetryButton();
        }
    }

    void ToFreezeSlider(){
        if (timer >= gameOverFreezeDelay){
           // gameOverSlider.GetComponent<Rigidbody2D>().simulated = false;
            timer = 0;
            state += 1;
        } else {
            timer += Time.deltaTime;
        }
    }

    void SlideTextUp(){
        if (!coroutineActive)
            StartCoroutine(IncreaseState(smoothTime));
        gameOverText.transform.localPosition = Vector3.SmoothDamp(gameOverText.transform.localPosition, targetPosition, ref gameOverVelocity, smoothTime);
    }

    void ToBeginCountdown(){
        if (timer >= scoreCountdownDelay){
            scoreObject.SetActive(true);
            timer = 0;
            state += 1;
        } else {
            timer += Time.deltaTime;
        }
    }

    void PerformCountdown(){
        if (!coroutineActive)
            StartCoroutine(CountUp());
        if (finishedCounting){
            state += 1;
        }
    }

    void WaitToRevealRetryButton(){
        if (!coroutineActive)
            StartCoroutine(IncreaseState(1));
    }

    void ShowRetryButton(){
        RetryButton.SetActive(true);
    }

    public IEnumerator CountUp(){
        coroutineActive = true;
        float delay = 0.01f;
        while (internalScore < targetScore){
            yield return new WaitForSeconds(delay);

            int increment = targetScore - internalScore;

            if (increment > 100) {
                increment = Mathf.Min(100, increment/10);
            } 

            internalScore += increment;
            scoreCounter.text = internalScore.ToString();
        }
        finishedCounting = true;
        coroutineActive = false;
    }

    public IEnumerator IncreaseState(float delay){
        coroutineActive = true;
        yield return new WaitForSeconds(delay);
        state+=1;
        coroutineActive = false;
    }

    public void Restart(){
        Scene loadedLevel = SceneManager.GetActiveScene ();
        SceneManager.LoadScene (loadedLevel.buildIndex);
    }


}