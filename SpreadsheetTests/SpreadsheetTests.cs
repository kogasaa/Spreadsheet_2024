//using SpreadsheetUtilities;
//using SS;
//using System.Xml;
//using System.Xml.Linq;
///// <summary>
///// Author:    Bingying Wang
///// Partner:   None
///// Date:      02/18/2024
///// Course:    CS 3500, University of Utah, School of Computing
///// Copyright: CS 3500 and Bingying - This work may not 
/////            be copied for use in Academic Coursework.
/////
///// I, Bingying Wang, certify that I wrote this code from scratch and
///// did not copy it in part or whole from another source.  All 
///// references used in the completion of the assignments are cited 
///// in my README file.
/////
///// File Contents
///// This is the test file for Spreadsheet project. And its tests includes
///// all the test cases including edge cases.
///// </summary>
//namespace SpreadsheetTests
//{

//    [TestClass]
//    public class SpreadsheetTests
//    {

//        [TestMethod]
//        public void SetAndGetCellContentsText()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            string cellName = "A1";
//            string content = "Hello World";
//            ss.SetContentsOfCell(cellName, content);
//            Assert.AreEqual(content, ss.GetCellContents(cellName));
//        }

//        [TestMethod]
//        public void SetAndGetCellContentsNumber()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            string cellName = "B2";
//            string content = "123";
//            ss.SetContentsOfCell(cellName, content);
//            Assert.AreEqual(double.Parse(content), ss.GetCellContents(cellName));
//        }

//        [TestMethod]
//        public void SetAndGetCellContentsFormula()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            string cellName = "C3";
//            string content = "=A1+B2";
//            ss.SetContentsOfCell(cellName, content);
//            Formula expectedFormula = new Formula("A1+B2");
//            Assert.AreEqual(expectedFormula, ss.GetCellContents(cellName));
//        }


//        [TestMethod]
//        [ExpectedException(typeof(CircularException))]
//        public void TestCircularDependency()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "=B1");
//            ss.SetContentsOfCell("B1", "=C1");
//            ss.SetContentsOfCell("C1", "=A1"); // This should throw a CircularException
//        }


//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void TestInvalidCellName()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("1A", "Invalid Name"); // Cell names starting with a digit are invalid
//        }

//        [TestMethod]
//        public void CellContentUpdatesDependencies()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "5");
//            ss.SetContentsOfCell("B1", "=A1 * 2");
//            Assert.AreEqual(10.0, ss.GetCellValue("B1")); // Check initial dependency

//            ss.SetContentsOfCell("A1", "10"); // Update A1
//            Assert.AreEqual(20.0, ss.GetCellValue("B1")); // Check updated dependency
//        }


//        [TestMethod]
//        public void GetNonExistentCell()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            Assert.AreEqual("", ss.GetCellContents("A10")); // Cell A10 was never set
//        }


//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void TestGetCellContentsInvalidName()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            ss.GetCellContents("1A"); // Should throw InvalidNameException




//        }//InvalidNameException

//        [TestMethod]
//        public void TestSetCellContentsWithEmptyString()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "Non-empty");
//            ss.SetContentsOfCell("A1", ""); // Clear the cell
//            Assert.AreEqual("", ss.GetCellContents("A1")); // Cell A1 should now be empty
//        }

//        [TestMethod]
//        public void TestSaveAndLoadPreservesFormulas()
//        {
//            string filename = "formulaTest.xml";
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "=B1 * 2");
//            ss.Save(filename);

//            Spreadsheet ssLoaded = new Spreadsheet(filename, s => true, s => s, "default");
//            Assert.AreEqual(new Formula("B1 * 2"), ssLoaded.GetCellContents("A1"));
//        }

//        [TestMethod]
//        public void TestChangedAfterSetContentsOfCell()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            Assert.IsFalse(ss.Changed); // Initially, should not be changed
//            ss.SetContentsOfCell("A1", "Hello World");
//            Assert.IsTrue(ss.Changed); // Should be changed after modification
//        }

//        [TestMethod]
//        public void TestRecalculateCells()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "2");
//            ss.SetContentsOfCell("B1", "=A1 * 3");
//            ss.SetContentsOfCell("A1", "4"); // Change A1, which B1 depends on
//            Assert.AreEqual(12.0, ss.GetCellValue("B1")); // B1 should now be 4 * 3 = 12
//        }

//        [TestMethod]
//        [ExpectedException(typeof(InvalidNameException))]
//        public void SetContentsOfCellInvalidName()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell(null, "Some Content"); // Should throw InvalidNameException
//        }

