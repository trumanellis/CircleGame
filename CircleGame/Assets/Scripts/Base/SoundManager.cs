using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance { get; set; }
    public bool _muted;
    public static bool isMuted;
    public Sound[] _sounds;
    public static List<Sound> playingSounds = new List<Sound>();
    private Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();

    private void Awake() {
        instance = this;
        isMuted = _muted;

        FillSounds();
    }

    public static Sound Play(string name, bool loop = false) {
        return Play(SoundManager.GetSound(name), loop);
    }

    public static Sound Play(Sound sound, bool loop = false) {
        if(sound == null) {
            Debug.LogError("Sound can not be null!");
            return null;
        }

        GameObject go = new GameObject("Sound Source (" + sound.name + ")");
        AudioSource source = go.AddComponent<AudioSource>();
        source.volume = sound.volume;
        source.pitch = source.pitch;
        source.loop = loop;
        source.clip = sound.clip;
        sound.source = source;
        playingSounds.Add(sound);
        go.AddComponent<SoundTracker>().Init(sound);
        source.Play();

        return sound;
    }

    public static Sound Pause(Sound sound) {
        return sound.Pause();
    }

    public static Sound GetSound(string name) {
        Sound sound;
        instance.sounds.TryGetValue(name.ToLower(), out sound);
        if(sound != null) return new Sound(sound.name, sound.clip);
        return null;
    }

    private void FillSounds() {
        for(int i = 0; i < _sounds.Length; i++)
            sounds.Add(_sounds[i].name.ToLower(), _sounds[i]);
    }
}

[System.Serializable]
public class Sound {
    public AudioClip clip;
    public string name;
    public delegate void SoundDelegate(Sound sound);
    public SoundDelegate onSoundEnded;

    public AudioSource source {get;set;}
    public bool isPlaying { get; set; }
    public float length { get { return clip.length; } }
    private float _volume = 1f;
    public float volume { get { return _volume; } set { _volume = value; } }
    private float _pitch = 1f;
    public float pitch { get { return _pitch; } set { _pitch = value; } }

    public Sound(string name, AudioClip clip) {
        this.name = name;
        this.clip = clip;
    }

    public Sound Play(bool loop = false) {
        SoundManager.Play(this, loop);
        isPlaying = true;
        return this;
    }

    public Sound Pause() {
        if(SoundManager.playingSounds.Contains(this))
            SoundManager.playingSounds[SoundManager.playingSounds.IndexOf(this)].Pause();
        else Debug.LogWarning("This sound is not playing and therefore cannot be paused");
        return this;
    }
}

public class SoundTracker : MonoBehaviour {
    private static int instanceId = 0;
    private Sound sound;
    private float soundTime;
    private float length;

    public void Init(Sound sound) {
        instanceId++;
        this.sound = sound;
        length = sound.source.clip.length;
    }

    private void Update() {
        soundTime = sound.source.time;
        if(soundTime >= length) SoundEnded();
    }

    private void SoundEnded() {
        if(sound.onSoundEnded != null) sound.onSoundEnded(sound);
        Destroy(gameObject);
    }
}
