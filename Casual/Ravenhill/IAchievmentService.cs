﻿using Casual.Ravenhill.Data;

namespace Casual.Ravenhill {
    public interface IAchievmentService : IService {
        bool HasAchievment(AchievmentData data);
        AchievmentInfo GetAchievment(AchievmentData data);
        AchievmentInfo GetAchievment(string id);
        bool IsTierUnlocked(AchievmentData data, int tier);
        void RewardAchievment(string id);
    }
}