//        [TestMethod]
//        public void GetValueFromUndefinedCell()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "=B1 + 2");
//            Assert.IsTrue(ss.GetCellValue("A1") is FormulaError); // Since B1 is undefined, A1 should result in a FormulaError
//        }

//        [TestMethod]
//        public void TestEmptySpreadsheetEnumeration()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            //Should be empty for a new spreadsheet
//            Assert.IsFalse(ss.GetNamesOfAllNonemptyCells().Any());
//        }

//        [TestMethod]
//        public void TestCellNameNormalization()
//        {
//            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "default");
//            ss.SetContentsOfCell("a1", "5");
//            Assert.AreEqual(5.0, ss.GetCellContents("A1"));
//            Assert.AreEqual(5.0, ss.GetCellContents("a1")); // Both should refer to the same cell due to normalization
//        }


//        [TestMethod]
//        public void TestDependencyGraphUpdates()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "=B1 + C1");
//            ss.SetContentsOfCell("B1", "1");
//            ss.SetContentsOfCell("C1", "2");

//            // Change formula in A1 to depend on different cells
//            ss.SetContentsOfCell("A1", "=D1 + E1");
//            ss.SetContentsOfCell("D1", "3");
//            ss.SetContentsOfCell("E1", "4");

//            // A1 should now reflect the values of D1 and E1, not B1 and C1
//            Assert.AreEqual(7.0, ss.GetCellValue("A1"));
//        }

//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void TestInvalidFormulaSyntax()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "=2 *"); // Invalid formula syntax
//        }

//        [TestMethod]
//        public void TestSpreadsheetVersioningOnLoad()
//        {
//            string filename = "versionTest.xml";
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "100");
//            ss.Save(filename);

//            Spreadsheet ssLoaded = new Spreadsheet(filename, s => true, s => s, ss.Version); // Load with correct version
//            Assert.AreEqual(100.0, ssLoaded.GetCellContents("A1"));

//            // Attempt to load with incorrect version should throw exception
//            Assert.ThrowsException<SpreadsheetReadWriteException>(() => new Spreadsheet(filename, s => true, s => s, "wrongVersion"));
//        }


//        [TestMethod]
//        public void TestCellValueAfterFormulaEvaluation()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "5");
//            ss.SetContentsOfCell("A2", "=A1 * 2");
//            Assert.AreEqual(10.0, ss.GetCellValue("A2")); // Should evaluate the formula based on A1's value
//        }

//        [TestMethod]
//        public void TestCellRecalculationOrder()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A3", "=A1 + A2");
//            ss.SetContentsOfCell("A1", "5");
//            ss.SetContentsOfCell("A2", "=A1 * 2");
//            ss.SetContentsOfCell("A1", "10"); // This should trigger recalculation of A2 and then A3 in order

//            Assert.AreEqual(20.0, ss.GetCellValue("A2")); // First, A2 should be updated based on new A1 value
//            Assert.AreEqual(30.0, ss.GetCellValue("A3")); // Then, A3 should be updated based on new A2 value
//        }


//        [TestMethod]
//        public void TestSpreadsheetChangedProperty()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            Assert.IsFalse(ss.Changed); // Initially false
//            ss.SetContentsOfCell("A1", "New Value");
//            Assert.IsTrue(ss.Changed); // Should be true after modification
//        }

//        [TestMethod]
//        public void TestRemovalOfCellContent()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "Temporary Data");
//            ss.SetContentsOfCell("A1", ""); // This should remove the cell content
//            Assert.IsFalse(ss.GetNamesOfAllNonemptyCells().Contains("A1")); // A1 should not be listed as a non-empty cell
//        }
//        [TestMethod, Timeout(2000)]
//        public void TestReferenceUpdatesInFormulas()
//        {
//            AbstractSpreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "10");
//            ss.SetContentsOfCell("A2", "=A1 * 2");
//            ss.SetContentsOfCell("A1", "20");
//            Assert.AreEqual(40.0, (double)ss.GetCellValue("A2"), 1e-9, "A2 should update based on A1's new value.");
//        }


//        [TestMethod, Timeout(2000)]
//        [ExpectedException(typeof(CircularException))]
//        public void TestSelfReferencingFormula()
//        {
//            AbstractSpreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "=A1 + 1"); // Self-referencing should throw an exception
//        }

//        [TestMethod, Timeout(2000)]
//        public void TestRecalculateDependentFormulasOnce()
//        {
//            AbstractSpreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "2");
//            ss.SetContentsOfCell("B1", "=A1 * 2");
//            ss.SetContentsOfCell("C1", "=B1 * 2");
//            ss.SetContentsOfCell("A1", "4"); // Update A1
//            Assert.AreEqual(8.0, (double)ss.GetCellValue("B1"), 1e-9);
//            Assert.AreEqual(16.0, (double)ss.GetCellValue("C1"), 1e-9, "C1 should only be recalculated once.");
//        }


