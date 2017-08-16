using Casual.Ravenhill.Data;
using UnityEngine;

namespace Casual.Ravenhill {
    public class PlayerService : RavenhillBaseListenerBehaviour, IPlayerService, ISaveable {

        private const int kBaseMaxHealth = 100;
        private const int kHealthRestoreInterval = 180;


        public int exp { get; private set; } = 0;

        public int level { get {
                return resourceService.levelExpTable.GetLevelForExp(exp);
            }
        } 

        public string playerName { get; private set; } = string.Empty;

        public string avatarId { get; private set; } = "Avatar1";

        public int silver { get; private set; } = 0;

        public int gold { get; private set; } = 0;

        public int maxHealth { get; private set; } = kBaseMaxHealth;

        public int health { get; private set; } = kBaseMaxHealth;

        public float healthTimer { get; set; } = kHealthRestoreInterval;

        public override void Start() {
            base.Start();
            engine.GetService<ISaveService>().Register(this);
        }

        public override void Update() {
            base.Update();
            healthTimer -= Time.deltaTime;
            if(healthTimer <= 0.0f ) {
                healthTimer += kHealthRestoreInterval;
                AddHealth(1);
            }
        }

        public void AddMaxHealth(int count ) {
            int oldCount = maxHealth;
            maxHealth += count;
            if(oldCount != maxHealth ) {
                eventService.SendEvent(new PlayerMaxHealthChangedEventArgs(oldCount, maxHealth));
            }
        }

        public void AddHealth(int count ) {
            int oldCount = health;
            health += Mathf.Abs(count);
            health = Mathf.Clamp(health, 0, maxHealth);
            if(oldCount != health) {
                eventService.SendEvent(new PlayerHealthChangedEventArgs(oldCount, health));
            }
        }

        public void RemoveHealth(int count) {
            int oldCount = health;
            health -= Mathf.Abs(count);
            health = Mathf.Clamp(health, 0, maxHealth);
            if(oldCount != health ) {
                eventService.SendEvent(new PlayerHealthChangedEventArgs(oldCount, health));
            }
        }

        public void AddCoins(PriceData coins) {
            switch(coins.type) {
                case MoneyType.gold: {
                        AddGold(coins.price);
                    }
                    break;
                case MoneyType.silver: {
                        AddSilver(coins.price);
                    }
                    break;
            }
        }

        public void AddSilver(int count ) {
            int oldCount = silver;
            silver += count;
            if(oldCount != silver ) {
                eventService.SendEvent(new PlayerSilverChangedEventArgs(oldCount, silver));
            }
        }


        public void SetSilver(int count) {
            int oldCount = silver;
            silver = count;
            if(oldCount != silver ) {
                eventService.SendEvent(new PlayerSilverChangedEventArgs(oldCount, silver));
            }
        }

        public void AddGold(int count ) {
            int oldCount = gold;
            gold += count;
            if(oldCount != gold ) {
                eventService.SendEvent(new PlayerGoldChangedEventArgs(oldCount, gold));
            }
        }

        public void SetGold(int count) {
            int oldCount = gold;
            gold = count;
            if(oldCount != gold ) {
                eventService.SendEvent(new PlayerGoldChangedEventArgs(oldCount, gold));
            }
        }

        public void AddExp(int count) {
            int prevLevel = level;
            int oldExp = exp;
            exp += count;
            int newLevel = level;
            if(oldExp != exp ) {
                eventService.SendEvent(new PlayerExpChangedEventArgs(oldExp, exp));
                if(oldExp != newLevel ) {
                    eventService.SendEvent(new PlayerLevelChangedEventArgs(prevLevel, newLevel));
                }
            }
        }

        public void SetExp(int count) {
            int prevLevel = level;
            int oldExp = exp;
            exp = count;
            int newLevel = level;
            if (oldExp != exp) {
                eventService.SendEvent(new PlayerExpChangedEventArgs(oldExp, exp));
                if (oldExp != newLevel) {
                    eventService.SendEvent(new PlayerLevelChangedEventArgs(prevLevel, newLevel));
                }
            }
        }

        public void SetAvatar(AvatarData avatar) {
            string oldAvatar = avatarId;
            avatarId = avatar.id;
            if(oldAvatar != avatarId ) {
                eventService.SendEvent(new PlayerAvatarChangedEventArgs(oldAvatar, avatarId));
            }
        }

        public void SetName(string newName) {
            string oldName = playerName;
            playerName = newName;
            if(oldName != playerName ) {
                eventService.SendEvent(new PlayerNameChangedEventArgs(oldName, playerName));
            }
        }

        public void Setup(object data) {

        }

        #region ISaveable
        public string GetSave() {
            UXMLWriteElement playerElement = new UXMLWriteElement(saveId);
            playerElement.AddAttribute("exp", exp);
            playerElement.AddAttribute("name", playerName);
            playerElement.AddAttribute("avatar", avatarId);
            playerElement.AddAttribute("silver", silver);
            playerElement.AddAttribute("gold", gold);
            return playerElement.ToString();
        }

        public bool Load(string saveStr) {
            if (string.IsNullOrEmpty(saveStr)) {
                InitSave();
            } else {
                UXMLDocument document = UXMLDocument.FromXml(saveStr);
                UXMLElement playerElement = document.Element(saveId);
                exp = playerElement.GetInt("exp", 0);
                playerName = playerElement.GetString("name", string.Empty);
                avatarId = playerElement.GetString("avatar", "Avatar1");
                silver = playerElement.GetInt("silver", 0);
                gold = playerElement.GetInt("gold", 0);
                isLoaded = true;
            }
            return true;
        }

        public void InitSave() {
            exp = 0;
            playerName = string.Empty;
            avatarId = "Avatar1";
            silver = 0;
            gold = 0;
            isLoaded = true;
        }

        public void OnRegister() {
        }

        public void OnLoaded() {
        }



        public string saveId => "player";

        public bool isLoaded { get; private set; } = false; 
        #endregion

        public override string listenerName => "player";
    }
}
