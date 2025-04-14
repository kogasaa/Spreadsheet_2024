```
Author:     Bingying Wang
Partner:    None
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  berylchen3
Repo:       https://github.com/uofu-cs3500-spring24/spreadsheet-berylchen3 
Date:       19-01-2024 
Project:    Formula Evaluator
Copyright:  CS 3500 and Bingying Wang - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:

An FormulaEvaluator project evaluates arithmetic expressions using standard infix notation. 
And the evaluator will support expressions with variables whose values are looked up 
via a delegate. 

Through the use of Stack and delegate, I have been able to identify variables and 
obtain numerical values as well as operational expressions. What's more edge 
cases are handled and corresponding ArgumentException will be thrown. 

The project is flexible and clear.

# Assignment Specific Topics

    1.Usage of Delegate: For this project, we use delegate to take a string variable name
    to get the corresponding int value. In an arithmetic expression it may have some variables
    for program to recognize. And delegate will lookup the variable in the expression.

    2.Stack-based Algorithm: In Evaluate function, we have two Stacks and one of them is 
    storing operators and the other one is storing values from the expression. Based on
    this data structure we ensure the accuracy to calculate arithmetic expression.

    3.Error Resolution: When processing the algorithm, we will meet different kinds of
    erros (i.e. a division by zero occurs or the valueStack is empty). ArgumentException will
    be thrown based on different situations.


# Consulted Peers:

No peers for Assignment one. Because I just figured out all the codes by myself, and the instruction
for algorithm is pretty clear. I know some of students in this class, we said hi to each other
but just didn't talk about this assignment. And it turns out they also figured out the
assignment without discussing with me.

# References:

    1. Regular Expression Language - Quick Reference
       - https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference

# Use of ChatGPT:

    line 67: else if (Regex.IsMatch(eachToken, @"^[a-zA-Z]+\d+$"))
    I don't know how to construct regular expression, so I used ChatGPT to help me better
    organize a regular expression.