//        [TestMethod, Timeout(2000)]
//        public void TestSaveAndReadLargeSpreadsheet()
//        {
//            string filename = "largeSpreadsheet.xml";
//            AbstractSpreadsheet ss = new Spreadsheet();
//            for (int i = 0; i < 100; i++)
//            {
//                ss.SetContentsOfCell("A" + i, i.ToString());
//            }
//            ss.Save(filename);
//            AbstractSpreadsheet ssLoaded = new Spreadsheet(filename, s => true, s => s, "default");
//            for (int i = 0; i < 100; i++)
//            {
//                Assert.AreEqual((double)i, ssLoaded.GetCellValue("A" + i));
//            }
//        }

//        [TestMethod, Timeout(2000)]
//        public void TestSpreadsheetPropertiesAfterLoading()
//        {
//            string filename = "testProperties.xml";
//            AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s, "testVersion");
//            ss.SetContentsOfCell("A1", "Hello World");
//            ss.Save(filename);

//            AbstractSpreadsheet ssLoaded = new Spreadsheet(filename, s => true, s => s, "testVersion");
//            Assert.IsTrue(ssLoaded.Changed, "Spreadsheet should not be marked as changed immediately after loading.");
//            Assert.AreEqual("testVersion", ssLoaded.Version, "Loaded spreadsheet should retain the original version.");
//        }


//        [TestMethod, Timeout(2000)]
//        public void TestClearingCellAfterSetting()
//        {
//            AbstractSpreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "hello");
//            ss.SetContentsOfCell("A1", ""); // Clear the cell
//            Assert.AreEqual("", ss.GetCellContents("A1"), "Cell A1 should be empty after being cleared.");
//        }


//        [TestMethod, Timeout(2000)]
//        [ExpectedException(typeof(CircularException))]
//        public void TestReferenceChainWithCircularDependency()
//        {
//            AbstractSpreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "=B1");
//            ss.SetContentsOfCell("B1", "=C1");
//            ss.SetContentsOfCell("C1", "=A1"); // This should create a circular dependency
//        }


//        [TestMethod, Timeout(2000)]
//        [ExpectedException(typeof(CircularException))]
//        public void TestUpdatingCellCausesIndirectCircularDependency()
//        {
//            AbstractSpreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "=B1");
//            ss.SetContentsOfCell("B1", "5");
//            ss.SetContentsOfCell("C1", "=A1");
//            ss.SetContentsOfCell("B1", "=C1"); // This update should cause a circular dependency
//        }

//        [TestMethod, Timeout(2000)]
//        public void TestFormulaEvaluationWithErrors()
//        {
//            AbstractSpreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "=A2/A3"); // Assume A2 and A3 are not set yet
//            Assert.IsInstanceOfType(ss.GetCellValue("A1"), typeof(FormulaError), "A1 should evaluate to FormulaError due to division by zero.");
//            ss.SetContentsOfCell("A2", "text"); // Invalid operation: text divided by nothing
//            Assert.IsInstanceOfType(ss.GetCellValue("A1"), typeof(FormulaError), "A1 should still be a FormulaError due to invalid operation.");
//        }

//        [TestMethod, Timeout(2000)]
//        public void TestRenamingCellThroughNormalization()
//        {
//            AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "default");
//            ss.SetContentsOfCell("a1", "5");
//            Assert.AreEqual(5.0, ss.GetCellContents("A1"), "Cell name should be normalized to uppercase.");
//        }

//        [TestMethod, Timeout(2000)]
//        public void TestGetValueOfNonexistentCell()
//        {
//            AbstractSpreadsheet ss = new Spreadsheet();
//            Assert.AreEqual("", ss.GetCellValue("A10"), "Getting value of a nonexistent cell should return an empty string.");
//        }


//        [TestMethod, Timeout(2000)]
//        public void TestDependencyTrackingOverMultipleUpdates()
//        {
//            AbstractSpreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "=B1 + C1");
//            ss.SetContentsOfCell("B1", "1");
//            ss.SetContentsOfCell("C1", "2");
//            ss.SetContentsOfCell("B1", "3");
//            ss.SetContentsOfCell("C1", "=B1");
//            Assert.AreEqual(6.0, ss.GetCellValue("A1"), "A1 should update correctly with changes in B1 and C1.");
//        }

//        [TestMethod, Timeout(2000)]
//        [ExpectedException(typeof(SpreadsheetReadWriteException))]
//        public void TestLoadingUnsupportedVersion()
//        {
//            new Spreadsheet("nonexistent.xml", s => true, s => s, "unsupportedVersion"); // Assuming this version does not match
//        }


