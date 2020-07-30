using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

namespace GameBase.Localization
{
  public class LocParam : ILocalizationParamsManager
  {

    [HideInInspector]
    public string p0 ;
    [HideInInspector]
    public string p1 ;
    [HideInInspector]
    public string p2 ;
    [HideInInspector]
    public string p3 ;
    [HideInInspector]
    public string p4 ;
    [HideInInspector]
    public string p5 ;


    public string GetParameterValue(string param)
    {

    //  Debug.LogError(param);
      if (param == "0")
      {
        return p0;
      }

      if (param == "1")
      {
        return p1;
      }

      if (param == "2")
      {
        return p2;
      }

      if (param == "3")
      {
        return p3;
      }

      if (param == "4")
      {
        return p4;
      }

      if (param == "5")
      {
        return p5;
      }

      return DefaultContent.BLANK;
    }
  }

}