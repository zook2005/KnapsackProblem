using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KnapsackProblem.Helpers;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem
{
    internal class Program
    {
        public static MyWriter Sw;

        /// <param name="args">args[0] = maximum size, args[1] = path to input file</param>
        private static void Main(string[] args)
        {
            bool random;
            random = false;
            random = true;

            //report = true;
            //report = false;

            List<Item> items;
            uint size;

            string InputFilePath = "";
            string reportPath = "";

            if (!random)
            {

                string sizeString;

                //var sizeString = args[0];
                sizeString = "50";
                size = uint.Parse(sizeString); //todo: try catch parsing

                //InputFilePath = args[1];

                //InputFilePath = PathToInputFileA; size = 126;
                //InputFilePath = PathToInputFileB; size = 630;

                //InputFilePath = PathToInputFileAa; size = 126;
                //InputFilePath = PathToInputFileAb; size = 126;
                //InputFilePath = PathToInputFileAc; size = 126;
                //InputFilePath = PathToInputFileAd; size = 126;
                //InputFilePath = PathToInputFileBa; size = 630;
                //InputFilePath = PathToInputFileCa; size = 1000;
                //InputFilePath = PathToInputFileDa; size = 555;
                //InputFilePath = PathToInputFileEa; size = 727;
                InputFilePath = PathToInputFileFa; size = 359;

                //InputFilePath = PathToInputFileGa; size = 330;

                

                //InputFilePath = BnBPathToInputFileA;  size = 100;
                //InputFilePath = BnBPathToInputFileAa; size = 100;
                //InputFilePath = BnBPathToInputFileAb; size = 100;
                //InputFilePath = BnBPathToInputFileBa; size = 96;
                //InputFilePath = BnBPathToInputFileFa; size = 359;
                
                
                
                //InputFilePath = BnBPathToInputFileB; size = 630;
                //InputFilePath = BnBPathToInputFileBa1; size = 630;



                //InputFilePath = bugPath; size = 400;
                //not relevant here//InputFilePath = buggyPath; size = 400;

                items = IoHelpers.ParseInputFile(InputFilePath).ToList();

            }
            else
            {
                const uint maxvalue = 200;
                const uint itemsMaxSize = 200;
                const uint n = 1500;
                size = 5432;
                items = IoHelpers.CreateItemsRandomly(n, itemsMaxSize, maxvalue).ToList();
                InputFilePath = IoHelpers.CreateInputFile(items, DefaultinputFilesPath, null);
            }

            reportPath = IoHelpers.CreateOutputFile(InputFilePath);
            
           

            var fractional = Knapsacks.GreedyFractionalKnapsack(items.ToList(), size); //using a copy of original list
            Trace.WriteLine("Fractional greedy returned: " + fractional);

            using (Sw = new MyWriter(reportPath))
            {
                //BrancAndBoundPack.Main.EntryPoint(reportPath, items, size, InputFilePath, fractional);
                //LocalSearchPack.Main.EntryPoint(reportPath, items, size, InputFilePath, fractional);
                GeneticPack.Main.EntryPoint(reportPath, items, size, InputFilePath, fractional);
            }
        }



        const string BnBPathToInputFileB = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\BnB\Input B.txt";
        const string BnBPathToInputFileBa1 = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\BnB\Input B a1.txt";
        const string BnBPathToInputFileA = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Bnb\BnbInputA.txt";
        const string BnBPathToInputFileAa = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Bnb\BnbInputAa.txt";
        const string BnBPathToInputFileAb = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Bnb\BnbInputAb.txt";
        const string BnBPathToInputFileBa = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Bnb\BnbInputBa.txt";
        const string BnBPathToInputFileFa = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\BnB\BnbInputFa.txt";

        const string MyInputFileA = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\MyInputA.txt";
        const string PathToInputFileA = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input A.txt";
        const string PathToInputFileB = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input B.txt";

        const string PathToInputFileAa = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input A a.txt";
        const string PathToInputFileAb = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input A b.txt";
        const string PathToInputFileAc = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input A c.txt";
        const string PathToInputFileAd = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input A d.txt";
        const string PathToInputFileBa = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input B a.txt";
        const string PathToInputFileCa = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input C a.txt";
        const string PathToInputFileDa = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input D a.txt";
        const string PathToInputFileEa = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input E a.txt";
        const string PathToInputFileFa = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input F a.txt";
        const string PathToInputFileGa = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Input G a.txt";


        const string bugPath = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Bug\2zovxq35.bke-Shuffled";
        const string DefaultinputFilesPath = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles";
        const string buggyPath = @"C:\Users\rhaba\Dropbox\OpenU\סמסטר 4\Source Code\KnapsackProblem\Tests\InputFiles\Bug\buggy-input-fixed";


    }

}
