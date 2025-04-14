
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
namespace DevelopmentTests
{
    /// <summary>
    /// Author:    Bingying Wang
    /// Partner:   None
    /// Date:      01/25/2024
    /// Course:    CS 3500, University of Utah, School of Computing
    /// Copyright: CS 3500 and Bingying - This work may not 
    ///            be copied for use in Academic Coursework.
    ///
    /// I, Bingying Wang, certify that I wrote this code from scratch and
    /// did not copy it in part or whole from another source.  All 
    /// references used in the completion of the assignments are cited 
    /// in my README file.
    ///
    /// File Contents
    ///
    /// This is the tester project for DependencyGraph.
    /// And I test all methods inDependencyGraph.
    ///    
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }
        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }
        ///<summary>
        ///It should be possibe to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void EnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void ReplaceThenEnumerate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }
        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();
            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }
            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }
            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new
                HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new
                HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

        //Student tests

        /// <summary>
        /// test this[string s]
        /// </summary>
        [TestMethod()]
        public void TestDependeeSizeSmall()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("1", "0");
            t.AddDependency("1", "2");
            t.AddDependency("0", "2");
            Assert.AreEqual(2, t["2"]);
        }

        /// <summary>
        /// test dependees' size with 0
        /// </summary>
        [TestMethod()]
        public void TestDependeeSizeZero()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("1", "0");
            t.AddDependency("1", "2");
            t.AddDependency("0", "2");
            Assert.AreEqual(0, t["3"]);
        }

        /// <summary>
        /// test dependees' large size
        /// </summary>
        [TestMethod()]
        public void TestDependeeSizeLarge()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("1", "0");
            t.AddDependency("2", "0");
            t.AddDependency("3", "0");
            t.AddDependency("4", "0");
            t.AddDependency("5", "0");
            t.AddDependency("6", "0");
            t.AddDependency("7", "0");
            t.AddDependency("8", "0");
            t.AddDependency("9", "0");
            t.AddDependency("10", "0");
            t.AddDependency("11", "0");
            t.AddDependency("12", "0");
            t.AddDependency("13", "0");
            t.AddDependency("14", "0");
            t.AddDependency("15", "0");
            t.AddDependency("16", "0");
            Assert.AreEqual(16, t["0"]);
        }
        /// <summary>
        /// test HasDependents returning true
        /// </summary>
        [TestMethod()]
        public void TestHasDependentsTrue()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("1", "0");
            t.AddDependency("1", "2");
            t.AddDependency("0", "2");
            Assert.IsTrue(t.HasDependents("1"));
        }
        /// <summary>
        /// test HasDependents returning false
        /// </summary>
        [TestMethod()]
        public void TestHasDependentsFalse()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("1", "0");
            t.AddDependency("1", "2");
            t.AddDependency("0", "2");
            Assert.IsFalse(t.HasDependents("2"));
        }
        /// <summary>
        /// test HasDependenees returing true
        /// </summary>
        [TestMethod()]
        public void TestHasDependeesTrue()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("1", "0");
            t.AddDependency("1", "2");
            t.AddDependency("0", "2");
            Assert.IsTrue(t.HasDependees("2"));
        }
        /// <summary>
        /// test HasDependenees returning false
        /// </summary>
        [TestMethod()]
        public void TestHasDependeesFalse()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("1", "0");
            t.AddDependency("1", "2");
            t.AddDependency("0", "2");
            Assert.IsFalse(t.HasDependees("d"));
        }
        /// <summary>
        /// test HasDependees returing false even if the key exists
        /// </summary>
        [TestMethod()]
        public void TestHasDependeesContainsKeyFalse()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("1", "0");
            t.AddDependency("1", "2");
            t.AddDependency("0", "2");
            t.AddDependency("", "3");
            t.ReplaceDependees("3", new List<string>());
            Assert.IsFalse(t.HasDependees("3"));
        }
        /// <summary>
        /// test HasDependents returing false even if the key exists
        /// </summary>
        [TestMethod()]
        public void TestHasDependentsContainsKeyFalse()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("1", "0");
            t.AddDependency("1", "2");
            t.AddDependency("0", "2");
            t.AddDependency("3", "3");
            t.ReplaceDependents("3", new List<string>());
            Assert.IsFalse(t.HasDependents("3"));
        }
        /// <summary>
        /// test one dependency exists in DependencyGraph
        /// </summary>
        [TestMethod]
        public void RemoveDependencyOneExist()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.RemoveDependency("c", "d");

            Assert.AreEqual(2, t.Size);

            t.RemoveDependency("a", "b");

            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        /// test no dependency exists in DependencyGraph
        /// </summary>
        [TestMethod]
        public void RemoveDependencyNoExist()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.RemoveDependency("a", "b");
            t.RemoveDependency("a", "c");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        /// test adding duplicates with AddDependency
        /// </summary>
        [TestMethod]
        public void AddDependencyAddDuplicates()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            Assert.AreEqual(2, t.Size);
        }

        /// <summary>
        /// test adding one pair with AddDependency
        /// </summary>
        [TestMethod]
        public void AddDependencyAddOnePair()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            Assert.AreEqual(1, t.Size);
        }


        /// <summary>
        /// test replace one pair with ReplaceDependents
        /// </summary>
        [TestMethod]
        public void ReplaceDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            List<string> list = new List<string>();
            list.Add("c");
            list.Add("d");
            t.ReplaceDependents("a", list);
            Assert.AreEqual(2, t.Size);
        }


        /// <summary>
        /// test replace one pair with ReplaceDependees
        /// </summary>
        [TestMethod]
        public void ReplaceDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            List<string> list = new List<string>();
            list.Add("c");
            list.Add("d");
            t.ReplaceDependees("b", list);
            Assert.AreEqual(2, t.Size);
        }
    }
}
