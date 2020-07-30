// State manager function is to switch Unity Scenes and manage smooth transition.
// SetState is only method needed by developer from this method.
using Assets.Scripts.Utils.ExtensionMethods;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
  private SwitchSceneScreen switchSceneScreen;
  private AsyncOperation asyncOperation;


  public GameState CurrentState { get; private set; }
  public RootBase ActiveRoot { get { return CurrentState.ToRootBase(); } }
  public Signal<GameState> OnStateChanged = new Signal<GameState>();

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Switches game to specified scene (state).
  /// </summary>
  /// <param name="targetState"></param>
  /// The scene (state) you want to switch to. If you try to swith to the same state that is currently running and restartState == false, this call would be ignored.
  /// <param name="withAnim"></param>
  /// Shows switch animation if true.
  /// <param name="restartState"></param>
  /// Set it to True if you want to call the same state game is currently in and you want to reload and restart it.
  public void SetState(GameState targetState, bool withAnim = true, bool restartState = false)
  {

    if (CurrentState == targetState && !restartState)
    {
      return;
    }

    Game.TimerManager.HaltAll();
    Game.UiManager.CloseAll();

    StartCoroutine(EnableSceneAsync(targetState, withAnim));
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  ///  Used by Loading LoadingScript at the very start of the game. There is no need to use this method by Developer. Use SetState to switch states instead. 
  /// </summary>
  public void SwitchToFirstState()
  {
    GameState state = Game.PreferredFirstState == GameState.None ? GameStateEx.FindFirstState() : Game.PreferredFirstState;
    SetState(state, true, true);
  }

  //---------------------------------------------------------------------------------------------------------------
  private IEnumerator EnableSceneAsync(GameState targetState, bool withAnim = true)
  {

    // Spawning prefab with scene change animation
    if (switchSceneScreen == null && withAnim)
    {
      SpawnSwitchScreen();
    }


    // start scene loading 
    asyncOperation = SceneManager.LoadSceneAsync(targetState.ToSceneName(), LoadSceneMode.Single);
    // switch off autoactivation
    asyncOperation.allowSceneActivation = false;

    // wait until operation ends
    while (asyncOperation.progress < 0.9f)
    {
      yield return null;
    }

    // play animation if needed
    if (withAnim)
    {
      var wait = true;
      // run FadeIn animation.
      switchSceneScreen.FadeIn(() => wait = false);
      // waiting for callback.
      yield return new WaitWhile(() => wait);
    }

    // Unloading active root (it will turns off it's children)
    if (ActiveRoot != null)
      ActiveRoot.Unload();

    // cleaning everything and closing pop ups.
    GC.Collect();
    Resources.UnloadUnusedAssets();
    Game.UiManager.CloseAll();

    // allow animation of  new scene
    asyncOperation.allowSceneActivation = true;
    // updating CurrentState и ActiveRoot
    CurrentState = targetState;

    // Loading new root (root will take care about its objects)
    if (ActiveRoot != null)
      ActiveRoot.Load();

    // FadeOut animations
    if (withAnim)
    {
      switchSceneScreen.FadeOut();
    }

    Game.UiManager.StartQueueProcessor();
    OnStateChanged.Invoke(CurrentState);
  }

  //---------------------------------------------------------------------------------------------------------------
  private void SpawnSwitchScreen()
  {
    SwitchSceneScreen switchSceneRef = Resources.LoadAll<SwitchSceneScreen>("").First();
    if (switchSceneRef == null)
    {
      Debug.LogError("<color=red>Cant find [SwitchSceneScreen]\n</color>");
      return;
    }

    switchSceneScreen = Instantiate(switchSceneRef, transform).GetComponent<SwitchSceneScreen>();
  }

}
