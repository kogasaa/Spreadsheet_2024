using System.Diagnostics;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FormulaEvaluator
{
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
    /// The project contains a method that evaluates arithmetic 
    /// expressions using standard infix notation. And the evaluator will 
    /// support expressions with variables whose values are looked up via a delegate.
    /// 
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// Delegate that using a string input which is the 
        /// variable name to get the corresponding value
        /// </summary>
        /// <param name="variable_name">The name of the variable</param>
        /// <returns>The corresponding value from the variable</returns>
        public delegate int Lookup(string variable_name);


        /// <summary>
        /// evaluates arithmetic 
        /// expressions using standard infix notation. And the evaluator will 
        /// support expressions with variables whose values are 
        /// looked up via a delegate.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <param name="variableEvaluator">The delegate to look up the variable's value</param>
        /// <returns>The int value to be calculated from the expression</returns>
        /// <exception cref="ArgumentException">when a invalid
        /// expression occurs an exception should be thrown</exception>
        public static int Evaluate(string expression,
                                   Lookup variableEvaluator)
        {
            Stack<int> valueStack = new();
            Stack<string> operatorStack = new();

            string[] tokens = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            foreach (string token in tokens)
            {
                string eachToken = token.Trim();//To get rid of white spaces
                if (string.IsNullOrEmpty(eachToken)) continue;

                //if t is an int
                if (int.TryParse(eachToken, out int number))
                {
                    simpleEvaluation(operatorStack, valueStack, number);
                }

                //if t is a variable
                else if (Regex.IsMatch(eachToken, @"^[a-zA-Z]+\d+$"))
                {
                    //Proceed as above, using the looked-up value of t instead of t
                    int variableValue = variableEvaluator(eachToken);
                    if (variableValue == null)
                        throw new ArgumentException("The variable value is null");
                    simpleEvaluation(operatorStack, valueStack, variableValue);

                }

                //t is + or -
                else if (eachToken == "+" || eachToken == "-")
                {
                    plusMinusEvaluation(operatorStack, valueStack);
                    operatorStack.Push(eachToken);//Push t onto the operator stack
                }

                //t is * or /
                else if (eachToken == "*" || eachToken == "/")
                {
                    operatorStack.Push(eachToken);//Push t onto the operator stack
                }

                //t is a left parenthesis "("
                else if (eachToken == "(")
                {
                    operatorStack.Push(eachToken);//Push t onto the operator stack
                }

                //t is a right parenthesis ")"
                else if (eachToken == ")")
                {
                    plusMinusEvaluation(operatorStack, valueStack);

                    if (operatorStack.Count != 0)
                    {
                        if (operatorStack.Peek() != "(")
                            throw new ArgumentException("A '(' isn't found where expected");
                        operatorStack.Pop();

                        multDivideEvaluation(operatorStack, valueStack);
                    }
                    else
                        throw new ArgumentException("A '(' isn't found where expected");
                }
            }
            //invoke finalEvaluation to calculate the final result
            return finalEvaluation(operatorStack, valueStack);
        }

        /// <summary>
        /// Performs simple evaluation when in Evaluate method the token is an Integer
        /// </summary>
        /// <param name="operatorStack">Stack that contains operators</param>
        /// <param name="valueStack">Stack that contains values</param>
        /// <param name="number">the int value from token</param>
        /// <exception cref="ArgumentException">throw exception when 
        /// the value stack is empty or division by zero occurs</exception>
        private static void simpleEvaluation(Stack<string> operatorStack, Stack<int> valueStack, int number)
        {
            //If * or / is at the top of the operator stack,
            //pop the value stack, pop the operator stack,
            //and apply the popped operator to the popped number and t.
            //Push the result onto the value stack.
            if (operatorStack.Count > 0 &&
                        (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
            {
                if (valueStack.Count == 0)
                    throw new ArgumentException("the value stack is empty");
                int value = valueStack.Pop();
                string oper = operatorStack.Pop();
                if (oper == "*")
                    valueStack.Push(value * number);
                else
                {
                    if (number == 0)
                        throw new ArgumentException("Division by zero occurs.");
                    valueStack.Push(value / number);
                }

            }
            //Otherwise, push t onto the value stack.
            else
                valueStack.Push(number);
        }

        /// <summary>
        /// Perform a sepcific evaluation when in Evaluate method the token is "+", "-" or ")"
        /// </summary>
        /// <param name="operatorStack">Stack that contains operators</param>
        /// <param name="valueStack">Stack that contains values</param>
        /// <exception cref="ArgumentException">throw an exception when 
        /// the value stack contains fewer than 2 values</exception>
        private static void plusMinusEvaluation(Stack<string> operatorStack, Stack<int> valueStack)
        {
            //If + or - is at the top of the operator stack, pop the value stack twice and the
            //operator stack once, then apply the popped operator to the popped numbers,
            //then push the result onto the value stack.
            if (operatorStack.Count > 0 &&
                (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
            {
                if (valueStack.Count < 2)
                    throw new ArgumentException("The value stack contains fewer than 2 values");

                int val1 = valueStack.Pop();
                int val2 = valueStack.Pop();
                string oper = operatorStack.Pop();
                if (oper == "+")
                {
                    valueStack.Push(val1 + val2);
                }
                else
                {
                    valueStack.Push(val2 - val1);
                }
            }
        }

        /// <summary>
        /// Perform a sepcific evaluation when in Evaluate method the token is ")".
        /// </summary>
        /// <param name="operatorStack">Stack that contains operators</param>
        /// <param name="valueStack">Stack that contains values</param>
        /// <exception cref="ArgumentException">throw exception when 
        /// the value stack contains fewer than 2 values or division by zero occurs</exception>
        private static void multDivideEvaluation(Stack<string> operatorStack, Stack<int> valueStack)
        {
            //If * or / is at the top of the operator stack,
            //pop the value stack twice and the operator stack once.
            //Apply the popped operator to the popped numbers.
            //Push the result onto the value stack.
            if (operatorStack.Count > 0 &&
                (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
            {
                if (valueStack.Count < 2)
                    throw new ArgumentException("The value stack contains fewer than 2 values");

                int val1 = valueStack.Pop();
                int val2 = valueStack.Pop();
                string oper = operatorStack.Pop();
                if (oper == "*")
                {
                    valueStack.Push(val1 * val2);
                }
                else
                {
                    if (val1 == 0)
                        throw new ArgumentException("division by zero occurs.");
                    valueStack.Push(val2 / val1);
                }
            }
        }

        /// <summary>
        /// Final evaluation when the last token has been processed.
        /// </summary>
        /// <param name="operatorStack">Stack that contains operators</param>
        /// <param name="valueStack">Stack that contains values</param>
        /// <returns>an int value calculated from the expression</returns>
        /// <exception cref="ArgumentException">throw an exception when
        /// there isn't exactly one value on the value stack</exception>
        private static int finalEvaluation(Stack<string> operatorStack, Stack<int> valueStack)
        {
            //Operator stack is empty
            if (operatorStack.Count == 0)
            {
                //Value stack should contain a single number
                if (valueStack.Count != 1)
                    throw new ArgumentException("There isn't exactly one value on the value stack");

                return valueStack.Pop();
            }
            //Operator stack is not empty
            else
            {
                //There should be exactly one operator on the operator stack,
                //and it should be either + or -. There should be exactly two values
                //on the value stack. Apply the operator to the two values and report
                //the result as the value of the expression.
                if (operatorStack.Count != 1)
                    throw new ArgumentException("There isn't exactly one value on the value stack");
                if (valueStack.Count != 2)
                    throw new ArgumentException("There isn't exactly two numbers on the value stack");

                string oper = operatorStack.Peek();
                int val1 = valueStack.Pop();
                int val2 = valueStack.Pop();
                if (oper == "+")
                    return val1 + val2;
                else if (oper == "-")
                    return val2 - val1;
                else
                    throw new ArgumentException("operator is neither '+' nor '-'");
            }
        }
    }
}
