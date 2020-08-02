using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
namespace Things
{
  public abstract class ThingWithParticles : AThing
  {
    [SerializeField]
    private ParticleSystem VanishPoof;

    //---------------------------------------------------------------------------------------------------------------
    public override void Vanish()
    {
      if (this.IsVanished)
      {
        return;
      }
      this.IsVanished = true;

      if (this.VanishPoof == null)
      {
        Destroy(this.gameObject);
        return;
      }
      this.DisableCollider();

      GameObject poof = GameObject.Instantiate(this.VanishPoof.gameObject, this.transform);
      poof.transform.localScale = new Vector3(3, 3, 3);
      this.Models.ClearModel();
      ParticleSystem ps = poof.GetComponent<ParticleSystem>();
      ps.OnStop(() => { Destroy(this.gameObject); } );
    }

  }
}
