```
Author:     Bingying Wang, Bingkun Han
Partner:    Bingkun Han and Bingying Wang
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  berylchen3, kogasaa
Repo:       https://github.com/uofu-cs3500-spring24/assignment-six-gui-functioning-spreadsheet-bingkunbingying.git
Date:       03-03-2024 
Project:    Spreadsheet
Copyright:  CS 3500 and Bingying Wang and Bingkun Han - This work may not be copied for use in Academic Coursework.
```

# Comments to Spreadsheet:

The Spreadsheet program is done now and we can generate a user-friendly Spreadsheet GUI. Like a GoogleSheet, 
we have the functionalities to type in either a number or a formula into an entry, and after click on Enter key
or click on other cells, we can get a correct calculation result. If we type in an invalid formula or value, 
a window including the error message will be poped up.

# Assignment Specific Topics

    1.Usage of MAUI: Practice using XML to implement MainPage.xaml to accomplish all the functionalities for 
    Spreadsheet.

    2.GUI Design: Learn how to make a better GUI with this class and meet all requirements.

    3.Pair Programming: Learn how to collaborate with peers and work together to make Spreadsheet GUI.
    Pair Programming is one of the most important things to be successfull in coding.

# Best (Team) Practices
     Our team is BingkunBingying team. The memebers are Bingkun Han and BIngying Wang. We have really good cooraperating experiences. 
     There are two examples. The First one is when we wrting code in turn. We can find different problems and list them out. Two 
     viewpoints and find more bugs and improve the software a lot. The second experience is the advantages of specific skills of coding.
     For Bingkun, he is  familar with open and save files. Bingying is good at debugging and create a new feature. we do what we good 
     at and this us a lot of time. and increase the correctness of code.

     the needed improvement in the future is the naming and commenting. IN this pair programming experience, unclear name and complex code
     with no comments create big barrier for another programmer. It takes each a lot of time to try understanding and start add more code. 
     In the future, I think we need usually add comments and come up easy name. then give an detailed explaination before another peer and
     more code on that.


    the needed improvement in the future is the naming and commenting. IN this pair programming experience, unclear name and complex code with no comments create big barrier for 
    another programmer. It takes each a lot of time to try understanding and start add more code. In the future, I think we need usually add comments and come up easy name. then give 
    an detailed explaination before another peer and more code on that.

# Partnership:

Bingkun Han and Bingying Wang collaborate really well. I, Bingying Wang can guarantee that both of us have almost completed
half of the assignment, which is a perfect amount for individuals of pair programming.
We work together basically all methods, especially on generating gird and accomplish the functionalities. Bingying Wang debugged
more and fix some errors like Formula Widget didn't reponse when hit enter. Bingkun Han made more efforts on Save and Open file
methods.


# Branching

We always work together in person, so we only have one branch: main.


# Additional Features and Design Decisions

Under File menu, there are four different options: 
    New: to create a new Spreadsheet, and a warning message will pop up if user has not saved the previous file yet.
    Save: to save the current Spreadsheet, and user will choose a path to store there Spreadsheet.
    Open: to load/open a .sprd file from the system.
    Help: a helo menu to help user quickly understand what does our Spreadsheet have and how to use it.

We also have several features for our Spreadsheet program as this is a self-designed assignment:
      1. A Clear Button: We add a clear button in GUI, and if user clicked on that buttion, all the cells contains any
      contents will be cleared at once. It saves a lot of time for user to refresh the UI and user also doesn't
      have to create a new Spreasheet to have a "cleaned" Spreadsheet.
      2. Selected Cell's Color is different: Once a cell/entry was selected, the user can directly tell the difference
      between other not selected cells. Because the selected cell's background color is gray and other cells' color is
      kind of light purple.
      3. A Big Red 'U' in UI: After generating a Spreadsheet, there is an interesting feature in our grid. A big red 'U'
      exists in grid.


# Examples of Good Software Practice (GSP)

1. Use of helper methods: Almost every method in MainPage.xaml.cs is private, and they are used to help MainPage.xaml.

2. Separation of Concerns: Our project maintains a clear separation of concerns: the Spreadsheet class 
   handles the logic related to cell content and dependency management, while separate classes like Cell
   and DependencyGraph manage individual cell data and the relationships between cells, respectively.  
   This separation allows for easier maintenance and understanding of the system, as each part can be developed,
   tested, and debugged independently.

3. Clear and short method names and variable names: GetCellValue method directly indicates that we are going to
   get the value from the corresponding cell.


# Consulted Peers:

Yanxia Bu and we discussed this assignment. We talked about how to create the grid of a Spreadsheet, and how can we save
and load the file.

# References
  https://github.com/uofu-cs3500-spring24/ForStudents/tree/main/Examples/XMLDemo
  https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.savefiledialog?view=windowsdesktop-8.0

# chatGPT:
   
    use chatGPT to learn about XML documentation and examples.