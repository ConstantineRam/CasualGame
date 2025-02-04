﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class YesNoPopup : GenericPopup
{

  [SerializeField]
  protected Text infoText;

  private YesNoPopupData data;

  public override void Activate(object data)
  {
    if (data is YesNoPopupData)
    {
      this.data = (YesNoPopupData) data;
      infoText.text = this.data.text;
    }
    else
      this.data = new YesNoPopupData();
  }

  public virtual void ButtonYesClick()
  {
    if (data.confirmAction != null)
    {
      data.confirmAction.Invoke();
    }
    CloseSelf();
  }

  public virtual void ButtonNoClick()
  {
    if (data.cancelAction != null)
    {
      data.cancelAction.Invoke();
    }

      CloseSelf();
  }


  public class YesNoPopupData
  {
    public Action confirmAction;
    public Action cancelAction;
    public string text;
  }
}
