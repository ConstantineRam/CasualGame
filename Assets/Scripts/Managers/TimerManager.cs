// This manager allow you to start a timer that will make a callback once done and destroy itself.
// The more important feature, you can assign timer to a timelayer (check TimeScaleProvider for more info), so you can start and stop specific timers simultaneously.

// TODO: if layer was paused all new timers added to layer should start paused too.
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimerManager
{

  private List<Timer> activeTimers = new List<Timer>();

  //---------------------------------------------------------------------------------------------------------------
  public TimerManager(TimeScaleProvider timeScaleProvider)
  {
    List<TimeScaleLayer> layersList = Enum.GetValues(typeof(TimeScaleLayer)).OfType<TimeScaleLayer>().ToList();
    foreach (TimeScaleLayer layer in layersList)
    {
      var localLayer = layer;
      timeScaleProvider.ListenUpdate(layer, scale => Update(localLayer, scale));
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  private void Update(TimeScaleLayer layer, float timeScale)
  {
    activeTimers
      .FindAll(t => t.Layer == layer)
      .ForEach(t => t.Update(timeScale));

    activeTimers.RemoveAll(t => !t.IsWorking);
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Creates and returns timer.
  /// </summary>
  /// <param name="time">MS</param>
  /// <param name="callback">This method is to be called once timer will finishes. Callback instance should exist when it happens, otherwise error will occur.</param>
  /// <param name="layer">Assignes timer to specific layer if set.</param>
  /// <returns></returns>
  public Timer Start(float time, Action callback, TimeScaleLayer layer = TimeScaleLayer.Common, bool StartPaused = false)
  {
    if (time < 0) time = 0;
    Timer timer = new Timer(time, callback, layer);
    if (StartPaused)
    {
      timer.Pause();
    }
 
    activeTimers.Add(timer);

    return timer;
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Stops execution of that specific Timer.
  /// Stopped Timer will be deleted during next Update.
  /// </summary>
  /// <param name="timer"></param>
  public void Stop(Timer timer)
  {
    if (timer == null) return;
    activeTimers.FindAll(t => t == timer)
      .ForEach(t => t.Stop());
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Stops every single Timer that has this callback.
  /// </summary>
  /// <param name="callback"></param>
  public void Stop(Action callback)
  {
    if (callback == null) return;
    activeTimers.FindAll(t => t.Callback == callback)
      .ForEach(t => t.Stop());
  }

  //---------------------------------------------------------------------------------------------------------------
  public void HaltAll()
  {
    foreach (Timer t in activeTimers)
    {
      t.Halt();
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public void StopAll(TimeScaleLayer layer)
  {
    activeTimers.FindAll(t => t.Layer == layer)
      .ForEach(t => t.Stop());
  }


  //---------------------------------------------------------------------------------------------------------------
  //Pause time flow of all timers of specific TimeScaleLayer, however paused timers unlike stopped or halted timers will not be deleted during next Update and could be resumed.
  public void PauseAll(TimeScaleLayer layer)
  {
    activeTimers.FindAll(t => t.Layer == layer)
      .ForEach(t => t.Pause());
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Pause time flow of specific timer, however paused timers unlike stopped or halted timers will not be deleted during next Update and could be resumed.
  /// </summary>
  /// <param name="timer"></param>
  public void Pause(Timer timer)
  {
    if (timer == null)
    {
      return;
    }

    activeTimers.FindAll(t => t == timer)
      .ForEach(t => t.Pause());
  }

  //---------------------------------------------------------------------------------------------------------------
  public void Resume(Timer timer)
  {
    if (timer == null)
    {
      return;
    }

    activeTimers.FindAll(t => t == timer)
      .ForEach(t => t.Resume());
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ResumeAll(TimeScaleLayer layer)
  {
    
    activeTimers.FindAll(t => t.Layer == layer)
      .ForEach(t => t.Resume());

  }

  //***************************************************************************************************************
  public class Timer
  {
    public Action Callback { get; private set; }
    public bool IsWorking { get; private set; }
    public bool IsPaused { get; private set; }
    public TimeScaleLayer Layer { get; private set; }

    private float initTimeNeeded;
    private float timeNeeded;
    private float timePassed;
    private bool isRepeated;

    public Timer(float time, Action callback, TimeScaleLayer layer)
    {
      initTimeNeeded = timeNeeded = time;
      Callback = callback;

      timePassed = 0;

      IsWorking = true;
      IsPaused = false;
      Layer = layer;
    }

    public Timer AddTime(float time)
    {
      timeNeeded += time;
      return this;
    }

    public Timer SetTime(float time)
    {
      if (time < 0) time = 0;
      timeNeeded = time;
      return this;
    }

    public Timer SetCallBack(Action callback)
    {
      this.Callback = callback;
      return this;
    }

    public Timer SetRepeated(bool value)
    {
      isRepeated = value;
      return this;
    }

    public Timer SetLayer(TimeScaleLayer newLayer)
    {
      Layer = newLayer;
      return this;
    }

    public void Update(float timeScale)
    {
      if (IsPaused || !IsWorking)
      {
        return;
      }

      timePassed += Time.deltaTime * timeScale;

      if (timePassed > timeNeeded)
      {
        if (isRepeated)
        {
          timeNeeded += initTimeNeeded;
        } 
        else
        {
          this.Stop();
        } 

        if (Callback != null)
          Callback.Invoke();
      }
    }

    public float GetTimeLeft()
    {
      return Mathf.Max(0, timeNeeded - timePassed);
    }

    public void Complete()
    {
      timePassed = timeNeeded;
      Update(1);
    }

    public void StopAndExecute()
    {
      if (!IsWorking) return;
      if (Callback != null)
      {
        Callback.Invoke();
      }

      this.Stop();
    }

    public void Pause()
    {
      IsPaused = true;
    }

    public void Resume()
    {
      IsPaused = false;
    }

    public void Stop()
    {
      if (!IsWorking) return;
      IsWorking = false;
    }

    /// <summary>
    /// Stops timer and erases callback. Hard stop.
    /// </summary>
    public void Halt()
    {
      Callback = null;
      this.Stop();
    }

    private void RemoveSelf()
    {

    }
  }

  
}

public static class TimerUtils
{
  public static bool IsNull(this TimerManager.Timer t)
  {
    return t == null ? true : false;
  }
}

