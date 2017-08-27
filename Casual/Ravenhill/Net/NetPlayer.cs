namespace Casual.Ravenhill.Net {
    public class NetPlayer {
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
    }
}
