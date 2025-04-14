```
Author:     Bingying Wang
Partner:    None
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  berylchen3
Repo:       https://github.com/uofu-cs3500-spring24/spreadsheet-berylchen3 
Date:       25-01-2024 
Project:    Dependency Graph
Copyright:  CS 3500 and Bingying Wang - This work may not be copied for use in Academic Coursework.
```

# Comments to Dependency Graphs:

A DependencyGraph in the context of a spreadsheet application is used to manage and track the 
relationships between different variables. We know the value of one cell might depends on the values of others.
This is common in spreadsheets where you have formulas. For instance, if Cell A1 has a formula that 
sums A2 and A3 (like = A1 + A2), A1 is dependent on A2 and A3. And we need to figure out A2 and A3's value
first, and then A1's.

The project is flexible and clear, and I have added enough comments.

# Assignment Specific Topics

    1.Usage of Dictionary: For this project, I use two Dictionary as the basic data structure
    to store dependents and dependees. Because we want to make sure a constant time when getting
    a value from a secific key.

    2.Usage of HashSet: For the value type of Dictionary, I choose to use a HashSet<string>. Because
    a HashSet<string> can successfully avoid duplicates when adding the same values into our dictionaries.

    3.Error Resolution: When trying to get the value based on the given key in our dictionary, we need
    to use .ContainsKey() first to make sure the key does exist. Otherwise, an exception might be thrown.


# Consulted Peers:

No peers for Assignment two. Because I just figured out all the codes by myself, and the instruction
for algorithm is pretty clear. I know some of students in this class, we said hi to each other
but just didn't talk about this assignment. And it turns out they also figured out the
assignment without discussing with me.