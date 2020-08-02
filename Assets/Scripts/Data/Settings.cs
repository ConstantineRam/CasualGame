// This is the shell for Unity Player prefs.
using I2.Loc;
using System;
using System.Linq;


public class Settings
{
  [PlayerPrefsValue("GameProgress", 1)]
  private int gameProgress;
  public int GameProgress { get { return gameProgress; } set { gameProgress = value; SaveSettings(); } }

  [PlayerPrefsValue("SessionMoney", 0)]
  private int sessionMoney;
  public int SessionMoney { get { return sessionMoney; } set { sessionMoney = value; SaveSettings(); } }

  [PlayerPrefsValue("SoftCurrency", 0)]
  private int softCurrency;
  public int SoftCurrency { get { return softCurrency; } set { softCurrency = value; SaveSettings(); } }

  [PlayerPrefsValue("UnlocksShownSC", 0)]
  private int unlocksShownSC;
  [Obsolete("We may not use it in future. Consider removing.", false)]
  public int UnlocksShownSC { get { return unlocksShownSC; } set { unlocksShownSC = value; SaveSettings(); } }

  [PlayerPrefsValue("UIScale", 1f)]
  private float uIScale;
  public float UIScale { get { return uIScale; } set { uIScale = value; SaveSettings(); } }

  [PlayerPrefsValue("DevMode", false)]
  private bool devMode;
  public bool DevMode { get { return devMode; } set { devMode = value; SaveSettings(); } }

  [PlayerPrefsValue("isPremium", false)]
  private bool isPremium;
  public bool IsPremium { get { return isPremium; } set { isPremium = value; SaveSettings(); } }

  [PlayerPrefsValue("soundEnabled", true)]
  private bool soundEnabled;
  public bool SoundEnabled { get { return soundEnabled; } set { soundEnabled = value; Game.Events.SoundEnabled.Invoke(soundEnabled); SaveSettings(); } }

  [PlayerPrefsValue("musicEnabled", true)]
  private bool musicEnabled;
  public bool MusicEnabled { get { return musicEnabled; } set { musicEnabled = value; Game.Events.MusicEnabled.Invoke(musicEnabled); SaveSettings(); } }

  [PlayerPrefsValue("vibrationEnabled", true)]
  private bool vibrationEnabled;
  public bool VibrationEnabled { get { return vibrationEnabled; } set { vibrationEnabled = value; SaveSettings(); } }

  [PlayerPrefsValue("BestScore", 0)]
  private int bestScore;
  public int BestScore { get { return bestScore; } set { bestScore = value; Game.Events.BestScoreChanged.Invoke(bestScore); SaveSettings(); } }

  [PlayerPrefsValue("FirstLaunch", true)]
  private bool isFirstLaunch;
  public bool IsFirstLaunch { get { return isFirstLaunch; } set { isFirstLaunch = value; SaveSettings(); } }

  // Tutorial Manager
  [PlayerPrefsValue("TutorialManagerActive", true)]
  private bool isTutorialActive;
  public bool IsTutorialActive { get { return isTutorialActive; } set { isTutorialActive = value; SaveSettings(); } }

  // Tutorial Manager
  [PlayerPrefsValue("ActionTutorialFinished", false)]
  private bool isActionTutorialFinished;
  public bool IsActionTutorialFinished { get { return isActionTutorialFinished; } set { isActionTutorialFinished = value; SaveSettings(); } }

  // Tutorial Manager
  [PlayerPrefsValue("TutorialManagerStage", 0)]
  private int tutorialStage;
  public int TutorialStage { get { return tutorialStage; } set { tutorialStage = value; SaveSettings(); } }

  // Difficulty Manager
  [PlayerPrefsValue("WinCount", 0)]
  private int winCount;
  public int WinCount { get { return winCount; } set { winCount = value; SaveSettings(); } }



  public LanguageKind Language
  {
    get
    {
      LanguageKind kind;
      string lng = LocalizationManager.CurrentLanguage;
      if (Enum.GetNames(typeof(LanguageKind)).ToList().Contains(lng))
      {
        kind = (LanguageKind) Enum.Parse(typeof(LanguageKind), lng, true);
      }
      else
      {
        kind = LanguageKind.English;
      }


      return kind;
    }
    set
   {
      LocalizationManager.CurrentLanguage = value.ToString();
      Game.Events.LanguageChanged.Invoke(Language);
    }
  }

  public Settings()
  {
    LoadSettings();
  }

  public void LoadSettings()
  {
    PlayerPrefsUtils.Fill(this);
  }

  public void SaveSettings()
  {
    PlayerPrefsUtils.Save(this);
  }
}
