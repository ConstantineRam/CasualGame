using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMono : MonoBehaviour
{
  #region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  void Start()
  {
    this.OnStart();
  }

  //---------------------------------------------------------------------------------------------------------------
  void Awake()
  {
    this.OnAwake();
  }

  //---------------------------------------------------------------------------------------------------------------
  void Update()
  {
    this.OnUpdate();
  }
  #endregion

  #region Protected
  protected virtual void OnAwake() { }
  protected virtual void OnStart() { }
  protected virtual void OnUpdate() { }
  #endregion
}
