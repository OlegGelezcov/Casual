using Casual.Ravenhill.Data;
using Casual.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class StoreView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.store;

        public override bool isModal => true;

        public override int siblingIndex => 2;

        private InventoryTab currentTab = InventoryTab.Foods;

        public override void Setup(object data = null) {
            base.Setup(data);

            if (data != null) {
                currentTab = (InventoryTab)data;
            }

            Dictionary<InventoryTab, Toggle> toggleMap = new Dictionary<InventoryTab, Toggle> {
                [InventoryTab.Foods] = foodToggle,
                [InventoryTab.Bonuses] = bonusToggle,
                [InventoryTab.Ingredients] = ingredientToggle,
                [InventoryTab.Misc] = miscToggle,
                [InventoryTab.Tools] = toolToggle
            };

            foodToggle.SetListener((isOn) => {
                if (isOn) {
                    currentTab = InventoryTab.Foods;
                    listView.Clear();
                    listView.Setup(new ListView<Data.InventoryItemData>.ListViewData {
                        dataList = resourceService.foodList.Cast<InventoryItemData>()
                        .Where(obj => obj.IsSellable).ToList()
                    });
                }
            }, engine.GetService<IAudioService>());

            toolToggle.SetListener((isOn) => {
                if (isOn) {
                    currentTab = InventoryTab.Tools;
                    listView.Clear();
                    listView.Setup(new ListView<InventoryItemData>.ListViewData {
                        dataList = resourceService.toolList.Cast<InventoryItemData>()
                        .Where(obj => obj.IsSellable).ToList()
                    });
                }
            }, engine.GetService<IAudioService>());

            bonusToggle.SetListener((isOn) => {
                if (isOn) {
                    currentTab = InventoryTab.Bonuses;
                    listView.Clear();
                    listView.Setup(new ListView<InventoryItemData>.ListViewData {
                        dataList = resourceService.bonusList.Cast<InventoryItemData>()
                        .Where(obj => obj.IsSellable).ToList()
                    });
                }
            }, engine.GetService<IAudioService>());

            ingredientToggle.SetListener((isOn) => {
                if (isOn) {
                    currentTab = InventoryTab.Ingredients;
                    listView.Clear();
                    listView.Setup(new ListView<InventoryItemData>.ListViewData {
                        dataList = resourceService.ingredientList.Cast<InventoryItemData>()
                        .Where(obj => obj.IsSellable).ToList()
                    });
                }
            }, engine.GetService<IAudioService>());

            miscToggle.SetListener((isOn) => {
                if (isOn) {
                    currentTab = InventoryTab.Misc;
                    List<InventoryItemData> list = new List<InventoryItemData>();
                    list.AddRange(resourceService.weaponList.Cast<InventoryItemData>().Where(obj => obj.IsSellable));
                    list.AddRange(resourceService.chargerList.Cast<InventoryItemData>().Where(obj => obj.IsSellable));
                    list.AddRange(resourceService.storyChargerList.Cast<InventoryItemData>().Where(obj => obj.IsSellable));
                    listView.Clear();
                    listView.Setup(new ListView<InventoryItemData>.ListViewData {
                        dataList = list
                    });
                }
            }, engine.GetService<IAudioService>());

            toggleMap[currentTab].isOn = true;

            inventoryButton.SetListener(() => {
                engine.StartCoroutine(CorOpenInventory(currentTab));
            }, engine.GetService<IAudioService>());

            bankButton.SetListener(() => engine.StartCoroutine(CorOpenBank()), engine.GetService<IAudioService>());
        }

        private System.Collections.IEnumerator CorOpenBank() {
            Close();
            yield return new WaitForSeconds(0.3f);
            viewService.ShowView(RavenhillViewType.bank);
        }

        private System.Collections.IEnumerator CorOpenInventory(InventoryTab tab) {
            Close();
            yield return new WaitForSeconds(0.3f);
            viewService.ShowView(RavenhillViewType.inventory, tab);
        }
    }

    

    public partial class StoreView : RavenhillCloseableView {

        [SerializeField]
        private InventoryItemDataListView listView;

        [SerializeField]
        private Toggle foodToggle;

        [SerializeField]
        private Toggle toolToggle;

        [SerializeField]
        private Toggle bonusToggle;

        [SerializeField]
        private Toggle ingredientToggle;

        [SerializeField]
        private Toggle miscToggle;

        [SerializeField]
        private Button bankButton;

        [SerializeField]
        private Button inventoryButton;

    }
}
