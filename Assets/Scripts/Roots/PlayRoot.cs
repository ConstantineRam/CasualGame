using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using GameBase;
using Things;
using UI;

public class PlayRoot : RootBase
{
  #region data

  [SerializeField]
  private WorldController WorldsStorage;

  public AWorld World { get; private set; }

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

  private bool SessionEnded = false;

  #endregion

  #region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  protected override sealed void OnAwake()
  {
    this.World = this.WorldsStorage.Activate(Game.Settings.GameProgress);

  }

  //---------------------------------------------------------------------------------------------------------------
  private void Start()
  {
    
    Game.Swipe.ResetLimits();
    Game.Swipe.SetInitiaAngle(135);
    Game.Events.GameWon.Listen(this.SessionEnd);
    Game.Events.GameLost.Listen(this.SessionFailed);

    Game.PlayRoot = this;
    Game.Settings.SessionMoney = 0;
    if (!Game.AudioManager.HasActiveMusic())
    {
      Game.AudioManager.PlayMusic(AudioId.Music, loop: true, volume: DefaultContent.DefaultMusicVolume);
    }

    this.Things.ProcessChildrenThings();
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
  private void SessionFailed()
  {
    if (this.SessionEnded)
    {
      return;
    }
    this.SessionEnded = true;

    this.PauseSession();
    Game.Settings.SessionMoney = 0;
    FlexiblePopUp.Instantiate("SessionFailed", Game.Canvas.transform, lockTime: 1f).OnClose(() => { Game.StateManager.SetState(GameState.Menu); });
    Game.AudioManager.PlaySound(AudioId.ArcadeNegative07);

  }

  //---------------------------------------------------------------------------------------------------------------
  private void SessionEnd()
  {
    if (this.SessionEnded)
    {
      return;
    }
    this.SessionEnded = true;
    Game.Settings.GameProgress = Game.Settings.GameProgress + 1;
    this.PauseSession();

    (int stars, int rewardPerStar) reward = this.TasksController.GetStarsAndReward();
    SessionRewardScreen.SessionData sd = new SessionRewardScreen.SessionData
    {
      StartingAmount = 0,
      PushedAmount = Game.Settings.SessionMoney,
      Stars = reward.stars,
      RewardPerStar = reward.rewardPerStar
    };

    FlexiblePopUp.Instantiate("SessionResult", Game.Canvas.transform, lockTime: 1f, data: sd).OnClose(()=> { Game.StateManager.SetState(GameState.Menu); } );
    Game.AudioManager.PlaySound(AudioId.ArcadePositive08);
    
  }
  #endregion








}
