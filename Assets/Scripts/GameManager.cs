using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameObject Player;
    public static GameObject Planet;

    public static bool IsStartPage;
    public static int TutorialStage = -1;
    public static int WaveNum = -1;
    //10: done
    //0: lift off
    //1: collect rocks
    //2: switch weapon
    //3: build planet
    //4: 

    public Wave[] waves;

    private Subscription<TutorialStageChangeEvent> tutorial_event_subscription;
    private Subscription<DeathEvent> death_event_subscription;
    private Subscription<WaveNumChangeEvent> wave_num_change_subscription;

    public GameObject blackPanel;

    private GameObject asteroidSpawner;
    private GameObject asteroidSpawners;

    private int[] resolution = new int[2] { 1920, 1080 };


    [System.Serializable]
    public class Wave
    {
        public int num;
        public float interval;
        public float error;
        public float speed;

        public override string ToString() {
            return "num=" + num + "; interval=" + interval + "; error=" + error + "; speed=" + speed;
        }
    }

    // only called once
    private void Start()
    {
        setResolution();
        asteroidSpawner = GameAssets.GetPrefab("AsteroidSpawner");
        Debug.Log("Start " + asteroidSpawner + Time.time);
        tutorial_event_subscription = EventBus.Subscribe<TutorialStageChangeEvent>(_OnTutorialStageChangeEvent);
        death_event_subscription = EventBus.Subscribe<DeathEvent>(_OnDeathEvent);
        wave_num_change_subscription = EventBus.Subscribe<WaveNumChangeEvent>(_OnWaveNumChangeEvent);
    }

    // every time before OnSceneLoaded
    private void Awake()
    {
        Debug.Log("awake" + Time.time);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public static void loadMainGame()
    {
        SceneManager.LoadScene("Main");
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name + Time.time);
        init();
        IsStartPage = scene.name == "Start";

        if (!IsStartPage && TutorialStage == -1)
        {
            EventBus.Publish(new TutorialStageChangeEvent(0));
            Debug.Log("first attempt");
        }
    }
    private void init()
    {
        //CurrentTarget = GameObject.Find("Planet 1");
        Player = GameObject.Find("Player");
        Planet = GameObject.Find("Planet");
        asteroidSpawners = GameObject.Find("AsteroidSpawners");
        Debug.Log(asteroidSpawners);
        if (Player == null || Planet == null)
        {
            //Debug.LogError("init error");
        }

        //GameObject lineGO = Instantiate(GameAssets.GetPrefab("Line"), Vector3.zero, Quaternion.identity);
        //LineController lineController = lineGO.GetComponent<LineController>();
        //lineController.a1 = CurrentTarget;
        //lineController.a2 = Player;
        //lineController.maxDistance = float.MaxValue;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus))
        {
            if (resolution[0] <= 4096)
            {
                resolution[0] += 160;
                resolution[1] += 90;
                setResolution();
            }
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            if (resolution[0] >= 540)
            {
                resolution[0] -= 160;
                resolution[1] -= 90;
                setResolution();
            }
        }
    }

    private void setResolution()
    {
        Screen.SetResolution(resolution[0], resolution[1], false);
    }

    public static void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void _OnTutorialStageChangeEvent(TutorialStageChangeEvent e)
    {
        TutorialStage = e.newTutorialStage;
        if (TutorialStage == 0)
        {
            //if (!asteroidSpawners)
            //    asteroidSpawners = GameObject.Find("AsteroidSpawners");
            ToastManager.ToastInstruction("hold space to lift off");
            Debug.Log(asteroidSpawner);
            GameObject go = Instantiate(asteroidSpawner, asteroidSpawners.transform);
            StartCoroutine(go.GetComponent<AsteroidSpawner>().SpawnAsteroidWave(200, 0, 0, 0, false));
        }
        else if (TutorialStage == 1)
            ToastManager.ToastInstruction("left click to fire");
        else if (TutorialStage == 2)
            ToastManager.ToastInstruction("collect rocks for ammo supply");
        else if (TutorialStage == 3)
            ToastManager.ToastInstruction("use number keys to\nswitch weapons");
        else if (TutorialStage == 4)
        {
            if (WaveNum < 5)
            {
                ToastManager.ToastInstruction("defend the planet!");
                EventBus.Publish(new WaveNumChangeEvent(0));
            }
            else
            {
                ToastManager.ToastInstruction("drag and drop moons on the bottom to defend against asteroids");
            }
        }
        else if (TutorialStage == 5) 
        {
            if (WaveNum == 5)
                ToastManager.ToastInstruction("hover over the moon and\nuse number keys\nto deploy or upgrade weapons");
        }
        else if (TutorialStage == 6)
        {
            if (WaveNum == 5)
                EventBus.Publish(new WaveNumChangeEvent(5));
        }
    }

    void _OnDeathEvent(DeathEvent e)
    {
        if (IsStartPage)
            return;

        if (TutorialStage <= 1 && e.deadGameObject.name == "StartAsteroid")
        {
            EventBus.Publish(new TutorialStageChangeEvent(2));
        } else if (TutorialStage <= 2 && e.deadGameObject.CompareTag("AsteroidDebris"))
        {
            EventBus.Publish(new TutorialStageChangeEvent(3));
        } else if (e.deadGameObject == Planet)
        {
            StartCoroutine(GameOver());
        }
    }

    void _OnWaveNumChangeEvent(WaveNumChangeEvent e)
    {
        WaveNum = e.newWaveNum;
        Debug.Log("wave: " + WaveNum);

        if (WaveNum == 5 && TutorialStage != 6)
        {
            EventBus.Publish(new TutorialStageChangeEvent(TutorialStage));
            return;
        }

        Wave w;
        if (WaveNum >= waves.Length)
            w = getNewWave();
        else
            w = waves[WaveNum];
        Debug.Log(w.ToString());
        //Debug.Log(asteroidSpawner);
        //Debug.Log(asteroidSpawners);
        GameObject go = Instantiate(asteroidSpawner, asteroidSpawners.transform);

        float delay = WaveNum < 10 ? WaveNum : 10;
        StartCoroutine(go.GetComponent<AsteroidSpawner>().SpawnAsteroidWave(w.num, w.interval, w.error, w.speed, delay:delay));
        if (WaveNum != 0)
        {
            ToastManager.ToastInstruction("wave " + WaveNum);
        }
    }

    private Wave getNewWave()
    {
        Wave w = new Wave();
        int d = WaveNum - waves.Length;
        Wave lastWave = waves[^1];
        w.num = lastWave.num + d * 5;
        w.interval = Mathf.Max(lastWave.interval - d * 0.05f, 0.05f);
        w.error = Mathf.Max(lastWave.error - d * 0.1f, 0);
        w.speed = Mathf.Min(lastWave.speed + d * 0.1f, 3);
        return w;
    }

    IEnumerator GameOver()
    {
        PlayerController pc = Player.GetComponent<PlayerController>();
        pc.freezeControl();

        if (WaveNum <= 20)
        {
            ToastManager.ToastInstruction("mission failed\nwave: " + WaveNum);
        }
        else
        {
            ToastManager.ToastInstruction("well done\nwave: " + WaveNum);
        }
        yield return new WaitForSeconds(10);
        restart();
    }
}

public class TutorialStageChangeEvent
{
    public int newTutorialStage;

    public TutorialStageChangeEvent(int _newTutorialStage) { newTutorialStage = _newTutorialStage; }
}

public class WaveNumChangeEvent
{
    public int newWaveNum;

    public WaveNumChangeEvent(int _newWaveNum) { newWaveNum = _newWaveNum; }
}