using System.Collections;
using GameBase;
using UnityEngine;
using UnityEngine.UI;

public class SingleTask : MonoBehaviour
{
  #region Internal data
  public bool Fulfilled { get; private set; }
  [SerializeField]
  public Image FullfilledImage;
  [SerializeField]
  public Image TaskImage;

  public SingleModel Model { get; private set; }
  #endregion

  //---------------------------------------------------------------------------------------------------------------
  public void AssignTask(SingleModel model)
  {
    if (model.IsNull())
    {
      Debug.LogError("Attempt to assign null model to task.");
      return;
    }
    this.Model = model;
    this.TaskImage.sprite = this.Model.GetUISprite;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void MakeFulfilled()
  {
    this.FullfilledImage.gameObject.SetActive(true);
  }
  #region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  void Start()
  {
    this.FullfilledImage.gameObject.SetActive(false);
  }
  #endregion
}
