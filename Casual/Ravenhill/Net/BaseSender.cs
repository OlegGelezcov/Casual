namespace Casual.Ravenhill.Net {
    public class BaseSender : ISender {
        private string id;
        private string avatar;
        private int level;
        private string name;

        public BaseSender(string id, string name, string avatar, int level) {
            this.id = id;
            this.avatar = avatar;
            this.level = level;
            this.name = name;
        }

        public string GetAvatar() {
            return avatar;
        }

        public string GetId() {
            return id;
        }

        public int GetLevel() {
            return level;
        }

        public string GetName() {
            return name;
        }
    }
}
