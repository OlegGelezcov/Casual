using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ravtool {
    public class StringEntry : IStringEntry {

        public string Key { get; private set; } = string.Empty;

        public string EnText { get; private set; } = string.Empty;

        public string RuText { get; private set; } = string.Empty;

        public StringEntry(XElement element) {
            Key = element.Attribute("name").Value;

            XElement enElement = element.Element("en");
            EnText = enElement?.Value ?? string.Empty;

            XElement ruElement = element.Element("ru");
            RuText = ruElement?.Value ?? string.Empty;
        }

        public override string ToString() {
            return $"{Key}=>{Environment.NewLine}\t\t{EnText}{Environment.NewLine}\t\t{RuText}{Environment.NewLine}";
        }
    }
}
