using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public class Inventory : RavenhillGameElement, ISaveElement {

        private readonly Dictionary<InventoryItemType, Dictionary<string, InventoryItem>> items = new Dictionary<InventoryItemType, Dictionary<string, InventoryItem>>();

        public void AddItem(InventoryItem item) {
            if(items.ContainsKey(item.data.type)) {
                Dictionary<string, InventoryItem> filtered = items[item.data.type];
                if(filtered.ContainsKey(item.id)) {
                    filtered[item.id].AddCount(item.count);
                } else {
                    filtered.Add(item.id, item);
                }
            } else {
                items.Add(item.data.type, new Dictionary<string, InventoryItem> {
                    [item.id] = item
                });
            }
            RavenhillEvents.OnInventoryItemAdded(item.data.type, item.id, item.count);
            RavenhillEvents.OnInventoryChanged(item.data.type, item.id, item.count);
        }

        public bool RemoveItem(InventoryItemType type, string id, int count) {
            if (items.ContainsKey(type)) {
                Dictionary<string, InventoryItem> filtered = items[type];
                if (filtered.ContainsKey(id)) {
                    InventoryItem targetItem = filtered[id];
                    if (targetItem.count >= count) {
                        targetItem.RemoveCount(count);
                        if (targetItem.count <= 0) {
                            filtered.Remove(targetItem.id);
                        }
                        RavenhillEvents.OnInventoryItemRemoved(type, id, count);
                        RavenhillEvents.OnInventoryChanged(type, id, count);
                        return true;
                    }
                }
            }
            return false;
        }

        public void RemoveItems(InventoryItemType type) {
            if(items.ContainsKey(type)) {
                Dictionary<string, InventoryItem> toDelete = new Dictionary<string, InventoryItem>();
                foreach(var kvp in items[type]) {
                    toDelete.Add(kvp.Key, kvp.Value);
                }
                foreach(var kvp in toDelete) {
                    RemoveItem(kvp.Value.data.type, kvp.Key, kvp.Value.count);
                }
            }
        }

        public int ItemCount(InventoryItemType type, string id ) {
            Dictionary<string, InventoryItem> filtered = null;
            if(items.TryGetValue(type, out filtered)) {
                InventoryItem item = null;
                if(filtered.TryGetValue(id, out item)) {
                    return item.count;
                }
            }
            return 0;
        }

        public UXMLWriteElement GetSave() {
            UXMLWriteElement root = new UXMLWriteElement("inventory");
            foreach(InventoryItem item in ItemList ) {
                root.Add(item.GetSave());
            }
            return root;
        }

        public void InitSave() {
            items.Clear();
        }

        public void Load(UXMLElement element) {
            items.Clear();
            foreach(UXMLElement itemElement in element.Elements("item")) {
                InventoryItem item = new InventoryItem();
                item.Load(itemElement);
                if(item.count > 0 && item.data != null) {
                    if(items.ContainsKey(item.data.type)) {
                        items[item.data.type][item.id] = item;
                    } else {
                        items.Add(item.data.type, new Dictionary<string, InventoryItem> {
                            [item.id] = item
                        });
                    }
                }
            }
        }

        public List<InventoryItem> ItemList {
            get {
                List<InventoryItem> list = new List<InventoryItem>();
                foreach(var filtered in items ) {
                    foreach(var itemPair in filtered.Value ) {
                        if(itemPair.Value.count > 0 ) {
                            list.Add(itemPair.Value);
                        }
                    }
                }
                return list;
            }
        }




    }
}
