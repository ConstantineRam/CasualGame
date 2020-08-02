using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Things
{
  public class Veggie : ThingWithParticles
  {
    #region Protected logic
    //---------------------------------------------------------------------------------------------------------------
    protected override sealed void OnAwake()
    {
      base.OnAwake();

      if (Game.StateManager.CurrentState == GameState.Menu)
      {
        this.DisableCollider();
      }
    }

    //---------------------------------------------------------------------------------------------------------------
    protected override sealed void OnClick()
    {
      if (Game.StateManager.CurrentState == GameState.Menu)
      {
        return;
      }

      Game.PlayRoot.Tasks.VeggieClicked(this); 
    }
    #endregion

  }
}
