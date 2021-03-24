using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public const string JABHITARMOUR = "Jab Hit Armour";
    public const string JABHITUNARMOURED = "Jab Hit Unarmoured";
    public const string HEAVYHITARMOUR = "Heavy Hit Armour";
    public const string HEAVYHITUNARMOURED = "Heavy Hit Unarmoured";
    public const string JABMISS = "Jab Miss";
    public const string ARMOURBREAK = "Armour Break";
    public const string JUMP = "Jump Sounds";
    public const string HEAVYMISS = "Heavy Miss";
    public const string AERIALMISS = "Aerial Miss";

    public Sound[] sounds;

    public LibraryLink[] links;

    public static AudioManager instance;

    public Dictionary<string, AudioLibrary> libraries;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
        }

        libraries = new Dictionary<string, AudioLibrary>();
        foreach (LibraryLink l in links) {
            libraries.Add(l.name, l.library);
        }
    }

    void Start()
    {
        Play("AlphaMusic");
    }

    public void Play (string name)
    {
        if (libraries.ContainsKey(name))
        {
            libraries[name].PlaySound();
        }
        
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.source.Play();
        //Note: to play a sound from another script, use: FindObjectOfType<AudioManager>().Play(NAMEOFCONSTGOESHERE);
    }


    [System.Serializable]
    public struct LibraryLink
    {
        public string name;
        public AudioLibrary library;
    }
}
