using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Sorting;

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

            Sorting.Heap<string>.HeapSort(ref testList);

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

        [TestMethod]
        public void TestHeapMinEmptySimple()
        {

            var testHeap = new Heap<string>();
            Random rd = new Random();
            int size = 1000000;

            //Heap comeca vazio
            int heapTreeSizeBefore = 0;
            for (int i = 1; i <= size; i++)
            {
                testHeap.HeapAdd(rd.Next(1, int.MaxValue), i.ToString());
                Assert.IsTrue(testHeap.HeapTreeSize() > heapTreeSizeBefore);
                heapTreeSizeBefore = testHeap.HeapTreeSize();
            }

            int minElementBefore = -1;
            heapTreeSizeBefore = size;
            for (int i = 1; i <= size; i++)
            {
                var minElement = testHeap.HeapExtractMin();
                Assert.IsTrue(minElementBefore <= minElement.Item1);
                Assert.IsTrue(testHeap.HeapTreeSize() < heapTreeSizeBefore);
                heapTreeSizeBefore = testHeap.HeapTreeSize();
            }
        }

        [TestMethod]
        public void TestHeapMinFull()
        {
            Random rd = new Random();
            int size = 1000000;

            //Heap comeca cheio
            List<Tuple<int, int, string>> testList = new List<Tuple<int, int, string>>();

            for (int i = 1; i <= size; i++)
            {
                testList.Add(new Tuple<int, int, string>(rd.Next(1, int.MaxValue), i, i.ToString()));
            }

            var testHeap = new Heap<string>(testList);

            for (int i = 1; i < size; i++)
            {
                Assert.IsNotNull(testHeap.HeapGetKey(i));
                testHeap.HeapChangeKey(rd.Next(1, int.MaxValue), i);
            }

            int minElementBefore = -1;
            int heapPositionSizeBefore = size;
            int heapTreeSizeBefore = size;
            for (int i = 1; i < size; i++)
            {
                var minElement = testHeap.HeapExtractMin();
                Assert.IsTrue(minElementBefore <= minElement.Item1);
                Assert.IsTrue(testHeap.HeapPositionSize() < heapPositionSizeBefore);
                heapPositionSizeBefore = testHeap.HeapPositionSize();
                Assert.IsTrue(testHeap.HeapTreeSize() < heapTreeSizeBefore);
                heapTreeSizeBefore = testHeap.HeapTreeSize();
            }
        }

        [TestMethod]
        public void TestHeapMinFullInf()
        {
            Random rd = new Random();
            int size = 1000000;

            //Heap comeca cheio
            List<Tuple<int, int, string>> testList = new List<Tuple<int, int, string>>();

            testList.Add(new Tuple<int, int, string>(0, 1, "1"));
            for (int i = 2; i <= size; i++)
            {
                testList.Add(new Tuple<int, int, string>(int.MaxValue, i, i.ToString()));
            }

            var testHeap = new Heap<string>(testList);

            for (int i = 1; i < size; i++)
            {
                Assert.IsNotNull(testHeap.HeapGetKey(i));
                testHeap.HeapChangeKey(rd.Next(1, int.MaxValue), i);
            }

            int minElementBefore = -1;
            int heapPositionSizeBefore = size;
            int heapTreeSizeBefore = size;
            for (int i = 1; i < size; i++)
            {
                var minElement = testHeap.HeapExtractMin();
                Assert.IsTrue(minElementBefore <= minElement.Item1);
                Assert.IsTrue(testHeap.HeapPositionSize() < heapPositionSizeBefore);
                heapPositionSizeBefore = testHeap.HeapPositionSize();
                Assert.IsTrue(testHeap.HeapTreeSize() < heapTreeSizeBefore);
                heapTreeSizeBefore = testHeap.HeapTreeSize();
            }
        }
    }
}
