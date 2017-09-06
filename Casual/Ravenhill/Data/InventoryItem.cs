using UnityEngine;

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
            string curid = element.GetString("id", string.Empty);
            InventoryItemType type = element.GetEnum<InventoryItemType>("type");
            count = element.GetInt("count", 0);

            Debug.Log($"loaded item {curid}-{type}-{count}".Colored(ColorType.green));

            if(string.IsNullOrEmpty(curid)) {
                InitSave();
            } else {
                data = resourceService.GetInventoryItemData(type, curid);
                if(data == null ) {
                    Debug.Log($"item data is null {curid}-{type}".Colored(ColorType.fuchsia));
                    InitSave();
                } else {
                    Debug.Log($"item data not null {curid}-{type}".Colored(ColorType.grey));
                }
            }
        }

        public void InitSave() {
            data = null;
            count = 0;
        }
    }
}
