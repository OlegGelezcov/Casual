using System;
using Casual.Ravenhill.Data;

namespace Casual.Ravenhill {
    public abstract class Condition : RavenhillGameElement {

        public abstract bool Check(IQuestService service, IQuest quest);
        public abstract ConditionType type { get; }
        public abstract string GetStateString(IQuestService service, IQuest quest);

        public static Condition FromXml(UXMLElement element) {
            ConditionType type = element.GetEnum<ConditionType>("type");
            switch(type) {
                case ConditionType.has_collectable: {
                        return new HasCollectableCondition(element.GetString("id"));
                    }
                case ConditionType.has_collection: {
                        return new HasCollectionCondition(element.GetString("id"));
                    }
                case ConditionType.has_story_collection: {
                        return new HasStoryCollectionCondition(element.GetString("id"));
                    }
                case ConditionType.last_search_room: {
                        return new LastSearchRoomCondition(element.GetString("id"));
                    }
                case ConditionType.level_ge: {
                        return new LevelGeCondition(element.GetInt("value"));
                    }
                case ConditionType.quest_completed: {
                        return new QuestCompletedCondition(element.GetString("id"));
                    }
                case ConditionType.random: {
                        return new RandomCondition(element.GetFloat("value"));
                    }
                case ConditionType.room_mode: {
                        return new RoomModeCondition(element.GetEnum<RoomMode>("value"));
                    }
                case ConditionType.search_counter_ge: {
                        return new SearchCounterGeCondition(element.GetInt("value"));
                    }
                default: {
                        return new NoneCondition();
                    }
            }
        }

    }

    public class NoneCondition : Condition {
        public override bool Check(IQuestService service, IQuest quest) {
            return true;
        }
        public override ConditionType type => ConditionType.none;

        public override string GetStateString(IQuestService service, IQuest quest) {
            return $"{type} => {Check(service, quest)}";
        }
    }

    public class LevelGeCondition : Condition {

        public int level { get; }

        public LevelGeCondition(int level) {
            this.level = level;
        }

        public override bool Check(IQuestService service, IQuest quest) {
            return (service.playerLevel >= level);
        }

        public override ConditionType type => ConditionType.level_ge;

        public override string GetStateString(IQuestService service, IQuest quest) {
            return $"{type}: level >= {level} => {Check(service, quest)}";
        }
    }

    public class QuestCompletedCondition : Condition {
        public string id { get; }

        public QuestCompletedCondition(string id) {
            this.id = id;
        }

        public override bool Check(IQuestService service, IQuest quest) {
            return service.IsCompleted(id);
        }

        public override ConditionType type => ConditionType.quest_completed;

        public override string GetStateString(IQuestService service, IQuest quest) {
            return $"{type}: {id} completed => {Check(service, quest)}";
        }
    }

    public class HasStoryCollectionCondition : Condition {
        public string id { get; }

        public HasStoryCollectionCondition(string id) {
            this.id = id;
        }

        public override bool Check(IQuestService service, IQuest quest) {
            return service.IsHasStoryCollection(id);
        }

        public override ConditionType type => ConditionType.has_story_collection;

        public override string GetStateString(IQuestService service, IQuest quest) {
            return $"{type}: has story collection {id} => {Check(service, quest)}";
        }
    }

    public class RoomModeCondition : Condition {
        public RoomMode mode { get; }
        
        public RoomModeCondition(RoomMode mode) {
            this.mode = mode;
        }

        public override bool Check(IQuestService service, IQuest quest) {
            return service.IsRoomMode(mode);
        }

        public override ConditionType type => ConditionType.room_mode;

        public override string GetStateString(IQuestService service, IQuest quest) {
            return $"{type}: is room mode {mode} => {Check(service, quest)}";
        }
    }

    public class RandomCondition : Condition {

        public float prob { get; }

        public RandomCondition(float prob) {
            this.prob = prob;
        }

        public override bool Check(IQuestService service, IQuest quest) {
            return UnityEngine.Random.value < prob;
        }

        public override ConditionType type => ConditionType.random;

        public override string GetStateString(IQuestService service, IQuest quest) {
            return $"{type}: random <= {prob} => {Check(service, quest)}";
        }
    }

    public class HasCollectionCondition : Condition {

        public string id { get; }

        public HasCollectionCondition(string id) {
            this.id = id;
        }

        public override bool Check(IQuestService service, IQuest quest) {
            return service.IsHasCollection(id);
        }

        public override ConditionType type => ConditionType.has_collection;

        public override string GetStateString(IQuestService service, IQuest quest) {
            return $"{type}: has collection {id} => {Check(service, quest)}";
        }
    }

    public class LastSearchRoomCondition : Condition {

        public string id { get; }

        public LastSearchRoomCondition(string id) {
            this.id = id;
        }

        public override bool Check(IQuestService service, IQuest quest) {
            return service.IsLastSearchRoom(id);
        }

        public override ConditionType type => ConditionType.last_search_room;

        public override string GetStateString(IQuestService service, IQuest quest) {
            return $"{type}: last search room is {id} => {Check(service, quest)}";
        }
    }

    public class HasCollectableCondition : Condition {
        public string id { get; }

        public HasCollectableCondition(string id) {
            this.id = id;
        }

        public override bool Check(IQuestService service, IQuest quest) {
            return service.IsHasCollectable(id);
        }

        public override ConditionType type => ConditionType.has_collectable;

        public override string GetStateString(IQuestService service, IQuest quest) {
            return $"{type}: has collectable {id} => {Check(service, quest)}";
        }
    }

    public class SearchCounterGeCondition : Condition {
        public int count { get; }

        public SearchCounterGeCondition(int count) {
            this.count = count;
        }

        public override string GetStateString(IQuestService service, IQuest quest) {
            return $"{type}: {count} <= {service.searchCounter} => {Check(service, quest)}";
        }

        public override ConditionType type => ConditionType.search_counter_ge;

        public override bool Check(IQuestService service, IQuest quest) {
            return service.searchCounter >= count;
        }
    }
}
