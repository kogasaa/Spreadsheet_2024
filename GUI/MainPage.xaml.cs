using SpreadsheetUtilities;
using System.Text;
using CommunityToolkit.Maui.Storage;
using SS;
using System.Collections.Generic;
using Microsoft.Maui.Storage;


/// <summary>
/// Author:    Bingying Wang and Bingkun Han
/// Partner:   Bingkun Han and Bingying Wang
/// Date:      03/03/2024
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Bingying - This work may not 
///            be copied for use in Academic Coursework.
///
/// Bingying Wang and Bingkun Han, certify that we wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
/// This is MainPage.xaml.cs file, and we implement all the methods and accomplish
/// all the functions for our Spreadsheet GUI in this project. 
/// The Spreadsheet GUI also supports features such as loading from and saving to files,
/// cell value change notifications, and undo/redo operations to provide comprehensive 
/// spreadsheet functionalities.It implements interfaces for cell validation and normalization 
/// to ensure consistency and correctness of cell names and contents.
/// 
/// </summary>

namespace GUI
{
    /// <summary>
    /// MainPage class implements ContentPage interface. It serves as the user interface for the spreadsheet GUI.
    /// This page displays the spreadsheet grid, allowing users to click on individual cells, input data, 
    /// and apply formulas. It also provides tools for file management such as opening, saving, and 
    /// creating new spreadsheet files, and additional functionalities like clear, check cell's 
    /// formatting, and help with calculate an input formula and return the result.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        private int columnCount;
        private int rowCount;
        private SS.AbstractSpreadsheet mySpreadsheet;

        //This is a dictionary stores all cells with cell name and its entry in grid
        private Dictionary<string, Entry> CellName_EntryPairs = new Dictionary<string, Entry>();

        //This is a dictionary stores all cells' input formulas
        private Dictionary<string, string> pendingFormulas = new Dictionary<string, string>();

        //This is a dictionary stores each entry's original color
        private Dictionary<Entry, Color> originalCellColors = new Dictionary<Entry, Color>();//keep the temp color

        /// <summary>
        /// This is to initialize all the elements in the GUI of spreadsheet:
        ///     1. It will create a 0 - 99 top lebels and A-Z left side labels
        ///     2. It will create a 26*99 entry - grid. all entries will be stored in the CellName - Entry dictionary
        ///     3. It will add the events of entry focused, unfocused, text changed, and completed.
        ///     4. also add feature of seleted cell will change the color
        /// </summary>
        public MainPage()
        {
            columnCount = 26;
            rowCount = 99;
            mySpreadsheet = new SS.Spreadsheet(n => true, n => n.ToUpper(), "six") ;
            InitializeComponent();
            InitializeTopLabels();//add both top labels and left labels into Spreadsheet
            InitializeGrid();//add grid into Spreadsheet

            selectedCellContent.Completed += OnFormulaEntered;
            selectedCellContent.TextChanged += OnContentBarTextChanged;

            //We do this is to fit the requirement:
            //always has a default selected cell name,
            //and we choose A1 as the start
            CellName_EntryPairs["A1"].Focus();

            //Set the default selected cell with the cell selected and its color changed
            SetDefaultSelectedCell();

        }

        /// <summary>
        /// This is the method implements clear button's function.
        /// All cells' contents will be cleaned from this method.
        /// </summary>
        private void ClearAllCells()
        {
            foreach (var cellName in CellName_EntryPairs.Keys)
            {
                //Set each cell's text to be an empty string
                CellName_EntryPairs[cellName].Text = "";
               
                //because we use mySpreadsheet to manage the content of a cell,
                //we also need to "clear" mySpreadsheet
                mySpreadsheet.SetContentsOfCell(cellName, "");
            }

            //all the background's stored formulas should be cleared
            pendingFormulas.Clear();

            //update UI
            selectedCell.Text = "";
            selectedCellValue.Text = "";
            selectedCellContent.Text = "";

        }

        /// <summary>
        /// This is the method to invoke ClearAllCells().
        /// And this method name will be used in MainPage.xaml
        /// </summary>
        /// <param name="sender">
        /// The source of the event, typically the button that was clicked.
        /// </param>
        /// Event data passed by the event
        /// <param name="e">
        /// </param>
        void OnClearButtonClick(object sender, EventArgs e)
        {
            ClearAllCells();
        }

