using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using Csv2XmlLib;

namespace Csv2Xml
{
  /// <summary>
  /// Command line options
  /// </summary>
  internal class Options
  {
    [Option('i', "in", Required = true, HelpText = "Input csv file.")]
    public string Input { get; set; }

    [Option('o', "out", Required = false, DefaultValue = "output.xml", HelpText = "Output xml file.")]
    public string Output { get; set; }

    [Option("header", Required = false, DefaultValue = true, HelpText = "Csv value contains header.")]
    public bool HasHeader { get; set; }

    [Option("separator", Required = false, DefaultValue = ",", HelpText = "Csv value separator.")]
    public string Separator { get; set; }

    [Option("root", Required = false, DefaultValue = "Root", HelpText = "Name of the root node.")]
    public string RootName { get; set; }

    [Option("entry", Required = false, DefaultValue = "Entry", HelpText = "Name of the entry node.")]
    public string EntryName { get; set; }

    [OptionList("h2k", Required = false, Separator = ',', HelpText = "Header to key mappings (header1:key1,header2:key2,....")]
    public IEnumerable<string> HeaderToKey { get; set; }
  }

  public class Program
  {
    public static void Main(string[] args)
    {
      var opts = new Options();
      var result = Parser.Default.ParseArguments(args, opts);

      if (!result)
      {
        Console.WriteLine(HelpText.AutoBuild(opts));
        return;
      }

      var settings = new CsvToXmlSettings()
      {
        RootNodeName = opts.RootName,
        EntryNodeName = opts.EntryName,
        Header = opts.HasHeader,
        Separator = opts.Separator
      };
      if (opts.HeaderToKey != null)
      {
        settings.SetHeaderToKeyMappings(opts.HeaderToKey.ToArray());
      }

      var converter = new CsvToXmlConverter(opts.Input, settings);

      var ret = converter.Convert();

      ret.Save(opts.Output);
    }
  }
}
