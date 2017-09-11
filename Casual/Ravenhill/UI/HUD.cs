using Casual.Ravenhill.Data;
using Casual.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public class HUD : RavenhillBaseView {

        private UpdateTimer healthUpdateTimer { get; } = new UpdateTimer();

#pragma warning disable 0649
        [SerializeField]
        private Image m_AvatarIconImage;

        private Image avatarIconImage => m_AvatarIconImage;

        [SerializeField]
        private ImageProgress m_ExpProgressImage;

        private ImageProgress expProgressImage => m_ExpProgressImage;

        [SerializeField]
        private NumericTextProgress m_ExpText;

        private NumericTextProgress expText => m_ExpText;


        [SerializeField]
        private Text m_LevelText;

        private Text levelText => m_LevelText;

        [SerializeField]
        private Text m_PlayerNameText;

        private Text playerNameText => m_PlayerNameText;

        [SerializeField]
        private NumericTextProgress m_HealthText;

        private NumericTextProgress healthText => m_HealthText;

        [SerializeField]
        private Text m_HealthTimerText;

        private Text healthTimerText => m_HealthTimerText;

        [SerializeField]
        private NumericTextProgress m_GoldText;

        private NumericTextProgress goldText => m_GoldText;

        [SerializeField]
        private NumericTextProgress m_SilverText;

        private NumericTextProgress silverText => m_SilverText;

        [SerializeField]
        private Button m_AvatarButton;

        private Button avatarButton => m_AvatarButton;

        [SerializeField]
        private Button m_SettingsButton;

        private Button settingsButton => m_SettingsButton;

        [SerializeField]
        private Button m_ChangeNameButton;

        private Button changeNameButton => m_ChangeNameButton;

        [SerializeField]
        private Button m_AddGoldButton;

        private Button addGoldButton => m_AddGoldButton;

        [SerializeField]
        private Button m_AddSilverButton;

        private Button addSilverButton => m_AddSilverButton;

        [SerializeField]
        private Button m_HelpButton;

        private Button helpButton => m_HelpButton;

        [SerializeField]
        private Button m_BankButton;

        private Button bankButton => m_BankButton;

        [SerializeField]
        private Button m_StoreButton;

        private Button storeButton => m_StoreButton;

        [SerializeField]
        private Button m_CollectionsButton;

        private Button collectionsButton => m_CollectionsButton;

        [SerializeField]
        private Button m_InventoryButton;

        private Button inventoryButton => m_InventoryButton;

        [SerializeField]
        private Button m_SocialButton;

        private Button socialButton => m_SocialButton;

        [SerializeField]
        private Button m_GiftButton;

        private Button giftButton => m_GiftButton;

        [SerializeField]
        private Button m_GameCenterButton;

        private Button gameCenterButton => m_GameCenterButton;

        [SerializeField]
        private Button m_DialogButton;

        private Button dialogButton => m_DialogButton;

        [SerializeField]
        private Button m_QuestButton;

        private Button questButton => m_QuestButton;

        [SerializeField]
        private Button m_JournalButton;

        private Button journalButton => m_JournalButton;

        [SerializeField]
        private Button m_AchievmentButton;

        private Button achievmentButton => m_AchievmentButton;
#pragma warning restore 0649




        public override RavenhillViewType viewType => RavenhillViewType.hud;

        public override bool isModal => false;

        public override int siblingIndex => -10;

        public override void Setup(object data = null) {
            Debug.Log("show HUD");
            base.Setup(data);
            healthUpdateTimer.Setup(1, UpdateHealthTimer);
            UpdatePlayerAvatar();
            UpdateExp();
            UpdateLevel();
            UpdateName();
            UpdateHealthText();
            UpdateGold();
            UpdateSilver();

            settingsButton.SetListener(() => {
                Debug.Log("Click Settings");
            });

            changeNameButton.SetListener(() => {
                Debug.Log("Change Name Button");
            });

            addGoldButton.SetListener(() => {
                Debug.Log("add gold ");
                viewService.ShowView(RavenhillViewType.bank);
            });

            addSilverButton.SetListener(() => {
                Debug.Log("add silver");
                viewService.ShowView(RavenhillViewType.bank);
            });

            bankButton.SetListener(() => {
                Debug.Log("show bank");
                viewService.ShowView(RavenhillViewType.bank);
            });

            storeButton.SetListener(() => {
                Debug.Log("show store");
                viewService.ShowView(RavenhillViewType.store, InventoryTab.Foods);
            });

            collectionsButton.SetListener(() => {
                Debug.Log("show collections");
                viewService.ShowView(RavenhillViewType.collections);

            });

            inventoryButton.SetListener(() => {
                Debug.Log("show inventory");
                viewService.ShowView(RavenhillViewType.inventory, InventoryTab.Foods);
            });

            socialButton.SetListener(() => {
                Debug.Log("show social");
            });

            giftButton.SetListener(() => {
                Debug.Log("show gifts");
            });

            gameCenterButton.SetListener(() => {
                Debug.Log("show game center");
            });

            dialogButton.SetListener(() => {
                Debug.Log("show dialogs");
                viewService.ShowView(RavenhillViewType.famehall);
            });

            questButton.SetListener(() => {
                Debug.Log("show quests");
            });

            journalButton.SetListener(() => {
                Debug.Log("show journal");
                viewService.ShowView(RavenhillViewType.journal);
            });

            achievmentButton.SetListener(() => {
                Debug.Log("show achievments");
                viewService.ShowView(RavenhillViewType.achievments_view);
            });

            avatarButton.SetListener(() => viewService.ShowView(RavenhillViewType.avatars_view));
        }

        public override void OnEnable() {
            base.OnEnable();

            RavenhillEvents.PlayerAvatarChanged += OnAvatarChanged;
            RavenhillEvents.PlayerExpChanged += OnPlayerExpChanged;
            RavenhillEvents.PlayerLevelChanged += OnPlayerLevelChanged;
            RavenhillEvents.PlayerNameChanged += OnPlayerNameChanged;
            RavenhillEvents.PlayerHealthChanged += OnPlayerHealthChanged;
            RavenhillEvents.PlayerMaxHealthChanged += OnPlayerMaxHealthChanged;
            RavenhillEvents.PlayerGoldChanged += OnPlayerGoldChanged;
            RavenhillEvents.PlayerSilverChanged += OnPlayerSilverChanged;
            RavenhillEvents.SaveableLoaded += OnSaveableLoaded;
        }

        public override void OnDisable() {
            base.OnDisable();

            RavenhillEvents.PlayerAvatarChanged -= OnAvatarChanged;
            RavenhillEvents.PlayerExpChanged -= OnPlayerExpChanged;
            RavenhillEvents.PlayerLevelChanged -= OnPlayerLevelChanged;
            RavenhillEvents.PlayerNameChanged -= OnPlayerNameChanged;
            RavenhillEvents.PlayerHealthChanged -= OnPlayerHealthChanged;
            RavenhillEvents.PlayerMaxHealthChanged -= OnPlayerMaxHealthChanged;
            RavenhillEvents.PlayerGoldChanged -= OnPlayerGoldChanged;
            RavenhillEvents.PlayerSilverChanged -= OnPlayerSilverChanged;
            RavenhillEvents.SaveableLoaded -= OnSaveableLoaded;
        }

        private void OnSaveableLoaded(ISaveable saveable ) {
            if(saveable.saveId == "player") {
                Setup();
            }
        }

        private void OnPlayerSilverChanged(int oldSilver, int newSilver ) {
            UpdateSilver();
        }

        private void OnPlayerGoldChanged(int oldGold, int newGold ) {
            UpdateGold();
        }

        private void OnPlayerMaxHealthChanged(int oldMaxHealth, int newMaxHealth ) {
            UpdateHealthText();
        }

        private void OnPlayerHealthChanged(float oldHealth, float newHealth ) {
            UpdateHealthText();
        }

        private void OnPlayerNameChanged(string oldName, string newName ) {
            UpdateName();
        }

        private void OnPlayerLevelChanged(int oldLevel, int newLevel ) {
            UpdateLevel();
        }

        private void OnPlayerExpChanged(int oldExp, int newExp ) {
            UpdateExp();
        }

        private void OnAvatarChanged(string oldAvatar, string newAvatar ) {
            UpdatePlayerAvatar();
        }

        public override void Update() {
            base.Update();
            healthUpdateTimer.Update();
        }

        private void UpdatePlayerAvatar() {

            AvatarData data = resourceService.GetAvatarData(playerService.avatarId);
            if(data != null ) {
                avatarIconImage.overrideSprite = resourceService.GetSprite(data);
            } else {
                avatarIconImage.overrideSprite = resourceService.transparent;
            }
        }

        private void UpdateExp() {
            expProgressImage.SetValue(resourceService.levelExpTable.GetProgress(playerService.exp));
            expText.postfix = "/" + resourceService.levelExpTable.GetExp(playerService.level + 1);
            expText.SetValue(playerService.exp);
        }

        private void UpdateLevel() {
            levelText.text = playerService.level.ToString();
        } 

        private void UpdateName() {
            playerNameText.text = playerService.playerName;
        }

        private void UpdateHealthText() {
            healthText.postfix = "/" + playerService.maxHealth.ToString();
            healthText.SetValue(Mathf.FloorToInt(playerService.health));
        }

        private void UpdateHealthTimer(float realDelta) {
            healthTimerText.text = Utility.FormatMS(playerService.healthTimer);
        }

        private void UpdateGold() {
            goldText.SetValue(playerService.gold);
        }

        private void UpdateSilver() {
            silverText.SetValue(playerService.silver);
        }
    }
}
