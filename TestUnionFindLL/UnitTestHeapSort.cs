using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class UnitTestHeapSort
    {
        [TestMethod]
        public void SortBasic()
        {
            List<Tuple<int,string>> listteste = new List<Tuple<int,string>>();
            Random rd = new Random();
            for (int i = 0; i < 10000000; i++)
			{
                listteste.Add(new Tuple<int,string>(rd.Next(),i.ToString()));
			}
            Sorting.Sorting<string>.HeapSort(ref listteste);

            for (int i = 1; i < listteste.Count; i++)
            {
                Assert.IsTrue(listteste[i - 1].Item1 <= listteste[i].Item1);
            }
        }
    }
}
