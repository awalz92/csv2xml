using System.Collections.Generic;
using System.Linq;

namespace Csv2XmlLib
{
    public class CsvToXmlSettings
    {
        public bool Header = true;
        public string Separator = ",";
        public string RootNodeName = "Root";
        public string EntryNodeName = "Entry";
        public Dictionary<string, string> HeaderToKeyMappings = new Dictionary<string, string>();

        public void SetHeaderToKeyMappings(IEnumerable<string> headerToKey)
        {
            if (headerToKey == null)
                return;
            
            foreach (var it in headerToKey)
            {
                var values = it.Split(':');
                HeaderToKeyMappings.Add(values.First(), values.Last());
            }
        }

        public string HeaderToKey(string header)
        {
            if (HeaderToKeyMappings.ContainsKey(header))
                return HeaderToKeyMappings[header];
            return null;
        }
    }
}
