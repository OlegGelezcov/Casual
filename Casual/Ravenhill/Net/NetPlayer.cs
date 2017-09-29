using System;
using System.Collections.Generic;

namespace Casual.Ravenhill.Net {
    public class NetPlayer : INetUser, ISender, ISaveElement, INullObject {

        public string id { get; private set; }
        public string name { get; private set; }
        public string avatarId { get; private set; }
        public int level { get; private set; }
        public bool isValid { get; private set; }

        public static NetPlayer Null => new NetPlayer(string.Empty, string.Empty, string.Empty, 1, false);

        public bool IsNull => string.IsNullOrEmpty(id) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(avatarId) && (level == 1) && (!isValid);

        public NetPlayer(string id, string name, string avatarId, int level, bool valid) {
            this.id = id;
            this.name = name;
            this.avatarId = avatarId;
            this.level = level;
            this.isValid = valid;
        }

        public NetPlayer(Dictionary<string, object> dict, bool isValid) {
            LoadFromDictionary(dict);
            this.isValid = isValid;
        }

        public void CopyFrom(NetPlayer player) {
            this.id = player.id;
            this.name = player.name;
            this.avatarId = player.avatarId;
            this.level = player.level;
            this.isValid = player.isValid;
        }

        public void LoadFromDictionary(Dictionary<string, object> dict) {
            id = dict.GetOrDefault("user_id", string.Empty).ToString();
            name = dict.GetOrDefault("user_name", string.Empty).ToString();
            avatarId = dict.GetOrDefault("avatar_id", "Avatar1").ToString();

            string strLevel = dict.GetOrDefault("level", "1").ToString();
            int result;
            if(int.TryParse(strLevel, out result )) {
                level = result;
            } else {
                level = 1;
            }
        }

        public void SetName(string name) {
            this.name = name;
        }

        public void SetAvatar(string avatar) {
            this.avatarId = avatar;
        }

        public void SetLevel(int level) {
            this.level = level;
        }

        public NetPlayer() {

        }

        public string GetId() {
            return id;
        }

        public int GetLevel() {
            return level;
        }

        public string GetAvatar() {
            return avatarId;
        }

        public string GetName() {
            return name;
        }

        public override string ToString() {
            return $"Id: {id}, name: {name}, avatar: {avatarId}, level: {level}, is valid: {isValid}";
        }

        public UXMLWriteElement GetSave() {
            UXMLWriteElement writeElement = new UXMLWriteElement("local_player");
            writeElement.AddAttribute("id", id);
            writeElement.AddAttribute("name", name);
            writeElement.AddAttribute("avatar", avatarId);
            writeElement.AddAttribute("level", level);
            writeElement.AddAttribute("is_valid", isValid);
            return writeElement;
        }

        public void Load(UXMLElement element) {
            if(element != null ) {
                id = element.GetString("id");
                name = element.GetString("name");
                avatarId = element.GetString("avatar");
                level = element.GetInt("level");
                isValid = element.GetBool("is_valid");
            }
        }

        public void InitSave() {
            id = string.Empty;
            name = string.Empty;
            avatarId = string.Empty;
            level = 1;
            isValid = false;
        }
    }
}
