using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBase;

[StaticConstructorOnStartup]
public static class DirectionsMod 
{
  private static int[,] mods = new int[(int) Direction.LastAll + 1, 2]
  {
    {0,  1}, //Direction.Up
    {0, -1}, //Direction.Down
    {-1, 0}, //Direction.Left
    { 1, 0},// Direction.Right

    { -1,1}, //Direction.UpLeft
    { 1,1}, //Direction.UpRight
    { -1,-1},//Direction.DownLeft
    { 1, -1} //Direction.DownRight
  };

  private static float[] angle = new float[(int) Direction.LastAll + 1]
  {
    0f, //Direction.Up
    180f, //Direction.Down
    90f, //Direction.Left
    -90f,// Direction.Right

    45f, //Direction.UpLeft
    -45f, //Direction.UpRight
    135f,//Direction.DownLeft
    -135f //Direction.DownRight
  };

  private static Vector2[] vectors2 = new Vector2[(int) Direction.LastAll + 1]
{
    new Vector2(0, 1), //Direction.Up
    new Vector2(0, -1), //Direction.Down
    new Vector2(-1, 0), //Direction.Left
    new Vector2(1, 0),// Direction.Right

    new Vector2(-1, 1), //Direction.UpLeft
    new Vector2(1, 1), //Direction.UpRight
    new Vector2(-1, -1),//Direction.DownLeft
    new Vector2(1, -1) //Direction.DownRight
};

  private static Direction[] Clockwise4 = new Direction[(int) Direction.LastBasic + 1]
  {
    Direction.Right, //Direction.Up
    Direction.Left, //Direction.Down
    Direction.Up, //Direction.Left
    Direction.Down,// Direction.Right
  };

  private static Direction[] CounterClockwise4 = new Direction[(int) Direction.LastBasic + 1]
  {
    Direction.Left, //Direction.Up
    Direction.Right, //Direction.Down
    Direction.Down, //Direction.Left
    Direction.Up,// Direction.Right
  };

  private static Direction[] ClockwiseAll = new Direction[(int) Direction.LastAll + 1]
  {
    Direction.UpRight, //Direction.Up
    Direction.DownLeft, //Direction.Down
    Direction.UpLeft, //Direction.Left
    Direction.DownRight,// Direction.Right

    Direction.Up, //Direction.UpLeft
    Direction.Right, //Direction.UpRight
    Direction.Left,//Direction.DownLeft
    Direction.Down //Direction.DownRight
  };




  private static Direction[] CounterClockwiseAll = new Direction[(int) Direction.LastAll + 1]
  {
    Direction.UpLeft, //Direction.Up
    Direction.DownRight, //Direction.Down
    Direction.DownLeft, //Direction.Left
    Direction.UpRight,// Direction.Right

    Direction.Left, //Direction.UpLeft
    Direction.Up, //Direction.UpRight
    Direction.Down,//Direction.DownLeft
    Direction.Right //Direction.DownRight
  };

  private static Direction[] reverse = new Direction[(int) Direction.LastAll + 1] 
  { Direction.Down, Direction.Up, Direction.Right, Direction.Left,
  Direction.DownRight, Direction.DownLeft, Direction.UpRight, Direction.UpLeft};

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Returns next direction clockwise 
  /// </summary>
  public static Direction Rotate4Clockwise(this Direction dir)
  {
    if (dir > Direction.LastBasic)
    {
      return Direction.Up;
    }
    return DirectionsMod.Clockwise4 [(int)dir];
  }
  //---------------------------------------------------------------------------------------------------------------
  public static Direction Rotate4CounterClockwise(this Direction dir)
  {
    if (dir > Direction.LastBasic)
    {
      return Direction.Down;
    }
    return DirectionsMod.CounterClockwise4 [(int) dir];
  }

  //---------------------------------------------------------------------------------------------------------------
  public static Direction RotateAllClockwise(this Direction dir)
  {

    return DirectionsMod.ClockwiseAll [(int) dir];
  }

  //---------------------------------------------------------------------------------------------------------------
  public static bool IsBasic(this Direction dir)
  {
    if (dir <= Direction.LastBasic)
    {
      return true;
    }

    return false;
  }
  //---------------------------------------------------------------------------------------------------------------
  public static Vector2 AsVector2(this Direction dir)
  {
    if (dir == Direction.None)
    {
      return Vector2.zero;
    }
    return vectors2[(int) dir];
  }

  //---------------------------------------------------------------------------------------------------------------
  public static Vector3 ApplyToCoords(this Direction dir, Vector3 coords, float scale = 1f)
  {
    if (coords == null)
    {
      return Vector3.zero;
    }

    return new Vector3(coords.x + (dir.GetMod(Axis.x) * scale), coords.y + (dir.GetMod(Axis.y) * scale), coords.z);
  }

