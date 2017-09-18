namespace Casual.Ravenhill.Data {
    public class KeyValueProperty {


        public string Key { get; private set; }
        
        private string Value { get; set; }

        public KeyValueProperty(UXMLElement element)
            : this(element.GetString("key"), element.GetString("value")) { }

        public KeyValueProperty(string key, string value) {
            Key = key;
            Value = value;
        }

        public string ValueAsString => Value;

        public int ValueAsInt {
            get {
                int val;
                if(int.TryParse(Value, out val)) {
                    return val;
                }
                return 0;
            }
        }

        public float ValueAsFloat {
            get {
                float val;
                if(float.TryParse(Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out val)) {
                    return val;
                }
                return 0.0f;
            }
        }

        public bool ValueAsBool {
            get {
                bool val;
                if(bool.TryParse(Value, out val)) {
                    return val;
                }
                return false;
            }
        }
    }
}
