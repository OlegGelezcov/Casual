using Casual.Ravenhill.Data;
using Casual.Ravenhill.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill {

    public enum GameEventName : int {
        none = 0,
        touch = 1,
        view_added = 2,
        view_removed = 3,
        resource_loaded = 4,
        search_object_collected = 5,
        search_started = 6,
        search_progress_changed = 7,
        search_object_activated = 8,
        search_text_stroked = 9,
        game_mode_changed = 10,
        room_mode_changed = 11,
        search_session_started = 12,
        search_session_ended = 13,
        room_level_changed = 14,
        room_progress_changed = 15,
        search_mode_changed = 16,
        room_record_time_changed = 17,
        room_unlocked = 18,
        saveable_loaded = 19,

        player_silver_changed = 20,
        player_gold_changed = 21,
        player_exp_changed = 22,
        player_level_changed = 23,
        player_name_changed = 24,
        player_avatar_changed = 25,
        player_health_changed = 26,
        player_max_health_changed = 27
    }

    public class PlayerMaxHealthChangedEventArgs : IntValueChangedEventArgs {
        public PlayerMaxHealthChangedEventArgs(int oldValue, int newValue)
            : base(oldValue, newValue, GameEventName.player_max_health_changed) { }
    }

    public class PlayerHealthChangedEventArgs : IntValueChangedEventArgs {
        public PlayerHealthChangedEventArgs(int oldValue, int newValue )
            : base(oldValue, newValue, GameEventName.player_health_changed) { }
    }

    public class PlayerAvatarChangedEventArgs : StringValueChangedEventArgs {
        public PlayerAvatarChangedEventArgs(string oldValue, string newValue)
            : base(oldValue, newValue, GameEventName.player_avatar_changed) { }
    }

    public class PlayerNameChangedEventArgs : StringValueChangedEventArgs {
        public PlayerNameChangedEventArgs(string oldValue, string newValue)
            : base(oldValue, newValue, GameEventName.player_name_changed ) { }
    }

    public class PlayerLevelChangedEventArgs : IntValueChangedEventArgs {
        public PlayerLevelChangedEventArgs(int oldLevel, int newLevel)
            : base(oldLevel, newLevel, GameEventName.player_level_changed) { }
    }

    public class PlayerExpChangedEventArgs : IntValueChangedEventArgs {
        public PlayerExpChangedEventArgs(int oldExp, int newExp)
            : base(oldExp, newExp, GameEventName.player_exp_changed ) { }
    }

    public class PlayerGoldChangedEventArgs : IntValueChangedEventArgs {
        public PlayerGoldChangedEventArgs(int oldGold, int newGold)
            : base(oldGold, newGold, GameEventName.player_gold_changed) { }
    }

    public class PlayerSilverChangedEventArgs : IntValueChangedEventArgs {

        public PlayerSilverChangedEventArgs(int oldSilver, int newSilver)
            : base(oldSilver, newSilver, GameEventName.player_silver_changed ) { }
    }


    public class StringValueChangedEventArgs : ValueChangedEventArgs<string> {
        public StringValueChangedEventArgs(string oldValue, string newValue, GameEventName eventName)
            : base(oldValue, newValue, eventName) { }
    }

    public class IntValueChangedEventArgs : ValueChangedEventArgs<int> {

        public IntValueChangedEventArgs(int oldValue, int newValue, GameEventName eventName)
            : base(oldValue, newValue, eventName) {
        }
    }

    public class ValueChangedEventArgs<T> : RavenhillEventArgs {
        public T oldValue { get; }
        public T newValue { get; }
        public ValueChangedEventArgs(T oldValue, T newValue, GameEventName eventName ) 
            : base(eventName) {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }
    }

    public class SaveableLoadedEventArgs : RavenhillEventArgs {

        public ISaveable saveable { get; }

        public SaveableLoadedEventArgs(ISaveable saveable)
            : base(GameEventName.saveable_loaded) {
            this.saveable = saveable;
        }
    }

    public class RoomUnlockedEventArgs : RavenhillEventArgs {
        public RoomInfo roomInfo { get; }

        public RoomUnlockedEventArgs(RoomInfo roomInfo )
            : base(GameEventName.room_unlocked ) {
            this.roomInfo = roomInfo;
        }
    }

    public class RoomRecordTimeChangedEventArgs : RavenhillEventArgs {
        public int oldRecordTime { get; }
        public int newRecordTime { get; }
        public RoomInfo roomInfo { get; }

        public RoomRecordTimeChangedEventArgs(int oldTime, int newTime, RoomInfo roomInfo)
            : base(GameEventName.room_record_time_changed) {
            this.oldRecordTime = oldTime;
            this.newRecordTime = newRecordTime;
            this.roomInfo = roomInfo;
        }
    }

    public class RoomProgressChangedEventArgs : RavenhillEventArgs {
        public int oldRoomProgress { get; }
        public int newRoomProgress { get; }
        public RoomInfo roomInfo { get; }

        public RoomProgressChangedEventArgs(int oldRoomProgress, int newRoomProgress, RoomInfo roomInfo)
            : base(GameEventName.room_progress_changed ) {
            this.oldRoomProgress = oldRoomProgress;
            this.newRoomProgress = newRoomProgress;
            this.roomInfo = roomInfo;
        }
    }

    public class SearchModeChangedEventArgs : RavenhillEventArgs {
        public SearchMode oldSearchMode { get; }
        public SearchMode newSearchMode { get; }
        public RoomInfo roomInfo { get; }

        public SearchModeChangedEventArgs(SearchMode oldSearchMode, SearchMode newSearchMode, RoomInfo roomInfo)
            : base(GameEventName.search_mode_changed) {
            this.oldSearchMode = oldSearchMode;
            this.newSearchMode = newSearchMode;
            this.roomInfo = roomInfo;
        }
    }

    public class RoomLevelChangedEventArgs : RavenhillEventArgs {

        public RoomLevel oldRoomLevel { get; }
        public RoomLevel newRoomLevel { get; }
        public RoomInfo roomInfo { get; }

        public RoomLevelChangedEventArgs(RoomLevel oldRoomLevel, RoomLevel newRoomLevel, RoomInfo roomInfo )
            : base(GameEventName.room_level_changed) {
            this.oldRoomLevel = oldRoomLevel;
            this.newRoomLevel = newRoomLevel;
            this.roomInfo = roomInfo;
        }
    }

    public class SearchSessionEndedEventArgs : RavenhillEventArgs {
        public SearchSession session { get; }

        public SearchSessionEndedEventArgs(SearchSession session)
            : base(GameEventName.search_session_ended) {
            this.session = session;
        }
    }

    public class SearchSessionStartedEventArgs : RavenhillEventArgs {

        public SearchSession session { get; }

        public SearchSessionStartedEventArgs(SearchSession session)
            : base(GameEventName.search_session_started) {
            this.session = session;
        }
     }

    public class RoomModeChangedEventArgs : RavenhillEventArgs {
        public RoomMode oldRoomMode { get; private set; }
        public RoomMode newRoomMode { get; private set; }

        public RoomModeChangedEventArgs(RoomMode oldRoomMode, RoomMode newRoomMode)
            : base(GameEventName.room_mode_changed) {
            this.oldRoomMode = oldRoomMode;
            this.newRoomMode = newRoomMode;
        }
    }

    public class GameModeChangedEventArgs : RavenhillEventArgs {
        public GameModeName oldGameModeName { get; }
        public GameModeName newGameModeName { get; }

        public GameModeChangedEventArgs(GameModeName oldGameModeName, GameModeName newGameModeName)
            : base(GameEventName.game_mode_changed) {
            this.oldGameModeName = oldGameModeName;
            this.newGameModeName = newGameModeName;
        }
    }

    public class SearchTextStrokedEventArgs : RavenhillEventArgs {

        public SearchText searchText { get; }
        public SearchObjectData searchObjectData { get; }

        public SearchTextStrokedEventArgs(SearchText searchText, SearchObjectData searchObjectData)
            : base(GameEventName.search_text_stroked) {
            this.searchText = searchText;
            this.searchObjectData = searchObjectData;
        }
    }

    public class SearchObjectActivatedEventArgs : RavenhillEventArgs {

        public SearchObjectData searchObjectData { get; private set; }
        public BaseSearchableObject searchableObject { get; private set; }

        public SearchObjectActivatedEventArgs(SearchObjectData data, BaseSearchableObject searchableObject )
            : base(GameEventName.search_object_activated) {
            this.searchObjectData = data;
            this.searchableObject = searchableObject;
        }
    }

    public class SearchProgressChangedEventArgs : RavenhillEventArgs {
        public int foundedCount { get; private set; }
        public int totalCount { get; private set; }

        public SearchProgressChangedEventArgs(int foundedCount, int totalCount)
            : base(GameEventName.search_progress_changed) {
            this.foundedCount = foundedCount;
            this.totalCount = totalCount;
        }
    }

    public class SearchObjectCollectedEventArgs : RavenhillEventArgs {

        public SearchObjectData searchObjectData { get; private set; }
        public ISearchableObject targetSearchableObject { get; private set; }

        public SearchObjectCollectedEventArgs(SearchObjectData searchObjectData, ISearchableObject targetSearchableObject)
            : base(GameEventName.search_object_collected) {
            this.searchObjectData = searchObjectData;
            this.targetSearchableObject = targetSearchableObject;
        }
    }

    public class SearchStartedEventArgs : RavenhillEventArgs {
        public SearchStartedEventArgs() : base(GameEventName.search_started) { }
    }

    public class TouchEventArgs : RavenhillEventArgs {

        public Vector2 position { get; }

        public TouchEventArgs(Vector2 position) 
            : base(GameEventName.touch) {
            this.position = position;
        }
    }

    public class RavenhillViewAddedArgs : RavenhillEventArgs {

        public RavenhillViewType viewType { get; }

        public RavenhillViewAddedArgs(RavenhillViewType viewType)
            : base(GameEventName.view_added) {
            this.viewType = viewType;
        }
    }

    public class RavenhillViewRemovedEventArgs : RavenhillEventArgs {
        public RavenhillViewType viewType { get; }

        public RavenhillViewRemovedEventArgs(RavenhillViewType viewType)
            : base(GameEventName.view_removed) {
            this.viewType = viewType;
        }
    }

    public class RavenhillResourceLoadedEventArgs : RavenhillEventArgs {
        public RavenhillResourceLoadedEventArgs() : base(GameEventName.resource_loaded) { }
    }
}
