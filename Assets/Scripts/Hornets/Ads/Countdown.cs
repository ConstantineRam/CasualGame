using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
  #region Internal data
  [SerializeField]
  private FlexiblePopUp MyPopup;
  [SerializeField]
  private TextMesh Text;
  #endregion

  #region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  void Start()
    {
    this.Text.text = Mathf.Round(this.MyPopup.LockTime).ToString();
    }

  //---------------------------------------------------------------------------------------------------------------
  void Update()
    {
    this.Text.text = Mathf.Round(this.MyPopup.RemainingLockTime).ToString();
    if (this.MyPopup.RemainingLockTime <= 3)
    {
      Text.color = Color.red;
    }
  }
  #endregion
}
