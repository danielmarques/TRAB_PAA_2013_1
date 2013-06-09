using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Graphs;
using Main;
using System.IO;

namespace AllUnitTests
{
    [TestClass]
    public class UnitTestPrim
    {
        [TestMethod]
        public void TestPrim()
        {
            string testFolder = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Tests\");
            var files = Directory.EnumerateFiles(testFolder, "*.in", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var listFromFile = Program.ExportedReadInputFile(file);
                Graph testGraph = new Graph(listFromFile);

                int risk1 = testGraph.Prim(PrimType.PQEdge, 1);

                //Assert.IsTrue(risk1 > 0);

                Console.WriteLine("Arquivo {0} : {1}", file, risk1);
            }        
        }
    }
}
