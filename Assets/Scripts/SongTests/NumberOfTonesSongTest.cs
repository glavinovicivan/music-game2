using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NumberOfTonesSongTest", menuName = "ScriptableObjects/SongTests/NumberOfTonesSongTest")]
public class NumberOfTonesSongTest : SongTest
{
    public int NumberOfTones;
    protected override bool SongCorrectCheck(Song _song)
    {
        return _song.GetSongNumberOfTones() >= NumberOfTones;
    }
}