        /// <summary>
        /// This method set up a default cell when a new Spreadsheet is created.
        /// And we choose the default cell to be the first cell A1 in the grid.
        /// When a new Spreadsheet was created, A1 will be selected automatically.
        /// And A1's background color is gray.
        /// </summary>
        private void SetDefaultSelectedCell()
        {
            //define the default cell name
            string defaultCellName = "A1";

            //check if the default cell exists in the dictionary
            if (CellName_EntryPairs.TryGetValue(defaultCellName, out Entry ? defaultCellEntry))
            {
                //focus to the default cell entry
                defaultCellEntry.Focus();

                //update the UI
                selectedCell.Text = defaultCellName;
                selectedCellValue.Text = mySpreadsheet.GetCellValue(defaultCellName)?.ToString() ?? "";

                //update or display the cell's content in the formula bar
                string defaultCellContent = mySpreadsheet.GetCellContents(defaultCellName)?.ToString() ?? "";
                selectedCellContent.Text = defaultCellContent.StartsWith("") ? defaultCellContent : "" + defaultCellContent;

                //Since we have a feature that a selected cell's color is different,
                //we need to turn A1's background's color to gray
                if (!originalCellColors.ContainsKey(defaultCellEntry))
                {
                    originalCellColors[defaultCellEntry] = defaultCellEntry.BackgroundColor;
                }
                defaultCellEntry.BackgroundColor = Color.FromRgba(200, 200, 200, 255);
            }
        }

