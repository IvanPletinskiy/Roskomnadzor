using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayScript : MonoBehaviour {

	public GameObject Roskomnadzor;

	public GameObject adButton;

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
    public Image bonus;

	float timer1s = 0;
    float timer5s = 0;

    private int score; //счёт для надписи Score 
    private int clicks1s=0; //кликов за прошлую секунду
    private int clicks5s=0; //кликов за 5 секунд
	private int baseMultiplayer = 1;//базовый множитель, увеличивается при достижении отметки 100, 1000, 10000 ip,
	int scoreToMultiple=100;
    private int multiplayer5sBonus = 1; // множитель клика за 5 секунд (чем чаще кликает пользователь, тем он больше) обновляется каждые 5 сек
    private int multiplayer = 1; //итоговый множитель
	int multipleX10=1;

    bool isMultitouch;
    bool isOnetouch;

    void Start () {
		//Preferences.setScore (0);
        multiplayer = Preferences.getMultiplayer();
		animBackground.SetActive (true);
    }

	void Update () {
		if (Preferences.getScore () >= scoreToMultiple) {
			baseMultiplayer++;
			scoreToMultiple*=10;
		}
        updateScore();
        updateMultiplayer();

        if (Roskomnadzor.transform.localScale.x > 1.7f) {
			Roskomnadzor.transform.localScale = new Vector2(Roskomnadzor.transform.localScale.x-0.02f,Roskomnadzor.transform.localScale.y-0.02f);
		}
		if (timer1s <= 1) { // это еще проще
<<<<<<< HEAD
			timer1s += Time.deltaTime;
				
=======
            timer1s += Time.deltaTime;		
			print (clicks1s);
            
            generateBonus();
>>>>>>> f542ff1175c75e23a8f34dbf1826ed3b88fb03f4
		}
        else { 
			float ips = (float) clicks1s;
			print (ips);
			ipsText.text = ips.ToString()+ " IP/s";
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
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				switch (hit.collider.name) {
				case "Back":
					animLeft.GetComponent<Animator> ().runtimeAnimatorController = leftClose;
					animRight.GetComponent<Animator> ().runtimeAnimatorController = rightClose;
					StartCoroutine (loadMainMenu ());
					break;
				case "Roscomnadzor":
					hit.collider.transform.localScale = new Vector2 (Roskomnadzor.transform.localScale.x + 0.1f, Roskomnadzor.transform.localScale.y + 0.1f);
					clicks1s++;
					clicks5s++;
					Preferences.setScore (Preferences.getScore()+multiplayer);
					break;
				case "Ad":
					print ("Ad");
					//запуск рекламы
					multipleX10 = 10;
					hit.collider.gameObject.SetActive (false);
					StartCoroutine (wait5Minutes());
					StartCoroutine (wait10Minutes());
					break;

				}
			}
		}
        if (Input.touchCount == 1)
        {
            if (isOnetouch)
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.name == "Roscomnadzor")
                    {
                        click();
                    }
                }
                isOnetouch = false;
            }
        }
        else if (Input.touchCount > 1)
        {
            if (isMultitouch)
            {
                Touch[] touches = Input.touches;
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Ray ray = mainCamera.ScreenPointToRay(touches[i].position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.name == "Roscomnadzor")
                        {
                            click();
                        }
                    }
                }
                isMultitouch = false;
            }
        }
        else
        {
            isMultitouch = true;
            isOnetouch = true;
        }
    }


    private void generateBonus()
    {
       // Image image = Instantiate(bonus, new Vector3(0f, -1f, 0f), new Quaternion()) as Image;
       // image.overrideSprite =  Resources.Load<Sprite>("Pictures/viber");
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

    private void updateScore() { //обновляем текст Score
		scoreText.text = Preferences.getScore().ToString();
    }

    private void updateMultiplayer() { //обновляем multiplayer и текст Multiplayer
		multiplayer = baseMultiplayer * multiplayer5sBonus* multipleX10;
        multiplayerText.text = "X" + multiplayer.ToString();
    }

    private void click() { // метод обработки клика, увеличиваем счёт и все нажатия
        Debug.Log("click");
        clicks1s++;
        clicks5s++;
        score += 1 * multiplayer;
    }

    IEnumerator loadMainMenu() {
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene ("Main menu");
	}

	IEnumerator wait5Minutes(){
		yield return new WaitForSeconds (30);//5 минут (изменил на 30 секунд для теста)
		multipleX10 =1;
	}
	IEnumerator wait10Minutes(){
		yield return new WaitForSeconds (60);//10 минут (изменил на 60 секунд для теста)
		adButton.SetActive(true);
	}
}
