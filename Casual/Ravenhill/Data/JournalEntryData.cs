namespace Casual.Ravenhill.Data {
    public class JournalEntryData : IconData {

        public string startTextId { get; private set; }
        public string endTextId { get; private set; }
        public string hintTextId { get; private set; }
        public JournalEntryType type { get; private set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            startTextId = element.GetString("start_text");
            endTextId = element.GetString("end_text");
            hintTextId = element.GetString("hint_text");
            type = element.GetEnum<JournalEntryType>("type");
        }

        public bool hasEndText {
            get {
                return endTextId.IsValid();
            }
        }

        public bool hasHintText {
            get {
                return hintTextId.IsValid();
            }
        }
    }

    public enum JournalEntryType : byte {
        none = 0,
        story = 1,
        side = 2,
        
    }
}
