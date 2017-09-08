using Casual.Ravenhill.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Casual.Ravenhill.UI {
    public partial class RoomBuffsView : RavenhillUIBehaviour {

        private readonly List<RoomMapBuffView> views = new List<RoomMapBuffView>();
        private string roomId;

        public void Setup(string roomId) {
            this.roomId = roomId;
            //this.parent = parent;
            Clear();
            INpcService npcService = engine.GetService<INpcService>();
            foreach(BuffData data in npcService.GetBuffs(roomId)) {
                AddView(data);
            }
            //gameObject.GetOrAdd<RectTransformBinding>().Bind(parent, offset, 0.3f);
        }

        public void Clear() {
            views.ForEach(view => {
                if(view && view.gameObject) {
                    Destroy(view.gameObject);
                }
            });
            views.Clear();
        }

        private void AddView(BuffData data ) {
            GameObject instance = Instantiate<GameObject>(buffViewPrefab);
            RoomMapBuffView view = instance.GetComponent<RoomMapBuffView>();
            view.Setup(data);
            instance.transform.SetParent(layout, false);
            views.Add(view);
        }
    }

    public partial class RoomBuffsView : RavenhillUIBehaviour {

        [SerializeField]
        private Transform layout;

        [SerializeField]
        private GameObject buffViewPrefab;
    }
}
