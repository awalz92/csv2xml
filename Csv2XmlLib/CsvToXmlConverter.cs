using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Csv2XmlLib
{
    public class CsvToXmlConverter
    {
        public string FilePath;
        public CsvToXmlSettings Settings;

        public CsvToXmlConverter(string fileName, CsvToXmlSettings settings = null)
        {
            FilePath = fileName;
            Settings = settings ?? new CsvToXmlSettings();
        }

        public XDocument Convert()
        {
            if (FilePath == null)
                throw new ArgumentNullException();

            // read csv entries
            var lines = File.ReadAllLines(FilePath, Encoding.Default);

            string[] headers = lines.First().Split(Settings.Separator.ToCharArray()).Select(it => it.Trim('"')).ToArray();
            int skip = 1;
            if (!Settings.Header)
            {
                headers = Enumerable.Range(0, headers.Count()).Select(it => it.ToString()).ToArray();
                skip = 0;
            }

            var csvEntries = new List<CsvEntry>();
            foreach (var line in lines.Skip(skip))
            {
                var values = line.Split(Settings.Separator.ToCharArray());
                var csvEntry = new CsvEntry();
                for (int i = 0; i < headers.Count(); i++)
                {
                    var key = Settings.HeaderToKey(headers[i]) ?? headers[i];
                    var value = values[i].Trim('"').Replace(@"\n", Environment.NewLine);

                    csvEntry.KeyValues.Add(key, value);
                }
                csvEntries.Add(csvEntry);
            }
            
            // write to xml
            var doc = new XDocument();

            var root = new XElement(Settings.RootNodeName);

            foreach (var csvEntry in csvEntries)
            {
                var newEntry = new XElement(Settings.EntryNodeName);
                newEntry.Add(csvEntry.KeyValues.Select(it => 
                    new XElement("String", 
                        new XElement("Key", it.Key),
                        new XElement("Value", it.Value))));
                root.Add(newEntry);
            }

            doc.Add(root);

            System.Diagnostics.Debug.WriteLine(doc);

            return doc;
        }
    }
}
