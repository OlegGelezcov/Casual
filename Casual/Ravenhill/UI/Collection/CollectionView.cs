using Casual.Ravenhill.Data;
using Casual.UI;
using UnityEngine;

namespace Casual.Ravenhill.UI {
    public partial class CollectionView : ListItemView<CollectionData> {


        public override void Setup(CollectionData data) {
            base.Setup(data);

            for(int i = 0; i < collectableViews.Length; i++ ) {
                if(i < data.collectableIds.Count ) {
                    CollectableData collectableData = resourceService.GetCollectable(data.collectableIds[i]);
                    if(collectableData != null ) {
                        collectableViews[i].Setup(collectableData);
                    }
                }
            }

            chargersView.Setup(data);

            collectionItemView.Setup(data);
        }
    }

    public partial class CollectionView : ListItemView<CollectionData> {

        [SerializeField]
        private CollectableItemView[] collectableViews;

        [SerializeField]
        private CollectionChargersView chargersView;

        [SerializeField]
        private CollectionItemView collectionItemView;
    }
}
