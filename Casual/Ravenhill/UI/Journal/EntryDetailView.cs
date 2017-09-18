using Casual.Ravenhill.Data;
using Casual.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class EntryDetailView : RavenhillUIBehaviour {

        public void Setup(JournalEntryInfo info ) {

            if(info == null ) {
                Debug.Log($"Entry info is null".Colored(ColorType.red));
                Clear();
                return;
            }

            nameText.text = resourceService.GetString(info.Data.nameId);
            startText.text = resourceService.GetString(info.Data.startTextId);
            hintText.text = resourceService.GetString(info.Data.hintTextId);
            pictureView.Setup(info);

            if(info.IsEndTextOpened && info.Data.hasEndText ) {
                endText.text = resourceService.GetString(info.Data.endTextId);
            } else {
                endText.text = string.Empty;
            }

            storyProgressParent.DeactivateSelf();
            if (!info.IsEndTextOpened ) {
                QuestData questData = resourceService.GetQuest(info.Data);
                if(questData != null && questData.type == Data.QuestType.story) {
                    var questInfo = engine.GetService<IQuestService>().GetInfo(questData);
                    if(questInfo != null ) {
                        if(questInfo.state == QuestState.started ) {
                            storyProgressParent.ActivateSelf();

                            int currentCounter = ravenhillGameModeService.searchCounter;
                            int conditionCount = questData.GetSearchCounterConditionValue();
                            storyProgress.SetValue(0, Mathf.Clamp01((float)currentCounter / (float)conditionCount));
                            endText.text = string.Empty;
                        }
                    }
                }
            }


            var journalService = engine.GetService<IJournalService>();
            if(journalService.IsLast(info)) {
                nextButton.DeactivateSelf();
            } else {
                nextButton.ActivateSelf();
                nextButton.SetListener(() => {
                    var nextInfo = journalService.GetNext(info);
                    if(nextInfo != null ) {
                        Setup(nextInfo);
                    }
                }, engine.GetService<IAudioService>());
            }

            if(journalService.IsFirst(info)) {
                prevButton.DeactivateSelf();
            } else {
                prevButton.ActivateSelf();
                prevButton.SetListener(() => {
                    JournalEntryInfo prevInfo = journalService.GetPrev(info);
                    if(prevInfo != null ) {
                        Setup(prevInfo);
                    }
                }, engine.GetService<IAudioService>());
            }
        }

        private void Clear() {
            nameText.text = string.Empty;
            startText.text = string.Empty;
            hintText.text = string.Empty;
            pictureView.Clear();
            endText.text = string.Empty;
            storyProgressParent.DeactivateSelf();

        }
    }

    public partial class EntryDetailView : RavenhillUIBehaviour {

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Text startText;

        [SerializeField]
        private Text hintText;

        [SerializeField]
        private EntryPictureView pictureView;

        [SerializeField]
        private Text endText;

        [SerializeField]
        private GameObject storyProgressParent;

        [SerializeField]
        private ImageProgress storyProgress;

        [SerializeField]
        private Button nextButton;

        [SerializeField]
        private Button prevButton;

    }
}
