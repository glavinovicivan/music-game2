using System.Collections.Generic;
using System;

[Serializable]
public struct SongLine
{
    public string Instrument;
    public List<Note> Notes;

    public void RemoveTrailingPauses()
    {
        for (int i = Notes.Count - 1; i >= 0; i--)
        {
            if (Notes[i].Tone == ETones.Pause)
            {
                Notes.RemoveAt(i);
            }
            else
            {
                return;
            }
        }
    }
}