        /// <summary>
        /// This is the method to initialize the top labels for our Spreadsheet.
        /// Top labels will be 26 English characters.
        /// Left labels will be numbers from 1 to 99.
        /// </summary>
        private void InitializeTopLabels()
        {
            var labels = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            
            foreach (var label in labels)
            {
                TopLabels.Add(
                new Border
                {
                    Stroke = Color.FromRgb(0, 0, 0),
                    StrokeThickness = 1,
                    HeightRequest = 35,
                    WidthRequest = 70,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Content =
                        new Label
                        {
                            Text = $"{label}",
                            BackgroundColor = Color.FromRgba(65, 0, 130, 145),
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                }
                );
            }

            for (int i = 0; i < 100; i++)
            {
                LeftLabels.Add(
                new Border
                {
                    Stroke = Color.FromRgb(0, 0, 0),
                    StrokeThickness = 1,
                    HeightRequest = 35,
                    WidthRequest = 70,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Content =
                        new Label
                        {
                            Text = i == 0 ? "" : $"{i}",//when i = 0, cell's content is empty; otherwise, content is i
                            BackgroundColor = Color.FromRgba(65, 0, 130, 145),
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                }
                );
            }
        }


        /// <summary>
        /// This is the method to initialize Spreadsheet's grid.
        /// We are going to generate 99x26 size grid for our Spreadsheet.
        /// What's more, each entry in grid will have different functionalities
        /// to accomplish the cores for a Spreadsheet.
        /// </summary>
        private void InitializeGrid()
        {

            for (int i = 0; i < rowCount; i++)
            {
                var rowLayout = new HorizontalStackLayout { Spacing = 0 };

                for (int j = 0; j < columnCount; j++)
                {
                    var entry = new Entry
                    {
                        Text = "",
                        BackgroundColor = Color.FromRgb(200, 200, 250),
                        HorizontalTextAlignment = TextAlignment.Center,
                        AutomationId = $"{i}-{j}", //Store row and column in AutomationId
                        VerticalOptions = LayoutOptions.Center,
                    };

                    originalCellColors[entry] = entry.BackgroundColor;//store colors into dictionary

                    //add the envents for the Each Cell Entry
                    //      1. Focused - when we put mouse on entry
                    //      2. UnFocused - when we Mover mouse from the entry
                    //      3. TextChanged - when entry's Text is changed
                    //      4. Completed - when we press enter in when we focused on a cell entry
                    entry.Focused += OnCellFocused;
                    entry.Unfocused += OnCellUnfocused;
                    entry.TextChanged += OnCellTextChanged;
                    entry.Completed += OnCellCompleted;

                    //I use this part to create A-Z by ASCII code to create the cell name
                    //I also put all the entry and cell name in the CellName_EntryPairs Dictionary.
                    //So in the future if we want to delete a cell's content,
                    //I would not use CellName_EntryPairs.Remove(cellName);
                    //but use this - CellName_EntryPairs[cellName].Text = "";
                    char letter = (char)(65 + j);
                    string cellName = $"{letter}{i+1}";
                    CellName_EntryPairs[cellName] = entry;


                    //below is how I generate a red 'U' in my grid
                    var cellBorder = new Border();
                    if ((i == 32 && j == 9) || (i == 33 && j == 10) || (i == 33 && j == 15) || (i == 32 && j == 16))
                    {
                        cellBorder = new Border
                        {
                            Stroke = Color.FromRgb(255, 0, 0),
                            StrokeThickness = 1,
                            HeightRequest = 35,
                            WidthRequest = 70,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            Content = entry
                        };
                    }
                    else if ((i > 6 && i < 32) && (j == 9 || j == 16))
                    {
                        cellBorder = new Border
                        {
                            Stroke = Color.FromRgb(255, 0, 0),
                            StrokeThickness = 1,
                            HeightRequest = 35,
                            WidthRequest = 70,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            Content = entry
                        };
                    }

                    else if (i == 34 && (j > 10 && j < 15))
                    {
                        cellBorder = new Border
                        {
                            Stroke = Color.FromRgb(255, 0, 0),
                            StrokeThickness = 1,
                            HeightRequest = 35,
                            WidthRequest = 70,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            Content = entry
                        };
                    }

                    //All the other cells' color is the same (light purple)
                    else
                    {
                        cellBorder = new Border
                        {
                            Stroke = Color.FromRgb(0, 0, 0),
                            StrokeThickness = 1,
                            HeightRequest = 35,
                            WidthRequest = 70,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            Content = entry
                        };
                    }
                    rowLayout.Children.Add(cellBorder);
                }
                
                Grid.Children.Add(rowLayout);
            }

        }


        /// <summary>
        /// This method updates the UI to reflect the focused cell (if the cell is selected)
        /// by setting the background color of the focused cell's background color
        /// to gray and updating the formula widget and selected cell's value display.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCellFocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            var (row, col) = GetCellPosition(entry);
            var cellName = $"{(char)('A' + col)}{row + 1}";
            CellName_EntryPairs[cellName] = entry;
            if (entry != null)
            {
                //check A1's color
                if (cellName != "A1" && CellName_EntryPairs.ContainsKey("A1"))
                {
                    var a1Entry = CellName_EntryPairs["A1"];
                    if (originalCellColors.ContainsKey(a1Entry))
                    {
                        //upodate A1's color to light purple
                        a1Entry.BackgroundColor = originalCellColors[a1Entry];
                    }
                }

                //When the cell is clicked, its background color is gray
                if (!originalCellColors.ContainsKey(entry))
                {
                    originalCellColors[entry] = entry.BackgroundColor;
                }

                //set color to be gray for selected cell
                entry.BackgroundColor = Color.FromRgba(200, 200, 200, 255);

                //update UI
                selectedCell.Text = cellName;
                selectedCellValue.Text = mySpreadsheet.GetCellValue(cellName)?.ToString() ?? "";
                string content = mySpreadsheet.GetCellContents(cellName)?.ToString() ?? "";
                selectedCellContent.Text = content.StartsWith("") ? content : "" + content;
            }
        }

        /// <summary> 
        /// After clicking on return or clicking on other cells, this method will
        /// update the cell's content based on any pending formula, and ensuring the 
        /// spreadsheet's internal storage is updated appropriately.
        /// If the cell contained a formula that was being edited but not yet committed, 
        /// this method attempts to commit that formula to the
        /// spreadsheet. If the formula is invalid, an error message is displayed, 
        /// and the cell's content reverts to its last valid state.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Entry control
        /// representing the spreadsheet cell that lost focus.
        /// </param>
        /// <param name="e">Event data containing information about the focus event
        /// </param>
        private void OnCellUnfocused(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry)
            {
                // Restore the original background color
                if (originalCellColors.TryGetValue(entry, out Color? originalColor))
                {
                    entry.BackgroundColor = originalColor;
                }


                var (row, col) = GetCellPosition(entry);
                string cellName = $"{(char)('A' + col)}{row + 1}";

                //if it unfocus and the selectedCellName changed, I will not unfocuse a cell
                //when the selected cell name still be same but unfocus, because in this situation
                //the mouse should be in the selected cell content bar, we still want it focused without
                //return to original content.
                if (pendingFormulas.TryGetValue(cellName, out string? formula))
                {
                    //calculate the formula here
                    try
                    {
                        mySpreadsheet.SetContentsOfCell(cellName, formula);
                        var value = mySpreadsheet.GetCellValue(cellName);
                        if (!selectedCellContent.IsFocused)
                        {
                            entry.Text = value.ToString();
                        }
                    }
                    catch (Exception ex) //when the formula is invalid
                    {
                        DisplayAlert("Error", $"Exception thrown in {cellName}: {ex.Message}", "OK");
                        entry.Text = mySpreadsheet.GetCellValue(cellName).ToString();

                        //if it was wrong it will go back to original content:
                        //  1. IF  original is formula it will put pending formula the old formula
                        //  2. other situaion, pending formula will delete this cell as key
                        if (mySpreadsheet.GetCellContents(cellName).GetType() == typeof(Formula))
                        {
                            pendingFormulas[cellName] = "=" + mySpreadsheet.GetCellContents(cellName).ToString();
                        }
                        else
                        {
                            pendingFormulas.Remove(cellName);
                        }
                    }
                }
                if (string.IsNullOrEmpty(selectedCell.Text))
                {
                    CellName_EntryPairs[cellName].Text = "";
                }                               
            }
        }

