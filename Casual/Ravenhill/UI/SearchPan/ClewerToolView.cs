using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.UI{
    using Casual.Ravenhill.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class ClewerToolView : BaseToolView {

        public override void Setup() {
            base.Setup();
            ToolData toolData = resourceService.GetTool(toolId);
            button.SetListener(() => {
                if (playerService.GetItemCount(toolData) > 0) {
                    if (IsValid(toolData)) {
                        //playerService.UseItem(toolData, OnUse);
                        MessageBoxView.Data msgBoxData = new MessageBoxView.Data {
                            content = "[Do you want to complete the room instantly?]",
                            textPanColor = ControlColor.white,
                            buttonConfigs = new ButtonConfig[] {
                                  new ButtonConfig {
                                       buttonName = "[Yes]",
                                        color = ControlColor.green,
                                        action = () => {
                                            playerService.UseItem(toolData, OnUse);
                                        }
                                  },
                                  new ButtonConfig {
                                      buttonName = "[No]",
                                      color = ControlColor.red,
                                      action = ()=>{ }
                                  }
                              }
                        };
                        viewService.ShowView(RavenhillViewType.message_box_view, msgBoxData);
                    }
                } else {
                    viewService.ShowView(RavenhillViewType.buy_item_view, toolData);
                }
            });
        }
        protected override void OnUse(InventoryItemData data) {
            base.OnUse(data);
            SearchManager.WinRoomInstantly();
        }
    }
}
