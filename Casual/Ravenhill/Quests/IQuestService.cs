using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public interface IQuestService : IService {
        int playerLevel { get; }
        bool IsCompleted(string id);
        bool IsHasStoryCollection(string id);
        bool IsRoomMode(RoomMode mode);
        bool IsHasCollection(string id);
        bool IsLastSearchRoom(string id);
        bool IsHasCollectable(string id);
        int searchCounter { get; }

        List<QuestInfo> startedQuestList { get; }
        List<QuestInfo> completedQuestList { get; }
        QuestInfo GetInfo(QuestData data);
        bool RewardQuest(string id);
        void ShowRewardExplicit(QuestInfo quest);
        List<QuestInfo> GetQuests(Func<QuestInfo, bool> predicate);

    }
}
