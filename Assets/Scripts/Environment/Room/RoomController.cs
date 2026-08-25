using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField]
    private List<AudioSourceKeys> m_audioSources;

    public void EnterRoom()
    {
        gameObject.SetActive(true);
        foreach (AudioSourceKeys _audioSource in m_audioSources)
        {
            _audioSource.AudioSource.enabled = true;
            _audioSource.AudioSource.Play();
        }        
    }

    public void ExitRoom()
    {
        foreach(AudioSourceKeys _audioSource in m_audioSources)
        {
            _audioSource.AudioSource.Stop();
        }
        gameObject.SetActive(false);
    }

    public void StartAudioSource(string _key)
    {
        GetAudioSource(_key).Play();
    }

    public void StopAudioSource(string _key)
    {
        GetAudioSource(_key).Stop();
    }

    private AudioSource GetAudioSource(string _key)
    {
        AudioSourceKeys audioSourceToStart = m_audioSources.FirstOrDefault(x => x.Key == _key);
        if (audioSourceToStart != null)
        {
            return audioSourceToStart.AudioSource;
        }
        else
        {
            throw new KeyNotFoundException("No audio source with matching key found in this room!");
        }
    }
}

[Serializable]
public class AudioSourceKeys
{
    public string Key;
    public AudioSource AudioSource;
}