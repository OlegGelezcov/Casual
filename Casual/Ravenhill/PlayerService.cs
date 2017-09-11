using Casual.Ravenhill.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Casual.Ravenhill {

    public class PlayerService : RavenhillGameBehaviour, IPlayerService, ISaveable {

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

        public float health { get; private set; } = kBaseMaxHealth;

        public float healthTimer { get; set; } = kHealthRestoreInterval;

        private Inventory inventory { get; } = new Inventory();
        private Wishlist wishlist { get; } = new Wishlist();

        #region Wishlist
        public bool AddToWishlist(InventoryItemData data) {
            return wishlist.Add(data);
        }

        public bool RemoveFromWishlist(InventoryItemData data ) {
            return wishlist.Remove(data);
        }

        public bool IsWishlistContains(InventoryItemData data) {
            return wishlist.IsContains(data);
        }

        public bool IsWishlistFull {
            get {
                return wishlist.IsFull;
            }
        }

        public List<InventoryItemData> WishItems => wishlist.itemList;

        public int WishlistCount => wishlist.Count;
        #endregion

        public int GetItemCount(InventoryItemData data) {
            return inventory.ItemCount(data.type, data.id);
        }



        public void AddItem(InventoryItem item) {
            inventory.AddItem(item);
        }

        public bool RemoveItem(InventoryItemType type, string id, int count) {
            return inventory.RemoveItem(type, id, count);
        }

        public void RemoveItems(InventoryItemType type) {
            inventory.RemoveItems(type);
        }

        public void UseItem(InventoryItemData data ) {
            Debug.Log($"Use item: {data.type}");
        }


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
                RavenhillEvents.OnPlayerMaxHealthChanged(oldCount, maxHealth);
            }
        }

        public void AddHealth(float count ) {
            float oldCount = health;
            health += Mathf.Abs(count);
            health = Mathf.Clamp(health, 0, maxHealth);
            if(!Mathf.Approximately(oldCount, health)) {
                RavenhillEvents.OnPlayerHealthChanged(oldCount, health);
            }
        }

        public void RemoveHealth(float count) {
            float oldCount = health;
            health -= Mathf.Abs(count);
            health = Mathf.Clamp(health, 0, maxHealth);
            if(!Mathf.Approximately(oldCount, health)) {
                RavenhillEvents.OnPlayerHealthChanged(oldCount, health);
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

        public void RemoveCoins(PriceData coins ) {
            switch(coins.type ) {
                case MoneyType.gold: {
                        RemoveGold(coins.price);
                    }
                    break;
                case MoneyType.silver: {
                        RemoveSilver(coins.price);
                    }
                    break;
            }
        }

        public bool HasCoins(PriceData coins ) {
            switch(coins.type) {
                case MoneyType.gold: {
                        return gold >= coins.price;
                    }
                case MoneyType.silver: {
                        return silver >= coins.price;
                    }
            }
            return false;
        }

        public void AddSilver(int count ) {
            int oldCount = silver;
            silver += count;
            if(oldCount != silver ) {
                RavenhillEvents.OnPlayerSilverChanged(oldCount, silver);
            }
        }


        public void SetSilver(int count) {
            int oldCount = silver;
            silver = count;
            if(oldCount != silver ) {
                RavenhillEvents.OnPlayerSilverChanged(oldCount, silver);
            }
        }

        public void RemoveSilver(int count) {
            if(silver >= count ) {
                int oldSilver = silver;
                silver -= count;
                if(oldSilver != silver ) {
                    RavenhillEvents.OnPlayerSilverChanged(oldSilver, silver);
                }
            }
        }

        public void AddGold(int count ) {
            int oldCount = gold;
            gold += count;
            if(oldCount != gold ) {
                RavenhillEvents.OnPlayerGoldChanged(oldCount, gold);
            }
        }

        public void SetGold(int count) {
            int oldCount = gold;
            gold = count;
            if(oldCount != gold ) {
                RavenhillEvents.OnPlayerGoldChanged(oldCount, gold);
            }
        }

        public void RemoveGold(int count ) {
            if(gold >= count ) {
                int oldCount = gold;
                gold -= count;
                if (oldCount != gold) {
                    RavenhillEvents.OnPlayerGoldChanged(oldCount, gold);
                }
            }
        }

        public void AddExp(int count) {
            int prevLevel = level;
            int oldExp = exp;
            exp += count;
            int newLevel = level;
            if(oldExp != exp ) {
                RavenhillEvents.OnPlayerExpChanged(oldExp, exp);

                if(oldExp != newLevel ) {
                    RavenhillEvents.OnPlayerLevelChanged(prevLevel, newLevel);
                }
            }
        }

        public void SetExp(int count) {
            int prevLevel = level;
            int oldExp = exp;
            exp = count;
            int newLevel = level;
            if (oldExp != exp) {
                RavenhillEvents.OnPlayerExpChanged(oldExp, exp);

                if (oldExp != newLevel) {
                    RavenhillEvents.OnPlayerLevelChanged(prevLevel, newLevel);
                }
            }
        }

        public void SetAvatar(AvatarData avatar) {
            string oldAvatar = avatarId;
            avatarId = avatar.id;
            if(oldAvatar != avatarId ) {
                RavenhillEvents.OnPlayerAvatarChanged(oldAvatar, avatarId);
            }
        }

        public void SetName(string newName) {
            string oldName = playerName;
            playerName = newName;
            if(oldName != playerName ) {
                RavenhillEvents.OnPlayerNameChanged(oldName, playerName);
            }
        }

        public void AddToInventory(InventoryItem item) {
            AddItem(item);
        }

        public bool RemoveFromInventory(InventoryItemType type, string id, int count ) {
            return RemoveItem(type, id, count);
        }

        public int ItemCount(InventoryItemType type, string id) {
            return inventory.ItemCount(type, id);
        }

        public void Setup(object data) {

        }

        public bool Buy(StoreItemData data ) {
            if(HasCoins(data.price)) {
                RemoveCoins(data.price);
                var itemData = resourceService.GetInventoryItemData(data.itemType, data.itemId);
                if(itemData != null ) {
                    AddItem(new InventoryItem(itemData, data.count));
                    Debug.Log($"After buy StoreItemData items was added {data.count}{itemData.id}");
                    return true;
                }
            }
            return false;
        }

        #region ISaveable
        public string GetSave() {
            UXMLWriteElement playerElement = new UXMLWriteElement(saveId);
            playerElement.AddAttribute("exp", exp);
            playerElement.AddAttribute("name", playerName);
            playerElement.AddAttribute("avatar", avatarId);
            playerElement.AddAttribute("silver", silver);
            playerElement.AddAttribute("gold", gold);
            playerElement.AddAttribute("health", health);
            playerElement.AddAttribute("maxhealth", maxHealth);
            playerElement.AddAttribute("time", Utility.unixTime);

            playerElement.Add(inventory.GetSave());
            playerElement.Add(wishlist.GetSave());
            
            Debug.Log($"SAVE PLAYER: {playerElement.ToString()}".Colored(ColorType.yellow));
            return playerElement.ToString();
        }

        public bool Load(string saveStr) {
            if (string.IsNullOrEmpty(saveStr)) {
                InitSave();
            } else {
                Debug.Log($"LOAD PLAYER {saveStr}");
                UXMLDocument document = UXMLDocument.FromXml(saveStr);
                UXMLElement playerElement = document.Element(saveId);
                exp = playerElement.GetInt("exp", 0);
                playerName = playerElement.GetString("name", string.Empty);
                avatarId = playerElement.GetString("avatar", "Avatar1");
                silver = playerElement.GetInt("silver", 0);
                gold = playerElement.GetInt("gold", 0);
                health = playerElement.GetFloat("health", kBaseMaxHealth);
                maxHealth = playerElement.GetInt("maxhealth", kBaseMaxHealth);
                int time = playerElement.GetInt("time", 0);
                if(time > 0 ) {
                    int interval = Utility.unixTime - time;
                    if(interval < 0 ) {
                        interval = 0;
                    }
                    float restoreSpeed = 1.0f / (float)kHealthRestoreInterval;
                    float restoreCount = restoreSpeed * time;
                    health += restoreCount;
                    health = Mathf.Clamp(health, 0, maxHealth);
                }


                var inventoryElement = playerElement.Element("inventory");
                if(inventoryElement != null ) {
                    inventory.Load(inventoryElement);
                } else {
                    inventory.InitSave();
                }

                var wishlistElement = playerElement.Element("wishlist");
                if(wishlistElement != null ) {
                    wishlist.Load(wishlistElement); 
                } else {
                    wishlist.InitSave();
                }

                Debug.Log($"Player Loaded: exp-{exp}, name-{playerName}, avatar-{avatarId}, silver-{silver}, gold-{gold}");

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
            inventory.InitSave();
            isLoaded = true;
        }

        public void OnRegister() {
        }

        public void OnLoaded() {
        }



        public string saveId => "player";

        public bool isLoaded { get; private set; } = false; 
        #endregion
    }
}
