//using EasyMobile;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class PausePopup : GenericPopup
{
  [SerializeField]
  private MyToggleController musicToggle;
  [SerializeField]
  private MyToggleController soundToggle;

  [SerializeField]
  private Button ClearPrefs;

  [SerializeField]
  private Text VersionText;

  [SerializeField]
  private LanguageSelectItem prefab;

  [SerializeField]
  private RectTransform languagesContainer;

  private List<LanguageSelectItem> languageItems;
  private LanguageKind selectedLanguage;

  private PausePopupData data = new PausePopupData();

  public class PausePopupData
  {
    public bool isOpenedFromActionPhase = false;
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void Activate(object data)
  {
#if UNITY_STANDALONE
    this.ClearPrefs.gameObject.SetActive(true);
#else
    this.ClearPrefs.gameObject.SetActive(false);
#endif

#if !UNITY_IOS
    //this.RestoreBtn.SetActive(false);
#endif
    this.VersionText.text = "v. " + Application.version;
    if (data != null && data is PausePopupData)
    {
      this.data = (PausePopupData) data;
    }


    //this.PrepareLanguageBtns();

    if (musicToggle != null)
    {
      musicToggle.SetState(Game.Settings.MusicEnabled);
    }
    if (soundToggle != null)
    {
      soundToggle.SetState(Game.Settings.SoundEnabled);
    }

  }
  //---------------------------------------------------------------------------------------------------------------
  public void ClearPrefsDebug()
  {
#if UNITY_STANDALONE
    PlayerPrefs.DeleteAll();
#endif
  }

  //---------------------------------------------------------------------------------------------------------------
  private void PrepareLanguageBtns()
  {
    languageItems = new List<LanguageSelectItem>();
    foreach (LanguageKind kind in Enum.GetValues(typeof(LanguageKind)))
    {
      LanguageSelectItem item = Instantiate(prefab, languagesContainer);
      item.Init(kind);
      item.ListenOnClick(OnItemClicked);
      languageItems.Add(item);

      if (Game.Settings.Language == kind)
      {
        item.SetAsSelected(true);
        selectedLanguage = kind;
      }
    }
  }
  //---------------------------------------------------------------------------------------------------------------
  private void OnItemClicked(LanguageKind kind)
  {
    selectedLanguage = kind;
    languageItems.Where(i => i.languageKind != kind).ToList().ForEach(i => i.SetAsSelected(false));
    Game.Settings.Language = selectedLanguage;
  }
  //---------------------------------------------------------------------------------------------------------------
  public override void CloseSelf()
  {
    base.CloseSelf();
  }


  //---------------------------------------------------------------------------------------------------------------
  public void OnSoundChanged(bool value)
  {
    Game.Settings.SoundEnabled = value;
  }

  public void OnMusicChanged(bool value)
  {
    Game.Settings.MusicEnabled = value;
  }



  //---------------------------------------------------------------------------------------------------------------
  public void OnExitClick()
  {
    //GameServices.ReportScore(Game.ActionRoot.actionPhaseSingleSessionDataStorage.DefeatedBlobs, EM_GameServicesConstants.Leaderboard_BlobsInOneRun);
    //GameServices.ReportScore(Game.ActionRoot.Score, EM_GameServicesConstants.Leaderboard_BestScore);

    //Game.StateManager.SetState(GameState.Menu);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnResumeClick()
  {

    CloseSelf();
  }



}
