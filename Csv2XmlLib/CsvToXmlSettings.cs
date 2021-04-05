using System.Collections.Generic;
using System.Linq;

namespace Csv2XmlLib
{
  /// <summary>
  /// Settings for CSV to XML conversion
  /// </summary>
  public class CsvToXmlSettings
  {
    public bool Header { get; set; } = true;
    public string Separator { get; set; } = ",";
    public string RootNodeName { get; set; } = "Root";
    public string EntryNodeName { get; set; }  = "Entry";
    public Dictionary<string, string> HeaderToKeyMappings { get; set; } = new Dictionary<string, string>();

    public void SetHeaderToKeyMappings(IEnumerable<string> headerToKey)
    {
      if (headerToKey == null)
      {
        return;
      }

      foreach (var it in headerToKey)
      {
        var values = it.Split(':');
        HeaderToKeyMappings.Add(values.First(), values.Last());
      }
    }

    public string HeaderToKey(string header)
    {
      return HeaderToKeyMappings.ContainsKey(header) ? HeaderToKeyMappings[header] : null;
    }
  }
}