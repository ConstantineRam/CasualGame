using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//It may not be Abstract for now, still should be named as it.
public class AWorld : MonoBehaviour
{
  [SerializeField]
  private GameObject AThingsStorage;
  public GameObject Things => this.AThingsStorage;
  #region Protected logic
  //---------------------------------------------------------------------------------------------------------------
  protected virtual void OnActivate() { }
  #endregion
  //---------------------------------------------------------------------------------------------------------------
  public void Activate ()
   {
    this.gameObject.SetActive(true);
    this.OnActivate();
   }

}
