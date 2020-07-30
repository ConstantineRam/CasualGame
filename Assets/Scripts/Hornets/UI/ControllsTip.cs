using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControllsTip : MonoBehaviour
{
  #region Internal data
  [SerializeField]
  private Image Left;
  [SerializeField]
  private Image Right;
  private readonly Color FadeColor = new Color(1f, 1f, 1f, 0.4f);
  private readonly Color FullColor = new Color(1f, 1f, 1f, 0.9f);
  #endregion;
  #region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  private void Start()
  {
    Game.Events.DirectionChanged.Listen(this.ShowTip);
  }

  #endregion
  //---------------------------------------------------------------------------------------------------------------
  public void ShowTip(Direction dir)
  {
    this.Left.color = this.FadeColor;
    this.Right.color = this.FadeColor;
    if (dir == Direction.None)
    {
      return;
    }

    if (dir == Direction.Left)
    {
      this.Left.color = this.FullColor;
      return;
    }

    if (dir == Direction.Right)
    {
      this.Right.color = this.FullColor;
      return;
    }
  }
}
