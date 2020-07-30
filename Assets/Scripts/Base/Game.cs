//This is the Core of the framework. Developer can access most of the game logic by Game.RequiredManager.DoSomething().
// The rule of framework: Game has logic, GameData has consistent data loaded during LoadingScene.
// Game specific logic is stored in Roots, game unrelated logic in other managers (NOTE: Tutorial manager has game specific data that's why it's
// considered poor manager and should be reworked).
//
// Check managers to find out more about their functions.
using System;
using UnityEngine;
//using EasyMobile;
using UnityEngine.Scripting;
using I2;
using GameBase;

using GameBase.Localization;

public class Game : MonoBehaviour
{
  public static Boolean InitStarted { get; set; }
 // public static Boolean GameStarted { get; private set; }
  public static GameState PreferredFirstState { get; set; }

// Preserve is used because there is a change that these methods could be stripped by compiler when making build for iOS.
    [Preserve] public static StateManager StateManager { get; private set; }
    [Preserve] public static Settings Settings { get; private set; }
    [Preserve] public static AudioManager AudioManager { get; private set; }
    [Preserve] public static PoolManager PoolManager { get; private set; }
    [Preserve] public static UIManager UiManager { get; private set; }
    [Preserve] public static Events Events { get; private set; }
    [Preserve] public static TimeScaleProvider TimeScale { get; private set; }
    [Preserve] public static TimerManager TimerManager { get; private set; }
    [Preserve] public static CoroutineProvider CoroutineProvider { get; private set; }
    [Preserve] public static TrackingManager Tracking { get; private set; }
    [Preserve] public static TutorialManager TutorialManager { get; private set; }
    [Preserve] public static AdsManager AdsManager { get; private set; }




    [Preserve] public static Game_Camera Game_Camera { get; private set; }
    [Preserve] public static Camera Camera { get { return Game.Game_Camera.Camera;  } }

    [Preserve] public static Game_Canvas Game_Canvas { get; private set; }
    [Preserve] public static Canvas Canvas { get { return Game.Game_Canvas.Canvas; } }

    // [Preserve] public static IAPManager IAPManager { get; private set; }
    // [Preserve] public static AdsManager AdsManager { get; private set; }

    [Preserve] public static PlayRoot PlayRoot { get; set; }
    [Preserve] public static MenuRoot MenuRoot{ get; set; }

    //[Preserve] public static LocParam LocParam { get; private set; }
    [Preserve] public static SwipeManager Swipe { get; set; }

  public static bool IsDebug { get { return Debug.isDebugBuild; } }


  #region MonoBehaviour

  //---------------------------------------------------------------------------------------------------------------
  void Awake() //_Game uses only awake, so it will be initiated before everything else.
  {
 
    InitStarted = true;
    DontDestroyOnLoad(gameObject);


    Application.runInBackground = true;
    Application.backgroundLoadingPriority = ThreadPriority.Low;
    Screen.sleepTimeout = SleepTimeout.NeverSleep;
#if UNITY_STANDALONE
    Screen.SetResolution((int) (Screen.height * 9 / 16f), Screen.height, false);
#endif
    Application.targetFrameRate = 60;


    // Easy Mobile part. Remove if not used.
    //if (!RuntimeManager.IsInitialized())
    //{
    //  Debug.Log("starting EasyMobile");
    //  RuntimeManager.Init();

    //  if (!RuntimeManager.IsInitialized())
    //  {
    //    Debug.LogError("Easy Mobile not initialized.");
    //  }
    //}

    StateManager = GetComponentInChildren<StateManager>();
    AudioManager = GetComponentInChildren<AudioManager>();
    PoolManager = GetComponentInChildren<PoolManager>();
    UiManager = GetComponentInChildren<UIManager>();
    TutorialManager = GetComponentInChildren<TutorialManager>();

    Swipe = GetComponentInChildren<SwipeManager>();
    Game_Camera = GetComponentInChildren<Game_Camera>();
    Game_Canvas = GetComponentInChildren<Game_Canvas>();

    
    // LocParam = GetComponentInChildren<LocParam>();

    //  IAPManager = GetComponentInChildren<IAPManager>();
    AdsManager = GetComponentInChildren<AdsManager>();


    Settings = new Settings();
    Events = new Events();
    CoroutineProvider = new CoroutineProvider(this);
    TimeScale = new TimeScaleProvider();
    TimerManager = new TimerManager(TimeScale);
    Tracking = new TrackingManager();


    GameBase.UnityDataInitializer.CopyUnityData();
 

  }

  //---------------------------------------------------------------------------------------------------------------
  void Update()
  {
    if (TimeScale != null)
      TimeScale.Update();
  }

  //---------------------------------------------------------------------------------------------------------------
  void OnApplicationQuit()
  {
    Events.GameClosed.Invoke();
  }
  #endregion

}



