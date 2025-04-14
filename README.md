```
Author:     Bingying Wang and Bingkun Han
Partner:    Bingkun Han and Bingying Wang
Start Date: 16-Jan-2024
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  berylchen3    kogasaa
Repo:       https://github.com/uofu-cs3500-spring24/assignment-six-gui-functioning-spreadsheet-bingkunbingying.git
Commit Date: 03-03-2024
Solution:   Spreadsheet
Copyright:  CS 3500 and Bingying Wang and Bingkun Han - This work may not be copied for use in Academic Coursework.
```

# Overview of the Spreadsheet functionality

The Spreadsheet program is done now and we can generate a user-friendly Spreadsheet GUI. Like a GoogleSheet, 
we have the functionalities to type in either a number or a formula into an entry, and after click on Enter key
or click on other cells, we can get a correct calculation result. If we type in an invalid formula or value, 
a window including the error message will be poped up.

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

We used previous project: Spreadsheet, to accomplish the functionalities to get the value from variables and calculate
a formula as a cell's content.
We believe our Spreadsheet is well designed and user-friendly. If you have any suggestions, please let us know!



# Time Expenditures:

    Hours Estimated/Worked         Assignment                       Note
          20    /   11    - Assignment 1 - Formula Evaluator     Spent 1 hour on README.md, otherwise good.
          16    /   10    - Assignment 2 - Dependency Graph      Spent 2 hours on AddDependency method, 
                                                                 and 3 hours on writing and testing tests
          20    /   27    - Assignment 3 - Formula               Spent 8 hours to figure out how to throw 
                                                                 exception in the second constructor.
                                                                 Spent 5 hours on writing and debugging tests.
                                                                 Spent 1.5 hours on README.md.
          20    /   16    - Assignment 4 - Spreadsheet           Spent 5 hours to figure out how to throw 
                                                                 CircularException. 1 hour to write tests.
          30    /   29    - Assignment 5 - Spreadsheet           Spent 15 hours to figure out how to write XML.
                                                                 2 hours to write tests.
          24    /   50    - Assignment 6 - Spreadsheet           Spent around 30 hours to figure out how to create gird
                            Front-End Graphical User Interface   and how to add functionalities to each cell. Spent
                                                                 6 hours to fix different bugs. Spent 4 hours to add
                                                                 features: the background color will be changed
                                                                 if the cell is selected, and add a clear button to
                                                                 clean all cells' contents.

   conclusion: In reflecting on our recent project, we have observed that our time estimation skills require further refinement.
   Initially, we provided a time estimate for the completion of our assignment, which, in hindsight, turned out to be significantly
   underestimated. The actual time it took to complete the work far exceeded our initial expectations. 
   This discrepancy has been a valuable learning experience, teaching us the complexity and unpredictability inherent in 
   software development tasks.
          

# Partnership:
Bingkun Han and Bingying Wang collaborate really well. I, Bingying Wang can guarantee that both of us have almost completed
half of the assignment, which is a perfect amount for individuals of pair programming.
We work together basically all methods, especially on generating gird and accomplish the functionalities. Bingying Wang debugged
more and fix some errors like Formula Widget didn't reponse when hit enter. Bingkun Han made more efforts on Save and Open file
methods.

# Branching
We always work together in person, so we only have one branch: main.

# Consulted Peers:

Yanxia Bu and we discussed this assignment. We talked about how to create the grid of a Spreadsheet, and how can we save
and load the file.


# Examples of Good Software Practice (GSP)

1. Use of helper methods: Almost every method in MainPage.xaml.cs is private, and they are used to help MainPage.xaml.

2. Separation of Concerns: Our project maintains a clear separation of concerns: the Spreadsheet class 
   handles the logic related to cell content and dependency management, while separate classes like Cell
   and DependencyGraph manage individual cell data and the relationships between cells, respectively.  
   This separation allows for easier maintenance and understanding of the system, as each part can be developed,
   tested, and debugged independently.

3. Clear and short method names and variable names: GetCellValue method directly indicates that we are going to
   get the value from the corresponding cell.


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

# References
  https://github.com/uofu-cs3500-spring24/ForStudents/tree/main/Examples/XMLDemo
  https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.savefiledialog?view=windowsdesktop-8.0