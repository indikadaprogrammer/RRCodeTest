using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRCodeTestAutomatedTests
{
    public class UnitTestHelper
    {

        #region "constants"

        //When execute MSTest from command prompt, it builds and executes from  bin\Debug\TestResults\<hostname>\Out folder.
        //To allow smooth execution from both command prompt and run tests by right clicking in visual studio, build path 
        //for test project has been changed to bin\Debug\RightClickRun\HostName\Out
        public static string TestInputsFolder = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../TestInputs/");
        public static string TestOutputsFolder = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../TestOutputs/");

        #endregion

        #region "Helper Methods"

        public static void CopyFile(string inputFolder, string outputFolder, string fileName, bool overwrite)
        {
            Console.WriteLine("Copy file {0} from {1} to {2}", fileName, inputFolder, outputFolder);
            File.Copy(Path.Combine(inputFolder, fileName), Path.Combine(outputFolder, fileName), overwrite);
        }


        #endregion

    }
}
