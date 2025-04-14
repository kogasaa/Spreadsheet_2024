```
Author:     Bingying Wang
Partner:    None
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  berylchen3
Repo:       https://github.com/uofu-cs3500-spring24/spreadsheet-berylchen3 
Date:       02-04-2024 
Project:    Formula
Copyright:  CS 3500 and Bingying Wang - This work may not be copied for use in Academic Coursework.
```

# Comments to Formula:

For this assignment I am creating a Formula class which is a more generalized version of your 
FormulaEvaluator work. This formula class will be utilized in the upcoming spreadsheet assignment.
Formula.cs is much longer than any other previous projects.

The Formula.cs file contains two classes and a struct:
class Formula: a partially implemented class called Formula.cs. 
I copied over and modified my Assignment One code to generalize your infix expression evaluator. 
class FormulaFormatException: Used to report syntactic errors in a formula
struct FormulaError: A return value from the the formula evaluator (i.e., when the formula is bad).



# Assignment Specific Topics

    1.Usage of private helper methods: For this project, I used a lot of private helper methods
    to make my project easier to implement. I have private helper methods to determine if an input
    formula is valid based on a certian syntax format.

    2.Usage of throw exception in constructor: We don't have to throw exception everytime when
    calculating the input formula. We can directly check the user input at beginning in my second
    constructor.

    3.Usage of instruct: We have something differeing with class for Formula project. And
    there is an instruct FormulaError. And in our Evaluate method we are not goning to throw and 
    exceptions but return instuct (because instuct can be considered as a value type).


# Consulted Peers:

No peers for Assignment two. Because I just figured out all the codes by myself, and the instruction
for algorithm is pretty clear. I know some of students in this class, we said hi to each other
but just didn't talk about this assignment. And it turns out they also figured out the
assignment without discussing with me.

# Use of ChatGPT:

    line 233:  return Regex.IsMatch(variable, @"^[a-zA-Z_][a-zA-Z0-9_]*$") && validator(variable);
    I don't know how to construct regular expression, so I used ChatGPT to help me better
    organize a regular expression.

    GetType()
    Asked ChatGPT to give me a method to get the type of object.

    line 570 - line 574: 
    // Use regular expression to normalize variable names (case-insensitive)
            string normalizedFormula = Regex.Replace(formula, @"[a-zA-Z_][a-zA-Z0-9_]*", match =>
            {
                return match.Value.ToUpperInvariant(); // Normalize variable names to uppercase
            });
    Used ChatGPT to have a way to convert variable names into upercase.

    line 583 - line 588:
    // Use regular expression to match and round numbers consistently
            normalizedFormula = Regex.Replace(normalizedFormula, @"(\d+(\.\d*)?([eE][+-]?\d+)?|\.\d+)", match =>
            {
                double value = double.Parse(match.Value);
                return value.ToString("R"); // Use "R" format specifier to round consistently
            });
    Used ChatGPT to have a way to round-up a double values consistently.

    line 590 - line 591:
    // Remove whitespace and normalize
            normalizedFormula = Regex.Replace(normalizedFormula, @"\s", "");
            });
    Used ChatGPT to have a way to remove white spaecs from the string.

