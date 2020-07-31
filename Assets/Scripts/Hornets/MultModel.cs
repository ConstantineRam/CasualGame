using System.Collections;
using System;
using UnityEngine;

namespace GameBase
{
  public class MultModel : MonoBehaviour
  {
    #region Internal data
    [SerializeField]
    private SingleModel[] ModelPrefabs;
    public SingleModel CurrentModel { get; private set; } = null;
    protected GameObject ModelObject { get; private set; } = null;

    public bool HasActiveModel => this.CurrentModel != null;
    public int CurrentModelNum { get; private set; }

    #endregion

    //---------------------------------------------------------------------------------------------------------------
    public void InitRandomModel()
    {
      this.ClearModel();

      this.CurrentModelNum = UnityEngine.Random.Range(0, this.ModelPrefabs.Length);
      this.CurrentModel = this.ModelPrefabs[this.CurrentModelNum];
      this.ModelObject = GameObject.Instantiate(this.CurrentModel.GetModelObject, this.transform);
      if (this.ModelObject == null)
      {
        this.CurrentModel = null;
        Debug.LogError("Can't Instantiate model.");
        return;
      }

      this.ModelObject.transform.localPosition = Vector3.zero;
      this.name = this.ModelObject.name;

    }

    #region Private Logic
    //---------------------------------------------------------------------------------------------------------------
    private void ClearModel()
    {
      if (this.HasActiveModel)
      {
        Destroy(this.ModelObject);
        this.CurrentModel = null;
        this.name = DefaultContent.blank;
      }
    }
    #endregion
  }
  [Serializable]
  public class SingleModel
  {
    [SerializeField]
    private GameObject ModelObject;
    public GameObject GetModelObject => this.ModelObject;
  }

  public static class SingleModelUtils
  {
    public static bool IsNull(this SingleModel sm) => sm == null;
    public static bool IsEqual(this SingleModel sm, SingleModel Compare)
    {
      if (sm.IsNull())
      {
        return false;
      }

      if (Compare.IsNull())
      {
        return false;
      }

      if (sm.GetModelObject == Compare.GetModelObject)
      {
        return true;
      }

      return false;
    }
  }
}