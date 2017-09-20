using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace ravtool {
    public class StringSource : IStringSource {

        public IEnumerable<IStringEntry> Strings { get; private set; }

        public StringSource(string xmlDirectory) {
            List<IStringEntry> list = new List<IStringEntry>();

            foreach(string file in Directory.EnumerateFiles(xmlDirectory, "*.xml")) {
                list.AddRange(ReadFile(file));
            }
            Strings = list;
        }



        private IEnumerable<IStringEntry> ReadFile(string file) {
            XDocument document = XDocument.Load(file);
            foreach(XElement element in document.Element("strings").Elements("string")) {
                yield return new StringEntry(element);
            }
        }
    }
}
