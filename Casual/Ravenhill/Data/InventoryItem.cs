namespace Casual.Ravenhill.Data {
    public class InventoryItem : RavenhillGameElement, IIdObject, ISaveElement  {
        public InventoryItemData data { get; private set; }
        public int count { get; private set; }

        public string id {
            get {
                return data?.id ?? string.Empty;
            }
        }

        public InventoryItem() { }

        public InventoryItem(InventoryItemData data, int count) {
            this.data = data;
            this.count = count;
        }

        public InventoryItemType type {
            get {
                return data.type;
            }
        }

        public void AddCount(int cnt) {
            count += cnt;
        }

        public void RemoveCount(int cnt) {
            count -= cnt;
        }

        public UXMLWriteElement GetSave() {
            UXMLWriteElement writeElement = new UXMLWriteElement("item");
            writeElement.AddAttribute("id", id);
            writeElement.AddAttribute("type", data.type.ToString());
            writeElement.AddAttribute("count", count);
            return writeElement;
        }

        public void Load(UXMLElement element) {
            string curId = element.GetString("id", string.Empty);
            InventoryItemType type = element.GetEnum<InventoryItemType>("type");
            int count = element.GetInt("count", 0);
            if(string.IsNullOrEmpty(id)) {
                InitSave();
            } else {
                data = resourceService.GetInventoryItemData(type, id);
                if(data == null ) {
                    InitSave();
                }
            }
        }

        public void InitSave() {
            data = null;
            count = 0;
        }
    }
}