//        [TestMethod, Timeout(2000)]
//        public void TestGetCellValueValidCell()
//        {
//            AbstractSpreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "5");
//            Assert.AreEqual(5.0, ss.GetCellValue("A1"), "Cell A1 should have a value of 5.");
//        }


//        [TestMethod, Timeout(2000)]
//        public void TestGetCellValueNonexistentCell()
//        {
//            AbstractSpreadsheet ss = new Spreadsheet();
//            Assert.AreEqual("", ss.GetCellValue("B2"), "Nonexistent cell B2 should return an empty string.");
//        }

//        [TestMethod, Timeout(2000)]
//        public void TestGetXmlWithDoubleContent()
//        {
//            // Create a new spreadsheet and set a cell to a double value
//            AbstractSpreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "123.456");
//            ss.SetContentsOfCell("B2", "-78.9");
//            ss.SetContentsOfCell("A2", "ksfnanf");
//            ss.SetContentsOfCell("B3", "=A1+A2");

//            // Get the XML representation of the spreadsheet
//            string xml = ss.GetXML();

//            // Parse the XML to verify the contents
//            XDocument doc = XDocument.Parse(xml);

//            // Find the cell elements in the XML
//            IEnumerable<XElement> cells = doc.Descendants("cell");

//            // Create a dictionary to store the cell names and contents from the XML for easy lookup
//            Dictionary<string, string> cellContents = cells.ToDictionary(
//                cell => cell.Element("name").Value,
//                cell => cell.Element("contents").Value);

//            // Check that the contents match what was set earlier
//            Assert.AreEqual("123.456", cellContents["A1"], "The content of cell A1 should be the double value '123.456'");
//            Assert.AreEqual("-78.9", cellContents["B2"], "The content of cell B2 should be the double value '-78.9'");

//            // Optionally, you can also verify that the XML structure conforms to expected format
//            Assert.IsTrue(cells.All(cell => cell.Element("contents") != null), "Every cell should contain a 'contents' element.");
//        }


//        [TestMethod, Timeout(2000)]
//        [ExpectedException(typeof(SpreadsheetReadWriteException))]
//        public void TestSaveThrowsUnauthorizedAccessException()
//        {
//            // Attempt to save to a directory that typically requires elevated permissions
//            string filename = "/unauthorizedDir/testUnauthorizedAccessException.txt";
//            AbstractSpreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "Test");
//            // This should result in UnauthorizedAccessException
//            ss.Save(filename);
//        }

//        // jiatest 
//        /// <summary>
//        /// testing the value for old value type formula
//        /// </summary>
//        [TestMethod, Timeout(2000)]
//        [ExpectedException(typeof(CircularException))]
//        public void TestTracingBackToOldValueFormula()
//        {
//            Spreadsheet s = new Spreadsheet();
//            s.SetContentsOfCell("F5", "=1");
//            s.SetContentsOfCell("A1", "=F5");
//            s.SetContentsOfCell("A2", "=F5-10");
//            s.SetContentsOfCell("A1", "=A1+A2");
//            s.SetContentsOfCell("A2", "=A2+A1");
//        }
//        /// <summary>
//        /// get the XML
//        /// </summary>
//        [TestMethod]
//        public void GetXML()
//        {
//            AbstractSpreadsheet s = new Spreadsheet(s => true, s => s, "hello world");
//            s.SetContentsOfCell("A1", "a");
//            s.SetContentsOfCell("A2", "6.6");
//            s.SetContentsOfCell("A3", "=A2");
//            AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s, "hello world");
//            s.SetContentsOfCell("A1", "a");
//            s.SetContentsOfCell("A2", "6.6");
//            s.SetContentsOfCell("A3", "=A2");
//            string xml = ss.GetXML();
//            Assert.AreEqual(ss.GetXML(), xml);
//        }
//        [TestMethod, Timeout(2000)]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void TestSCOFExceptions()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            ss.SetContentsOfCell("A1", "string");

//            ss.SetContentsOfCell("A1", "A2");
//            ss.SetContentsOfCell("A1", "=2A");
//            ss.SetContentsOfCell("A1", "");
//            ss.SetContentsOfCell("A1", null);
//        }

//        [TestMethod, Timeout(2000)]
//        public void GetSavedVersion_ValidVersion_ReturnsVersion()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            // Arrange: Create a valid XML string with version info
//            string xmlContent = "<spreadsheet version=\"1.0\"></spreadsheet>";
//            string filename = Path.GetTempFileName();
//            File.WriteAllText(filename, xmlContent);

//            // Act: Retrieve the version from the file
//            string version = ss.GetSavedVersion(filename);

