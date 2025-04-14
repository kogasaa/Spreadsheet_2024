
using FormulaEvaluator;

/// <summary>
/// Author:    Bingying Wang
/// Partner:   None
/// Date:      01/17/2024
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
/// This is the tester project for FormulaEvaluator.
/// And I mainly test Evaluate method.
///    
/// </summary>

class Test
{
    /// <summary>
    /// method that invoking different tests
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        testVariable();
        testVariableAdd();
        testUnknownVar();
        testAdd();
        testSubtract();
        testMultiply();
        testDivide();
        testComplexExpression();
        testComplexExpression2();
        testInvalidExpression();//divide by zero
        testNegative();
        testIncludeNegative();
        testMultWithAdd();
        testMultWithParentheseAdd();
        testDelegate();

        Console.Read();
    }

    static int simpleLookup(string variable)
    {
        return 0;
    }

    static void testVariable()
    {
        Console.WriteLine("M3 = " + Evaluator.Evaluate("M3", s => 12));
    }

    static void testVariableAdd()
    {
        Console.WriteLine("M3 + 3 = " + Evaluator.Evaluate("M3 + 3", s => 12));
    }

    static void testUnknownVar()
    {
        try
        {
            Console.WriteLine("M3 + 3 = " + Evaluator.Evaluate("M3 + 3", s => { throw new ArgumentException("Unknown variable"); }));
        }
        catch (Exception e)
        {
            Console.WriteLine("Test Invalid Expression: " + e.Message);
        }
    }

    static void testAdd()
    {
        Console.WriteLine("5 + 5 = " + Evaluator.Evaluate("5 + 5", null));
    }

    static void testSubtract()
    {
        Console.WriteLine("5 - 4 = " + Evaluator.Evaluate("5 - 4", null));
    }

    static void testMultiply()
    {
        Console.WriteLine("5 * 3 = " + Evaluator.Evaluate("5 * 3", null));

    }

    static void testDivide()
    {
        Console.WriteLine("20 / 2 = " + Evaluator.Evaluate("20 / 2", null));
    }

    static void testComplexExpression()
    {

        Console.WriteLine("(2 + 3) * 4 - 5 = " + Evaluator.Evaluate("(2 + 3) * 4 - 5", null));
    }

    static void testComplexExpression2()
    {
        Console.WriteLine("4 / (5 - 1) = " + Evaluator.Evaluate("4 / (5 - 1)", null));
    }

    static void testInvalidExpression()
    {
        try
        {
            Console.WriteLine("Invalid Expression: 5 / 0 = " + Evaluator.Evaluate("5 / 0", null));
        }
        catch (Exception e)
        {
            Console.WriteLine("Test Invalid Expression: " + e.Message);
        }
    }

    static void testNegative()
    {
        try
        {
            Console.WriteLine("negative number : -5" + Evaluator.Evaluate("-5", null));
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid Expression: " + e.Message);
        }
    }

    static void testIncludeNegative()
    {
        Console.WriteLine("5 + (0 - 5) / 5 = " + Evaluator.Evaluate("5 + (0 - 5) / 5", null));
    }

    static void testMultWithAdd()
    {
        Console.WriteLine("5 + 4 * 5 = " + Evaluator.Evaluate("5 + 4 * 5", null));
    }

    static void testMultWithParentheseAdd()
    {
        Console.WriteLine("(5 + 4) * 5 = " + Evaluator.Evaluate("(5 + 4) * 5", null));
    }

    static void testDelegate()
    {
        Console.WriteLine("A5 + 5 / 5 = " + Evaluator.Evaluate("A5 + 5 / 5", simpleLookup));
    }
}



