using GameBase;
using UnityEngine;
using UnityEngine.UI;

public class MenuRoot : RootBase
{
  [SerializeField]
  private Text LevelText;
  public readonly Signal UpdateUnlocksRequest = new Signal();
  #region Internal data

  #endregion

  #region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  private void Start()
  {
    this.LevelText.text = "Level " + Game.Settings.GameProgress.ToString();
    Game.Swipe.SetLimitsVertical(18, 21);
    Game.Swipe.SetLimitsHorisontal(-194, -155);
    Game.Swipe.SetInitiaAngle(-166, 12);
    if (!Game.AudioManager.HasActiveMusic())
    {
      Game.AudioManager.PlayMusic(AudioId.Music, loop: true, volume: DefaultContent.DefaultMusicVolume);
    }

    Game.MenuRoot = this;
    if (Game.Settings.IsFirstLaunch)
    {
      Game.StateManager.SetState(GameState.Play, withAnim: false);
      return;
    }


  }
  #endregion
  #region Internal logic
  //---------------------------------------------------------------------------------------------------------------
  private void StartAction()
  {
    Game.StateManager.SetState(GameState.Play, withAnim: false);
  }

  #endregion

  //---------------------------------------------------------------------------------------------------------------
  public void OnStartActionPressed()
  {
    Game.AudioManager.PlaySound(AudioId.ArcadeCLick);
    if (Game.Settings.GameProgress > DefaultContent.LevelsWithoutAds)
    {
      Game.UiManager.Open(PopupId.Ok, "You just enjoyed some cool ads.", CallBack: () => 
      { Game.AdsManager.Show(Game.MenuRoot.StartAction, Game.MenuRoot.StartAction, AdsType.VIDEO); });
      return;
    }

    this.StartAction();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OpenMenu()
  {
    Game.AudioManager.PlaySound(AudioId.ArcadeCLick);
    Game.UiManager.Open(PopupId.GameMenu);
  }


  //---------------------------------------------------------------------------------------------------------------
  public override void Unload()
  {
    Game.PlayRoot = null;
    base.Unload();
  }

}
