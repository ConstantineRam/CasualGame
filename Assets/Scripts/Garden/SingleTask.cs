using System.Collections;
using GameBase;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
  public const float TaskRenderScale = 25;
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
    this.TaskRender.transform.localScale = new Vector3(TaskRenderScale, TaskRenderScale, TaskRenderScale);
    this.TaskRender.transform.localPosition = new Vector3(0, -40, -60);
    this.TaskRender.transform.localRotation = Quaternion.identity;
    this.TaskRender.transform.localRotation =
       Quaternion.Euler(this.TaskRender.transform.localRotation.x-60,
       this.TaskRender.transform.localRotation.y+30,
       this.TaskRender.transform.localRotation.z);

    this.TaskRender.transform.SetAsFirstSibling();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void MakeFulfilled()
  {
    this.transform.DOPunchScale(new Vector3 (1.1f, 1.1f, 1.1f), 0.3f, 0,0);
    this.FullfilledImage.gameObject.SetActive(true);
    this.Fulfilled = true;
    Destroy(this.TaskRender.gameObject);
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
