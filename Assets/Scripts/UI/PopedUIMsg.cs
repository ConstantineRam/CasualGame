using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace UI
{
  public class PopedUIMsg : AWorldUIObject
  {
    [SerializeField]
    private float ShakeTime = 0.2f;
    [SerializeField]
    private Image MyImage;
    //---------------------------------------------------------------------------------------------------------------
    public static PopedUIMsg Pop(ObjectPoolName PoolName, Vector3 destination)
    {
      PopedUIMsg msg = PopedUIMsg.Instance(PoolName, destination) as PopedUIMsg;
      if (msg == null)
      {
        return msg;
      }
      msg.Animate();
      return msg;
    }

    //---------------------------------------------------------------------------------------------------------------
    private void Animate()
    {
      if (this.transform is RectTransform rt)
      {
        rt.DOShakeScale(ShakeTime, 0.3f, 0,0).SetAutoKill().OnComplete(this.ReturnToPool);
      }
    }

    //---------------------------------------------------------------------------------------------------------------
    public override sealed void OnPop()
    {
      this.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------------------
    public override sealed void OnReturnedToPool()
    {
    }
  }
}
