using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Revive : MonoBehaviour
{
  [SerializeField]
  private Button button;
  [SerializeField]
  private Text text;
  private FlexiblePopUp Caller;
  public bool HasCaller => !this.Caller.IsNull();

  //---------------------------------------------------------------------------------------------------------------
  void OnEnable()
  {

    if (Game.AdsManager.IsAvailable())
    {
      this.button.interactable = true;
      this.text.text = "Watch Ad to revive";
      return;
    }

    this.button.interactable = false;
    this.text.text = "No video Avaliable";
  }

  //---------------------------------------------------------------------------------------------------------------
  public void AssignCaller(FlexiblePopUp popup)
  {

    if (popup.IsNull())
    {
      this.gameObject.SetActive(false);
      return;
    }

    this.Caller = popup;
  }


  //---------------------------------------------------------------------------------------------------------------
  public void OnButtonPressed()
  {
    if (this.HasCaller)
    {
      this.Caller.Kill();
      this.Caller = null;
    }

    if (!Game.AdsManager.IsAvailable())
    {
      return;
    }

    Game.AdsManager.Show(VideoShown, VideoNotShown);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void VideoShown()
  {
    Game.PlayRoot.UnPauseSession();
    this.Caller = null;
    this.gameObject.SetActive(false);
  }
  //---------------------------------------------------------------------------------------------------------------
  public void VideoNotShown()
  {
    this.Caller = null;
    Game.Events.SessionEnded.Invoke();
  }
}
