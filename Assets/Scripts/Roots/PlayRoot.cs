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
    Game.Swipe.ResetLimits();
    Game.Swipe.SetInitiaAngle(135);
    Game.Events.SessionEnded.Listen(this.SessionEnd);
    Game.PlayRoot = this;
    Game.Settings.SessionMoney = 0;
    if (!Game.AudioManager.HasActiveMusic())
    {
      Game.AudioManager.PlayMusic(AudioId.Music, loop: true);
    }

    this.TasksController.CreateTasks();

    

    if (Game.Settings.IsFirstLaunch)
    {
      FlexiblePopUp.Instantiate("StartTip", Game.Canvas.transform, lockTime: 1.6f);
      Game.Settings.IsFirstLaunch = false;
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

    FlexiblePopUp.Instantiate("SessionResult", Game.Canvas.transform, lockTime: 1f ).OnClose(()=> { Game.StateManager.SetState(GameState.Menu); } );
    Game.AudioManager.PlaySound(AudioId.GongLow);
    
  }
  #endregion








}
