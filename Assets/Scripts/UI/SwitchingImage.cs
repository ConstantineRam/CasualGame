using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  public class SwitchingImage : BasicMono
  {
    [SerializeField]
    private Image MyImage;
    [SerializeField]
    private Sprite SwitchSprite;
    public bool HasSwitchSprite => this.SwitchSprite != null;
    private Sprite InitialSprite;
    private Sprite SecondarySprite;
    //---------------------------------------------------------------------------------------------------------------
    public void Switch(UsedSprite sprite)
    {
      if (sprite == UsedSprite.secondary)
      {
        this.SwitchSprite = this.InitialSprite;
        this.MyImage.sprite = this.SecondarySprite;
        return;
      }


      this.SwitchSprite = this.SecondarySprite;
      this.MyImage.sprite = this.InitialSprite;

    }

    //---------------------------------------------------------------------------------------------------------------
      public void Switch()
    {
      Sprite temp = this.MyImage.sprite;
      this.MyImage.sprite = this.SwitchSprite;
      this.SwitchSprite = temp;
    }
    #region Protected
    //---------------------------------------------------------------------------------------------------------------
    protected override void OnAwake()
    {
      base.OnAwake();
      if (this.MyImage == null)
      {
        this.MyImage = this.GetComponent<Image>();
      }

      if (!this.HasSwitchSprite)
      {
        Debug.LogError ("SwitchingImage at "+ this.name + " got null at switching sprite.");
      }

      this.InitialSprite = this.MyImage.sprite;
      this.SecondarySprite = this.SwitchSprite;
    }
    #endregion

    public enum UsedSprite
    {
      initial = 0,
      secondary = 1
    }
  }
}
