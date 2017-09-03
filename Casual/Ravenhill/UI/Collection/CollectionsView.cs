using Casual.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill.UI {
    public partial class CollectionsView : RavenhillCloseableView {

        public override RavenhillViewType viewType => RavenhillViewType.collections;

        public override bool isModal => true;

        public override int siblingIndex => 2;

        public override void Setup(object data = null) {
            base.Setup(data);

            wishlistView.Setup();
            listView.Clear();
            listView.Setup(new ListView<Data.CollectionData>.ListViewData {
                dataList = resourceService.collectionList
            });
        }
    }

    public partial class CollectionsView : RavenhillCloseableView {

        [SerializeField]
        private CollectionListView listView;

        [SerializeField]
        private WishlistView wishlistView;

    }
}
