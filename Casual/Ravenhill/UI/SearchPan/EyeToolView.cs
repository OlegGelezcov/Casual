using System.Linq;

namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;

    public partial class EyeToolView : BaseToolView {

        protected override void OnUse(InventoryItemData data) {
            base.OnUse(data);
            var searchbaleObj = FindObjectsOfType<BaseSearchableObject>().Where(bso => bso.isActive && !bso.isCollected).FirstOrDefault();
            if (searchbaleObj) {
                Vector3 position = canvasService.GetUIWorldPosition(GetComponent<RectTransform>());
                position.z = -30;
                GameObject instance = Instantiate<GameObject>(effectPrefab, position, Quaternion.identity);
                instance.GetComponentInChildren<EyeToolObject>().SetTarget(searchbaleObj.transform);
                searchbaleObj.SetCollectType(CollectType.Eye);
            }
        }

        protected override bool IsValid(ToolData data) {
            return FindObjectsOfType<EyeToolObject>().Length == 0;
        }
    }

    public partial class EyeToolView : BaseToolView {



        [SerializeField]
        private GameObject effectPrefab;
    }
}
