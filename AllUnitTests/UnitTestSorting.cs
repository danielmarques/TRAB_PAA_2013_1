using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

//Testes da classe sorting
namespace UnitTests
{
    [TestClass]
    public class UnitTestSorting
    {
        [TestMethod]
        public void TestHeapSort()
        {
            List<Tuple<int,string>> testList = new List<Tuple<int,string>>();
            Random rd = new Random();
            for (int i = 0; i < 1000000; i++)
			{
                testList.Add(new Tuple<int,string>(rd.Next(),i.ToString()));
			}
            Sorting.Sorting<string>.HeapSort(ref testList);

            for (int i = 1; i < testList.Count; i++)
            {
                Assert.IsTrue(testList[i - 1].Item1 <= testList[i].Item1);
            }
        }

        [TestMethod]
        public void TestCountingSort()
        {
            List<Tuple<int, string>> testList = new List<Tuple<int, string>>();
            Random rd = new Random();
            for (int i = 0; i < 1000000; i++)
            {
                testList.Add(new Tuple<int, string>(rd.Next(100000), i.ToString()));
            }
            testList = Sorting.Sorting<string>.CountingSort(testList, 100000);

            for (int i = 1; i < testList.Count; i++)
            {
                Assert.IsTrue(testList[i - 1].Item1 <= testList[i].Item1);
            }
        }
    }
}