  //---------------------------------------------------------------------------------------------------------------
  public static Direction RotateAllCounterClockwise(this Direction dir)
  {

    return DirectionsMod.CounterClockwiseAll[(int) dir];
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Returns angle to use for Unity Rotation
  /// </summary>
  public static float GetAngle(this Direction dir)
  {
    return DirectionsMod.angle[(int) dir];
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Returns random direction of Up, Down, Left or Right.
  /// </summary>
  public static Direction GetRandom4
  {
    get { return (Direction) UnityEngine.Random.Range((int) Direction.FirstBasic, (int) Direction.LastBasic + 1); }
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  ///  Returns random direction of Up, Down, Left, Right, UpLeft, UpRight, DownLeft or DownRight.
  /// </summary>
  public static Direction GetRandomAll
  {
    get { return (Direction) UnityEngine.Random.Range((int) Direction.FirstAll, (int) Direction.LastAll + 1); }
    
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Get's direction and returns it reversed. For example gets Up returns Down.
  /// </summary>
  /// <param name="usedDir"></param>
  /// <returns></returns>
  public static Direction GetReverse(this Direction usedDir)
  {
    return DirectionsMod.reverse[(int) usedDir];
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Returns mod ranges -1, 0 or 1 based on Axis (x or y) and direction.
  /// You can use mod can be used to coordinates. For example creature at location 10,10 and you want to move it up.
  /// You request GetMod (Up, x) and get this mod and apply it to x.
  /// </summary>
  public static int GetMod(this Direction usedDir, Axis axis)
  {
    return DirectionsMod.mods[(int) usedDir, (int) axis];
  }
  // merges two basic direction if possible
  public static Direction Merge(this Direction usedDir, Direction dir)
  {
    if (!usedDir.IsBasic())
    {
      return Direction.None;
    }

    if (!dir.IsBasic())
    {
      return Direction.None;
    }

    if (usedDir == Direction.Up)
    {
      if (dir == Direction.Left)
      {
        return Direction.UpLeft;
      }

      if (dir == Direction.Right)
      {
        return Direction.UpRight;
      }
    }

    if (usedDir == Direction.Down)
    {
      if (dir == Direction.Left)
      {
        return Direction.DownLeft;
      }

      if (dir == Direction.Right)
      {
        return Direction.DownRight;
      }
    }

    if (usedDir == Direction.Left)
    {
      if (dir == Direction.Up)
      {
        return Direction.UpLeft;
      }

      if (dir == Direction.Down)
      {
        return Direction.DownLeft;
      }
    }

    if (usedDir == Direction.Right)
    {
      if (dir == Direction.Up)
      {
        return Direction.UpRight;
      }

      if (dir == Direction.Down)
      {
        return Direction.DownRight;
      }
    }
    return Direction.None;
  }
  //---------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns dataTomod with already applied mod that ranged  -1, 0 or 1 based on Axis (x or y) and direction.
    /// You can use mod can be used to coordinates. For example creature at location 10,10 and you want to move it up.
    /// You request GetMod (Up, x) and get this mod and appled it to x.
    /// </summary>
    public static int AppyMod(this Direction usedDir, int dataToMod, Axis axis)
  {
    return dataToMod + usedDir.GetMod( axis);
  }
  public static float AppyMod(this Direction usedDir, float dataToMod, Axis axis)
  {
    return dataToMod + usedDir.GetMod(axis);
  }

  //---------------------------------------------------------------------------------------------------------------
  public static Vector3 TweenDir(this Direction usedDir)
  {
    return new Vector3(usedDir.GetMod(Axis.x), usedDir.GetMod(Axis.y), 0f);
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// 
  /// </summary>
  /// <param name="usedDir"></param>
  /// <param name="x1"></param>
  /// <param name="y1"></param>
  /// <param name="x2"></param>
  /// <param name="y2"></param>
  /// <param name="counter"></param>
  /// <param name="depthStep"></param>
  /// How deep we want to go in or our of specified line
  /// <returns></returns>
  public static SideDirReport GetSideCoord(this Direction usedDir, int x1, int y1, int x2, int y2, int counter, int depthStep = 0)
  {
    SideDirReport report = new SideDirReport();
    report.coord = new Vector2Int(0,0);
    if (!usedDir.IsBasic())
    {
      report.error = true;
      return report;
    }

    if (usedDir == Direction.Up)
    {
      if (usedDir.SideSize(x1, y1, x2, y2) < counter)
      {
        report.error = true;
        return report;
      }

      report.error = false;
      report.coord.x = x1 + counter;
      report.coord.y = y2 + depthStep;
      return report;
    } // up


    if (usedDir == Direction.Down)
    {
      if (usedDir.SideSize(x1, y1, x2, y2) < counter)
      {
        report.error = true;
        return report;
      }

      report.coord.x = x1 + counter;
      report.coord.y = y1 - depthStep;
      return report;
    } // down


    if (usedDir == Direction.Left)
    {
      if (usedDir.SideSize(x1, y1, x2, y2) < counter)
      {
        report.error = true;
        return report;
      }

      report.error = false;
      report.coord.x = x1 - depthStep;
      report.coord.y = y1 + counter;
      return report;
    } // left


    if (usedDir == Direction.Right)
    {
      if (usedDir.SideSize(x1, y1, x2, y2) < counter)
      {
        report.error = true;
        return report;
      }

      report.error = false;
      report.coord.x = x2 - depthStep;
      report.coord.y = y1 + counter;
      return report;
    } // right

    report.error = true;
    return report;
  }

  //---------------------------------------------------------------------------------------------------------------
  public static int SideSize(this Direction usedDir, int x1, int y1, int x2, int y2)
  {
    if (!usedDir.IsBasic())
    {
      return 0;
    }

    if (usedDir == Direction.Up | usedDir == Direction.Down)
    {
      return (x2 - x1 + 1);
    }

    if (usedDir == Direction.Left | usedDir == Direction.Right)
    {
      return (y2 - y1 + 1);
    }

    return 0;
  }
}

public struct SideDirReport
{
  public Vector2Int coord;
  public bool error;
}
