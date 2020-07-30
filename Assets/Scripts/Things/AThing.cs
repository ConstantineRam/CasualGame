using System.Collections;
using GameBase;
using UnityEngine;

namespace Things
{

  // To save time we start it as mono. If needed we will make it poolable.
  /// <summary>
  /// Basic class for all clickable 3D objects for our hyper casual games.
  /// </summary>
  public abstract class AThing : MonoBehaviour
  {
    
    #region Internal data
    [SerializeField]
    [Tooltip("Assign MultModel script that contains models you want to object to use.")]
    private MultModel Models;
    #endregion

    public bool HasMultipleModels => this.Models != null;
    public SingleModel CurrentModel => this.HasMultipleModels ? this.Models.CurrentModel : null;
    // TODO: TBD
    //[SerializeField]
    //[Tooltip("Uncheck if you plan to manually assign model.")]
    //private bool RandomModel;
    //public bool IsRandomModel => this.RandomModel;

    //---------------------------------------------------------------------------------------------------------------
    public virtual void Vanish()
    {
      Destroy(this);
    }

    #region MonoBehaviour
    //---------------------------------------------------------------------------------------------------------------
    void Awake()
    {
      if (Models == null)
      {
        this.Models = this.GetComponent<MultModel>();
      }

      this.AssignModel();
      this.OnAwake();
    }

    //---------------------------------------------------------------------------------------------------------------
    void Update()
    {
      this.OnUpdate();
    }
    #endregion

    #region Private logic
    //---------------------------------------------------------------------------------------------------------------
    protected virtual void OnUpdate()
    {
      if (TouchSimulator.Input.touches.Length <= 0)
      {
        return;
      }

      Touch t = TouchSimulator.Input.GetTouch(0);
      if (t.phase == TouchPhase.Moved)
      {
        return;
      }

      Vector2 vTouchPos = t.position;
      Ray ray = Game.Camera.ScreenPointToRay(vTouchPos);

      RaycastHit vHit;
      if (!Physics.Raycast(ray.origin, ray.direction, out vHit))
      {
        return;
      }

      if (vHit.collider.gameObject != this.gameObject)
      {
        return;
      }

      this.OnClick();
    }
    //---------------------------------------------------------------------------------------------------------------
    private void AssignModel()
    {
      if (!this.HasMultipleModels)
      {
        return;
      }

      this.Models.InitRandomModel();
    }

    #endregion

    #region Protected logic
    //---------------------------------------------------------------------------------------------------------------
    protected virtual void OnAwake() {}
    //---------------------------------------------------------------------------------------------------------------
    protected abstract void OnClick();
    #endregion
  }
}
