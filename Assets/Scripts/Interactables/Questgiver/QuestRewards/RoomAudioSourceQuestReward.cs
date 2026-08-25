using UnityEngine;

[CreateAssetMenu(fileName = "RoomAudioSourceReward", menuName = "ScriptableObjects/QuestRewards/RoomAudioSourceReward")]

public class RoomAudioSourceQuestReward : QuestReward
{
    public string AudioSourceKey;
    public bool ShouldSourceBeActive;

    public override void ApplyReward(PlayerController _playerController, IInteractable _questgiverController)
    {
        RoomController roomController = _questgiverController.GetRoom();

        if (ShouldSourceBeActive)
        {
            roomController.StartAudioSource(AudioSourceKey);
        }
        else
        {
            roomController.StopAudioSource(AudioSourceKey);
        }
    }
}