        /// <summary>
        /// This method is called when the user finishes editing a cell's content
        /// and presses the Enter key or navigates away from the cell, 
        /// effectively committing the entered data. 
        /// </summary>
        /// <param name="sender">
        /// representing the spreadsheet cell where data was entered.
        /// </param>
        /// <param name="e">
        /// A completion event
        /// </param>
        private void OnCellCompleted(object sender, EventArgs e)
        {

            if (sender is Entry entry)
            {
                entry.Unfocus();//treating entry like the entry is not focused
            }
        }


        /// <summary>
        /// When the text within a cell changes, this method will be invoked. This method updates the spreadsheet's
        /// underlying data model with a new content entered by the user. If the new content is a formula (start with '='),
        /// it is stored in a pending state until the cell loses focus or the content
        /// is explicitly committed. Otherwise, the cell's content is directly updated in the spreadsheet model,
        /// and any dependent cells are recalculated as needed.
        /// </summary>
        /// <param name="sender">
        /// The source of the event, representing the spreadsheet cell's content that was edited.
        /// </param>
        /// Event contains the information after the text was changed
        /// <param name="e"></param>
        private void OnCellTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                var (row, col) = GetCellPosition(entry);
                string cellName = $"{(char)('A' + col)}{row + 1}";

                //we start to store a formula
                if (entry.Text.StartsWith("="))
                {
                    pendingFormulas[cellName] = entry.Text;
                    CellName_EntryPairs[cellName] = entry;
                    selectedCellContent.Text = entry.Text;
                }
                else
                {
                    //If a cell is text changed and it was focused,
                    //the entry's text will be same as the content in the cell
                    if(entry.IsFocused == true)
                    {
                        //If it is not a formula, directly set up cell's content
                        IEnumerable<string>reCalculatedCellNames = mySpreadsheet.SetContentsOfCell(cellName, entry.Text);

                        //This is part i do recalculate all the cells needed. Like a1 = 1, a2 = a1+1. if a1 =1, a2 will be 2
                        foreach(string reCalculatedCellName in reCalculatedCellNames)
                        {
                            CellName_EntryPairs[reCalculatedCellName].Text = mySpreadsheet.GetCellValue(reCalculatedCellName).ToString();
                        }
                        CellName_EntryPairs[cellName] = entry;
                        pendingFormulas.Remove(cellName); //remove the temporary formula
                        selectedCellContent.Text = entry.Text;
                    }
                }
                selectedCellValue.Text = mySpreadsheet.GetCellValue(cellName).ToString();
            }
        }


        /// <summary>
        /// When the text within the formula bar changes. This method is invoked whenever
        /// the user types or edits text in the formula bar.
        /// </summary>
        /// <param name="sender">
        /// The source of the event, which is the formula bar control
        /// </param>
        /// Event on text changed
        /// <param name="e"></param>
        private void OnContentBarTextChanged(object? sender, TextChangedEventArgs e)
        {
            string cellName = selectedCell.Text;
            if (!string.IsNullOrEmpty(selectedCell.Text))
            {
                CellName_EntryPairs[cellName].Text = selectedCellContent.Text;
                
            }
        }

        /// <summary>
        /// This method gets a cell's postion
        /// </summary>
        /// <param name="entry">
        /// The entry we want to get its postion
        /// </param>
        /// <returns>
        /// return the positon with row number and column number
        /// </returns>
        private (int, int) GetCellPosition(Entry entry)
        {
            var positions = entry.AutomationId.Split('-');
            return (int.Parse(positions[0]), int.Parse(positions[1]));
        }

        /// <summary>
        /// When a formula is entered or modified in the formula widget/bar and then clicked on Enter or other cells. 
        /// This method validates and updates the formula for the currently selected cell in the spreadsheet. 
        /// If the formula is valid, the cell's value in the spreadsheet model is updated,
        /// and the GUI will display the new value. If the formula is invalid, an error message is displayed, 
        /// and the cell's value remains unchanged.
        /// </summary>
        /// <param name="sender">
        /// The source of the event
        /// </param>
        /// <param name="e">
        /// Event for formula widget
        /// </param>
        private void OnFormulaEntered(object sender, EventArgs e)
        {
            //make sure a cell is selected
            if (string.IsNullOrEmpty(selectedCell.Text))
            {
                return;
            }

            //get the selected cell's name and its formula from formula bar
            string cellName = selectedCell.Text;
            string formula = selectedCellContent.Text;

            try
            {
                //put the formula into the corresponding cell
                mySpreadsheet.SetContentsOfCell(cellName, formula);
                var result = mySpreadsheet.GetCellValue(cellName);

                //make sure UI is updated in main thread
                MainThread.BeginInvokeOnMainThread(() => {
                    UpdateCellValue(cellName, result?.ToString());
                });
            }
            catch (Exception ex)
            {
                //Error handling
                MainThread.BeginInvokeOnMainThread(() => {
                    DisplayAlert("Error", $"Error processing formula: {ex.Message}", "OK");
                });
            }
        }

        /// <summary>
        /// This method will update the cell's value.
        /// If the cell is exist in dictionary that stores all the cell-value pairs,
        /// we will get that cell and update its value.
        /// </summary>
        /// <param name="cellName">
        /// The cell we want to update its value
        /// </param>
        /// <param name="value">
        /// The new value for the cell is updated
        /// </param>
        private void UpdateCellValue(string cellName, string value)
        {
            if (CellName_EntryPairs.TryGetValue(cellName, out Entry ? cellEntry))
            {
                if (cellEntry != null)
                {
                    cellEntry.Text = value;
                }
            }
            
        }



        /// <summary>
        /// This is the method of saving a file from selecting the save option from menu bar.
        /// It will prompt the user to pick a folder & file name to save to.
        /// 
        /// EXCEpTIon: If there is a any exception thrown when we open a file, we will show
        ///             a Alert upon the screen.
        /// </summary>
        /// <param name="sender">ignore</param>
        /// <param name="e">ignore</param>
        private async void FileMenuSaveAsync(Object sender, EventArgs e)
        {
            //prepare a token for task cancellation
            CancellationToken cancellationToken = new CancellationToken();
            try
            {
                //convert data into XML and store it into memory stream
                MemoryStream Stream = new MemoryStream(Encoding.Default.GetBytes(mySpreadsheet.GetXML()));
                var fileSaveResult = await FileSaver.SaveAsync("Spreadsheet.sprd", Stream, cancellationToken);

            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Saving Files has error: {ex.Message}", "OK");
            }
        }


        /// <summary>
        /// This is the method of opening a file from selecting a file from file explorer
        /// If it selected, it will get all the cell contents and created in the UI Panel
        /// 
        /// EXCEpTIon: If there is a any exception thrown when we open a file, we will show
        ///             an Alert upon the screen.
        /// </summary>
        /// <param name="sender">ignore</param>
        /// <param name="e">ignore</param>
        private async void FileMenuOpenAsync(Object sender, EventArgs e)
        {
            //This is part will show the user a warning that he does not save the file
            if (!await WarningForNotSaving())
            {
                return;
            }

            //This is to get the path of the spreadsheet file in the file explorer, and then
            //try to get all cell information and move them to the spreadsheet gui view
            var pickedFile = await FilePicker.Default.PickAsync();
            if (pickedFile != null)
            {
                


                //First get the full path the selected open file then recreate a spreadsheet, and draw the VALUES
                //of each cell in the UI
                string fileName = pickedFile.FullPath;
                try
                {
                    ClearAllCells();
                    mySpreadsheet = new Spreadsheet(fileName, n => true, n => n.ToUpper(), "six");
                    IEnumerable<string> allNonEmptyCellNames = mySpreadsheet.GetNamesOfAllNonemptyCells();
                    foreach (string cellName in allNonEmptyCellNames)
                    {
                        CellName_EntryPairs[cellName].Text = mySpreadsheet.GetCellValue(cellName).ToString();
                    }
                }
                catch(Exception ex)
                {
                    DisplayAlert("Error", $"Opening Files has error: {ex.Message}", "OK");
                }
            }
        }

        /// <summary>
        /// This is to create a new spreadsheet, we just remove every text in the each non empty cell and 
        /// set the spreadsheet system as a new spreadsheet system, nothing in the spreadsheet.
        /// </summary>
        /// <param name="sender">ignore</param>
        /// <param name="e">ignore</param>
        private async void FileMenuNew(Object sender, EventArgs e)
        {
            //This is part will show the user a warning that he does not save the file
            if(!await WarningForNotSaving())
            {
                return;
            }

            //This is the part to remove everything in the spreadsheet to make it like new one.
            IEnumerable<string> pendingDeleteCells = mySpreadsheet.GetNamesOfAllNonemptyCells().ToList();
            foreach (string pendingDeleteCellName in pendingDeleteCells)
            {
                CellName_EntryPairs[pendingDeleteCellName].Text = "";
                pendingFormulas.Clear();
            }
            mySpreadsheet = new Spreadsheet(n=>true, n=>n.ToUpper(), "six");
        }

        /// <summary>
        /// Showing the warning if you want to continue creating a new GUI without saving the previous one
        /// </summary>
        /// <returns> True if the user wishes to continue anyways. 
        /// </returns>
        private async Task<bool> WarningForNotSaving()
        {
            if (mySpreadsheet.Changed)
            {
                return await DisplayAlert("Warning", "You have not save your spreadsheet,\n Are you sure you want to continue?", "Yes", "No");
            }
            return true;
        }

        /// <summary>
        /// This method is used to create a pop-up window for help menu
        /// </summary>
        /// <param name="sender">
        /// The source of the event
        /// </param>
        /// <param name="e">
        /// Event data stores all the messages
        /// </param>
        private async void HelpWindowsPopUp(Object sender, EventArgs e)
        {
            DisplayAlert("Help", $"This is the Spreadsheet GUI We designed. It has these features:\n\n" +
                $"1. you can type a value or formula in the cell entry in the grid\n" +
                $"2. there are three widgets on the top of the spreadsheet, from left to right,\n" +
                " The first is to show the seleted cell name\n" +
                $" the second is to show the content of this cell contained\n" +
                $" The third one is to show the value in this cell\n" +
                $" The fourth one is a clear button and if you click it, all the cell contents will be cleared\n"+
                $"3. there are from A1 - Z99 cells in this spreadsheet, which has 2574 cells\n" +
                $"4. The Special Features are: A clear button to clear all cell contents; " +
                $"Once a cell was selected, its background color is different;" +
                $"we create a big red U in the spreadsheet", "OK");

        }
    }
}