using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using CommandLine;
using CommandLine.Text;
using CommandLine.Parsing;
using HintaseurantaKarkkainen.Models;

namespace HintaseurantaKarkkainen
{
    class MainEntryPoint
    {

        public static List<int> BenchmarkList = new List<int>(); 

        static void Main(string[] args)
        {
            // Tarkistetaan ovatko kaikki argumentit syötetty
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                long programStartTime = Libs.GetUnixTimestamp();
                // Kaikki argumentit syötetty oikein. Jatketaan

                // Luodaan tiedosto XML-ia varten
                FileWriter.InitXmlFileStart(options.OutputFileName);

                Console.WriteLine("Populating categories"); // DEBUG
                Categories.PopulateCategories();

                Console.WriteLine("Downloader Init"); // DEBUG
                var downloader = new Downloader();

                Console.WriteLine("Parser Init"); // DEBUG
                var parser = new Parser();

                // Haetaan kaikki kategoriat annetun merkkijonon avulla
                List<Category> categories = options.CategoriesCommandLine != "all" 
                    ? Categories.GetCategoriesByString(options.CategoriesCommandLine) 
                    : Categories.GetAllCategories();
                Console.WriteLine("Categories total: {0}", categories.Count); // DEBUG

                // Parsataan haetut kategoriat
                foreach (Category category in categories)
                {
                    // Tehdään AJAX-kutsu
                    string stringResponse = downloader.MakePostRequest(category.AjaxUrl, category.AjaxBodyData);

                    // Haetaan kaikki tuotteet AJAX-kutsusta
                    List<Product> products = parser.ParseJsonUsingDynamicJson(stringResponse, downloader, options.SleepTime);
                    if (products != null && products.Count > 0)
                    {
                        // Kirjoitetaan tiedostoon
                        long startTime = Libs.GetUnixTimestamp(); // DEBUG
                        Console.Write("Starting writing to file... "); // DEBUG
                        FileWriter.WritePartProductListToXmlFile(options.OutputFileName, products);
                        long endTime = Libs.GetUnixTimestamp(); // DEBUG
                        Console.Write("Success, took: {0} ms\n", endTime - startTime); // DEBUG
                    }
                }

                long programEndTime = Libs.GetUnixTimestamp();
                Console.WriteLine("Everything done. Total time took: {0} seconds. Average iteration time: {1} ms.Exiting", 
                    (programEndTime - programStartTime) / 1000, BenchmarkList.Sum() / BenchmarkList.Count);

            }

        }
    }

    /**
     *  Luokka command-line -argumentteja varten
     */
    class Options
    {
        [Option("outputFile", Required = true,
            HelpText = "File name to save")]
        public string OutputFileName{ get; set; }

        [Option("sleepTime", Required = true,
            HelpText = "Time to sleep before making another web request to server")]
        public int SleepTime { get; set; }

        [Option("categories", Required = true,
            HelpText = "Categories to parse. To parse everything use \"all\". See README for more information")]
        public string CategoriesCommandLine { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

}
