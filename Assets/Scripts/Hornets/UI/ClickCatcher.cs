using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClickCatcher : MonoBehaviour
{
  void Update()
  {
    return;
#if UNITY_EDITOR
    if (!Input.GetMouseButton(0))
    {
      return;
    }
#else
    if (Input.touchCount == 0)
    {
      return;
    }
#endif

    Vector3 screenPos = Input.mousePosition;
    screenPos.z = 10.0f;

    Vector3 worldPos = Game.Camera.ScreenToWorldPoint(screenPos);

    // get current position of this GameObject
    Vector3 newPos = transform.position;
    // set x position to mouse world-space x position
    newPos.x = worldPos.x;
    // apply new position
    transform.position = newPos;
  }
}
