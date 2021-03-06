﻿using Casual.Ravenhill.Data;
using Casual.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Casual.Ravenhill.UI {
    public partial class HUD : RavenhillBaseView {

        private UpdateTimer healthUpdateTimer { get; } = new UpdateTimer();

#pragma warning disable 0649
        [SerializeField]
        private Image m_AvatarIconImage;

        private Image avatarIconImage => m_AvatarIconImage;

        [SerializeField]
        private ImageProgress m_ExpProgressImage;

        private ImageProgress expProgressImage => m_ExpProgressImage;

        [SerializeField]
        private Text expText;


        [SerializeField]
        private Text m_LevelText;

        private Text levelText => m_LevelText;

        [SerializeField]
        private Text m_PlayerNameText;

        private Text playerNameText => m_PlayerNameText;


        [SerializeField]
        private Text hpText;

        [SerializeField]
        private Text m_HealthTimerText;

        private Text healthTimerText => m_HealthTimerText;

        [SerializeField]
        private Text goldText;

        [SerializeField]
        private Text silverText;

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

        //[SerializeField]
        //private Button m_BankButton;

        //private Button bankButton => m_BankButton;

        //[SerializeField]
        //private Button m_StoreButton;

        //private Button storeButton => m_StoreButton;

        //[SerializeField]
        //private Button m_CollectionsButton;

        //private Button collectionsButton => m_CollectionsButton;

        //[SerializeField]
        //private Button m_InventoryButton;

        //private Button inventoryButton => m_InventoryButton;

        //[SerializeField]
        //private Button m_SocialButton;

        //private Button socialButton => m_SocialButton;

        //[SerializeField]
        //private Button m_GiftButton;

        //private Button giftButton => m_GiftButton;

        //[SerializeField]
        //private Button m_GameCenterButton;

        //private Button gameCenterButton => m_GameCenterButton;

        //[SerializeField]
        //private Button m_DialogButton;

        //private Button dialogButton => m_DialogButton;

        //[SerializeField]
        //private Button m_QuestButton;

        //private Button questButton => m_QuestButton;

        //[SerializeField]
        //private Button m_JournalButton;

        //private Button journalButton => m_JournalButton;

        //[SerializeField]
        //private Button m_AchievmentButton;

        //private Button achievmentButton => m_AchievmentButton;
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
                viewService.ShowView(RavenhillViewType.settings_view);
            }, engine.GetService<IAudioService>());

            changeNameButton.SetListener(() => {
                Debug.Log("Change Name Button");
                EnterTextView.Data enterTextData = new EnterTextView.Data {
                    startInputText = playerService.PlayerName,
                    title = "Enter new name:",
                    OnCheck = (str) => str.IsName(),
                    OnSubmit = (name) => playerService.SetName(name)
                };
                viewService.ShowView(RavenhillViewType.enter_text_view, enterTextData);
            }, engine.GetService<IAudioService>());

            addGoldButton.SetListener(() => {
                Debug.Log("add gold ");
                viewService.ShowView(RavenhillViewType.bank);
            }, engine.GetService<IAudioService>());

            addSilverButton.SetListener(() => {
                Debug.Log("add silver");
                viewService.ShowView(RavenhillViewType.bank);
            }, engine.GetService<IAudioService>());

            bankButton.SetListener(() => {
                Debug.Log("show bank");
                viewService.ShowView(RavenhillViewType.bank);
            }, engine.GetService<IAudioService>());

            storeButton.SetListener(() => {
                Debug.Log("show store");
                viewService.ShowView(RavenhillViewType.store, InventoryTab.Foods);
            }, engine.GetService<IAudioService>());

            collectionsButton.SetListener(() => {
                Debug.Log("show collections");
                viewService.ShowView(RavenhillViewType.collections);

            }, engine.GetService<IAudioService>());

            inventoryButton.SetListener(() => {
                Debug.Log("show inventory");
                viewService.ShowView(RavenhillViewType.inventory, InventoryTab.Foods);
            }, engine.GetService<IAudioService>());

            socialButton.SetListener(() => {
                Debug.Log("show social");
                viewService.ShowView(RavenhillViewType.social_view);
            }, engine.GetService<IAudioService>());

            famehallButton.SetListener(() => viewService.ShowView(RavenhillViewType.famehall), engine.GetService<IAudioService>());

            journalButton.SetListener(() => {
                Debug.Log("show journal");
                viewService.ShowView(RavenhillViewType.journal);
            }, engine.GetService<IAudioService>());

            achievmentsButton.SetListener(() => {
                Debug.Log("show achievments");
                viewService.ShowView(RavenhillViewType.achievments_view);
            }, engine.GetService<IAudioService>());

            //giftButton.SetListener(() => {
            //    Debug.Log("show gifts");
            //}, engine.GetService<IAudioService>());

            //gameCenterButton.SetListener(() => {
            //    Debug.Log("show game center");
            //}, engine.GetService<IAudioService>());

            //dialogButton.SetListener(() => {
            //    Debug.Log("show dialogs");
            //    viewService.ShowView(RavenhillViewType.famehall);
            //}, engine.GetService<IAudioService>());

            //questButton.SetListener(() => {
            //    Debug.Log("show quests");
            //}, engine.GetService<IAudioService>());



            avatarButton.SetListener(() => viewService.ShowView(RavenhillViewType.avatars_view), engine.GetService<IAudioService>());

            contextBankButton?.SetListener(() => viewService.ShowView(RavenhillViewType.bank), engine.GetService<IAudioService>());

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
            expText.text = $"{playerService.exp}/{resourceService.levelExpTable.GetExp(playerService.level + 1)}";
        }

        private void UpdateLevel() {
            levelText.text = playerService.level.ToString();
        } 

        private void UpdateName() {
            playerNameText.text = playerService.PlayerName;
        }

        private void UpdateHealthText() {
            hpText.text = $"{Mathf.FloorToInt(playerService.health)}/{playerService.maxHealth}";
        }

        private void UpdateHealthTimer(float realDelta) {
            healthTimerText.text = Utility.FormatMS(playerService.healthTimer);
        }

        private void UpdateGold() {
            NumericTextProgress progress = goldText.GetComponent<NumericTextProgress>();
            if (progress == null) {
                goldText.text = playerService.gold.ToString();
            } else {
                progress.SetValue(playerService.gold);
            }
        }

        private void UpdateSilver() {
            NumericTextProgress progress = silverText.GetComponent<NumericTextProgress>();
            if (progress == null) {
                silverText.text = playerService.silver.ToString();
            } else {
                progress.SetValue(playerService.silver);
            }
        }
    }

    public partial class HUD : RavenhillBaseView {

        [SerializeField]
        private Button bankButton;

        [SerializeField]
        private Button storeButton;

        [SerializeField]
        private Button collectionsButton;

        [SerializeField]
        private Button inventoryButton;

        [SerializeField]
        private Button socialButton;

        [SerializeField]
        private Button achievmentsButton;

        [SerializeField]
        private Button famehallButton;

        [SerializeField]
        private Button journalButton;

        [SerializeField]
        private Button contextBankButton;
    }
}
