using Casual.Ravenhill.Data;
using Casual.Ravenhill.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill {

    public static class RavenhillEvents {

        public static event System.Action<bool, bool, int> SearchTimerPauseChanged;
        public static event System.Action SearchTimerCompleted;
        public static event System.Action<int, int> PlayerMaxHealthChanged;
        public static event System.Action<float, float> PlayerHealthChanged;
        public static event System.Action<string, string> PlayerAvatarChanged;
        public static event System.Action<string, string> PlayerNameChanged;
        public static event System.Action<int, int> PlayerLevelChanged;
        public static event System.Action<int, int> PlayerExpChanged;
        public static event System.Action<int, int> PlayerGoldChanged;
        public static event System.Action<int, int> PlayerSilverChanged;
        public static event System.Action<ISaveable> SaveableLoaded;
        public static event System.Action<RoomInfo> RoomUnlocked;
        public static event System.Action<int, int, RoomInfo> RoomRecordTimeChanged;
        public static event System.Action<int, int, RoomInfo> RoomProgressChanged;
        public static event System.Action<SearchMode, SearchMode, RoomInfo> SearchModeChanged;
        public static event System.Action<RoomLevel, RoomLevel, RoomInfo> RoomLevelChanged;
        public static event System.Action<SearchSession> SearchSessionEnded;
        public static event System.Action<SearchSession> SearchSessionStarted;
        public static event System.Action<RoomMode, RoomMode> RoomModeChanged;
        public static event System.Action<GameModeName, GameModeName> GameModeChanged;
        public static event System.Action<SearchText, SearchObjectData> SearchTextStroked;
        public static event System.Action<SearchObjectData, BaseSearchableObject> SearchObjectActivated;
        public static event System.Action<int, int> SearchProgressChanged;
        public static event System.Action<SearchObjectData, ISearchableObject> SearchObjectCollected;
        public static event System.Action SearchStarted;
        public static event System.Action<Vector2> Touch;
        public static event System.Action<RavenhillViewType> ViewAdded;
        public static event System.Action<RavenhillViewType> ViewRemoved;
        public static event System.Action ResourceLoaded;
        public static event System.Action<InventoryItemType, string, int> InventoryChanged;
        public static event System.Action<InventoryItemType, string, int> InventoryItemAdded;
        public static event System.Action<InventoryItemType, string, int> InventoryItemRemoved;

        public static event System.Action<InventoryItemData> AddedToWishlist;
        public static event System.Action<InventoryItemData> RemovedFromWishlist;

        public static event System.Action<JournalEntryInfo> JournalEntryAdded;
        public static event System.Action<JournalEntryInfo> JournalEntryRemoved;
        public static event System.Action<JournalEntryInfo> JournalEntryEndTextOpened;
        public static event System.Action<JournalEntryInfo> JournalEntryEndTextClosed;

        public static event System.Action<int> SearchCounterChanged;

        public static event System.Action<QuestInfo> QuestCompleted;
        public static event System.Action<QuestInfo> QuestStarted;
        public static event System.Action<QuestInfo> QuestRewarded;

        public static event System.Action<StoryCollectionData> StoryCollectionCharged;

        public static event System.Action<string, NpcInfo> NpcCreated;
        public static event System.Action<string, NpcData> NpcRemoved;

        public static event System.Action ExitCurrentScene;

        public static event System.Action<string> SceneLoaded;

        public static event System.Action<AchievmentInfo, AchievmentTierData> TierAchieved;
        public static event System.Action<AchievmentData, AchievmentTierData> AchievmentRewarded;

        public static event System.Action<BonusData> AlchemyCharged;

        public static event System.Action<BuffInfo> BuffAdded;
        public static event System.Action<BuffInfo> BuffRemoved;

        public static event System.Action<bool> MusicStateChanged;
        public static event System.Action<bool> SoundStateChanged;

        public static event System.Action<float, float> RoomModeSwitchTimerChanged;

        public static void OnRoomModeSwitchTimerChanged(float timer, float interval) {
            RoomModeSwitchTimerChanged?.Invoke(timer, interval);
        }

        public static void OnSoundStateChanged(bool enabled) {
            SoundStateChanged?.Invoke(enabled);
        }

        public static void OnMusicStateChanged(bool enabled ) {
            MusicStateChanged?.Invoke(enabled);
        }

        public static void OnBuffRemoved(BuffInfo buff ) {
            BuffRemoved?.Invoke(buff);
        }

        public static void OnBuffAdded(BuffInfo buff ) {
            BuffAdded?.Invoke(buff);
        }

        public static void OnAlchemyCharged(BonusData data ) {
            AlchemyCharged?.Invoke(data);
        }

        public static void OnAchievmentRewarded(AchievmentData achievmentData, AchievmentTierData tierData ) {
            AchievmentRewarded?.Invoke(achievmentData, tierData);
        }

        public static void OnTierAchieved(AchievmentInfo info, AchievmentTierData data) {
            TierAchieved?.Invoke(info, data);
        }

        public static void OnSceneLoaded(string sceneName ) {
            SceneLoaded?.Invoke(sceneName);
        }

        public static void OnExitCurrentScene() {
            ExitCurrentScene?.Invoke();
        }

        public static void OnNpcCreated(string roomId, NpcInfo npc) {
            NpcCreated?.Invoke(roomId, npc);
        }

        public static void OnNpcRemoved(string roomId, NpcData npc ) {
            NpcRemoved?.Invoke(roomId, npc);
        }

        public static void OnStoryCollectionCharged(StoryCollectionData storyCollectionData ) {
            StoryCollectionCharged?.Invoke(storyCollectionData);
        }

        public static void OnQuestRewarded(QuestInfo quest) {
            QuestRewarded?.Invoke(quest);
        }

        public static void OnQuestStarted(QuestInfo quest) {
            QuestStarted?.Invoke(quest);
        }

        public static void OnQuestCompleted(QuestInfo quest) {
            QuestCompleted?.Invoke(quest);
        }

        public static void OnSearchCounterChanged(int counter) {
            SearchCounterChanged?.Invoke(counter);
        }

        public static void OnJournalEntryAdded(JournalEntryInfo info) {
            JournalEntryAdded?.Invoke(info);
        }

        public static void OnJournalEntryRemoved(JournalEntryInfo info) {
            JournalEntryRemoved?.Invoke(info);
        }

        public static void OnJournalEntryEndTextOpened(JournalEntryInfo info ) {
            JournalEntryEndTextOpened?.Invoke(info);
        }

        public static void OnJournalEntryEndTextClosed(JournalEntryInfo info) {
            JournalEntryEndTextClosed?.Invoke(info);
        }

        public static void OnAddedToWishlist(InventoryItemData data) {
            AddedToWishlist?.Invoke(data);
        }

        public static void OnRemovedFromWishlist(InventoryItemData data) {
            RemovedFromWishlist?.Invoke(data);
        }

        public static void OnInventoryItemRemoved(InventoryItemType type, string id, int count) {
            InventoryItemRemoved?.Invoke(type, id, count);
        }
        public static void OnInventoryItemAdded(InventoryItemType type, string id, int count) {
            InventoryItemAdded?.Invoke(type, id, count);
        }
        public static void OnInventoryChanged(InventoryItemType type, string id, int count ) {
            InventoryChanged?.Invoke(type, id, count);
        }

        public static void OnSearchTimerPauseChanged(bool oldPaused, bool newPaused, int interval) {
            SearchTimerPauseChanged?.Invoke(oldPaused, newPaused, interval);
        }

        public static void OnSearchTimerCompleted() {
            SearchTimerCompleted?.Invoke();
        }

        public static void OnPlayerMaxHealthChanged(int prevValue, int newValue) {
            PlayerMaxHealthChanged?.Invoke(prevValue, newValue);
        }

        public static void OnPlayerHealthChanged(float prevValue, float newValue ) {
            PlayerHealthChanged?.Invoke(prevValue, newValue);
        }

        public static void OnPlayerAvatarChanged(string prevAvatar, string newAvatar) {
            PlayerAvatarChanged?.Invoke(prevAvatar, newAvatar);
        }

        public static void OnPlayerNameChanged(string prevName, string newName) {
            PlayerNameChanged?.Invoke(prevName, newName);
        }

        public static void OnPlayerLevelChanged(int prevLevel, int newLevel) {
            PlayerLevelChanged?.Invoke(prevLevel, newLevel);
        }

        public static void OnPlayerExpChanged(int prevExp, int newExp ) {
            PlayerExpChanged?.Invoke(prevExp, newExp);
        }

        public static void OnPlayerGoldChanged(int prevGold, int newGold ) {
            PlayerGoldChanged?.Invoke(prevGold, newGold);
        }

        public static void OnPlayerSilverChanged(int prevSilver, int newSilver ) {
            PlayerSilverChanged?.Invoke(prevSilver, newSilver);
        }

        public static void OnSaveableLoaded(ISaveable saveable) {
            SaveableLoaded?.Invoke(saveable);
        }

        public static void OnRoomUnlocked(RoomInfo roomInfo ) {
            RoomUnlocked?.Invoke(roomInfo);
        }

        public static void OnRoomRecordTimeChanged(int prevRecord, int newRecord, RoomInfo roomInfo) {
            RoomRecordTimeChanged?.Invoke(prevRecord, newRecord, roomInfo);
        }

        public static void OnRoomProgressChanged(int prevProgress, int newProgress, RoomInfo roomInfo) {
            RoomProgressChanged?.Invoke(prevProgress, newProgress, roomInfo);
        }

        public static void OnSearchModeChanged(SearchMode prevSearchMode, SearchMode newSearchMode, RoomInfo roomInfo) {
            SearchModeChanged?.Invoke(prevSearchMode, newSearchMode, roomInfo);
        }

        public static void OnRoomLevelChanged(RoomLevel prevLevel, RoomLevel newLevel, RoomInfo roomInfo) {
            RoomLevelChanged?.Invoke(prevLevel, newLevel, roomInfo);
        }

        public static void OnSearchSessionEnded(SearchSession session) {
            SearchSessionEnded?.Invoke(session);
        }

        public static void OnSearchSessionStarted(SearchSession session) {
            SearchSessionStarted?.Invoke(session);
        }

        public static void OnRoomModeChanged(RoomMode oldMode, RoomMode newMode) {
            RoomModeChanged?.Invoke(oldMode, newMode);
        }

        public static void OnGameModeChanged(GameModeName prevGameMode, GameModeName newGameMode) {
            GameModeChanged?.Invoke(prevGameMode, newGameMode);
        }

        public static void OnSearchTextStroked(SearchText searchText, SearchObjectData searchObjectData) {
            SearchTextStroked?.Invoke(searchText, searchObjectData);
        }

        public static void OnSearchObjectActivated(SearchObjectData data, BaseSearchableObject searchableObject) {
            SearchObjectActivated?.Invoke(data, searchableObject);
        }

        public static void OnSearchProgressChanged(int foundedCount, int totalCount) {
            SearchProgressChanged?.Invoke(foundedCount, totalCount);
        }

        public static void OnSearchObjectCollected(SearchObjectData searchObjectData, ISearchableObject targetSearchableObject) {
            SearchObjectCollected?.Invoke(searchObjectData, targetSearchableObject);
        }

        public static void OnSearchStarted() {
            SearchStarted?.Invoke();
        }

        public static void OnTouch(Vector2 position ) {
            Touch?.Invoke(position);
        }

        public static void OnViewAdded(RavenhillViewType type) {
            ViewAdded?.Invoke(type);
        }

        public static void OnViewRemoved(RavenhillViewType type) {
            ViewRemoved?.Invoke(type);
        }

        public static void OnResourceLoaded() {
            ResourceLoaded?.Invoke();
        }
    }

}
