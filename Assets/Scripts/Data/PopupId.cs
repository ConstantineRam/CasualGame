using System;

public enum PopupId
{
  NoPopUp = -1,

  first,
  [Description("Prefabs/UI/Popups/YesNoPopup")]
  YesNo,
  [Description("Prefabs/UI/Popups/OkPopup")]
  Ok,
  [Description("Prefabs/UI/Popups/PausePopup")]
  GameMenu,
  [Description("Prefabs/UI/Popups/GameOver")]
  GameOver,
  [Description("Prefabs/UI/Popups/GameWon")]
  GameWon,
  [Description("Prefabs/UI/Popups/Tutorial PopUp")]
  Tutorial,
  [Description("Prefabs/UI/Popups/SaveFileMissing")]
  SaveFileMissing,

  //Hornets
  [Description("Prefabs/UI/Popups/ShowUnlocksProgress")]
  ShowUnlocksProgress,

  lastPlusOne,
}



