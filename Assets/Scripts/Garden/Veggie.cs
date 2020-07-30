using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Things
{
  public class Veggie : AThing
  {
    #region Protected logic
    //---------------------------------------------------------------------------------------------------------------
    protected override sealed void OnClick()
    {
      Game.PlayRoot.VeggieClicked.Invoke(this);
    }
    #endregion

  }
}
