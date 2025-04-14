using SpreadsheetUtilities;
using System.ComponentModel.DataAnnotations;

namespace FormulaTests
{
    /// <summary>
    /// Author:    Bingying Wang
    /// Partner:   None
    /// Date:      02/04/2024
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
    /// This is the tester project for Formula solution's Formula.cs.
    /// And I test all methods Formula. If you think there is any test cases
    /// I missed, please let me know.
    ///    
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass]
    public class FormulaTests
    {
        [TestMethod]
        public void FormulaConstructorWithValidFormula()
        {
            string formula = "3 + 3";
            Formula f = new Formula(formula);

            Assert.AreEqual(6.0, f.Evaluate(null));
        }

        [TestMethod]
        public void FormulaConstructorWithInvalidFormulaWhiteSpace()
        {
            string formula = "2 + ";

            Assert.ThrowsException<FormulaFormatException>(() => new Formula(formula));
        }

        [TestMethod]
        public void FormulaConstructorValidFormula()
        {
            string formula = "x1 + y2";
            Func<string, string> normalizer = s => s.ToUpper();
            Func<string, bool> validator = s => s.Length == 2;

            Formula f = new Formula(formula, normalizer, validator);

            Assert.IsNotNull(f);
        }

        [TestMethod]
        public void FormulaConstructorWithNormalizationAndValidation_InvalidFormula()
        {
            string formula = "x1 + y2";
            Func<string, string> normalizer = s => s.ToUpper();
            Func<string, bool> validator = s => s.Length == 1;

            Assert.ThrowsException<FormulaFormatException>(() => new Formula(formula, normalizer, validator));
        }

        [TestMethod]
        public void FormulaEvaluate_ValidFormula()
        {
            string formula = "2 + 3";
            Func<string, double> lookup = s =>
            {
                if (s == "X") return 2.0;
                else if (s == "Y") return 3.0;
                else throw new ArgumentException();
            };
            Formula f = new Formula(formula);

            var result = f.Evaluate(lookup);

            Assert.AreEqual(5.0, result);
        }

        [TestMethod]
        public void FormulaEvaluate_DivisionByZero()
        {
            string formula = "1 / 0";
            Func<string, double> lookup = s => 1.0;
            Func<string, string> normalizer = s => s.ToUpper();
            Func<string, bool> validator = s => s.Length == 1;

            Formula f = new Formula(formula, normalizer, validator);

            //Use the Assert.IsInstanceOfType method to check if the result is a FormulaError
            object result = f.Evaluate(lookup);
            Assert.IsInstanceOfType(result, typeof(FormulaError));

            //Use .Reason to get the message
            FormulaError error = (FormulaError)result;
            Assert.AreEqual("There isn't exactly one value on the value stack", error.Reason);
        }

        [TestMethod]
        public void FormulaGetVariables()
        {
            HashSet<string> expect = new HashSet<string>();
            expect.Add("x1");
            expect.Add("Y2");
            expect.Add("z3");

            string formula = "x1 + Y2 + z3";
            Formula f = new Formula(formula);

            // Use CollectionAssert.AreEquivalent to compare the contents of the collections
            CollectionAssert.AreEquivalent(expect.ToList(), f.GetVariables().ToList());
        }

        [TestMethod]
        public void FormulaToString()
        {
            string formula = "x1 + Y2";
            Func<string, string> normalizer = s => s.ToUpper();
            Formula f = new Formula(formula, normalizer, s => true);

            var result = f.ToString();

            Assert.AreEqual("X1+Y2", result);
        }

        [TestMethod]
        public void FormulaEquality()
        {
            string formula1 = "x1 + y2";
            string formula2 = "X1 + Y2";
            Func<string, string> normalizer = s => s.ToUpper();
            Formula f1 = new Formula(formula1, normalizer, s => true);
            Formula f2 = new Formula(formula2, normalizer, s => true);

            Assert.AreEqual(f1, f2);
        }


        [TestMethod]
        public void FormulaConstructor_ValidFormula()
        {
            string validFormula = "2 * (x1 + 3) - 4 / 2";
            Assert.IsNotNull(new Formula(validFormula));
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_InvalidFormulaWithParentheses()
        {
            string invalidFormula = "2 * (x1 + 3 - 4 / 2";
            new Formula(invalidFormula);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_InvalidFormulaWithOperators()
        {
            string invalidFormula = "2 * * (x1 + 3) - 4 / 2";
            new Formula(invalidFormula);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_InvalidFormulaWithVariables()
        {
            string invalidFormula = "2 * (x1 + 3) - 4 / 2$";
            new Formula(invalidFormula);
        }


        [TestMethod]
        public void FormulaEvaluate_InvalidVariable()
        {
            string formula = "x1 + Y2";
            Func<string, double> lookup = s =>
            {
                if (s == "Y2") return 3.0;
                else throw new ArgumentException();
            };
            Formula f = new Formula(formula);
            var result = f.Evaluate(lookup);

            Assert.IsInstanceOfType(result, typeof(FormulaError));
            Assert.AreEqual("No such variable", ((FormulaError)result).Reason);
        }

        [TestMethod]
        public void FormulaEvaluate_InvalidVariableNormalization()
        {
            string formula = "x1 + y2";
            Func<string, double> lookup = s =>
            {
                if (s == "X1") return 2.0;
                else throw new ArgumentException();
            };
            Func<string, string> normalizer = s => s.ToUpper();

            Assert.ThrowsException<FormulaFormatException>(() => new Formula(formula, normalizer, s => s.Length == 1));
        }

        [TestMethod]
        public void FormulaGetHashCode_EqualFormulas()
        {
            string formula1 = "x1 + y2";
            string formula2 = "X1 + Y2";
            Func<string, string> normalizer = s => s.ToUpper();
            Formula f1 = new Formula(formula1, normalizer, s => true);
            Formula f2 = new Formula(formula2, normalizer, s => true);

            var hashCode1 = f1.GetHashCode();
            var hashCode2 = f2.GetHashCode();

            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void FormulaGetHashCode_DifferentFormulas()
        {
            string formula1 = "x1 + y2";
            string formula2 = "x3 - z4";
            Func<string, string> normalizer = s => s.ToUpper();
            Formula f1 = new Formula(formula1, normalizer, s => true);
            Formula f2 = new Formula(formula2, normalizer, s => true);

            var hashCode1 = f1.GetHashCode();
            var hashCode2 = f2.GetHashCode();

            Assert.AreNotEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void FormulaEquals_NullObject()
        {
            string formula = "x1 + y2";
            Func<string, string> normalizer = s => s.ToUpper();
            Formula f = new Formula(formula, normalizer, s => true);

            Assert.IsFalse(f.Equals(null));
        }

        [TestMethod]
        public void FormulaNotEqualOperator()
        {
            string formula1 = "x1 + y2";
            string formula2 = "x3 - z4";
            Func<string, string> normalizer = s => s.ToUpper();
            Formula f1 = new Formula(formula1, normalizer, s => true);
            Formula f2 = new Formula(formula2, normalizer, s => true);

            Assert.IsTrue(f1 != f2);
        }


        [TestMethod]
        public void FormulaConstructor_SyntaxChecks_EmptyFormula()
        {
            string emptyFormula = "";
            Func<string, string> normalizer = s => s.ToUpper();
            Func<string, bool> validator = s => s.Length == 1;

            Assert.ThrowsException<FormulaFormatException>(() => new Formula(emptyFormula, normalizer, validator));
        }

        [TestMethod]
        public void FormulaConstructor_SyntaxChecks_InvalidOperatorPlacement()
        {
            string invalidFormula = "2 * + 3";
            Func<string, string> normalizer = s => s.ToUpper();
            Func<string, bool> validator = s => s.Length == 1;

            Assert.ThrowsException<FormulaFormatException>(() => new Formula(invalidFormula, normalizer, validator));
        }

        [TestMethod]
        public void FormulaConstructor_SyntaxChecks_UnbalancedParentheses()
        {
            string invalidFormula = "(x1 + 3) + (4 / 2";
            Func<string, string> normalizer = s => s.ToUpper();
            Func<string, bool> validator = s => s.Length == 1;

            Assert.ThrowsException<FormulaFormatException>(() => new Formula(invalidFormula, normalizer, validator));
        }

        [TestMethod]
        public void FormulaConstructor_SyntaxChecks_InvalidVariableNormalization()
        {
            string invalidFormula = "x1 + y2";
            Func<string, double> lookup = s =>
            {
                if (s == "X1") return 2.0;
                else throw new ArgumentException();
            };
            Func<string, string> normalizer = s => s.ToUpper();

            Assert.ThrowsException<FormulaFormatException>(() => new Formula(invalidFormula, normalizer, s => s.Length == 1));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormulaConstructor_NullFormula()
        {
            string formula = null;
            Func<string, string> normalizer = s => s.ToUpper();
            Func<string, bool> validator = s => s.Length == 1;

            Formula f = new Formula(formula, normalizer, validator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormulaConstructor_NullNormalizer()
        {
            string formula = "x1 + y2";
            Func<string, string> normalizer = null;
            Func<string, bool> validator = s => s.Length == 1;

            Formula f = new Formula(formula, normalizer, validator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormulaConstructor_NullValidator()
        {

            string formula = "x1 + y2";
            Func<string, string> normalizer = s => s.ToUpper();
            Func<string, bool> validator = null;

            Formula f = new Formula(formula, normalizer, validator);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaConstructor_InvalidFormulaSyntax()
        {
            string invalidFormula = "x1 + +";
            Func<string, string> normalizer = s => s.ToUpper();
            Func<string, bool> validator = s => s.Length == 1;

            Formula f = new Formula(invalidFormula, normalizer, validator);
        }


        [TestMethod]
        public void FormulaEquality_SameFormulas()
        {
            string formula = "2 * (x1 + 3) - 4 / 2";
            Formula f1 = new Formula(formula);
            Formula f2 = new Formula(formula);

            Assert.IsTrue(f1 == f2);
        }

        [TestMethod]
        public void FormulaEquality_DifferentFormulas()
        {
            Formula f1 = new Formula("2 * (x1 + 3) - 4 / 2");
            Formula f2 = new Formula("x1 + 3 - 4 / 2 * 2");

            Assert.IsFalse(f1 == f2);
        }


        [TestMethod]
        public void FormulaEquality_OneNullFormula()
        {
            Formula f1 = new Formula("2 * (x1 + 3) - 4 / 2");
            Formula f2 = null;

            Assert.IsFalse(f1 == f2);
        }

        [TestMethod]
        public void FormulaEquality_EqualFormulasWithValidators()
        {
            Func<string, bool> validator = s => s.Length == 2;
            Formula f1 = new Formula("X1 + Y2", s => s.ToUpper(), validator);
            Formula f2 = new Formula("X1 + Y2", s => s.ToUpper(), validator);

            Assert.IsTrue(f1 == f2);
        }


        [TestMethod]
        public void FormulaEvaluate_SimpleAddition()
        {
            string formula = "2 + 3";
            Func<string, double> lookup = s => 0;
            Formula f = new Formula(formula);

            double result = (double)f.Evaluate(lookup);

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void FormulaEvaluate_SimpleSubtraction()
        {
            string formula = "5 - 3";
            Func<string, double> lookup = s => 0;
            Formula f = new Formula(formula);

            double result = (double)f.Evaluate(lookup);

            Assert.AreEqual(2, result);
        }

        

        [TestMethod]
        public void FormulaEvaluate_SimpleMultiplication()
        {
            string formula = "2 * 3";
            Func<string, double> lookup = s => 0;
            Formula f = new Formula(formula);

            double result = (double)f.Evaluate(lookup);

            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void FormulaEvaluate_SimpleDivision()
        {
            string formula = "6 / 2";
            Func<string, double> lookup = s => 0;
            Formula f = new Formula(formula);

            double result = (double)f.Evaluate(lookup);

            Assert.AreEqual(3, result);
        }


        [TestMethod]
        public void FormulaEvaluate_Variables()
        {
            string formula = "x1 + y2";
            Func<string, double> lookup = s =>
            {
                if (s == "x1") return 2.0;
                if (s == "y2") return 3.0;
                return 0;
            };
            Formula f = new Formula(formula);

            double result = (double)f.Evaluate(lookup);

            Assert.AreEqual(5, result);
        }


    }
}
