using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MelodySongTest", menuName = "ScriptableObjects/SongTests/MelodySongTest")]
public class MelodySongTest : SongTest
{
    [SerializeField]
    private List<Note> m_notes;
    [SerializeField]
    private bool m_matchAllLines;
    [SerializeField]
    private bool m_matchDurations = false;
    [SerializeField]
    private float m_maxDurationDeviation = 0.3f;

    protected override bool SongCorrectCheck(Song _song)
    {
        int numberOfCorrectLines = 0;
        foreach (SongLine songLine in _song.SongLines)
        {
            if (IsSongLineCorrect(songLine))
            {
                numberOfCorrectLines++;
            }
            else
            {
                if (m_matchAllLines)
                {
                    return false;
                }
            }
        }
        return numberOfCorrectLines > 0;
    }

    private bool IsSongLineCorrect(SongLine _songLine)
    {
        bool isSongLineCorrect = true;
        if (m_notes.Count != _songLine.Notes.Count)
        {
            return false;
        }
        for (int i = 0; i < m_notes.Count; i++)
        {
            if (m_notes[i].Tone != _songLine.Notes[i].Tone)
            {
                isSongLineCorrect = false;
            }
            if (m_matchDurations)
            {
                if (Mathf.Abs(m_notes[i].Duration - _songLine.Notes[i].Duration) > m_maxDurationDeviation)
                {
                    isSongLineCorrect = false;
                }
            }
        }
        return isSongLineCorrect;
    }
}
