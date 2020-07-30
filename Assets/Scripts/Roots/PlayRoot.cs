using UnityEngine;
using UnityEngine.UI;
using GameBase;
using Things;

public class PlayRoot : RootBase
{
  #region data

  [SerializeField]
  private TasksController TasksController;
  public TasksController Tasks => this.TasksController;

  [SerializeField]
  private ThingsController ThingsController;
  public ThingsController Things => ThingsController;

  [SerializeField]
  private Transform coinAcceptor;
  public Transform CoinAcceptor => this.coinAcceptor;

  [SerializeField]
  private CurrencyController currency;
  public CurrencyController Currency => currency;

  public readonly Signal<Veggie> VeggieClicked = new Signal<Veggie>();

  #endregion

  #region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  private void Start()
  {
    Game.PlayRoot = this;
    Game.Settings.SessionMoney = 0;
    if (!Game.AudioManager.HasActiveMusic())
    {
      Game.AudioManager.PlayMusic(AudioId.Music, loop: true);
    }

    this.PauseSession();
    this.TasksController.CreateTasks();
    return;

    Game.Events.SessionEnded.Listen(this.SessionEnd);

    if (Game.Settings.IsFirstLaunch)
    {

    }
  }
  #endregion


  //---------------------------------------------------------------------------------------------------------------
    public void PauseSession()
  {
    Game.TimerManager.PauseAll(TimeScaleLayer.Enemy);
    Game.TimerManager.PauseAll(TimeScaleLayer.Player);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void UnPauseSession()
  {

    Game.TimerManager.ResumeAll (TimeScaleLayer.Enemy);
    Game.TimerManager.ResumeAll(TimeScaleLayer.Player);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OpenMenu()
  {
    Game.UiManager.Open(PopupId.GameMenu);
  }


  //---------------------------------------------------------------------------------------------------------------
  public override void Unload()
  {

    Game.PlayRoot = null;
    base.Unload();
  }

  #region Internal logic
  //---------------------------------------------------------------------------------------------------------------
  private void SessionEnd()
  {
    this.PauseSession();


    Game.AudioManager.PlaySound(AudioId.GongLow);
    Game.TimerManager.Start(0.5f, () => {Game.StateManager.SetState(GameState.Menu); });
    
  }
  #endregion








}
