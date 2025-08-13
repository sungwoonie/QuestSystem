using System;
using System.Collections;
using System.Collections.Generic;
using StarCloudgamesLibrary;
using UnityEngine;

public class QuestManager : SingleTon<QuestManager>
{
    [SerializeField] private TextAsset guildQuestDataTable;

    private Dictionary<QuestCategory, List<QuestData>> questDatas;

    private Action<QuestType, int> questProgressAction;

    private static readonly IReadOnlyDictionary<DayType, Func<DateTime, DateTime, bool>> ResetCheckers = new Dictionary<DayType, Func<DateTime, DateTime, bool>>
    {
        { DayType.Daily,   (start, now) => start.Date != now.Date },
        { DayType.Weekly,  (start, now) => GetWeekStartMonday(start) != GetWeekStartMonday(now) },
        { DayType.Monthly, (start, now) => start.Year != now.Year || start.Month != now.Month },
    };


    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        InitializeQuestDatas();
    }

    private void Start()
    {
        SetUpResetTimer();
    }

    #endregion

    #region "Progress Action"

    public void ProgressQuest(QuestType questType, int amount)
    {
        questProgressAction?.Invoke(questType, amount);
    }

    public void AddProgressAction(Action<QuestType, int> action)
    {
        questProgressAction += action;
    }

    public void RemoveProgressAction(Action<QuestType, int> action)
    {
        questProgressAction -= action;
    }

    #endregion

    #region "Data"

    private void InitializeQuestDatas()
    {
        questDatas = new Dictionary<QuestCategory, List<QuestData>>();

        var loader = new DataParser<int, QuestData>(data => new QuestData(data));
        questDatas[QuestCategory.GuildQuest] = loader.LoadList(guildQuestDataTable);
    }

    public List<QuestData> GetQuestList(QuestCategory category)
    {
        return questDatas[category];
    }

    #endregion

    #region "Reset"

    private void SetUpResetTimer()
    {
        BackendTimeManager.instance.AddAction(ResetAllQuest, true);
    }

    private void ResetAllQuest(DateTime dateTime)
    {
        var userDatas = UserDatabaseController.instance.UserQuestData.questDatas;
        if(userDatas == null) return;

        var removeList = new List<string>();

        foreach(var userData in userDatas)
        {
            var data = userData.Value;
            if(!ResetCheckers.TryGetValue(data.dayType, out var checker)) continue;
            if(checker(data.startTime, dateTime)) removeList.Add(userData.Key);
        }

        foreach(var key in removeList) userDatas.Remove(key);
    }

    private static DateTime GetWeekStartMonday(DateTime dt)
    {
        var diff = ((int)dt.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
        return dt.Date.AddDays(-diff);
    }

    #endregion
}

[Serializable]
public enum QuestCategory
{
    GuildQuest
}