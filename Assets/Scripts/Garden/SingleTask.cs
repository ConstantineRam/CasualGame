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
  [SerializeField]
  public GameObject TaskRender;
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
    this.TaskRender = GameObject.Instantiate(this.Model.GetModelObject, this.transform);
    this.TaskRender.transform.localScale = new Vector3(40, 40, 40);
    this.TaskRender.transform.rotation = Quaternion.identity;
    this.TaskRender.transform.SetAsFirstSibling();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void MakeFulfilled()
  {
    this.FullfilledImage.gameObject.SetActive(true);
    this.Fulfilled = true;
    this.TaskRender.transform.localScale = new Vector3(10, 10, 10);
    this.FullfilledImage.transform.SetAsLastSibling();
  }
  #region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  void Start()
  {
    this.FullfilledImage.gameObject.SetActive(false);
  }
  #endregion
}
