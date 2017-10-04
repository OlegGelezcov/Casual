using Casual.Ravenhill;
using Casual.Ravenhill.Data;
using Casual.Ravenhill.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.UI {

   
    public class ListView<T> : GameBehaviour where T : IIdObject {

#pragma warning disable 0649
        [SerializeField]
        private Transform m_Layout;

        [SerializeField]
        private GameObject m_ItemPrefab;
#pragma warning restore 0649

        private Transform layout => m_Layout;

        private GameObject itemPrefab => m_ItemPrefab;


        private List<ListItemView<T>> views { get; } = new List<ListItemView<T>>();
        private List<ObjectChange> changes { get; } = new List<ObjectChange>();

        public class ListViewData {
            public List<T> dataList { get; set; }
        }

        private class ObjectChange {
            public T data { get; set; }
            public ListOpType op { get; set; }
        }

        public virtual void Setup(ListViewData data) {

            CompleteActions(CollectChanges(data));
        }

        private List<ObjectChange> CollectChanges(ListViewData data) {
            changes.Clear();

            foreach (T itemData in data.dataList) {
                if (HasItem(itemData)) {
                    changes.Add(new ObjectChange { data = itemData, op = ListOpType.Update });
                } else {
                    changes.Add(new ObjectChange { data = itemData, op = ListOpType.Add });
                }
            }

            foreach (ListItemView<T> view in views) {
                if (!HasData(data.dataList, view.data)) {
                    changes.Add(new ObjectChange { data = view.data, op = ListOpType.Remove });
                }
            }
            return changes;
        }

        private void CompleteActions(List<ObjectChange> changes) {
            changes.ForEach(change => {
                switch(change.op) {
                    case ListOpType.Add: {
                            AddItem(change.data);
                        }
                        break;
                    case ListOpType.Remove: {
                            RemoveItem(change.data);
                        }
                        break;
                    case ListOpType.Update: {
                            UpdateItem(change.data);
                        }
                        break;
                }
            });
            changes.Clear();
        }

        private void AddItem(T data) {
            GameObject go = Instantiate(itemPrefab);
            ListItemView<T> itemView = go.GetComponentInChildren<ListItemView<T>>();
            go.transform.SetParent(layout, false);
            itemView.Setup(data);
            views.Add(itemView);
        }

        private void RemoveItem(T data) {
            ListItemView<T> view = views.Find(itemView => itemView.data.id == data.id);
            if(view) {
                views.Remove(view);
                Destroy(view.gameObject);
            }
        }

        private void UpdateItem(T data) {
            ListItemView<T> view = views.Find(itemView => itemView.data.id == data.id);
            if(view) {
                view.Setup(data);
            }
        }

        private bool HasItem(T data) {
            ListItemView<T> view = views.Find(itemView => itemView.data.id == data.id);
            if(view) {
                return true;
            }
            return false;
        }

        private bool HasData(List<T> source, T data) {
            return (source.Find(it => it.id == data.id) != null);
        }

        public void Clear() {
            foreach(var view in views ) {
                if(view) {
                    Destroy(view.gameObject);
                }
            }
            views.Clear();
            changes.Clear();
        }
    }

    public abstract class ListItemView<T> : RavenhillGameBehaviour where T : IIdObject {

        public virtual void Setup(T data) {
            this.data = data;
        }

        public T data { get; private set; }

    }

    public enum ListOpType : byte {
        Add = 0,
        Remove = 1,
        Update = 2
    }

    public class InventoryItemListView : ListView<InventoryItem> {

        public override void Setup(ListViewData data) {
            Debug.Log($"inventory list count {data.dataList.Count}");
            base.Setup(data);
        }

    }

    public class InventoryItemDataListView : ListView<InventoryItemData> {

        public override void Setup(ListViewData data) {
            base.Setup(data);
        }
    }

    public class CollectionListView : ListView<CollectionData> {
        public override void Setup(ListViewData data) {
            base.Setup(data);
        }
    }

    public class QuestListView : ListView<QuestData> {
        public override void Setup(ListViewData data) {
            base.Setup(data);
        }
    }

    public class ScreenQuestListView : ListView<QuestInfo> {
        public override void Setup(ListViewData data) {
            base.Setup(data);
        }
    }

    public class AchievmentListView : ListView<AchievmentData> {
        public override void Setup(ListViewData data) {
            base.Setup(data);
            Debug.Log("Setup Achievment List View...");
        }
    }

    public class BuffListView : ListView<BuffInfo> {
        public override void Setup(ListViewData data) {
            base.Setup(data);
            Debug.Log("Setup Buff List");
        }
    }

    public class SocialGiftListView : ListView<NetGift> {
        public override void Setup(ListViewData data) {
            base.Setup(data);
        }
    }

    public class SocialFriendListView : ListView<NetPlayer> {

    }
}
