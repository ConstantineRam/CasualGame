// This manager loads, stores and plays all sounds and music in the game.
// How it organized:
// All ingame sounds and music 'types' are listed in AudioId enum.
// Every single sound/ music type listed in enum has ID and folder path.
//  example:
// [Description("Audio/Sound/Basic_click")]
//  ClickSound = 201,
// Once game starts AudioManager checks specified folder and assigned all sounds to specified id.
// in this case ClickSound will get all sounds from the folder "Audio/Sound/Basic_click" assigned to one Id.
// When this id is called Manager selects one random sound from the list of sounds assigned to this id.
//

using Assets.Scripts.Utils.ExtensionMethods;
using GameBase;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

public class AudioManager : MonoBehaviour
{
 // private TimerManager.Timer timer;

  [Preserve] private Dictionary<AudioId, SoundCollectionBlock> clipsCollection;

  //private List<AudioSource> activeMusicSources;
  // List of Active Music sources was removed, because we shouldn't have more that one 'active' music track at a time.
  // while it could be nice to have a soft switch between two sources, we should never have two tracks playing at the same time full volume.

  private AudioSource activeMusicSource;
  private List<AudioSource> MusicSourceKillList;
  private List<AudioSource> activeSoundSources;

  public const AudioId DEFAULT_AUDIO_CLIP_ID = AudioId.ClickSound;
  private AudioClip LastMusicClip = null;

  // allow to loop music with track changing.
  private bool MusicLoop = false;
  private float LoopVolume = 1f;
  private AudioId LoopId;

  #region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  private void Awake()
  {
    this.activeMusicSource = null;
    this.MusicSourceKillList = new List<AudioSource>();
    this.activeSoundSources = new List<AudioSource>();
    CacheClipsCollection();

    Game.Events.MusicEnabled.Listen(OnMusicEnabledChanged);
    Game.Events.SoundEnabled.Listen(OnSoundEnabledChanged);
  }

  //---------------------------------------------------------------------------------------------------------------
  private void Update()
  {

  }

  //---------------------------------------------------------------------------------------------------------------
  private void LateUpdate()
  {
    CheckAndDestroyFinishedSources();

    if (this.HasActiveMusic())
    {
      return;
    }

    if (!this.MusicLoop)
    {
      return;
    }

    this.PlayMusic(this.LoopId, true, this.LoopVolume);
    
  }
  #endregion

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  ///  Prevents more than one music track to be played in time.
  /// </summary>
  //[Obsolete("We no longer have more than one music source",false)]
  //private void RemoveExtraMusic()
  //{
  //  int counterSound = 0;
  //  foreach (AudioSource source in activeMusicSources)
  //  {
  //    if (source.isPlaying)
  //    {
  //      counterSound++;
  //      if (counterSound > 1)
  //      {
  //        source.Stop();
  //      }
  //    }

  //  }
  //}

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Returns true is there is a music track playing right now.
  /// </summary>
  /// <returns></returns>
  public bool HasActiveMusic()
  {
    return activeMusicSource == null ? false : activeMusicSource.isPlaying ? true : false;
  }

  #region Play, Stop
  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Start playing new sound with audioIdas a music track.
  /// </summary>
  /// <param name="audioId"></param>
  /// <param name="loop">if true will try to play another track from a collection attached to this ID.</param>
  /// <param name="volume"></param>

  /// <param name="overrideTime"></param>
  /// Disabled because lead to unpleasant glitches.
  /// <returns></returns>
  public AudioSource PlayMusic(AudioId audioId, bool loop = true, float volume = 1, float overrideTime = 0f)
  {
    // it was preventing proper loop with different tracks, because they have same ID.
    //if (IsPlaying(audioId))
    //{
    //  return GetActiveSourcesById(audioId).FirstOrDefault();
    //}
      

    SoundCollectionBlock soundCollection;
    clipsCollection.TryGetValue(audioId, out soundCollection);
    if (soundCollection == null)
    {
      Debug.Log(message: "<color=red>UNEXPECTED ERROR:</color> PlayMusic was called for ID:" + audioId.ToString() + ", but no collection with this ID exists. Ignored. Check the enum.");
      return null;
    }

    AudioClip clip;

    clip = soundCollection.GetClip(LastMusicClip);
    LastMusicClip = clip;
    if (clip == null)
    {
      Debug.Log(message: "<color=red>ERROR:</color> PlayMusic was called for ID:" + audioId.ToString() + ", but collection has no sounds and returns null. Ignored.");
      return null;
    }

    AudioSource source = gameObject.AddComponent<AudioSource>();
    source.volume = volume;
    //source.loop = loop;
    source.clip = clip;
    source.mute = !Game.Settings.MusicEnabled;

    this.MusicLoop = loop;
    this.LoopId = audioId;
    this.LoopVolume = volume;

    source.Play();

    this.StopAllMusic(overrideTime);

    this.activeMusicSource = source;
   
    return source;
  }

