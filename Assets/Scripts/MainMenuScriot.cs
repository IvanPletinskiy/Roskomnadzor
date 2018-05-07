using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScriot : MonoBehaviour {

	public Camera mainCamera;
	public GameObject music;

	void Start () {
        initializeGPS();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				switch (hit.collider.name) {
				    case "Play":
    					SceneManager.LoadScene ("Play");
					    break;
                    case "ShowLeaderboard":
                        showLeaderboard();
                        break;
                    case "OpenMarket":
                        Application.OpenURL("https://play.google.com/store/apps/details?id=com.handen.roskomnadzor");
                        break;
                    case "Music":
						Preferences.setMusic(Preferences.isMusic());
						music.SetActive (!Preferences.isMusic ());
                        break;
                    }
			}
		}
	}

    private void initializeGPS() {
        // Рекомендовано для откладки:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Активировать Google Play Games Platform
        PlayGamesPlatform.Activate();
        // Аутентификация игрока:
        if (!Social.localUser.authenticated && 
            Application.internetReachability != NetworkReachability.NotReachable) {
            Social.localUser.Authenticate((bool isAuthenticated) => {

            });
        }
    }

    private void showLeaderboard()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            showToast("Проверьте ваше подключение к Интернету");
        }
        else
        {
            if (Social.localUser.authenticated)
            {
                Social.ShowLeaderboardUI();
                Social.ReportScore(Preferences.getScore(), "CgkIwpTli-UWEAIQAA", (bool success) => { });
            }
            else
            {
                showToast("Авторизация...");
                Social.localUser.Authenticate((bool isAuthenticated) =>
                {
                    if (isAuthenticated)
                    {
                        Social.ShowLeaderboardUI();
                        Social.ReportScore(Preferences.getScore(), "CgkIwpTli-UWEAIQAA", (bool success) => { });
                    }
                    else
                        showToast("Произошла ошибка, не получается открыть таблицу рекордов");
                });
            }
        }
    }
    
    private void showToast(string toastText)
    {
        //Перед использованием в плагине на сцену установить Prefab из папки Demo Плагина  
        AndroidDialogAndToastBinding.instance.toastShort(toastText);
    }
}
