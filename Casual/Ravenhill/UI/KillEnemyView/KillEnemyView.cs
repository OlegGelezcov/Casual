using Casual.Ravenhill.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class KillEnemyView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.kill_enemy_view;

        public override bool isModal => true;

        public override int siblingIndex => 1;

        private NpcInfo info;

        public override void Setup(object data = null) {
            base.Setup(data);
            info = data as NpcInfo;
            if(info != null ) {
                nameText.text = resourceService.GetString(info.Data.nameId);
                descriptionText.text = resourceService.GetString(info.Data.descriptionId);
                iconImage.overrideSprite = resourceService.GetSprite(info.Data.largeIconKey, info.Data.largeIconPath);

                WeaponData weapon = resourceService.GetWeapon(info.Data.weaponId);
                weaponIconImage.overrideSprite = resourceService.GetSprite(weapon);
                weaponNameText.text = resourceService.GetString(weapon.nameId);

                int playerCount = playerService.GetItemCount(weapon);
                if(playerCount > 0 ) {
                    priceParent.gameObject.DeactivateSelf();
                    buttonText.text = resourceService.GetString("Loc_KillEnPanel_killButton");
                    buyOrKillButton.SetListener(() => {
                        playerService.RemoveItem(weapon.type, weapon.id, 1);
                        engine.Cast<RavenhillEngine>().DropItems(info.Data.rewards, null, () => !viewService.hasModals);
                        engine.GetService<INpcService>().Cast<NpcService>().RemoveNpc(info.RoomId);
                        Close();
                    });
                    weaponIconImage.SetAlpha(1);

                } else {
                    priceParent.gameObject.ActivateSelf();
                    priceText.text = weapon.price.price.ToString();
                    priceImage.overrideSprite = resourceService.GetPriceSprite(weapon.price);
                    buttonText.text = resourceService.GetString("Loc_KillEnPanel_buyButton");
                    buyOrKillButton.SetListener(() => {
                        if(playerService.HasCoins(weapon.price)) {
                            playerService.RemoveCoins(weapon.price);
                            playerService.AddItem(new InventoryItem(weapon, 1));
                            Setup(info);
                        } else {
                            viewService.ShowView(RavenhillViewType.bank);
                        }
                    });
                    weaponIconImage.SetAlpha(0.5f);
                }
            }
            closeBigButton.SetListener(Close);
        }
    }

    public partial class KillEnemyView : RavenhillCloseableView {

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private Image weaponIconImage;

        [SerializeField]
        private Text weaponNameText;

        [SerializeField]
        private Transform priceParent;

        [SerializeField]
        private Text priceText;

        [SerializeField]
        private Image priceImage;

        [SerializeField]
        private Button buyOrKillButton;

        [SerializeField]
        private Text buttonText;

        [SerializeField]
        private Button closeBigButton;
    }
}
