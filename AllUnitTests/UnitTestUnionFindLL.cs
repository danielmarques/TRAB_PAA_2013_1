﻿using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnionFind;

//Testes da classe Union FindLL
namespace UnitTests
{
    [TestClass]
    public class UnitTestUnionFindLL
    {
        [TestMethod]
        public void TestFindLLSimple()
        {
            int length = 10;
            IUnionFind uf = new UnionFindLL(length);

            for (int i = 1; i < length; i++)
            {
                Assert.AreEqual(i, uf.Find(i));
            }
        }

        [TestMethod]
        public void TestUnionLL()
        {
            int length = 10;
            IUnionFind uf = new UnionFindLL(length);

            uf.Union(1, 2);

            Assert.AreEqual(uf.Find(1), uf.Find(2));

            for (int i = 3; i < length; i++)
            {
                Assert.AreNotEqual(uf.Find(1), uf.Find(i));
                Assert.AreNotEqual(uf.Find(2), uf.Find(i));
            }

            uf.Union(3, 4);

            Assert.AreEqual(uf.Find(3), uf.Find(4));

            int set34 = uf.Find(3);

            for (int i = 5; i < length; i++)
            {
                Assert.AreNotEqual(uf.Find(3), uf.Find(i));
                Assert.AreNotEqual(uf.Find(4), uf.Find(i));
            }

            int set5 = uf.Find(5);

            uf.Union(uf.Find(3), uf.Find(5));

            Assert.AreEqual(uf.Find(3), uf.Find(5));
            Assert.AreEqual(uf.Find(4), uf.Find(5));

            Assert.AreEqual(set34, uf.Find(3));
            Assert.AreEqual(set34, uf.Find(4));
            Assert.AreEqual(set34, uf.Find(5));

            for (int i = 6; i < length; i++)
            {
                Assert.AreNotEqual(uf.Find(3), uf.Find(i));
                Assert.AreNotEqual(uf.Find(4), uf.Find(i));
                Assert.AreNotEqual(uf.Find(5), uf.Find(i));
            }

            for (int i = 6; i < length; i++)
            {
                Assert.AreEqual(i, uf.Find(i));
            }

            uf.Union(uf.Find(1), uf.Find(3));
            uf.Union(uf.Find(2), uf.Find(6));
            uf.Union(uf.Find(3), uf.Find(7));
            uf.Union(uf.Find(5), uf.Find(8));
            uf.Union(uf.Find(8), uf.Find(9));
            uf.Union(uf.Find(1), uf.Find(10));

            for (int i = 2; i < length; i++)
            {
                Assert.AreEqual(uf.Find(i - 1), uf.Find(i));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestUnionLLExceptions()
        {
            int length = 10;
            IUnionFind uf = new UnionFindLL(length);

            uf.Union(1, 2);
            uf.Union(2, 3);
            //uf.Union(3, 2);

        }
    }

}