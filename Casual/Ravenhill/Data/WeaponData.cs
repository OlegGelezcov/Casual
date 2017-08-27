using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public class WeaponData : InventoryItemData {

        public override PriceData price { get; protected set; }
        public float prob { get; private set; }
        public string invitationId { get; private set; }

        public override void Load(UXMLElement element) {
            base.Load(element);
            price = new PriceData(element);
            prob = element.GetFloat("prob");
            invitationId = element.GetString("invitation");
        }

        public override InventoryItemType type => InventoryItemType.Weapon;
        public override bool isUsableFromInventory => false;
        public override bool IsSellable => true;
    }
}