  //---------------------------------------------------------------------------------------------------------------
  public AudioSource PlaySound(AudioId audioId, float volume = 1)
  {
    SoundCollectionBlock soundCollection;
    clipsCollection.TryGetValue(audioId, out soundCollection);
    if (soundCollection == null)
    {
      Debug.Log(message: "<color=red>UNEXPECTED ERROR:</color> PlaySound was called for ID:" + audioId.ToString() + ", but no collection with this ID exists. Ignored. Check the enum.");
      return null;
    }

    AudioClip clip;
    clip = soundCollection.GetClip();
    if (clip == null)
    {
      Debug.Log(message: "<color=red>ERROR:</color> PlaySound was called for ID:" + audioId.ToString() + ", but collection has no sounds and returns null. Ignored.");
      return null;
    }

    AudioSource source = gameObject.AddComponent<AudioSource>();
    source.mute = !Game.Settings.SoundEnabled;
    source.clip = clip;

    source.Play();

    activeSoundSources.Add(source);

    return source;
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Stops AudioSource in duration time. If duration is above 0 sound will fade otherwise will stop right away.
  /// </summary>
  /// <param name="src"></param>
  /// <param name="duration"></param>
  public void StopAudio(AudioSource src, float duration = 0f)
  {
    StopAudio(ClipToId(src.clip), duration);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void StopAudio(AudioId id, float duration = 0f)
  {
    GetActiveSourcesById(id).ForEach(s => s.DOFade(0, duration).OnComplete( s.Stop ));
  }

  //---------------------------------------------------------------------------------------------------------------
  public void StopAllSounds(float duration = 0f)
  {
    foreach (AudioSource source in activeSoundSources)
    {
      source.DOFade(0, duration).OnComplete(source.Stop);
    }   
  }

  //---------------------------------------------------------------------------------------------------------------
  public void StopAllMusic(float duration = 0f)
  {
    if (activeMusicSource == null)
    {
      return;
    }

    AudioSource s = this.activeMusicSource;
    this.MusicSourceKillList.Add(s);
    this.activeMusicSource = null;
    s.DOFade(0, duration).OnComplete( s.Stop );
  }

  //---------------------------------------------------------------------------------------------------------------
  public void StopEverything(float duration = 0f)
  {
    StopAllMusic(duration);
    StopAllSounds(duration);
  }

  #endregion

  #region Subscription events
  //---------------------------------------------------------------------------------------------------------------
  private void OnMusicEnabledChanged(bool isEnabled)
  {
    //foreach (AudioSource source in activeMusicSources)
    //{
    //  source.DOKill();
    //  if (isEnabled) source.mute = !isEnabled;
    //  source.DOFade(isEnabled.To01(), 0.5f).OnComplete(() =>
    //  {
    //    source.mute = !isEnabled;
    //  });
    //}
    if (activeMusicSource == null)
    {
      return;
    }

    activeMusicSource.DOKill();
    if (isEnabled) activeMusicSource.mute = !isEnabled;
    activeMusicSource.DOFade(isEnabled.To01(), 0.5f).OnComplete(() =>
      {
        activeMusicSource.mute = !isEnabled;
      });
  }

  //---------------------------------------------------------------------------------------------------------------
  private void OnSoundEnabledChanged(bool isEnabled)
  {
    foreach (AudioSource source in activeSoundSources)
    {
      source.DOKill();
      if (isEnabled) source.mute = !isEnabled;
      source.DOFade(isEnabled.To01(), 0.5f).OnComplete(() =>
      {
        source.mute = !isEnabled;
      });
    }
  }
  #endregion

  #region Utils
  //---------------------------------------------------------------------------------------------------------------
  private void CacheClipsCollection()
  {
 
    clipsCollection = new Dictionary<AudioId, SoundCollectionBlock>();

    foreach (AudioId id in Enum.GetValues(typeof(AudioId)))
    {
      if (clipsCollection.ContainsKey(id))
      {
        Debug.Log(message: "<color=red>ERROR:</color> attempt to add AudioId with the same ID:" +id.ToString()+". Ignored. Check the enum.");
        continue;
      }

      SoundCollectionBlock soundCollectionBlock = new SoundCollectionBlock();
  
      string directoryPath = id.GetPath();
      
      var loadedClips = Resources.LoadAll(directoryPath, typeof(AudioClip)).Cast<AudioClip>().ToArray();
      Debug.Log( id.ToString() + " "+ loadedClips.Length);
      foreach (var singleClip in loadedClips)
       { 
        soundCollectionBlock.PushClip(singleClip);
       }

      clipsCollection.Add(id, soundCollectionBlock);

    } //foreach (AudioId id in Enum.GetValues(typeof(AudioId)))
  }

  //---------------------------------------------------------------------------------------------------------------
  private AudioId ClipToId(AudioClip clip)
  {
    if (clip == null)
    {
      Debug.Log(message: "<color=red>ERROR:</color>, <color=white>ClipToId</color> got null in <color=white>clip</color>. Return default value: " + DEFAULT_AUDIO_CLIP_ID + ".");
      return DEFAULT_AUDIO_CLIP_ID;
    }
    SoundCollectionBlock soundCollectionBlock;
    foreach (AudioId id in Enum.GetValues(typeof(AudioId)))
    {
      clipsCollection.TryGetValue(id, out soundCollectionBlock);
      if (soundCollectionBlock == null)
       {
        Debug.Log(message: "<color=red>UNEXPECTED ERROR:</color>, <color=white>ClipToId</color> can't retrive a value from dictionary <color=white>clipsCollection</color> for ID:" + id.ToString()+". Ignored.");
        continue;
       }
      if (soundCollectionBlock.HasClip(clip))
       {
        return id;
       }
    } // foreach (AudioId id in Enum.GetValues(typeof(AudioId)))
    Debug.Log(message: "<color=red>UNEXPECTED ERROR:</color>, <color=white>ClipToId</color> can't clip " + clip.name + " in game collection. Return default value: "+ DEFAULT_AUDIO_CLIP_ID+".");
    return DEFAULT_AUDIO_CLIP_ID;
  }

  //---------------------------------------------------------------------------------------------------------------
  private bool IsPlaying(AudioId id)
  {
    return !GetActiveSourcesById(id).IsEmpty();
  }

  //---------------------------------------------------------------------------------------------------------------
  private List<AudioSource> GetActiveSourcesById(AudioId id)
  {
    List<AudioSource> result = new List<AudioSource>();
    // Music
    if (activeMusicSource != null)
    {
      if (ClipToId(activeMusicSource.clip) == id)
      {
        result.Add(activeMusicSource);
      }
    }

    
    // Sound
    result.AddRange(activeSoundSources.Where(a => ClipToId(a.clip) == id));
    return result;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void CheckAndDestroyFinishedSources()
  {
    // Music
    List<AudioSource> toRemove = new List<AudioSource>();

    foreach (AudioSource source in this.MusicSourceKillList)
    {
      if (!source.isPlaying)
      {
        toRemove.Add(source);
      }
    }

    toRemove.ForEach(s => { MusicSourceKillList.Remove(s); Destroy(s); });

    // Sound
    toRemove.Clear();
    foreach (AudioSource source in activeSoundSources)
    {
      if (source.isPlaying) continue;
      toRemove.Add(source);
    }
    toRemove.ForEach(s => { activeSoundSources.Remove(s); Destroy(s); });
  }
  #endregion

}
