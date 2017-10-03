using Casual.Ravenhill.Data;
using System.Collections.Generic;
using System.Linq;

namespace Casual.Ravenhill {
    public class Wishlist : RavenhillGameElement, IWishlist, ISaveElement {

        private const int kMaxSize = 5;

        private Dictionary<string, InventoryItemData> collectables { get; } = new Dictionary<string, InventoryItemData>();

        #region IWishlist
        public bool Add(InventoryItemData data) {
            if(!collectables.ContainsKey(data.id)) {
                if(collectables.Count < kMaxSize ) {
                    collectables.Add(data.id, data);
                    RavenhillEvents.OnAddedToWishlist(data);
                    return true;
                }
            }
            return false;
        }
        public bool IsContains(InventoryItemData data) {
            return collectables.ContainsKey(data.id);
        }

        public bool Remove(InventoryItemData data) {
            bool success = collectables.Remove(data.id);
            if(success ) {
                RavenhillEvents.OnRemovedFromWishlist(data);
            }
            return success;
        }

        public bool IsFull => collectables.Count >= kMaxSize;

        public int Count => collectables.Count;
        #endregion

        public Dictionary<string, string> JsonCompatibale {
            get {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach(var c in collectables) {
                    dict.Add(c.Key, c.Value.type.ToString());
                }
                return dict;
            }
        }

        #region ISaveElement
        public UXMLWriteElement GetSave() {
            UXMLWriteElement root = new UXMLWriteElement("wishlist");
            foreach(var pair in collectables ) {
                UXMLWriteElement itemElement = new UXMLWriteElement("item");
                itemElement.AddAttribute("id", pair.Key);
                itemElement.AddAttribute("type", pair.Value.type.ToString());
                root.Add(itemElement);
            }
            return root;
        }

        public void InitSave() {
            collectables.Clear();
        }

        public void Load(UXMLElement element) {
            collectables.Clear();
            foreach(UXMLElement itemElement in element.Elements("item")) {
                string id = itemElement.GetString("id");
                InventoryItemType type = itemElement.GetEnum<InventoryItemType>("type");

                InventoryItemData data = resourceService.GetInventoryItemData(type, id);

                if(data != null ) {
                    collectables[data.id] = data;
                }
            }
        } 
        #endregion

        public List<InventoryItemData> itemList {
            get {
                List<InventoryItemData> list = new List<InventoryItemData>(collectables.Values);
                return list.OrderBy(c => c.id).ToList();
            }
        }
    }
}
