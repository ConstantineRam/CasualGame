using UnityEngine;
using TouchSimulator;
 
public class SwipeManager : MonoBehaviour
{
  private const float SwipeCheckValue = 0.906f;
  private const float MinSwipeLength = 5f;
  private Vector2 firstPressPos;
  private Vector2 secondPressPos;
  private Vector2 currentSwipe;
  private float X_AngleTemp;
  private float Y_AngleTemp;
  private float X_Angle = 0;
  private float Y_Angle = 0;

  //---------------------------------------------------------------------------------------------------------------
  void Update()
  {
    DetectSwipe();
  }

  //---------------------------------------------------------------------------------------------------------------
  private void DetectSwipe()
  {

    if (TouchSimulator.Input.touches.Length <= 0)
    {
      return;
    }

    Touch t = TouchSimulator.Input.GetTouch(0);


    if (t.phase == TouchPhase.Began)
      {
        firstPressPos = t.position;
      X_AngleTemp = X_Angle;
      Y_AngleTemp = Y_Angle;
    }
 


    if (t.phase == TouchPhase.Moved)
    {
      secondPressPos = t.position;
      currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
      if (currentSwipe.magnitude < MinSwipeLength)
      {
        return;
      }

      X_Angle = X_AngleTemp + (secondPressPos.x - firstPressPos.x) * 180 / Screen.width;
      Y_Angle = Y_AngleTemp + (secondPressPos.y - firstPressPos.y) * 90 / Screen.height;
      Vector3 v = Game.Camera.transform.rotation.eulerAngles;
      Game.Camera.transform.rotation = Quaternion.Euler(v.x, X_Angle,v.z);
    }

    return;
    //if (t.phase != TouchPhase.Ended)
    //{
    //  return;
    //}

    //secondPressPos = new Vector2(t.position.x, t.position.y);
    //currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);




      currentSwipe.Normalize();
    Game.Camera.transform.rotation = Quaternion.Euler(Game.Camera.transform.rotation.x + currentSwipe.x, Game.Camera.transform.rotation.y + currentSwipe.y, Game.Camera.transform.rotation.z);
//    Game.Camera.transform.rotation = new Vector3(currentSwipe.x, currentSwipe.y, Game.Camera.transform.rotation.z);

      // this code was used to process swipe into 2d env. 
      //for (int i = (int) Direction.FirstAll; i < (int) Direction.LastAll; i++)
      //{
      //  Direction dir = (Direction) i;
      //  if (Vector2.Dot(currentSwipe, dir.AsVector2()) > SwipeCheckValue)
      //  {
      //    Game.Events.Swipe.Invoke(dir);
      //    Debug.LogError(dir);
      //    return;
      //  }
      //}


  }
}