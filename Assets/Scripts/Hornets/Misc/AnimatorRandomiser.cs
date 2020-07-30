
using System.Collections.Generic;
using UnityEngine;

public class AnimatorRandomiser : MonoBehaviour
{

  //---------------------------------------------------------------------------------------------------------------
  void Start()
    {
    // Animator[] animations =  this.GetComponentsInChildren<Animator>();
    //if (animations != null)
    //{
    //  foreach (Animator an in animations)
    //  {
    //    an.Play(stateNameHash: 0, layer: -1, normalizedTime: Random.value);
    //  }
    //}


    Animator[] animations = this.GetComponents<Animator>();
    if (animations != null)
    {
      foreach (Animator an in animations)
      {
        an.Play(0, -1, Random.value);
      }
    }
  }
}
