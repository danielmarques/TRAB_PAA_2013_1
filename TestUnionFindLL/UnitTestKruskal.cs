using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Graphs;
using Main;
using System.IO;

//Testes do método Kuskal
namespace UnitTests
{
    [TestClass]
    public class UnitTestKruskal
    {
        [TestMethod]
        public void TestKruskal()
        {

            string testFolder = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Tests\");
            var files = Directory.EnumerateFiles(testFolder, "*.in", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var listFromFile = Program.ExportedReadInputFile(file);
                Graph testGraph = new Graph(listFromFile);

                int risk1 = testGraph.Kruskal(KruskalType.LinkedListUFCountingSort);
                int risk2 = testGraph.Kruskal(KruskalType.LinkedListUFHeapSort);
                int risk3 = testGraph.Kruskal(KruskalType.TreeUFCountingSort);
                int risk4 = testGraph.Kruskal(KruskalType.TreeUFHeapSort);

                Assert.IsTrue((risk1 == risk2) && (risk1 == risk3) && (risk1 == risk4));

                Console.WriteLine("Arquivo {0} : {1}", file, risk1);
            }           
        }
    }
}
