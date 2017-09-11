using System;
using System.Linq;

namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using Casual.UI;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class CollectableAddedNoteView : RavenhillBaseView {

        public override RavenhillViewType viewType => RavenhillViewType.collectable_added_note_view;

        public override bool isModal => false;

        public override int siblingIndex => 1;

        private CollectableData data;
        private CollectionData collectionData;

        public override void Setup(object objdata = null) {
            base.Setup(objdata);
            this.data = objdata as CollectableData;
            if(data == null ) {
                throw new ArgumentException("objdata");
            }

            collectionData = resourceService.GetCollection(data.collectionId);

            int index = 0;
            foreach(CollectableData collectableData in collectionData.collectableIds.Select(id => resourceService.GetCollectable(id))) {
                if(index < collectableViews.Length) {
                    collectableViews[index].Setup(collectableData);
                    index++;
                }
            }

            collectionView.Setup(collectionData);

            collectionNameText.text = resourceService.GetString(collectionData.nameId);

            StartCoroutine(CorEffect());
        }

        private System.Collections.IEnumerator CorEffect() {
            yield return new WaitForSeconds(.5f);

            if (data != null) {
                NoteCollectableView view = Find(data);
                if(view != null) {
                    view.gameObject.GetOrAdd<RectTransformAnimScale>().StartAnim(new MCFloatAnimData {
                        start = 1.0f,
                        end = 1.2f,
                        duration = 0.3f,
                        overwriteEasing = new OverwriteEasing { type = MCEaseType.EaseInOutCubic },
                        endAction = () => {
                            view.gameObject.GetOrAdd<RectTransformAnimScale>().StartAnim(new MCFloatAnimData {
                                start = 1.2f,
                                end = 1.0f,
                                duration = 0.3f,
                                overwriteEasing = new OverwriteEasing { type = MCEaseType.EaseInOutCubic },
                                endAction = () => viewService.RemoveView(RavenhillViewType.collectable_added_note_view, 0.5f)
                            });
                        }
                    });
                }
            }
        }

        private NoteCollectableView Find(CollectableData data) {
            return collectableViews.Where(view => view.Data.id == data.id).FirstOrDefault();
        }
    }

    public partial class CollectableAddedNoteView : RavenhillBaseView {

        [SerializeField]
        private NoteCollectableView[] collectableViews;

        [SerializeField]
        private NoteCollectionView collectionView;

        [SerializeField]
        private Text collectionNameText;
    }
}
