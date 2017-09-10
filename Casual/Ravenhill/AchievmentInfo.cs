using Casual.Ravenhill.Data;
using UnityEngine;

namespace Casual.Ravenhill {
    public class AchievmentInfo : RavenhillGameElement, ISaveElement {

        public const int MAX_TIER = 5;
        public const int NO_REWARD_TIER = 0;

        private AchievmentData data = null;

        public string Id { get; private set; }
        public int Count { get; private set; }
        public int Tier { get; private set; }
        public int RewardTier { get; private set; }

        public AchievmentInfo(AchievmentData data ) {
            this.data = data;
            Id = data.id;
            Count = 0;
            Tier = 0;
            RewardTier = NO_REWARD_TIER;
        }

        public AchievmentInfo(UXMLElement element) {
            Load(element);
        }

        public AchievmentData Data {
            get {
                if(data == null ) {
                    if(Id.IsValid()) {
                        data = resourceService.GetAchievment(Id);
                    }
                }
                return data;
            }
        }

        public bool HasReward => RewardTier != NO_REWARD_TIER;

        public bool IsMaxTier => Tier == MAX_TIER;

        public int CompletionPercent => (int)(100.0f * ((float)Count / (float)Data.TotalCount));

        //public int MyDiff {
        //    get {
        //        return Count - CurrentTierBaseCount;
        //    }
        //}

        //public int FullDiff {
        //    get {
        //        if(Tier > 0 && Tier < MAX_TIER ) {
        //            int nextTierValue = Data.GetTier(Tier + 1).count;
        //            return nextTierValue - CurrentTierBaseCount;
        //        }
        //        return Data.GetTier(MAX_TIER).count - Data.GetTier(MAX_TIER - 1).count;
        //    }
        //}

        //public int CurrentTierBaseCount {
        //    get {
        //        AchievmentTierData currentTier = Data.GetTier(Tier);
        //        return (currentTier == null) ? 0 : currentTier.count;
        //    }
        //}
        //public float CurrentTierProgress {
        //    get {
        //        if(Tier >= MAX_TIER ) {
        //            return 1.0f;
        //        }
        //        AchievmentTierData currentTier = Data.GetTier(Tier);
        //        int currentTierValue = (currentTier == null) ? 0 : currentTier.count;
        //        int nextTierValue = Data.GetTier(Tier + 1).count;
        //        int myDiff = Count - currentTierValue;
        //        if(myDiff < 0 ) {
        //            myDiff = 0;
        //        }
        //        int baseDiff = nextTierValue - currentTierValue;
        //        return Mathf.Clamp01((float)myDiff / (float)baseDiff);
        //    }
        //}

        public bool IsUnlocked(int tierLevel) {
            return tierLevel <= Tier;
        }

        public void SetCounter(int val) {
            bool isOldHasReward = HasReward;
            if(Tier < MAX_TIER ) {
                Count = val;
                UpdateState();
                bool isNewHasReward = HasReward;

                if(!isOldHasReward && isNewHasReward) {
                    RavenhillEvents.OnTierAchieved(this, Data.GetTier(RewardTier));
                }
            }
        }

        public void IncrementCounter(int cnt = 1) {
            bool isOldHasReward = HasReward;
            if(Tier < MAX_TIER) {
                Count += cnt;
                UpdateState();
                bool isNewHasReward = HasReward;
                if(!isOldHasReward && isNewHasReward) {
                    RavenhillEvents.OnTierAchieved(this, Data.GetTier(RewardTier));
                }
             }
        }

        private void UpdateState() {
            if(Tier < MAX_TIER ) {
                if(RewardTier == NO_REWARD_TIER ) {
                    var nextTier = Data.GetTier(Tier + 1);
                    if(nextTier != null ) {
                        if(Count >= nextTier.count ) {
                            RewardTier = nextTier.tier;
                        }
                    }
                }
            }
        }

        public void GoToNextTier() {
            if(Tier < MAX_TIER ) {
                RewardTier = NO_REWARD_TIER;
                Tier += 1;
            } else {
                RewardTier = NO_REWARD_TIER;
            }
            UpdateState();
        }

        public AchievmentTierData NextTier => Data.GetTier(Tier + 1);

        public void Load(UXMLElement element ) {
            Id = element.GetString("id");
            Count = element.GetInt("count");
            Tier = element.GetInt("tier");
            RewardTier = element.GetInt("reward_tier");
        }

        public UXMLWriteElement GetSave() {
            UXMLWriteElement element = new UXMLWriteElement("achievment");
            element.AddAttribute("id", Id);
            element.AddAttribute("count", Count);
            element.AddAttribute("tier", Tier);
            element.AddAttribute("reward_tier", RewardTier);
            return element;
        }

        public void InitSave() {
            Count = 0;
            Tier = 0;
            RewardTier = NO_REWARD_TIER;
        }
    }
}
