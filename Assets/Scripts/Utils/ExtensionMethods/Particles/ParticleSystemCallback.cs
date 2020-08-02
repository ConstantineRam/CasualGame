using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Extensions
{
  public delegate void ParticleSystemCustomCallback();
  public static class ParticleSystemUtils
  {
    //---------------------------------------------------------------------------------------------------------------
    public static ParticleSystem OnStop(this ParticleSystem t, ParticleSystemCustomCallback action)
    {
      if (t == null)
      {
        return t;
      }
      ParticleSystemCallback PSCallbackScript = t.gameObject.AddComponent<ParticleSystemCallback>();
      PSCallbackScript.AssignCallback(t, action);
      return t;
    }
  }

  public class ParticleSystemCallback : MonoBehaviour
  {
    public ParticleSystemCustomCallback Callback { get; internal set; }
    //---------------------------------------------------------------------------------------------------------------
    public void AssignCallback (ParticleSystem ps, ParticleSystemCustomCallback action)
    {
      if (ps == null)
      {
        return;
      }

      var main = ps.main;
      main.stopAction = ParticleSystemStopAction.Callback;
      this.Callback = action;
    }

    //---------------------------------------------------------------------------------------------------------------
    void OnParticleSystemStopped()
    {
      if (this.Callback == null)
      {
        return;
      }

      this.Callback.Invoke();
    }
  }

}