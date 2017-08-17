namespace Casual.Ravenhill.Data {

    public class IconData : IIdObject {
        public string id { get; private set; }
        public string nameId { get; private set; }
        public string descriptionId { get; private set; }
        public string iconPath { get; private set; }

        public virtual void Load(UXMLElement element) {
            id = element.GetString("id");
            nameId = element.GetString("name");
            if(element.HasAttribute("description")) {
                descriptionId = element.GetString("description");
            } else {
                descriptionId = string.Empty;
            }

            iconPath = element.GetString("icon");
        }

        public bool hasIcon {
            get {
                return (!string.IsNullOrEmpty(iconPath));
            }
        }

        public bool hasDescription {
            get {
                return (!string.IsNullOrEmpty(descriptionId));
            }
        }

        public override string ToString() {
            return $"id: {id}, name: {nameId}, description: {descriptionId}, icon: {iconPath}{System.Environment.NewLine}";
        }
    }
}
