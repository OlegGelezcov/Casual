using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class BaseToolView : RavenhillUIBehaviour {

        private SearchManager searchManager = null;

        public virtual void Setup() {
            ToolData toolData = resourceService.GetTool(toolId);
            button.SetListener(() => {
                if(playerService.GetItemCount(toolData) > 0 ) {
                    if(IsValid(toolData)) {
                        playerService.UseItem(toolData, OnUse);
                    }
                } else {
                    viewService.ShowView(RavenhillViewType.buy_item_view, toolData);
                }
            }, engine.GetService<IAudioService>());
        }

        protected virtual void OnUse(InventoryItemData data ) {

        }

        protected virtual bool IsValid(ToolData data) {
            return true;
        }

        protected SearchManager SearchManager {
            get {
                if(searchManager == null ) {
                    searchManager = FindObjectOfType<SearchManager>();
                }
                return searchManager;
            }
        }

    }

    public partial class BaseToolView : RavenhillUIBehaviour {

        [SerializeField]
        protected string toolId;

        [SerializeField]
        protected Button button;

    }
}
