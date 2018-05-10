using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AppodealAds.Unity.Api;

public class PlayScript : MonoBehaviour {

	public GameObject Roskomnadzor;

	public GameObject pointPath;

	public GameObject[] bonus;

	public GameObject adButton;

    int numberOfLogo = 10;

    public RuntimeAnimatorController left;
	public RuntimeAnimatorController leftClose;
	public RuntimeAnimatorController right;
	public RuntimeAnimatorController rightClose;

	public GameObject animBackground;
	public GameObject animLeft;
	public GameObject animRight;

	public Camera mainCamera;
    public Text scoreText;
    public Text ipsText;
    public Text multiplayerText;

	float timer1s = 0;
    float timer5s = 0;

	float speedOfBonus = 0.25f;

    private long score; //счёт для надписи Score 
    private int clicks1s = 0; //кликов за прошлую секунду
    private int clicks5s = 0; //кликов за 5 секунд
	private int baseMultiplayer = 1;//базовый множитель, увеличивается при достижении отметки 100, 1000, 10000 ip,
    int scoreToMultiple = 100;
    int logoMultiplayer = 1;
    private int multiplayer5sBonus = 1; // множитель клика за 5 секунд (чем чаще кликает пользователь, тем он больше) обновляется каждые 5 сек
    private int multiplayer = 1; //итоговый множитель
	int adMultiplayer = 1;

	bool isCreateNewLogo = true;

    bool isMultitouch;
    bool isOnetouch;

    void Start () {
		//Preferences.setScore (0);
        baseMultiplayer = Preferences.getBaseMultiplayer();
        score = Preferences.getScore();
		animBackground.SetActive(true);
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            StartCoroutine(loadMainMenu());
            animLeft.GetComponent<Animator>().runtimeAnimatorController = leftClose;
            animRight.GetComponent<Animator>().runtimeAnimatorController = rightClose;
        }
        for (int i = 0;i < numberOfLogo;i++){
			bonus[i].transform.localPosition = Vector3.Lerp (bonus[i].transform.localPosition, 
															 pointPath.transform.localPosition,
															 speedOfBonus);
		}

        if (score > 100)
            baseMultiplayer = score.ToString().Length;
        else
            baseMultiplayer = 1;

        updateScore();
        updateMultiplayer();

        adButton.SetActive(Appodeal.isLoaded(Appodeal.NON_SKIPPABLE_VIDEO));

        if (Roskomnadzor.transform.localScale.x > 1.7f) {
			Roskomnadzor.transform.localScale = new Vector2(Roskomnadzor.transform.localScale.x - 0.02f, 
															Roskomnadzor.transform.localScale.y - 0.02f);
		}
		if (timer1s <= 1) { // это еще проще
            timer1s += Time.deltaTime;		        
		}
        else { 
			float ips = (float) clicks1s * multiplayer;
			print (ips);
			ipsText.text = ips.ToString() + " IP/s";
			clicks1s = 0;
			timer1s = 0;
		}
        if (timer5s <= 5) {
			timer5s += Time.deltaTime;       
        }
        else {
			print (clicks5s);
			multiplayer5sBonus = countMultiplyaer5s ();
            clicks5s = 0;
            timer5s = 0;
        }
		Touch[] touches = Input.touches;
		for (int i = 0; i < touches.Length; i++) {
			Touch touch = touches [i];
			if (touch.phase == TouchPhase.Began) {
				Ray ray = mainCamera.ScreenPointToRay (touch.position);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					switch (hit.collider.name) {
					case "Back":
						print ("Back button");
						animLeft.GetComponent<Animator> ().runtimeAnimatorController = leftClose;
						animRight.GetComponent<Animator> ().runtimeAnimatorController = rightClose;
						StartCoroutine (loadMainMenu ());
						break;
					case "Roscomnadzor":
						if (Random.Range (1, 100) == 1) //вероятность включения лого (сейчас шанс выпадения - 1 к 3)
							generateBonus ();
						if(hit.collider.transform.localScale.x < 2)
							hit.collider.transform.localScale = new Vector2 (Roskomnadzor.transform.localScale.x + 0.1f, Roskomnadzor.transform.localScale.y + 0.1f);
						click();
						break;
					case "Ad":
						print ("Ad");
						//запуск рекламы
						showAd();
						adMultiplayer = 5;
						hit.collider.gameObject.SetActive (false);
						StartCoroutine(wait5Minutes());
						StartCoroutine(wait10Minutes());
						break;
					case "Collider":
						logoMultiplayer = 15;
						for (int a = 0; a < numberOfLogo; a++)
							bonus [a].SetActive (false);
						StartCoroutine (wait10Seconds ());
						break;
					}
				}
			}
    	}
	}

    void onDestroy() {
        Preferences.setBaseMultiplayer(baseMultiplayer);
        Preferences.setScore(score);
    }

    private void generateBonus() {
		if (isCreateNewLogo) {
            int randomLogo = Random.Range(0, numberOfLogo);
            bonus [randomLogo].SetActive (true);
			isCreateNewLogo = false;
		}
    }

    private int countMultiplyaer5s() {
        if (clicks5s <= 40)
            return 1;
        else if (clicks5s <= 60)
            return 2;
        else if (clicks5s <= 80)
            return 3;
        else if (clicks5s <= 100)
            return 4;
        else 
            return 5;
    }
		
	public static void positionPointGenerating(GameObject pointPath){
		pointPath.transform.localPosition = new Vector3 (Random.Range (-278, 286), Random.Range (-590, 511),pointPath.transform.localPosition.z);
	}

    private void updateScore() { //обновляем текст Score
		scoreText.text = score.ToString();
    }

    private void updateMultiplayer() { //обновляем multiplayer и текст Multiplayer
		multiplayer = baseMultiplayer * multiplayer5sBonus * adMultiplayer * logoMultiplayer;
        multiplayerText.text = "X" + multiplayer.ToString();
    }

    private void click() { // метод обработки клика, увеличиваем счёт и все нажатия
        clicks1s++;
        clicks5s++;
        score += multiplayer;
//		Preferences.setScore (Preferences.getScore() + multiplayer);
    }

	private void showAd() {
		Appodeal.show(Appodeal.NON_SKIPPABLE_VIDEO);
	}

    IEnumerator loadMainMenu() {
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene ("Main menu");
	}

	IEnumerator wait10Seconds() {
		yield return new WaitForSeconds (10);
		logoMultiplayer = 1;
		isCreateNewLogo = true;
	}

	IEnumerator wait5Minutes() {
		yield return new WaitForSeconds (30);//5 минут (изменил на 30 секунд для теста)
		adMultiplayer = 1;
	}
	IEnumerator wait10Minutes() {
		yield return new WaitForSeconds (60);//10 минут (изменил на 60 секунд для теста)
		adButton.SetActive(true);
	}
}
