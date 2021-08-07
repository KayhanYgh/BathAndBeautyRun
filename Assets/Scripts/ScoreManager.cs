using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Score
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;
        public bool isDemo;
        public bool isStarted;
        public bool Finished;
        public bool Won;
        public bool Lost;
        public Player.Player player;
        public List<Player.Player> npcs;

        [TabGroup("Progress")] public bool showTutorial;
        [TabGroup("Progress")] public GameObject progressMenu;
        [TabGroup("Progress")] public GameObject playerTouch;
        [TabGroup("Progress")] public GameObject startMenu;
        [TabGroup("Progress")] public GameObject tutorialMenu;
        [TabGroup("Progress")] public Transform startPoint;
        [TabGroup("Progress")] public Transform playerPoint;
        [TabGroup("Progress")] public Transform endPoint;
        [TabGroup("Progress")] public Slider distanceSlider;
        [TabGroup("Progress")] public Text scoreText;
        [TabGroup("Progress")] public Text currentLevelText;
        [TabGroup("Progress")] public Text nextLevelText;

        [TabGroup("Win ")] public GameObject winMenu;

        [TabGroup("Lose ")] public GameObject loseMenu;


        [TabGroup("Events")] public UnityEvent onWin;
        [TabGroup("Events")] public UnityEvent onLose;
        [TabGroup("Events")] public UnityEvent onStart;

        private void OnEnable()
        {
            Instance = this;
        }

        private void Start()
        {
            TinySauce.OnGameStarted();
            SetProgressBarTexts();

            distanceSlider.maxValue = Vector3.Distance(startPoint.position, endPoint.position);
            progressMenu.SetActive(false);
            playerTouch.SetActive(false);

            if (showTutorial)
            {
                tutorialMenu.SetActive(true);
                startMenu.SetActive(false);
            }
            else
            {
                tutorialMenu.SetActive(false);
                startMenu.SetActive(true);
            }
        }

        private int _currentLevelIndex;
        private int _nextLevelIndex;

        private void SetProgressBarTexts()
        {
            if (PlayerPrefs.HasKey("LastLevel"))
            {
                _currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
                PlayerPrefs.SetInt("LastLevel", _currentLevelIndex);
            }
            else
            {
                _currentLevelIndex = 1;
            }

            _nextLevelIndex = _currentLevelIndex + 1;
            currentLevelText.text = _currentLevelIndex.ToString();
            nextLevelText.text = _nextLevelIndex.ToString();
        }

        private int _frames;

        private void Update()
        {
            FindAllAliveNpcs();

            distanceSlider.value = Vector3.Distance(startPoint.position, playerPoint.position);

            _frames++;

            if (_frames >= 10)
            {
                try
                {
                    foreach (var item in npcs)
                    {
                        if (item._died)
                        {
                            if (npcs.Contains(item))
                            {
                                npcs.Remove(item);
                            }
                        }
                    }

                    if (npcs.Count <= 0 && EnemySpawner.Instance.remaining <= 0)
                    {
                        if (!Won)
                        {
                            player.StartCoroutine("SlowMotionProcess");
                        }

                        Win("");
                    }
                }
                catch
                {
                    //ignore
                }

                _frames = 0;
            }
        }

        private void FindAllAliveNpcs()
        {
            foreach (var item in FindObjectsOfType<Player.Player>())
            {
                if (!item.isPlayer)
                {
                    if (item._died)
                    {
                        if (npcs.Contains(item))
                        {
                            npcs.Remove(item);
                        }
                    }
                    else
                    {
                        if (!npcs.Contains(item))
                        {
                            npcs.Add(item);
                        }
                    }
                }
            }
        }

        public void Win(string reason)
        {
            if (slowMotionOnWin)
            {
                StartCoroutine(nameof(WinSlowMotion));
            }

            TinySauce.OnGameFinished(0);
            Finished = true;
            Won = true;
            Lost = false;
            Debug.Log(reason);
            winMenu.SetActive(true);


            onWin?.Invoke();

            if (isDemo)
            {
                StartCoroutine(nameof(LoadZeroScene));
            }
        }

        private IEnumerator LoadZeroScene()
        {
            yield return new WaitForSeconds(6);
            SceneManager.LoadScene(0);
        }

        public void Lose(string reason)
        {
            TinySauce.OnGameFinished(1);
            Finished = true;
            Won = false;
            Lost = true;
            Debug.Log(reason);
            onLose?.Invoke();

            loseMenu.SetActive(true);

            if (isDemo)
            {
                StartCoroutine(nameof(LoadZeroScene));
            }
        }

        public void StartGame()
        {
            isStarted = true;
            startMenu.SetActive(false);
            tutorialMenu.SetActive(false);
            progressMenu.SetActive(true);
            playerTouch.SetActive(true);
            onStart?.Invoke();
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(_currentLevelIndex);
        }

        public void LoadNextLevel()
        {
            PlayerPrefs.SetInt("LastLevel", _nextLevelIndex);

            if (_currentLevelIndex >= 2)
            {
                var randomLevel = Random.Range(1, 7);
                SceneManager.LoadScene(randomLevel);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        public bool slowMotionOnWin;

        private IEnumerator WinSlowMotion()
        {
            Time.timeScale = 0.3f;

            yield return new WaitForSecondsRealtime(2);
            Time.timeScale = 1f;
        }
    }
}