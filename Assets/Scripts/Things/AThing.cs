using System.Collections;
using GameBase;
using UnityEngine;

namespace Things
{

  // To save time we start it as mono. If needed we will make it poolable.
  /// <summary>
  /// Basic class for all clickable 3D objects for our hyper casual games.
  /// </summary>
  public abstract class AThing : MonoBehaviour, IVanishable
  {
    
    #region Internal data
    [SerializeField]
    [Tooltip("Assign MultModel script that contains models you want to object to use.")]
    protected MultModel Models;
    #endregion

    public bool HasMultipleModels => this.Models != null;
    public SingleModel CurrentModel => this.HasMultipleModels ? this.Models.CurrentModel : null;

    [SerializeField]
    private Collider MyCollider;
    // TODO: TBD
    //[SerializeField]
    //[Tooltip("Uncheck if you plan to manually assign model.")]
    //private bool RandomModel;
    //public bool IsRandomModel => this.RandomModel;

    //---------------------------------------------------------------------------------------------------------------
    private bool Vanished = false;
    public bool IsVanished { get => this.Vanished; protected set => this.Vanished = value; }

    //---------------------------------------------------------------------------------------------------------------
    public virtual void Vanish()
    {
      if (this.IsVanished)
      {
        return;
      }
      this.Vanished = true;
      Destroy(this.gameObject);
    }

    #region MonoBehaviour
    //---------------------------------------------------------------------------------------------------------------
    void Awake()
    {
      if (Models == null)
      {
        this.Models = this.GetComponent<MultModel>();
      }

      if (this.MyCollider == null)
      {
        this.MyCollider = this.GetComponent<Collider>();
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
    protected void DisableCollider()
    {
      this.MyCollider.enabled = false;
    }
    //---------------------------------------------------------------------------------------------------------------
    protected virtual void OnAwake() {}
    //---------------------------------------------------------------------------------------------------------------
    protected abstract void OnClick();
    #endregion
  }
}
