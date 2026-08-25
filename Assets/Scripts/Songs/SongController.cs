using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SongController : MonoBehaviour
{
    [SerializeField]
    private List<InstrumentSO> m_instruments;
    [SerializeField]
    private List<ScaleSO> m_scales;

    [SerializeField]
    private AudioSource m_beatAudioSource;

    private List<AudioSource> m_audioSources = new List<AudioSource>();
    private List<Coroutine> m_activeSongLineCoroutines = new List<Coroutine>();

    public void PlaySongLoop(Song _song)
    {
        StopAllCoroutines();

        m_beatAudioSource.Play();
        StartCoroutine(PlaySongLoopCoroutine(_song));
    }

    public void PlaySong(Song _song, bool _stopCoroutines = true)
    {
        if (_stopCoroutines)
        {
            StopAllCoroutines();
        }
        for (int i = 0; i < _song.SongLines.Count; i++)
        {
            if (m_audioSources.Count <= i)
            {
                m_audioSources.Add(gameObject.AddComponent<AudioSource>());
            }
            m_activeSongLineCoroutines.Add(StartCoroutine(PlaySongLine(_song.SongLines[i], m_audioSources[i])));
        }
        m_beatAudioSource.Play();
    }

    public void StopPlaying()
    {
        m_beatAudioSource.Stop();
        StopAllCoroutines();
        m_activeSongLineCoroutines.Clear();
    }

    private IEnumerator PlaySongLine(SongLine _songLine, AudioSource _audioSource)
    {
        InstrumentSO _instrument = m_instruments.FirstOrDefault(x => x.InstrumentName == _songLine.Instrument);
        foreach (Note _note in _songLine.Notes)
        {
            if (_note.Tone == ETones.Pause)
            {
                _audioSource.Stop();
            }
            else
            {
                _audioSource.clip = _instrument.ToneSoundPairings.FirstOrDefault(x => x.Tone == _note.Tone).AudioClip;
                _audioSource.Play();
            }
            yield return new WaitForSeconds(_note.Duration);
        }
        _audioSource.Stop();
    }

    private IEnumerator PlaySongLoopCoroutine(Song _song)
    {
        while (true)
        {
            PlaySong(_song, false);
            yield return new WaitForSeconds(_song.GetSongLength());
            foreach (Coroutine activeSongLine in m_activeSongLineCoroutines)
            {
                if (activeSongLine != null)
                {
                    StopCoroutine(activeSongLine);
                }
            }

            m_activeSongLineCoroutines.Clear();
        }
    }
}
