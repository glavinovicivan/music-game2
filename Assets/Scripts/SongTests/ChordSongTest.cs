using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChordSongTest", menuName = "ScriptableObjects/SongTests/ChordSongTest")]
public class ChordSongTest : SongTest
{
    public List<ETones> ChordTones;
    public bool ExactMatch = false;

    protected override bool SongCorrectCheck(Song _song)
    {
        for (int i = 0; i < _song.GetSongNumberOfTones(); i++)
        {
            List<ETones> chordTonesCopy = new List<ETones>(ChordTones);

            foreach (SongLine _songLine in _song.SongLines)
            {
                ETones tone = _songLine.Notes[i].Tone;
                if (ExactMatch && !ChordTones.Contains(tone))
                {
                    continue;
                }
                if (_songLine.Notes.Count > i && chordTonesCopy.Contains(_songLine.Notes[i].Tone))
                {
                    chordTonesCopy.Remove(tone);
                }
            }

            if(chordTonesCopy.Count == 0)
            {
                return true;
            }
        }
        return false;
    }
}
