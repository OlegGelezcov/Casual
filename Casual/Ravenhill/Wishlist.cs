using Casual.Ravenhill.Data;
using System.Collections.Generic;
using System.Linq;

namespace Casual.Ravenhill {
    public class Wishlist : RavenhillGameElement, IWishlist, ISaveElement {

        private const int kMaxSize = 5;

        private Dictionary<string, CollectableData> collectables { get; } = new Dictionary<string, CollectableData>();

        #region IWishlist
        public bool Add(CollectableData data) {
            if(!collectables.ContainsKey(data.id)) {
                if(collectables.Count < kMaxSize ) {
                    collectables.Add(data.id, data);
                    RavenhillEvents.OnAddedToWishlist(data);
                    return true;
                }
            }
            return false;
        }
        public bool IsContains(CollectableData data) {
            return collectables.ContainsKey(data.id);
        }

        public bool Remove(CollectableData data) {
            bool success = collectables.Remove(data.id);
            if(success ) {
                RavenhillEvents.OnRemovedFromWishlist(data);
            }
            return success;
        }

        public bool IsFull => collectables.Count >= kMaxSize;

        public int Count => collectables.Count;
        #endregion

        #region ISaveElement
        public UXMLWriteElement GetSave() {
            UXMLWriteElement root = new UXMLWriteElement("wishlist");
            foreach(var pair in collectables ) {
                UXMLWriteElement itemElement = new UXMLWriteElement("item");
                itemElement.AddAttribute("id", pair.Key);
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
                CollectableData data = resourceService.GetCollectable(id);
                if(data != null ) {
                    collectables[data.id] = data;
                }
            }
        } 
        #endregion

        public List<CollectableData> collectableList {
            get {
                List<CollectableData> list = new List<CollectableData>(collectables.Values);
                return list.OrderBy(c => c.id).ToList();
            }
        }
    }
}
