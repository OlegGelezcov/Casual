using Casual.Ravenhill.Data;
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
                        List<DropItem> dropItems = new List<DropItem>();
                        foreach(ToolData toolData in resourceService.toolList ) {
                            dropItems.Add(new DropItem(DropType.item, 1, toolData));
                        }
                        dropItems.Add(new DropItem(DropType.exp, 10));
                        dropItems.Add(new DropItem(DropType.gold, 10));
                        dropItems.Add(new DropItem(DropType.health, 10));
                        dropItems.Add(new DropItem(DropType.max_health, 10));
                        dropItems.Add(new DropItem(DropType.silver, 10));
                        engine.Cast<RavenhillEngine>().DropItems(dropItems);
                    }
                    return true;
            }
            return false;
        }
    }
}
