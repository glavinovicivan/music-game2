using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SongRecordingController : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_playbackSource;
    [SerializeField]
    private AudioSource m_metronomeSource;

    [SerializeField]
    private List<InstrumentSO> m_instruments;
    [SerializeField]
    private List<ScaleSO> m_scales;

    [SerializeField]
    private int m_numberOfTracks = 1;

    [SerializeField]
    private float m_noteDuration = 0.8f;

    [SerializeField]
    private int m_pausesAtBeginningOfSong = 3;

    private Song m_recordedSong;
    private SongLine m_currentRecordedLine;
    private ETones m_currentRecordedTone;

    private bool m_isRecordingTone;
    private bool m_isRecordingSong;

    private int m_currentInstrumentIndex;

    private DateTime m_timeSinceLastTonePress;
    private bool m_trackPauseDuration;

    #region PhaseControls
    public void StartRecordingSong()
    {
        if (!m_isRecordingSong)
        {
            m_recordedSong = new Song();
            m_currentRecordedLine = new SongLine() { Instrument = m_instruments[m_currentInstrumentIndex].InstrumentName, Notes = new List<Note>() };
            for (int i = 0; i < m_pausesAtBeginningOfSong; i++)
            {
                m_currentRecordedLine.Notes.Add(new Note() { Tone = ETones.Pause, Duration = m_noteDuration });
            }
            m_isRecordingSong = true;
            m_trackPauseDuration = false;

            m_metronomeSource.Play();
        }
    }

    public bool AdvanceRecordingPhase()
    {
        if (m_recordedSong.SongLines.Count == m_numberOfTracks - 1)
        {
            m_metronomeSource.Stop();

            m_recordedSong.AddSongLine(m_currentRecordedLine);
            m_isRecordingSong = false;
            return true;
        }

        m_recordedSong.AddSongLine(m_currentRecordedLine);
        m_currentRecordedLine = new SongLine() { Instrument = m_instruments[m_currentInstrumentIndex].InstrumentName, Notes = new List<Note>() };
        m_trackPauseDuration = false;
        m_timeSinceLastTonePress = DateTime.Now;

        return false;
    }

    public void UndoRecording()
    {
        m_recordedSong.UndoSongLine();
    }

    public void StartRecordingTone(int _toneNumber)
    {
        if (m_isRecordingSong && !m_isRecordingTone)
        {
            float pauseDuration = (float)(DateTime.Now - m_timeSinceLastTonePress).TotalSeconds;

            //adds beat length pauses between tones
            if (m_trackPauseDuration)
            {
                if (pauseDuration > m_noteDuration)
                {
                    int numberOfPauses = Mathf.RoundToInt(pauseDuration / m_noteDuration);
                    for (int i = 0; i < numberOfPauses; i++)
                    {
                        m_currentRecordedLine.Notes.Add(new Note() { Tone = ETones.Pause, Duration = m_noteDuration });
                    }
                    m_trackPauseDuration = false;
                }
            }

            //if its the start of the track, add pause to start on the right beat
            else if (m_recordedSong.SongLines.Count != 0)
            {
                int positionInSong = (int)((pauseDuration % m_recordedSong.GetSongLength()) / m_noteDuration);
                //m_currentRecordedLine.Notes.Add(new Note() { Tone = ETones.Pause, Duration = positionInSong * m_noteDuration });
                for (int i = 0; i < positionInSong; i++)
                {
                    m_currentRecordedLine.Notes.Add(new Note() { Tone = ETones.Pause, Duration = m_noteDuration });
                }
            }

            m_currentRecordedTone = _toneNumber - 1 < m_scales[0].Tones.Count ? m_scales[0].Tones[_toneNumber - 1] : m_scales[0].Tones.Last();
            m_playbackSource.clip = m_instruments[m_currentInstrumentIndex].ToneSoundPairings.FirstOrDefault(x => x.Tone == m_currentRecordedTone).AudioClip;
            m_playbackSource.Play();
            m_isRecordingTone = true;
        }
    }

    public void EndRecordingTone(float _duration, bool _stopPlaybackAudioSource = false)
    {
        if (m_isRecordingSong)
        {
            m_currentRecordedLine.Notes.Add(new Note() { Tone = m_currentRecordedTone, Duration = m_noteDuration });
            m_isRecordingTone = false;
            if (_stopPlaybackAudioSource)
            {
                m_playbackSource.Stop();
            }
            m_timeSinceLastTonePress = DateTime.Now;
            m_trackPauseDuration = true;
        }
    }
    #endregion

    #region QuestRewards
    public void AddInstrument(InstrumentSO _instrument)
    {
        if (!m_instruments.Contains(_instrument))
        {
            m_instruments.Add(_instrument);
        }
    }

    public void AddScale(ScaleSO _scale)
    {
        if (!m_scales.Contains(_scale))
        {
            m_scales.Add(_scale);
        }
    }

    public void AddSongTracks(int _tracksToAdd)
    {
        m_numberOfTracks += _tracksToAdd;
    }
    #endregion

    public void ChangeInstrument()
    {
        m_currentInstrumentIndex = (m_currentInstrumentIndex + 1) % m_instruments.Count;

        if (m_isRecordingSong && m_currentRecordedLine.Notes.Where(x => x.Tone != ETones.Pause).Count() == 0)
        {
            m_currentRecordedLine = new SongLine() { Instrument = m_instruments[m_currentInstrumentIndex].name, Notes = new List<Note>() };
            if (m_recordedSong.SongLines.Count == 0)
            {
                for (int i = 0; i < m_pausesAtBeginningOfSong; i++)
                {
                    m_currentRecordedLine.Notes.Add(new Note() { Tone = ETones.Pause, Duration = m_noteDuration });
                }
            }
            m_playbackSource.clip = (m_instruments[m_currentInstrumentIndex].ToneSoundPairings.Where(x => x.Tone == m_scales[0].Tones[0]).First().AudioClip);
            m_playbackSource.Play();
        }
    }

    public Song GetRecordedSong()
    {
        return m_recordedSong;
    }
}
