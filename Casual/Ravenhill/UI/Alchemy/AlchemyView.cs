using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.UI {
    using Casual.Ravenhill.Data;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class AlchemyView : RavenhillCloseableView {

        private int index = 0;

        public override RavenhillViewType viewType => RavenhillViewType.alchemy_view;

        public override bool isModal => true;

        public override int siblingIndex => 2;


        private List<BonusData> source = null;

        private readonly UpdateTimer combineUpdateTimer = new UpdateTimer();

        public override void Setup(object objdata = null) {
            base.Setup(objdata);

            if(source == null ) {
                source = resourceService.bonusList;
            }
            if(objdata == null ) {
                index = 0;
            } else {
                BonusData data = objdata as BonusData;
                if(data == null ) {
                    throw new ArgumentException(typeof(BonusData).Name);
                }
                index = source.FindIndex(b => b.id == data.id);
                if(index < 0 ) {
                    index = 0;
                }
            }

            BonusData bonusData = source[index];
            List<string> ingredientIds = bonusData.ingredientList;
            wishlistView.Setup();
            for(int i = 0; i < ingredientIds.Count; i++) {
                IngredientData ingredientData = resourceService.GetIngredient(ingredientIds[i]);
                if(i < ingredientViews.Length) {
                    ingredientViews[i].Setup(ingredientData);
                }
            }

            bonusView.Setup(bonusData);

            nextButton.SetListener(() => {
                if (source != null) {
                    index++;
                    if (index >= source.Count) {
                        index = 0;
                    }
                    BonusData nextData = source[index];
                    Setup(nextData);
                }
            });

            prevButton.SetListener(() => {
                if(source != null ) {
                    index--;
                    if(index < 0 ) {
                        index = source.Count - 1;
                    }
                    BonusData prevData = source[index];
                    Setup(prevData);
                }
            });


            combineButton.SetListener(() => {
                if(ravenhillGameModeService.IsAlchemyReadyToCharge(bonusData)) {
                    ravenhillGameModeService.ChargeAlchemy(bonusData);
                }
            });

            combineUpdateTimer.Setup(0.5f, (delta) => {
                if(bonusData != null ) {
                    combineButton.interactable = ravenhillGameModeService.IsAlchemyReadyToCharge(bonusData);
                }
            });
        }

        public override void OnEnable() {
            base.OnEnable();
            RavenhillEvents.AlchemyCharged += OnAlchemyCharged;
        }

        public override void OnDisable() {
            base.OnDisable();
            RavenhillEvents.AlchemyCharged -= OnAlchemyCharged;
        }

        private void OnAlchemyCharged(BonusData data ) {
            if(source != null && index >= 0 ) {
                Setup(source[index]);
            }
        }

    }

    public partial class AlchemyView : RavenhillCloseableView {

        [SerializeField]
        private WishlistView wishlistView;

        [SerializeField]
        private AlchemyIngredientView[] ingredientViews;

        [SerializeField]
        private AlchemyBonusView bonusView;

        [SerializeField]
        private Button combineButton;

        [SerializeField]
        private Button nextButton;

        [SerializeField]
        private Button prevButton;
    }
}
