using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AppodealAds.Unity.Api;

public class MainMenuScript : MonoBehaviour {

	public Camera mainCamera;
	public GameObject music;

	void Start () {
        initializeGPS();
		initializeAd ();
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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
		#if UNITY_ANDROID
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
		#endif
    }

	public void initializeAd()
	{
		Appodeal.disableLocationPermissionCheck();
		Appodeal.disableWriteExternalStoragePermissionCheck();
		//if (!Appodeal.isLoaded(Appodeal.NON_SKIPPABLE_VIDEO))
		{
			//if(Appodeal.isLoaded())
			Appodeal.disableNetwork("inmobi");
			string appKey = "4eb13d8e55c04d001c7d2c5214c815e881c9a95c0a15f8f9";
			Appodeal.initialize(appKey, Appodeal.NON_SKIPPABLE_VIDEO);
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
