using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConsoleApp;
using System.Data;
using System.Collections.Generic;
using System.IO;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        string filePath = "data.csv";

        [TestMethod]
        public void TestReadCsv()
        {
            Program p = new Program();
            DataTable dt = p.ReadCsvIntoDataTable(filePath);
            Assert.IsNotNull(dt);
        }

        [TestMethod]
        public void TestGetList1()
        {
            List<string> expectedList = new List<string>();
            expectedList.Add("Brown, 2");
            expectedList.Add("Clive, 2");
            expectedList.Add("Graham, 2");
            expectedList.Add("Howe, 2");
            expectedList.Add("James, 2");
            expectedList.Add("Owen, 2");
            expectedList.Add("Smith, 2");
            expectedList.Add("Jimmy, 1");
            expectedList.Add("John, 1");

            Program p = new Program();
            DataTable dt = p.ReadCsvIntoDataTable(filePath);
            List<string> list = p.GetList1(dt);

            CollectionAssert.AreEqual(expectedList, list);
        }

        [TestMethod]
        public void TestGetList2()
        {
            List<string> expectedList = new List<string>();
            expectedList.Add("65 Ambling Way");
            expectedList.Add("8 Crimson Rd");
            expectedList.Add("12 Howard St");
            expectedList.Add("102 Long Lane");
            expectedList.Add("94 Roland St");
            expectedList.Add("78 Short Lane");
            expectedList.Add("82 Stewart St");
            expectedList.Add("49 Sutherland St");

            Program p = new Program();
            DataTable dt = p.ReadCsvIntoDataTable(filePath);
            List<string> list = p.GetList2(dt);

            CollectionAssert.AreEqual(expectedList, list);
        }

        [TestMethod]
        public void TestWriteToFile()
        {
            List<string> list = new List<string>();
            list.Add("line 1");
            list.Add("line 2");

            Program p = new Program();
            p.WriteToFile(list, "Test.txt");

            Assert.IsTrue(File.Exists("Test.txt"));
        }        
    }
}
