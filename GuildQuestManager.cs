using System.Collections;
using System.Collections.Generic;
using StarCloudgamesLibrary;
using UnityEngine;

public class GuildQuestManager : MonoBehaviour
{
    public List<QuestData> questDatas;

    #region "Unity"

    private void Start()
    {
        InitializeProgress();

        Initialize();
    }

    #endregion

    #region "Initialize"

    private void InitializeProgress()
    {
        QuestManager.instance.AddProgressAction(ProgressQuest);
    }

    private void Initialize()
    {
        questDatas = QuestManager.instance.GetQuestList(QuestCategory.GuildQuest);
    }

    #endregion

    #region "Progress"

    public void ProgressQuest(QuestType questType, int amount)
    {
        if(GuildManager.instance.MyGuildInformation() == null) return;

        var targetQuests = questDatas.FindAll(x => x.questType == questType);
        if(targetQuests.Count == 0) return;

        foreach(var targetQuest in targetQuests)
        {
            UserDatabaseController.instance.ProgressQuest(targetQuest, amount);
        }
    }

    #endregion
}