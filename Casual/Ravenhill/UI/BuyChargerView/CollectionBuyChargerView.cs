using Casual.Ravenhill.Data;
using System;
using UnityEngine;

namespace Casual.Ravenhill.UI {
    public partial class CollectionBuyChargerView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.collection_buy_charger_view;

        public override bool isModal => true;

        public override int siblingIndex => 3;

        private CollectionData data;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);
            data = objdata as CollectionData;
            if(data == null ) {
                throw new ArgumentException(nameof(objdata));
            }

            for(int i = 0; i < data.chargers.Count; i++ ) {
                if(i < itemViews.Length ) {
                    itemViews[i].Setup(resourceService.GetCharger(data.chargers[i].id), data);
                }
            }
        }
    }

    public partial class CollectionBuyChargerView : RavenhillCloseableView {

        [SerializeField]
        private BuyChargerItemView[] itemViews;

    }

}