//            // Assert: Check the version is correct
//            Assert.AreEqual("1.0", version);

//            // Cleanup
//            File.Delete(filename);
//        }
//        [TestMethod, Timeout(2000)]
//        [ExpectedException(typeof(SpreadsheetReadWriteException))]
//        public void GetSavedVersion_ValidVersion_ReturnsVersion2()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            // Arrange: Create a valid XML string with version info
//            string xmlContent = "<sheet version=\"1.0\"></sheet>";
//            string filename = Path.GetTempFileName();
//            File.WriteAllText(filename, xmlContent);

//            // Act: Retrieve the version from the file
//            string version = ss.GetSavedVersion(filename);

//            // Assert: Check the version is correct
//            Assert.AreEqual("1.0", version);

//            // Cleanup
//            File.Delete(filename);
//        }

//        [TestMethod, Timeout(2000)]
//        [ExpectedException(typeof(SpreadsheetReadWriteException))]
//        public void GetSavedVersion_NoVersion_ThrowsException()
//        {
//            Spreadsheet ss = new Spreadsheet();
//            // Arrange: Create an XML string without version info
//            string xmlContent = "<spreadsheet></spreadsheet>";
//            string filename = Path.GetTempFileName();
//            File.WriteAllText(filename, xmlContent);

//            string version = ss.GetSavedVersion(filename);

//            // Expect an exception when the version is missing

//            // Cleanup
//            File.Delete(filename);
//        }
//        //[TestMethod, Timeout(2000)]
//        //[ExpectedException(typeof(InvalidNameException))]
//        //public void GetCellValue_invaildName()
//        //{
//        //    Spreadsheet ss = new Spreadsheet();
//        //    ss.GetCellValue("1A");
//        //    ss.GetCellValue("12.0");
//        //}
//    }
//}


/// <summary> 
/// Authors:   Joe Zachary
///            Daniel Kopta
///            Jim de St. Germain
/// Date:      Updated Spring 2022 
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 - This work may not be copied for use 
///                      in Academic Coursework.  See below. 
/// 
/// File Contents 
///
///   This file contains proprietary grading tests for CS 3500.  These tests cases
///   are for individual student use only and MAY NOT BE SHARED.  Do not back them up
///   nor place them in any online repository.  Improper use of these test cases
///   can result in removal from the course and an academic misconduct sanction.
///   
///   These tests are for your private use only to improve the quality of the
///   rest of your assignments
/// </summary>

using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using SpreadsheetUtilities;
using System.Threading;
using System.Xml;

namespace AS5_Grading_Tests
{

