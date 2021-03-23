using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : MonoBehaviour
{
    public Sound[] sounds;

    private int soundsIndex;
    private int[] previousArray;
    private int previousArrayIndex;


    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
        }
    }
    public void PlaySound()
    {
        var s = ChooseSound();
        s.source.Play();
        //Debug.Log("Got to here!");  
    }

    private Sound ChooseSound()
    {
        if (previousArray == null)
        {
            previousArray = new int[sounds.Length / 2];
        }
        if (previousArray.Length == 0)
        {
            return null;
        }
        else
        {
            do
            {
                soundsIndex = Random.Range(0, sounds.Length);
            } while (PreviousArrayContainsSoundIndex());
            previousArray[previousArrayIndex] = soundsIndex;

            previousArrayIndex++;
            if (previousArrayIndex >= previousArray.Length)
            {
                previousArrayIndex = 0;
            }
        }

        return sounds[soundsIndex];
    }

    private bool PreviousArrayContainsSoundIndex()
    {
        for (int i = 0; i < previousArray.Length; i++)
        {
            if (previousArray[i] == soundsIndex)
            {
                return true;
            }
        }
        return false;
    }
}
