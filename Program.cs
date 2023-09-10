using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DataReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "magmom_AFM1.txt";
            string outputPath = "PajaData.txt";

            //FileStream filestream = new FileStream("PajaData.txt", FileMode.Create);
            //var streamwriter = new StreamWriter(filestream);
            //streamwriter.AutoFlush = true;
            //Console.SetOut(streamwriter);
            //Console.SetError(streamwriter);
        
            Dictionary<double, Dictionary<string, List<Tuple<int, double>>>> mainDictionary = new Dictionary<double, Dictionary<string, List<Tuple<int, double>>>>();
            Dictionary<string, List<Tuple<int, double>>> valuesDictionary = new Dictionary<string, List<Tuple<int, double>>>();

            int ionX = 0;
            int ionY = 0;
            int ionZ = 0;
            double totalX = 0;
            double totalY = 0;
            double totalZ = 0;
            double aValue = 0;
            int counter = 0;
            int lineCounter = 0;
            var listOfSetsForOneMagTypeX = new List<Tuple<int, double>> { };
            var listOfSetsForOneMagTypeY = new List<Tuple<int, double>> { };
            var listOfSetsForOneMagTypeZ = new List<Tuple<int, double>> { };

            StreamReader sr = new StreamReader(filePath);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                if (line.StartsWith("a="))
                {
                    aValue = double.Parse(line.Substring(2).Trim());


                }
                else if (line.StartsWith("    ") || line.StartsWith("   "))
                {
                    string[] parts = line.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    counter++;

                    int numIon = int.Parse(parts[0]);

                    double total = double.Parse(parts[4]);



                    if (counter <= 24)
                    {
                        ionX = numIon;
                        totalX = total;
                        var oneSetOfValues1FromOneMagType = new Tuple<int, double>(ionX, totalX);
                        listOfSetsForOneMagTypeX.Add(oneSetOfValues1FromOneMagType);
                    }
                    if (counter > 24 && counter <= 48)
                    {
                        ionY = numIon;
                        totalY = total;
                        var oneSetOfValues2FromOneMagType = new Tuple<int, double>(ionY, totalY);
                        listOfSetsForOneMagTypeY.Add(oneSetOfValues2FromOneMagType);

                    }
                    if (counter > 48)
                    {
                        ionZ = numIon;
                        totalZ = total;
                        var oneSetOfValues3FromOneMagType = new Tuple<int, double>(ionZ, totalZ);
                        listOfSetsForOneMagTypeZ.Add(oneSetOfValues3FromOneMagType);
                    }
                }
                else if (line.StartsWith("----------------------------"))
                {
                    lineCounter++;

                    if (lineCounter == 6)
                    {
                        valuesDictionary.Add("x", listOfSetsForOneMagTypeX);
                        valuesDictionary.Add("y", listOfSetsForOneMagTypeY);
                        valuesDictionary.Add("z", listOfSetsForOneMagTypeZ);
                        mainDictionary.Add(aValue, valuesDictionary);
                        //PrintData(mainDictionary);
                        VypocetPrumeruA(mainDictionary);
                        valuesDictionary.Clear();
                        mainDictionary.Clear();
                        listOfSetsForOneMagTypeX.Clear();
                        listOfSetsForOneMagTypeY.Clear();
                        listOfSetsForOneMagTypeZ.Clear();

                        counter = 0;
                        lineCounter = 0;
                    }
                }
            }

            //PrintData(mainDictionary);
        }
        static void PrintData(Dictionary<double, Dictionary<string, List<Tuple<int, double>>>> mainDictionary)
        {


          foreach (var a in mainDictionary)
        {
            Console.WriteLine($"a= {a.Key}");
            PrintInnerDictionery(a.Value);

        }

        }

        static void PrintInnerDictionery (Dictionary<string, List<Tuple<int, double>>> innerDictionery)
        {
            foreach(var xyz in innerDictionery)
            {
                Console.WriteLine($"\nmagnetization ({xyz.Key})");
                PrintListOfTuples(xyz.Value);
                
            }
        }

        static void PrintListOfTuples(List<Tuple<int, double>> listOfTuples)
        {
            foreach ( var tuple in listOfTuples)
            {
                Console.WriteLine($"ion: {tuple.Item1}, total: {tuple.Item2}");
            }
        }

        static void VypocetPrumeruA(Dictionary<double, Dictionary<string, List<Tuple<int, double>>>> mainDictionary)
        {
            foreach (var a in mainDictionary)
            {
                Console.WriteLine("a= {0}", a.Key);
                VypocetPrumeruB(a.Value);

            }
        }

        static void VypocetPrumeruB(Dictionary<string, List<Tuple<int, double>>> innerDictionery)
        {
            foreach (var xyz in innerDictionery)
            {
                Console.WriteLine($"magnetization ({xyz.Key})");
                VypocetPrumeruC(xyz.Value);

            }
        }

        static void VypocetPrumeruC(List<Tuple<int, double>> listOfTuples)
        {
            double site1 = 0;
            double site2 = 0;
            double site3 = 0;
            double site4 = 0;
            foreach (var  tuple in listOfTuples)
            {   
                //Site 1: 1, 4, 7, 8
                //Site 2: 2, 3, 5, 6
                //Site 3: 9, 11, 13, 14
                //Site 4: 10, 12, 15, 16
                if (tuple.Item1 == 1 || tuple.Item1 == 4 || tuple.Item1 == 7 || tuple.Item1 == 8)
                {
                    site1 += tuple.Item2;
                }

                if (tuple.Item1 == 2 || tuple.Item1 == 3 || tuple.Item1 == 5 || tuple.Item1 == 6)
                {
                    site2 += tuple.Item2;
                }

                if (tuple.Item1 == 9 || tuple.Item1 == 11 || tuple.Item1 == 13 || tuple.Item1 == 14)
                {
                    site3 += tuple.Item2;
                }

                if (tuple.Item1 == 10 || tuple.Item1 == 12 || tuple.Item1 == 15 || tuple.Item1 == 16)
                {
                    site4 += tuple.Item2;
                }


            }
            site1 = site1 / 4;
            site2 = site2 / 4;
            site3 = site3 / 4;
            site4 = site4 / 4;
            double outputSite1 = Convert.ToDouble(site1.ToString("N3"));
            double outputSite2 = Convert.ToDouble(site2.ToString("N3"));
            double outputSite3 = Convert.ToDouble(site3.ToString("N3"));
            double outputSite4 = Convert.ToDouble(site4.ToString("N3"));

            Console.WriteLine("Site 1:     {0}", outputSite1);
            Console.WriteLine("Site 2:     {0}", outputSite2);
            Console.WriteLine("Site 3:     {0}", outputSite3);
            Console.WriteLine("Site 4:     {0}\n", outputSite4);
        }
    }
}




      
   