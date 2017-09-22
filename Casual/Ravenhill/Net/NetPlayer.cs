using System;

namespace Casual.Ravenhill.Net {
    public class NetPlayer : ISender, ISaveElement{
        public string id { get; private set; }
        public string name { get; private set; }
        public string avatarId { get; private set; }
        public int level { get; private set; }

        public bool isValid { get; private set; }

        public NetPlayer(string id, string name, string avatarId, int level, bool valid) {
            this.id = id;
            this.name = name;
            this.avatarId = avatarId;
            this.level = level;
            this.isValid = valid;
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
