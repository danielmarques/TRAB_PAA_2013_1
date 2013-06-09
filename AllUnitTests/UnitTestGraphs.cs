using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Graphs;
using Main;

namespace UnitTests
{
    [TestClass]
    public class UnitTestGraphs
    {
        [TestMethod]
        public void TestGraphAdjList()
        {
            string testFolder = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Tests\");
            var files = Directory.EnumerateFiles(testFolder, "*.in", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var listFromFile = Program.ExportedReadInputFile(file);
                Graph testGraph = new Graph(listFromFile);

                Console.WriteLine("Arquivo {0}", file);
            } 
        }

        [TestMethod]
        public void TestRandEdges()
        {
            string testFolder = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Tests\");
            var files = Directory.EnumerateFiles(testFolder, "*.in", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var listFromFile = Program.ExportedReadInputFile(file);
                Graph testGraph = new Graph(listFromFile);
                Graph testGraphRand = new Graph(listFromFile, 10);
                Graph testGraphRand2 = new Graph(listFromFile, 50);
                Console.WriteLine("Arquivo {0}", file);
            } 
        }
    }
}
