//using I2.Loc;
using System.Collections;
using UnityEngine;

public class LoadingScript : MonoBehaviour
{
  [SerializeField] private Game gameRef;

  [Space]
  [SerializeField] private float gameLogoDelay = 0f;



  #region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  private void Awake()
  {

#if UNITY_EDITOR
    Debug.Log("*************LOADING SCRIPT STARTED ***************");
#endif
    Instantiate(gameRef);
    StartCoroutine(WaitAndInitGame());

#if UNITY_STANDALONE
    this.gameLogoDelay = 0f;
#endif

#if UNITY_EDITOR
    Debug.Log("*************LOADING SCRIPT DONE ***************");
#endif
  }

  //---------------------------------------------------------------------------------------------------------------
  void Start()
  {

  }

  #endregion


  //---------------------------------------------------------------------------------------------------------------
  private IEnumerator WaitAndInitGame()
  {
    yield return new WaitForSeconds(gameLogoDelay);

    Game.Events.GameStarted.Invoke();

    if (Game.Settings.IsFirstLaunch)
    {
      Game.StateManager.SetState(GameState.Play);
    }
    else
    {
      Game.StateManager.SetState(GameState.Menu);
    }

    // Game.StateManager.SetState(GameState.Action);
    // Game.StateManager.SwitchToFirstState();
  }
}
