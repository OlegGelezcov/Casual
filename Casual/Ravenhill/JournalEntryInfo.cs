using Casual.Ravenhill.Data;

namespace Casual.Ravenhill {
    public class JournalEntryInfo : RavenhillGameElement, ISaveElement {

        public string Id { get; private set; }

        public bool IsEndTextOpened { get; private set; }
        
        private JournalEntryData data = null;

        public JournalEntryInfo() { }

        public JournalEntryInfo(string id ) {
            this.Id = id;
            this.IsEndTextOpened = false;
            CacheData();
        }

        public JournalEntryType type {
            get {
                return Data?.type ?? JournalEntryType.none;
            }
        }

        public JournalEntryData Data {
            get {
                return CacheData();
            }
        }

        public bool IsValid {
            get {
                return (Data != null);
            }
        }

        private JournalEntryData CacheData() {
            if(data == null || data.id != Id ) {
                data = resourceService.GetJournalEntry(Id);
            }
            return data;
        }

        public bool OpenEndText() {
            bool oldValue = IsEndTextOpened;
            IsEndTextOpened = true;
            return (oldValue != IsEndTextOpened);
        }

        public bool CloseEndText() {
            bool oldValue = IsEndTextOpened;
            IsEndTextOpened = false;
            return (oldValue != IsEndTextOpened);
        }

        public UXMLWriteElement GetSave() {
            UXMLWriteElement element = new UXMLWriteElement("entry");
            element.AddAttribute("id", Id);
            element.AddAttribute("is_end_text_opened", IsEndTextOpened);
            return element;
        }

        public void InitSave() {
            CloseEndText();
        }

        public void Load(UXMLElement element) {
            Id = element.GetString("id");
            IsEndTextOpened = element.GetBool("is_end_text_opened");
        }
    }
}
