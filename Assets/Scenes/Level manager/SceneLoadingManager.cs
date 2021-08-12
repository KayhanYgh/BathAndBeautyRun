using FlurrySDK;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SceneLoadingManager : MonoBehaviour
{
    public bool autoLoad;
    public InputField levelInputField;

    private void Awake()
    {
        //PlayerSettings.iOS.hideHomeButton = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Start()
    {
        InitFlurry();

        if (autoLoad)
            AutoLoad();
    }

    private void AutoLoad()
    {
        if (PlayerPrefs.HasKey("LastLevel"))
        {
            var lastLevel = PlayerPrefs.GetInt("LastLevel");

            if (lastLevel > 5)
            {
                var sceneToLoad = Random.Range(2, 5);
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                SceneManager.LoadScene(lastLevel);
            }
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(int.Parse(levelInputField.text));
    }



#if UNITY_ANDROID
    private string FLURRY_API_KEY = "TVD9Q6Q2THQSPKGZSBPJ";
#elif UNITY_IPHONE
    private string FLURRY_API_KEY = "TVD9Q6Q2THQSPKGZSBPJ";
#else
    private string FLURRY_API_KEY = "TVD9Q6Q2THQSPKGZSBPJ";
#endif

    void InitFlurry()
    {
        // Initialize Flurry.
        new Flurry.Builder().WithCrashReporting(true).WithLogEnabled(true).WithLogLevel(Flurry.LogLevel.VERBOSE).WithMessaging(true)
                  .Build(FLURRY_API_KEY);
    }
}