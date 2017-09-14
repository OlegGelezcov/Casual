using Casual.Ravenhill.Data;
using Casual.Ravenhill.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill {
    public class DebugService : RavenhillGameBehaviour, IDebugService {

#pragma warning disable 0649
        [SerializeField]
        private GUISkin m_Skin;

        [SerializeField]
        private float m_LineHeight = 30.0f;

        [SerializeField]
        private float m_LineSpace = 5.0f;

        [SerializeField]
        private int m_MaxMessages = 100;
#pragma warning restore 0649

        private GUIStyle m_ConsoleStyle;
        private GUIStyle m_TextfieldStyle;

        private string m_Input = string.Empty;
        private Vector2 m_ScrollPosition = Vector2.zero;
        private readonly List<LogMessage> m_Messages = new List<LogMessage>();

        public bool isOpened { get; private set; }

        public class LogMessage {
            public string text;
            public string color = "white";

            public string coloredText {
                get {
                    return string.Format("<color={0}>{1}</color>", color, text);
                }
            }
        }

        public override void Start() {
            m_ConsoleStyle = m_Skin.GetStyle("console");
            m_TextfieldStyle = m_Skin.GetStyle("textfield");

            //engine.GetService<IDebugCommandService>().Add("createmapcharacter", new CreateMapCharacterCommand("createmapcharacter"));
            Add("exitroomview", new ExitRoomViewCommand("exitroomview"));
            Add("test", new TestCommand("test"));
            Add("journal", new JournalCommand("journal"));
            Add("show", new ShowCommand("show"));
            Add("clear", new ClearCommand("clear"));
            Add("add", new AddCommand("add"));
        }


        void OnGUI() {

            if (isOpened) {
                float xMin = 0;
                float yMin = Screen.height * (1.0f - 1.0f / 3.0f);
                float xWidth = Screen.width;
                float yHeight = (1.0f / 3.0f) * Screen.height;
                GUI.BeginGroup(new Rect(xMin, yMin, xWidth, yHeight));
                GUI.Box(new Rect(0, 0, xWidth, yHeight), string.Empty);

                Rect viewRect = new Rect(0, 0, xWidth, (m_LineHeight + m_LineSpace) * m_Messages.Count);
                m_ScrollPosition = GUI.BeginScrollView(new Rect(0, 0, xWidth, yHeight - 30), m_ScrollPosition, viewRect);
                float currentY = 0.0f;
                for (int i = 0; i < m_Messages.Count; i++) {
                    GUI.Label(new Rect(0, currentY, 0, 0), m_Messages[i].coloredText, m_ConsoleStyle);
                    currentY += (m_LineHeight + m_LineSpace);
                }
                m_ScrollPosition.y = float.MaxValue;

                GUI.EndScrollView();
                m_Input = GUI.TextField(new Rect(0, yHeight - m_LineHeight, xWidth, m_LineHeight), m_Input, m_TextfieldStyle);
                GUI.EndGroup();

                if (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return) {
                    if (Event.current.type == EventType.KeyUp) {
                        ParseCommand(m_Input);
                    }
                }
            }

        }

        public override void Update() {
            base.Update();
            if(Input.GetKeyUp(KeyCode.BackQuote)) {
                isOpened = !isOpened;
            }
        }

        private void ParseCommand(string rawCommand) {
            if(!Execute(rawCommand)) {
                AddMessage($"invalid command: {rawCommand}".Colored(ColorType.red));
            }
        }

        private void LogCommand(string command) {
            AddMessage(string.Format("execute command: {0}", command), "orange");
        }

        public void AddMessage(string message, ColorType color) {
            AddMessage(message, Extensions.ColorType2HexString(color));
        }
        public void AddMessage(string message, string color = "white") {
            if (!string.IsNullOrEmpty(message)) {
                m_Messages.Add(new LogMessage { text = message, color = color });
                if (m_Messages.Count > m_MaxMessages) {
                    int deleteCount = m_Messages.Count - m_MaxMessages;
                    for (int i = 0; i < deleteCount; i++) {
                        m_Messages.RemoveAt(0);
                    }
                }
            }
        }

        private Dictionary<string, BaseCommand> commands { get; } = new Dictionary<string, BaseCommand>();

        public void Add(string name, BaseCommand command) {
            commands[name] = command;
        }

        public bool Execute(string command) {
            string name = GetName(command);
            if (commands.ContainsKey(name)) {
                return commands[name].Execute(command);
            }
            return false;
        }

        public void Setup(object data) {

        }

        private string GetName(string command) {
            string[] tokens = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length > 0) {
                return tokens[0].ToLower();
            }
            return string.Empty;
        }


    }

    public class ExitRoomViewCommand : BaseCommand {

        public ExitRoomViewCommand(string commandName)
            : base(commandName) { }

        public override bool Execute(string source) {
            bool success = GetBool(source, 1);
            SearchSession session = null;

            session = new SearchSession();
            
            session.SetRoomInfo(new RoomInfo("r4"));

            var bonuses = resourceService.bonusList.Select(b => new Data.InventoryItem(b, 1)).ToList();
            engine.GetService<IDebugService>().AddMessage($"bonus count {bonuses.Count}", ColorType.yellow);
            session.OnEndSession(success ? SearchStatus.success : SearchStatus.fail, 60, bonuses);
            viewService.ShowView(RavenhillViewType.exit_room_view, session);
            return true;
        }
    }

    public class TestCommand : BaseCommand {

        public TestCommand(string commandName) 
            : base(commandName) { }

        public override bool Execute(string source) {
            string testType = GetToken(source, 1);
            switch(testType) {
                case "drop": {
                        TestDrop();
                    }
                    return true;
                case "storycharge": {
                        TestStoryCharge();
                    }
                    return true;
                    
            }
            return false;
        }

        private void TestDrop() {
            List<DropItem> dropItems = new List<DropItem>();
            foreach (ToolData toolData in resourceService.toolList) {
                dropItems.Add(new DropItem(DropType.item, 1, toolData));
            }
            dropItems.Add(new DropItem(DropType.exp, 10));
            dropItems.Add(new DropItem(DropType.gold, 10));
            dropItems.Add(new DropItem(DropType.health, 10));
            dropItems.Add(new DropItem(DropType.max_health, 10));
            dropItems.Add(new DropItem(DropType.silver, 10));
            engine.Cast<RavenhillEngine>().DropItems(dropItems);
        }

        private void TestStoryCharge() {
            PlayerService player = engine.GetService<IPlayerService>().Cast<PlayerService>();
            player.RemoveItems(InventoryItemType.StoryCollection);
            player.RemoveItems(InventoryItemType.StoryCollectable);
            player.RemoveItems(InventoryItemType.StoryCharger);

            foreach(StoryCollectableData data in resourceService.storyCollectableList) {
                player.AddItem(new InventoryItem(data, 1));
            }
            resourceService.storyChargerList.ForEach(c => {
                player.AddItem(new InventoryItem(c, 1));
            });
            engine.GetService<IDebugService>().AddMessage("story test prepared", ColorType.brown);
        }
    }

    public class JournalCommand : BaseCommand {
        public JournalCommand(string commandName)
            : base(commandName) { }

        public override bool Execute(string source) {
            string actionName = GetToken(source, 1).ToLower();

            switch(actionName) {
                case "fill": {
                        foreach(var entryData in resourceService.journalList ) {
                            engine.GetService<IJournalService>().AddEntry(new JournalEntryInfo(entryData.id));
                        }
                        return true;
                    }
                case "end": {
                        foreach(var entryData in resourceService.journalList) {
                            engine.GetService<IJournalService>().OpenEndText(entryData.id);
                        }
                        engine.GetService<IDebugService>().AddMessage("all entries opens end text", ColorType.brown);
                        return true;
                    }
            }
            return false;
        }
    }

    public class ShowCommand : BaseCommand {
        public ShowCommand(string commandName)
            : base(commandName) { }

        public override bool Execute(string source) {
            string viewName = GetToken(source, 1).ToLower();
            switch(viewName) {
                case "quest_dialog_view": {
                        ShowQuestDialogView();
                    }
                    break;
                case "quest_start_view": {
                        ShowQuestStartView();
                    }
                    break;
                case "quest_end_view": {
                        ShowQuestEndView();
                    }
                    break;
                case "video": {
                        ShowVideoView();
                    }
                    break;
                case "achievment_rank_view": {
                        ShowAchievmentRankView();
                    }
                    break;
                case "msgbox": {
                        int btnCount = GetInt(source, 2);
                        ShowMsgBox(btnCount);
                    }
                    break;
                case "alchemy": {
                        ShowAlchemy();
                    }
                    break;
                case "hint": {
                        string offsetStr = GetToken(source, 2).ToLower();
                        Dictionary<string, HintView.OffsetType> offsets = new Dictionary<string, HintView.OffsetType> {
                            ["up"] = HintView.OffsetType.Up,
                            ["down"] = HintView.OffsetType.Down,
                            ["left"] = HintView.OffsetType.Left,
                            ["right"] = HintView.OffsetType.Right
                        };
                        HintView.OffsetType offset = offsets.GetOrDefault(offsetStr, HintView.OffsetType.Up);
                        ShowHint(offset);
                    }
                    break;
                case "daily": {
                        int day = GetInt(source, 2);
                        ShowDailyRewardView(day);
                    }
                    break;
            }
            return true;
        }

        private void ShowDailyRewardView(int day) {
            viewService.ShowView(RavenhillViewType.daily_reward_view, day);
        }

        private void ShowHint(HintView.OffsetType offset ) {
            HintView.TextData textData = new HintView.TextData {
                title = "Test hint title",
                text = "Lorem ipsum dolores ....",
                offsetType = offset,
                screenPosition = new Vector2(Screen.width / 2, Screen.height / 2)
            };
            viewService.ShowView(RavenhillViewType.hint_view, textData);
        }

        private void ShowAlchemy() {
            viewService.ShowView(RavenhillViewType.alchemy_view);
            engine.GetService<IDebugService>().AddMessage("Show alchemy view", ColorType.black);
        }

        private void ShowVideoView() {
            VideoView.Data data = new VideoView.Data {
                id = resourceService.videoList.RandomElement().id,
                completeAction = () => {
                    Debug.Log($"test video");
                }
            };
            viewService.ShowView(RavenhillViewType.video_view, data);
        }

        private void ShowQuestDialogView() {
            QuestData quest = resourceService.questList.RandomElement();
            bool isStart = false;
            if(UnityEngine.Random.Range(0, 10) % 2 == 0 ) {
                isStart = true;
            }

            QuestDialogView.Data initData = new QuestDialogView.Data { quest = quest, isStart = isStart };
            viewService.ShowView(RavenhillViewType.quest_dialog_view, initData);
        }

        private void ShowQuestStartView() {
            QuestData quest = resourceService.questList.RandomElement();
            viewService.ShowViewWithDelay(RavenhillViewType.quest_start_view, 1.0f, quest);
        }

        private void ShowQuestEndView( ) {
            viewService.ShowViewWithDelay(RavenhillViewType.quest_end_view, 0.5f, resourceService.questList.RandomElement());
        }

        private void ShowAchievmentRankView() {
            AchievmentRankView.Data data = new AchievmentRankView.Data {
                info = engine.GetService<IAchievmentService>().GetAchievment(resourceService.GetAchievment("A0001")),
                tier = resourceService.GetAchievment("A0001").tiers[0]
            };
            viewService.ShowView(RavenhillViewType.achievment_rank_view, data);
            engine.GetService<IDebugService>().AddMessage("Show achievment rank view", ColorType.aqua);
        }

        private void ShowMsgBox(int btnCount = 1) {
            if(btnCount == 1) {
                MessageBoxView.Data data = new MessageBoxView.Data {
                    content = "This is test content for debug",
                    textPanColor = ControlColor.red,
                    buttonConfigs = new ButtonConfig[] {
                            new ButtonConfig {
                                 buttonName = "First",
                                  color = ControlColor.green,
                                   action = () => Debug.Log("first action")
                            }
                       }
                };
                viewService.ShowView(RavenhillViewType.message_box_view, data);
            } else if(btnCount == 2 ) {
                MessageBoxView.Data data = new MessageBoxView.Data {
                    content = "This is test content for debug",
                    textPanColor = ControlColor.red,
                    buttonConfigs = new ButtonConfig[] {
                            new ButtonConfig {
                                 buttonName = "First",
                                  color = ControlColor.green,
                                   action = () => Debug.Log("first action")
                            },
                            new ButtonConfig {
                                buttonName = "Second",
                                color = ControlColor.yellow,
                                action = () => Debug.Log("Second action")
                            }
                       }
                };
                viewService.ShowView(RavenhillViewType.message_box_view, data);
            }
        }
    }

    public class ClearCommand : BaseCommand {
        public ClearCommand(string commandName)
            : base(commandName) { }

        public override bool Execute(string source) {
            string whatClear = GetToken(source, 1);
            switch(whatClear) {
                case "journal": {
                        engine.GetService<IJournalService>().Cast<JournalService>().InitSave();
                        engine.GetService<IDebugService>().AddMessage("journal cleared....");
                    }
                    break;
                case "quests": {
                        engine.GetService<IQuestService>().Cast<QuestService>().InitSave();
                        engine.GetService<IDebugService>().AddMessage("quests cleared....");
                    }
                    break;
            }
            return true;
        }
    }

    public class AddCommand : BaseCommand {
        public AddCommand(string commandName)
            : base(commandName) {

        }

        public override bool Execute(string source) {
            var player = engine.GetService<IPlayerService>().Cast<PlayerService>();
            string type = GetToken(source, 1);
            switch(type) {
                case "bonus": {
                        resourceService.bonusList.ForEach((b) => {
                            player.AddItem(new InventoryItem(b, 1));
                        });
                    }
                    break;
                case "tool": {
                        resourceService.toolList.ForEach(t => {
                            player.AddItem(new InventoryItem(t, 1));
                        });
                    }
                    break;
                case "food": {
                        resourceService.foodList.ForEach(f => {
                            player.AddItem(new InventoryItem(f, 1));
                        });
                    }
                    break;
                case "collectable": {
                        resourceService.collectableList.ForEach(c => player.AddItem(new InventoryItem(c, 1)));
                    }
                    break;
            }
            return true;
        }
    }
}
