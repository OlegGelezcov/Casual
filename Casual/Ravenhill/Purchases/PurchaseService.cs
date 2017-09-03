using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casual.Ravenhill.Data;
using UnityEngine;

namespace Casual.Ravenhill {
    public class PurchaseService : RavenhillGameBehaviour, IPurchaseService {

        public void OnPurchaseEnded(BankProductData product) {
            List<DropItem> dropItems = new List<DropItem>();

            if(!PriceData.IsNone(product.price)) {
                var extendedPrice = product.price.Extend(product.bonus);
                dropItems.Add(extendedPrice.ToDropItem());
            }

            if(product.rewards.Count > 0 ) {
                dropItems.AddRange(product.rewards);
            }

            engine.Cast<RavenhillEngine>().DropItems(dropItems);
        }

        public void PurchaseProduct(BankProductData product) {
           if(Application.isEditor) {
                new EditorStoreProvider().StartPurchase(this, product);
            }
        }

        public void Setup(object data) {
            
        }
    }


}
