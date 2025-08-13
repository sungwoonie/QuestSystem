using System;
using System.Collections;
using System.Collections.Generic;
using StarCloudgamesLibrary;
using UnityEngine;

public class QuestData
{
    public string id;
    public DayType dayType;
    public QuestType questType;
    public int count;

    public SCReward reward;

    public QuestData(Dictionary<string, string> data)
    {
        id = StringParser.ParseString(data["ID"]);
        dayType = StringParser.ParseEnum(data, "DayType", DayType.None);
        questType = StringParser.ParseEnum(data, "QuestType", QuestType.None);
        count = StringParser.ParseInt(data["Count"]);

        reward = new SCReward()
        {
            rewardName = $"{id}_QuestReward",
            rewardType = StringParser.ParseEnum(data, "RewardType", RewardType.None),
            rewardID = StringParser.ParseEnum(data, "RewardID", RewardID.None),
            amount = StringParser.ParseDouble(data, "RewardAmount")
        };
    }
}

[Serializable]
public enum DayType
{
    None = 0,

    Daily = 1,
    Weekly = 2,
    Monthly = 3
}

[Serializable]
public enum QuestType
{
    None = 0,

    NormalMonterKill = 1,
    BossMonsterKill = 2,

    DungeonChallenge = 10,
    AdvancementChallenge = 11,
    TowerOfPandaChallenge = 12,
    LimitBreakChallenge = 13,

    GetGold = 20,
    GetCash = 21,
    GetBeyondStone = 22,
    GetRelicStone = 23,
    GetEnhanceStone = 24,
    GetAwakeningStone = 25,
    GetSkillStone = 26,
    GetDice = 27,
    GetMagicStone = 28,
    GetMagicPower = 29,

    UseGold = 50,
    UseCash = 51,
    UseBeyondStone = 52,
    UseRelicStone = 53,
    UseEnhanceStone = 54,
    UseAwakeningStone = 55,
    UseSkillStone = 56,
    UseDice = 57,
    UseLowPetFood = 58,
    UseMiddlePetFood = 59,
    UseHighPetFood = 60,

    GachaEquipment = 100,
    GachaSkill = 101,
    GachaRelic = 102,
    GachaPet = 103,

    EnhanceEquipment = 201,
    EnhanceSkill = 202,
    EnhanceRelic = 203,

    WatchAD = 1000
}