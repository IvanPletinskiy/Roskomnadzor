using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayScript : MonoBehaviour {


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

    private int score; //счёт для надписи Score 
    private int clicks1s; //кликов за прошлую секунду
    private int clicks5s; //кликов за 5 секунд
    private int baseMultiplayer = 1; //базовый множитель, увеличивается при достижении отметки 100, 1000, 10000 ip
    private int multiplayer5sBonus = 1; // множитель клика за 5 секунд (чем чаще кликает пользователь, тем он больше) обновляется каждые 5 сек
    private int multiplayer = 1; //итоговый множитель

    bool isMultitouch;
    bool isOnetouch;

    void Start () {
        multiplayer = Preferences.getMultiplayer();
		animBackground.SetActive (true);
        StartCoroutine("updateIPS");
        StartCoroutine("multiplayer5s");
    }

	void Update () {
        updateScore();
        updateMultiplayer();
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

    private void updateScore() { //обновляем текст Score
        scoreText.text = score.ToString();
    }

    private void updateMultiplayer() { //обновляем multiplayer и текст Multiplayer
        multiplayer = baseMultiplayer * multiplayer5sBonus;
        multiplayerText.text = "X" + multiplayer.ToString();
    }

    private void click() { // метод обработки клика, увеличиваем счёт и все нажатия
        Debug.Log("click");
        clicks1s++;
        clicks5s++;
        score += 1 * multiplayer;
    }

    IEnumerator updateIPS() //корутина обновления ips, вызывается каждую сек
    {
        yield return new WaitForSeconds(1f);
        ipsText.text = (clicks1s * multiplayer).ToString() + " ip/s";
        clicks1s = 0;
        StartCoroutine("updateIPS");
    }

    IEnumerator multiplayer5s() //корутина обновления multiplayer5s, вызывается каждые 5 сек
    {
        yield return new WaitForSeconds(5f);
        double myDouble = 20 / clicks5s;
        int newMultiplayerBonus = (int) (myDouble * 5);
        if (newMultiplayerBonus > multiplayer5sBonus) {
            multiplayer5sBonus++;
 //           updateMultiplayer();
        }
        if (newMultiplayerBonus < multiplayer) {
            if(multiplayer5sBonus != 1)
                multiplayer5sBonus--;
//            updateMultiplayer();
        }
        clicks5s = 0;
        StartCoroutine("multiplayer5s");
    }

    IEnumerator loadMainMenu() {
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene ("Main menu");
	}
}
