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

  private float MinY = 0;
  private float MaxY = 0;

  private float MinX = 20;
  private float MaxX = 30;
  public bool HasLimits { get; private set; }
  #region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  void Update()
  {
    DetectSwipe();
  }
  #endregion
  //---------------------------------------------------------------------------------------------------------------
  public void SetInitiaAngle(float AngleX = 0, float AngleY = 0)
  {
    this.X_Angle = AngleX;
    this.Y_Angle = AngleY;
  }
  //---------------------------------------------------------------------------------------------------------------
  public void SetLimitsVertical(float minX, float maxX)
  {

    this.MinX = minX;
    this.MaxX = maxX;
  }
  //---------------------------------------------------------------------------------------------------------------
  public void SetLimitsHorisontal(float minY, float maxY)
  {
    this.HasLimits = true;
    this.MinY = minY;
    this.MaxY = maxY;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ResetLimitsHorisontal()
  {
    this.HasLimits = false;
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

    if (t.phase == TouchPhase.Ended)
    {
      if (Game.StateManager.CurrentState == GameState.Play)
      {
        if (Game.PlayRoot.SessionEnded)
        {
          return;
        }
        Game.PlayRoot.Things.SetColliderState = true;
      }
      return;
    }


    if (t.phase == TouchPhase.Moved)
    {

      if (Game.StateManager.CurrentState == GameState.Play)
      {
        if (Game.PlayRoot.SessionEnded)
        {
          return;
        }
        Game.PlayRoot.Things.SetColliderState = false;
      }
      secondPressPos = t.position;
      currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
      if (currentSwipe.magnitude < MinSwipeLength)
      {
        return;
      }

      X_Angle = X_AngleTemp - (secondPressPos.x - firstPressPos.x) * 180 / Screen.width;
      Y_Angle = Y_AngleTemp + (secondPressPos.y - firstPressPos.y) * 90 / Screen.height;
      Vector3 v = Game.Camera.transform.rotation.eulerAngles;

      if (HasLimits)
      {
        X_Angle = Mathf.Max(X_Angle, this.MinY);
        X_Angle = Mathf.Min(X_Angle, this.MaxY);
      }

      Y_Angle = Mathf.Max(Y_Angle, this.MinX);
      Y_Angle = Mathf.Min(Y_Angle, this.MaxX);

      Game.Camera.transform.rotation = Quaternion.Euler(Y_Angle, X_Angle,v.z);
    }

    return;


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