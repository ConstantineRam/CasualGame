﻿using UnityEngine;

namespace Assets.Scripts.Utils.ExtensionMethods
{
  public static class FloatEx
  {
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
      return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static float Remap01(this float value, float from1, float to1)
    {
      return (value - from1) / (to1 - from1);
    }

    public static Vector3 ToVector3(this float f)
    {
      return Vector3.one * f;
    }

    public static float RandomTo(this float f)
    {
      return UnityEngine.Random.Range(0, f);
    }

    public static Vector2 ToDirection(this float angle, Vector3 axis)
    {
      return Quaternion.AngleAxis(angle, Vector3.forward) * axis;
    }

    public static string FloatToTime(this float toConvert, string format)
    {
      switch (format)
      {
        case "00.0":
          return string.Format("{0:00}:{1:0}",
            Mathf.Floor(toConvert) % 60,//seconds
            Mathf.Floor((toConvert * 10) % 10));//miliseconds
        case "#0.0":
          return string.Format("{0:#0}:{1:0}",
            Mathf.Floor(toConvert) % 60,//seconds
            Mathf.Floor((toConvert * 10) % 10));//miliseconds
        case "00.00":
          return string.Format("{0:00}:{1:00}",
            Mathf.Floor(toConvert) % 60,//seconds
            Mathf.Floor((toConvert * 100) % 100));//miliseconds
        case "00.000":
          return string.Format("{0:00}:{1:000}",
            Mathf.Floor(toConvert) % 60,//seconds
            Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
        case "#00.000":
          return string.Format("{0:#00}:{1:000}",
            Mathf.Floor(toConvert) % 60,//seconds
            Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
        case "#0:00":
          return string.Format("{0:#0}:{1:00}",
            Mathf.Floor(toConvert / 60),//minutes
            Mathf.Floor(toConvert) % 60);//seconds
        case "#00:00":
          return string.Format("{0:#00}:{1:00}",
            Mathf.Floor(toConvert / 60),//minutes
            Mathf.Floor(toConvert) % 60);//seconds
        case "0:00.0":
          return string.Format("{0:0}:{1:00}.{2:0}",
            Mathf.Floor(toConvert / 60),//minutes
            Mathf.Floor(toConvert) % 60,//seconds
            Mathf.Floor((toConvert * 10) % 10));//miliseconds
        case "#0:00.0":
          return string.Format("{0:#0}:{1:00}.{2:0}",
            Mathf.Floor(toConvert / 60),//minutes
            Mathf.Floor(toConvert) % 60,//seconds
            Mathf.Floor((toConvert * 10) % 10));//miliseconds
        case "0:00.00":
          return string.Format("{0:0}:{1:00}.{2:00}",
            Mathf.Floor(toConvert / 60),//minutes
            Mathf.Floor(toConvert) % 60,//seconds
            Mathf.Floor((toConvert * 100) % 100));//miliseconds
        case "#0:00.00":
          return string.Format("{0:#0}:{1:00}.{2:00}",
            Mathf.Floor(toConvert / 60),//minutes
            Mathf.Floor(toConvert) % 60,//seconds
            Mathf.Floor((toConvert * 100) % 100));//miliseconds
        case "0:00.000":
          return string.Format("{0:0}:{1:00}.{2:000}",
            Mathf.Floor(toConvert / 60),//minutes
            Mathf.Floor(toConvert) % 60,//seconds
            Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
        case "#0:00.000":
          return string.Format("{0:#0}:{1:00}.{2:000}",
            Mathf.Floor(toConvert / 60),//minutes
            Mathf.Floor(toConvert) % 60,//seconds
            Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
      }
      return "error";
    }
  }
}