    /// <summary>
    ///This is a test class for SpreadsheetTest and is intended
    ///to contain all SpreadsheetTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AS5_Full_Spreadsheet_Grading_Tests
    {

        // Verifies cells and their values, which must alternate.
        public void VV(AbstractSpreadsheet sheet, params object[] constraints)
        {
            for (int i = 0; i < constraints.Length; i += 2)
            {
                if (constraints[i + 1] is double)
                {
                    Assert.AreEqual((double)constraints[i + 1], (double)sheet.GetCellValue((string)constraints[i]), 1e-9);
                }
                else
                {
                    Assert.AreEqual(constraints[i + 1], sheet.GetCellValue((string)constraints[i]));
                }
            }
        }


        // For setting a spreadsheet cell.
        public IEnumerable<string> Set(AbstractSpreadsheet sheet, string name, string contents)
        {
            List<string> result = new List<string>(sheet.SetContentsOfCell(name, contents));
            return result;
        }

        // Tests IsValid
        [TestMethod, Timeout(2000)]
        [TestCategory("1")]
        public void IsValidTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "x");
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("2")]
        [ExpectedException(typeof(InvalidNameException))]
        public void IsValidTest2()
        {
            AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'A', s => s, "");
            ss.SetContentsOfCell("A1", "x");
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("3")]
        public void IsValidTest3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "= A1 + C1");
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("4")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void IsValidTest4()
        {
            AbstractSpreadsheet ss = new Spreadsheet(s => s[0] != 'A', s => s, "");
            ss.SetContentsOfCell("B1", "= A1 + C1");
        }

        // Tests Normalize
        [TestMethod, Timeout(2000)]
        [TestCategory("5")]
        public void NormalizeTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "hello");
            Assert.AreEqual("", s.GetCellContents("b1"));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("6")]
        public void NormalizeTest2()
        {
            AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "");
            ss.SetContentsOfCell("B1", "hello");
            Assert.AreEqual("hello", ss.GetCellContents("b1"));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("7")]
        public void NormalizeTest3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("A1", "6");
            s.SetContentsOfCell("B1", "= a1");
            Assert.AreEqual(5.0, (double)s.GetCellValue("B1"), 1e-9);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("8")]
        public void NormalizeTest4()
        {
            AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "");
            ss.SetContentsOfCell("a1", "5");
            ss.SetContentsOfCell("A1", "6");
            ss.SetContentsOfCell("B1", "= a1");
            Assert.AreEqual(6.0, (double)ss.GetCellValue("B1"), 1e-9);
        }

        // Simple tests
        [TestMethod, Timeout(2000)]
        [TestCategory("9")]
        public void EmptySheet()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            VV(ss, "A1", "");
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("10")]
        public void OneString()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            OneString(ss);
        }

        public void OneString(AbstractSpreadsheet ss)
        {
            Set(ss, "B1", "hello");
            VV(ss, "B1", "hello");
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("11")]
        public void OneNumber()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            OneNumber(ss);
        }

        public void OneNumber(AbstractSpreadsheet ss)
        {
            Set(ss, "C1", "17.5");
            VV(ss, "C1", 17.5);
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("12")]
        public void OneFormula()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            OneFormula(ss);
        }

        public void OneFormula(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "5.2");
            Set(ss, "C1", "= A1+B1");
            VV(ss, "A1", 4.1, "B1", 5.2, "C1", 9.3);
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("13")]
        public void ChangedAfterModify()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Assert.IsFalse(ss.Changed);
            Set(ss, "C1", "17.5");
            Assert.IsTrue(ss.Changed);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("13b")]
        public void UnChangedAfterSave()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Set(ss, "C1", "17.5");
            ss.Save("changed.txt");
            Assert.IsFalse(ss.Changed);
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("14")]
        public void DivisionByZero1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            DivisionByZero1(ss);
        }

        public void DivisionByZero1(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "0.0");
            Set(ss, "C1", "= A1 / B1");
            Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("15")]
        public void DivisionByZero2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            DivisionByZero2(ss);
        }

        public void DivisionByZero2(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "5.0");
            Set(ss, "A3", "= A1 / 0.0");
            Assert.IsInstanceOfType(ss.GetCellValue("A3"), typeof(FormulaError));
        }



        [TestMethod, Timeout(2000)]
        [TestCategory("16")]
        public void EmptyArgument()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            EmptyArgument(ss);
        }

        public void EmptyArgument(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "C1", "= A1 + B1");
            Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("17")]
        public void StringArgument()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            StringArgument(ss);
        }

        public void StringArgument(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "hello");
            Set(ss, "C1", "= A1 + B1");
            Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("18")]
        public void ErrorArgument()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ErrorArgument(ss);
        }

        public void ErrorArgument(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "");
            Set(ss, "C1", "= A1 + B1");
            Set(ss, "D1", "= C1");
            Assert.IsInstanceOfType(ss.GetCellValue("D1"), typeof(FormulaError));
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("19")]
        public void NumberFormula1()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            NumberFormula1(ss);
        }

        public void NumberFormula1(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "C1", "= A1 + 4.2");
            VV(ss, "C1", 8.3);
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("20")]
        public void NumberFormula2()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            NumberFormula2(ss);
        }

        public void NumberFormula2(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "= 4.6");
            VV(ss, "A1", 4.6);
        }


        // Repeats the simple tests all together
        [TestMethod, Timeout(2000)]
        [TestCategory("21")]
        public void RepeatSimpleTests()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Set(ss, "A1", "17.32");
            Set(ss, "B1", "This is a test");
            Set(ss, "C1", "= A1+B1");
            OneString(ss);
            OneNumber(ss);
            OneFormula(ss);
            DivisionByZero1(ss);
            DivisionByZero2(ss);
            StringArgument(ss);
            ErrorArgument(ss);
            NumberFormula1(ss);
            NumberFormula2(ss);
        }

        // Four kinds of formulas
        [TestMethod, Timeout(2000)]
        [TestCategory("22")]
        public void Formulas()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formulas(ss);
        }

        public void Formulas(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "4.4");
            Set(ss, "B1", "2.2");
            Set(ss, "C1", "= A1 + B1");
            Set(ss, "D1", "= A1 - B1");
            Set(ss, "E1", "= A1 * B1");
            Set(ss, "F1", "= A1 / B1");
            VV(ss, "C1", 6.6, "D1", 2.2, "E1", 4.4 * 2.2, "F1", 2.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("23")]
        public void Formulasa()
        {
            Formulas();
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("24")]
        public void Formulasb()
        {
            Formulas();
        }


        // Are multiple spreadsheets supported?
        [TestMethod, Timeout(2000)]
        [TestCategory("25")]
        public void Multiple()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            AbstractSpreadsheet s2 = new Spreadsheet();
            Set(s1, "X1", "hello");
            Set(s2, "X1", "goodbye");
            VV(s1, "X1", "hello");
            VV(s2, "X1", "goodbye");
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("26")]
        public void Multiplea()
        {
            Multiple();
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("27")]
        public void Multipleb()
        {
            Multiple();
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("28")]
        public void Multiplec()
        {
            Multiple();
        }

        // Reading/writing spreadsheets
        [TestMethod, Timeout(2000)]
        [TestCategory("29")]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveToMissingFolderTest()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.Save("\\missing\\saveme.txt");
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("30")]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ReadFromMissingFileTest()
        {
            // should not be able to read 
            AbstractSpreadsheet ss = new Spreadsheet("q:\\missing\\save.txt", s => true, s => s, "");
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("31")]
        public void SaveThenReadTest1()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            Set(s1, "A1", "hello");
            s1.Save("save1.txt");
            s1 = new Spreadsheet("save1.txt", s => true, s => s, "default");
            Assert.AreEqual("hello", s1.GetCellContents("A1"));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("32")]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ReadInvalidXMLFileTest()
        {
            using (StreamWriter writer = new StreamWriter("save2.txt"))
            {
                writer.WriteLine("This");
                writer.WriteLine("is");
                writer.WriteLine("a");
                writer.WriteLine("test!");
            }
            AbstractSpreadsheet ss = new Spreadsheet("save2.txt", s => true, s => s, "");
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("33")]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ReadFileInvalidVersionTest()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            ss.Save("save3.txt");
            ss = new Spreadsheet("save3.txt", s => true, s => s, "version");
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("34")]
        public void VersionSavedCorrectlyTest()
        {
            AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s, "hello");
            ss.Save("save4.txt");
            Assert.AreEqual("hello", new Spreadsheet().GetSavedVersion("save4.txt"));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("35")]
        public void SaveThenReadTest2()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            using (XmlWriter writer = XmlWriter.Create("save5.txt", settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("contents", "hello");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A2");
                writer.WriteElementString("contents", "5.0");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A3");
                writer.WriteElementString("contents", "4.0");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A4");
                writer.WriteElementString("contents", "= A2 + A3");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet ss = new Spreadsheet("save5.txt", s => true, s => s, "");
            VV(ss, "A1", "hello", "A2", 5.0, "A3", 4.0, "A4", 9.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("36")]
        public void SaveThenReadTest3()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Set(ss, "A1", "hello");
            Set(ss, "A2", "5.0");
            Set(ss, "A3", "4.0");
            Set(ss, "A4", "= A2 + A3");
            ss.Save("save6.txt");
            using (XmlReader reader = XmlReader.Create("save6.txt"))
            {
                int spreadsheetCount = 0;
                int cellCount = 0;
                bool A1 = false;
                bool A2 = false;
                bool A3 = false;
                bool A4 = false;
                string name = "UNDEFINED";
                string contents = "UNDEFINED";

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "spreadsheet":
                                Assert.AreEqual("default", reader["version"]);
                                spreadsheetCount++;
                                break;

                            case "cell":
                                cellCount++;
                                break;

                            case "name":
                                reader.Read();
                                name = reader.Value;
                                break;

                            case "contents":
                                reader.Read();
                                contents = reader.Value;
                                break;
                        }
                    }
                    else
                    {
                        switch (reader.Name)
                        {
                            case "cell":
                                if (name.Equals("A1")) { Assert.AreEqual("hello", contents); A1 = true; }
                                else if (name.Equals("A2")) { Assert.AreEqual(5.0, Double.Parse(contents), 1e-9); A2 = true; }
                                else if (name.Equals("A3")) { Assert.AreEqual(4.0, Double.Parse(contents), 1e-9); A3 = true; }
                                else if (name.Equals("A4")) { contents = contents.Replace(" ", ""); Assert.AreEqual("=A2+A3", contents); A4 = true; }
                                else Assert.Fail();
                                break;
                        }
                    }
                }
                Assert.AreEqual(1, spreadsheetCount);
                Assert.AreEqual(4, cellCount);
                Assert.IsTrue(A1);
                Assert.IsTrue(A2);
                Assert.IsTrue(A3);
                Assert.IsTrue(A4);
            }
        }


        // Fun with formulas
        [TestMethod, Timeout(2000)]
        [TestCategory("37")]
        public void Formula1()
        {
            Formula1(new Spreadsheet());
        }
        public void Formula1(AbstractSpreadsheet ss)
        {
            Set(ss, "a1", "= a2 + a3");
            Set(ss, "a2", "= b1 + b2");
            Assert.IsInstanceOfType(ss.GetCellValue("a1"), typeof(FormulaError));
            Assert.IsInstanceOfType(ss.GetCellValue("a2"), typeof(FormulaError));
            Set(ss, "a3", "5.0");
            Set(ss, "b1", "2.0");
            Set(ss, "b2", "3.0");
            VV(ss, "a1", 10.0, "a2", 5.0);
            Set(ss, "b2", "4.0");
            VV(ss, "a1", 11.0, "a2", 6.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("38")]
        public void Formula2()
        {
            Formula2(new Spreadsheet());
        }
        public void Formula2(AbstractSpreadsheet ss)
        {
            Set(ss, "a1", "= a2 + a3");
            Set(ss, "a2", "= a3");
            Set(ss, "a3", "6.0");
            VV(ss, "a1", 12.0, "a2", 6.0, "a3", 6.0);
            Set(ss, "a3", "5.0");
            VV(ss, "a1", 10.0, "a2", 5.0, "a3", 5.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("39")]
        public void Formula3()
        {
            Formula3(new Spreadsheet());
        }
        public void Formula3(AbstractSpreadsheet ss)
        {
            Set(ss, "a1", "= a3 + a5");
            Set(ss, "a2", "= a5 + a4");
            Set(ss, "a3", "= a5");
            Set(ss, "a4", "= a5");
            Set(ss, "a5", "9.0");
            VV(ss, "a1", 18.0);
            VV(ss, "a2", 18.0);
            Set(ss, "a5", "8.0");
            VV(ss, "a1", 16.0);
            VV(ss, "a2", 16.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("40")]
        public void Formula4()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            Formula1(ss);
            Formula2(ss);
            Formula3(ss);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("41")]
        public void Formula4a()
        {
            Formula4();
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("42")]
        public void MediumSheet()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            MediumSheet(ss);
        }

        public void MediumSheet(AbstractSpreadsheet ss)
        {
            Set(ss, "A1", "1.0");
            Set(ss, "A2", "2.0");
            Set(ss, "A3", "3.0");
            Set(ss, "A4", "4.0");
            Set(ss, "B1", "= A1 + A2");
            Set(ss, "B2", "= A3 * A4");
            Set(ss, "C1", "= B1 + B2");
            VV(ss, "A1", 1.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 3.0, "B2", 12.0, "C1", 15.0);
            Set(ss, "A1", "2.0");
            VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 4.0, "B2", 12.0, "C1", 16.0);
            Set(ss, "B1", "= A1 / A2");
            VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("43")]
        public void MediumSheeta()
        {
            MediumSheet();
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("44")]
        public void MediumSave()
        {
            AbstractSpreadsheet ss = new Spreadsheet();
            MediumSheet(ss);
            ss.Save("save7.txt");
            ss = new Spreadsheet("save7.txt", s => true, s => s, "default");
            VV(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("45")]
        public void MediumSavea()
        {
            MediumSave();
        }


        // A long chained formula. Solutions that re-evaluate 
        // cells on every request, rather than after a cell changes,
        // will timeout on this test.
        // This test is repeated to increase its scoring weight
        [TestMethod, Timeout(6000)]
        [TestCategory("46")]
        public void LongFormulaTest()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        [TestMethod, Timeout(6000)]
        [TestCategory("47")]
        public void LongFormulaTest2()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        [TestMethod, Timeout(6000)]
        [TestCategory("48")]
        public void LongFormulaTest3()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        [TestMethod, Timeout(6000)]
        [TestCategory("49")]
        public void LongFormulaTest4()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        [TestMethod, Timeout(6000)]
        [TestCategory("50")]
        public void LongFormulaTest5()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        public void LongFormulaHelper(out object result)
        {
            try
            {
                AbstractSpreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("sum1", "= a1 + a2");
                int i;
                int depth = 100;
                for (i = 1; i <= depth * 2; i += 2)
                {
                    s.SetContentsOfCell("a" + i, "= a" + (i + 2) + " + a" + (i + 3));
                    s.SetContentsOfCell("a" + (i + 1), "= a" + (i + 2) + "+ a" + (i + 3));
                }
                s.SetContentsOfCell("a" + i, "1");
                s.SetContentsOfCell("a" + (i + 1), "1");
                Assert.AreEqual(Math.Pow(2, depth + 1), (double)s.GetCellValue("sum1"), 1.0);
                s.SetContentsOfCell("a" + i, "0");
                Assert.AreEqual(Math.Pow(2, depth), (double)s.GetCellValue("sum1"), 1.0);
                s.SetContentsOfCell("a" + (i + 1), "0");
                Assert.AreEqual(0.0, (double)s.GetCellValue("sum1"), 0.1);
                result = "ok";
            }
            catch (Exception e)
            {
                result = e;
            }
        }

    }
}
