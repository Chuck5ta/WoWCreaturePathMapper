﻿/*
 * This program is used to convert WoW continent level coordinated to SQL script, so that they can be added to the mangos.creature_movement database.
 * 
 * Author: Chuck E
 * Date: 28th of September, 2014
 * 
 * Version: 1.2.0.0
 * major change, minor change, small change, x
 * major change (addind support for more than one O/S)
 * minor change (new feature)
 * small change (bug fix)
 * 
 * Inspiration for this tool:
 * LazyBot's auto mapping of the gathering/scavanging path
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Collections;

           

namespace WoWCoordsToSQLScript
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool expandHelpPanel = true;
        private bool expandDelimiterToolPanel = true;
        private bool bGUID; // if we are using a GUID, then we are adding path data to the creature_movement table, otherwise it will be an 'entry' ID for the creature_movement_template table
        private bool bBirDirectionalPath; // one way path (e.g. circular) or bi-directional (back and forth along the same route) ?
        private ArrayList logfileList = new ArrayList();
        private ArrayList outputFileList = new ArrayList();

        private DefaultSettings defaultConfigSettings;

        public MainWindow()
        {
            InitializeComponent();
            initaliseStuff();
        }

        /*
         * This puts everything to their initial state, that needs setting thus (geez, brain fail!!!)
         */
        private void initaliseStuff()
        {
            rtbHelpInformation.IsReadOnly = true;

            defaultConfigSettings = new DefaultSettings();

            // load default settings from file
            defaultConfigSettings = FileHandling.loadDefaultSettings("configSettings.txt");

            // set interface to reflect the default settings
            txtGMCharactersName.Text = defaultConfigSettings.defaultNameOfCharacter;
            txtId.Text = defaultConfigSettings.defaultCreatureGUID;
            txtPoint.Text = defaultConfigSettings.defaultPoint;
            txtCsvFileName.Text = defaultConfigSettings.defaultCSVFileNameForSaving;
            txtDelimiterForSavedCSVFile.Text = defaultConfigSettings.defaultDelimiterForCSVFileSaving;
            txtCSVFileNameToLoad.Text = defaultConfigSettings.defaultCSVFileNameForLoading;
            txtDelimiterForLoadedCSVFile.Text = defaultConfigSettings.defaultDelimiterForCSVFileLoading;
            txtSQLScriptFileName.Text = defaultConfigSettings.defaultSQLScriptFileName;
            txtId.Text = defaultConfigSettings.defaultCreatureGUID;
            
            radBiDirectionalPath.IsChecked = defaultConfigSettings.defaultBiDirectionalPathCheckedState;
            radOneDirectionPath.IsChecked = defaultConfigSettings.defaultOneDirectionPathCheckedState;            
            radEntry.IsChecked = defaultConfigSettings.defaultEntryCheckedState;
            radGuid.IsChecked = defaultConfigSettings.defaultGuidCheckedState;

            bGUID = true;
            bBirDirectionalPath = false; 

        }

        private void btnGrabData_Click(object sender, RoutedEventArgs e)
        {
            bool characterNameEntered = false;
            if (txtGMCharactersName.Text == "")
            {
                // inform user of error
                txtMessagePanel.Text = "";
                txtMessagePanel.Text = "You must enter the name of the character you used to acquire the waypoint data!\r\n";
            }
            else
                characterNameEntered = true;
            bool guidEntered = false; // GUID in the creature table == ID in the creature_movement table
            if (txtId.Text == "")
            {
                // inform user of error
                txtMessagePanel.Text += @"You must enter the GUID of the character/NPC you acquired the waypoint data for!";
            }
            else
                guidEntered = true;

            // make sure the user has filled in the required fields - GM Character name and GUID of target creaure/NPC
            if (characterNameEntered && guidEntered)
            {

                // load the log file
                logfileList = FileHandling.loadFile("WoWChatLog.txt");
                // did we successfully load the data
                if (logfileList.Count > 0)
                {
                    string xOrdinate = "";
                    string yOrdinate = "";
                    string zOrdinate = "";
     //               string orientation = "";
                    // add to the list box
                    lbContinentCoordinates.Items.Clear(); // make sure the ListBox is empty

                    foreach (object logLine in logfileList)
                    {
                        int startIndex;
                        int endIndex;
                        int numberOfChars;
                        string delimiter = txtDelimiterForSavedCSVFile.Text;
                        string text = logLine.ToString();
                        string characterName = txtGMCharactersName.Text;
                        if (text.IndexOf(characterName) != -1) // locate coords entry
                        {
                            // retrieve the X ordinate
                            startIndex = text.IndexOf("XStart") + 6;
                            endIndex = text.IndexOf("XEnd");
                            // grab the data (X ordinate)
                            numberOfChars = endIndex - startIndex - 1;
                            if (numberOfChars>0)
                            {
                                xOrdinate = text.Substring(startIndex, numberOfChars);
                                xOrdinate.Replace(" ", string.Empty);
                                // lbContinentCoordinates.Items.Add(logLine);

                                // Y ordinate
                                startIndex = text.IndexOf("YStart") + 6;
                                endIndex = text.IndexOf("YEnd");
                                // grab the data (Y ordinate)
                                numberOfChars = endIndex - startIndex - 1;
                                yOrdinate = text.Substring(startIndex, numberOfChars);
                                yOrdinate.Replace(" ", string.Empty);

                                // Z ordinate
                                startIndex = text.IndexOf("ZStart") + 6;
                                endIndex = text.IndexOf("ZEnd");
                                // grab the data (Z ordinate)
                                numberOfChars = endIndex - startIndex - 1;
                                zOrdinate = text.Substring(startIndex, numberOfChars);
                                zOrdinate.Replace(" ", "");

                                // Orientation - remove for now, as too many of these made the animation look very jerky
                                /*
                                startIndex = text.IndexOf("OStart") + 6;
                                endIndex = text.IndexOf("OEnd");
                                // grab the data (orientation)
                                numberOfChars = endIndex - startIndex - 1;
                                orientation = text.Substring(startIndex, numberOfChars);
                                 */

                                lbContinentCoordinates.Items.Add(xOrdinate + delimiter + yOrdinate + delimiter + zOrdinate);
                            }
                        }

                        txtMessagePanel.Text = "Job done :-)";

                    }
                }
                else // the WoW log file was either empty or could not be loaded
                {
                    txtMessagePanel.Text = "PROBLEM: The WowChatLog.txt file was either empty or could not be loaded. \r\n";
                    txtMessagePanel.Text += "Have you copied it from the Logs folder of the WoW installation, to the folder of this tool?";
                }
            }
        }

        /*
         * This saves the coordinates and orientation to a file in CSV form (e.g. X, Y, Z, Orientation)
         */
        private void btnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            // place the contents of the ListBox into a list
            ArrayList listOfData = new ArrayList();
            for (int i = 0; i < lbContinentCoordinates.Items.Count; i++)
                listOfData.Add(lbContinentCoordinates.Items[i]);
            // save the data as is
            string fileName = txtCsvFileName.Text;
            if (txtCsvFileName.Text == "")
                FileHandling.saveData("defaultCSVFileName", listOfData);
            else
                FileHandling.saveData(fileName, listOfData);
        }

        /*
         * This saves the data as a SQL script to a text file.
         * In this case the SQL script adds records to the creature_movement table
         */
        private void btnConvertToSql_Click(object sender, RoutedEventArgs e)
        {
            outputFileList.Clear();
            string line;
            string[] componentpart;
            string sqlScript = "";
            string id = txtId.Text;
            int point = Convert.ToInt32(txtPoint.Text); // points in a path - means to know which order the entries are executed in - 1 to n
            char delimiter = txtDelimiterForSavedCSVFile.Text[0];
            string sTableName;
            string sGuidOrEntry;

            if (bGUID) // creature_movement
            {
                sTableName = "creature_movement";
                sGuidOrEntry = "id";
            }
            else // creature_movement_template
            {
                sTableName = "creature_movement_template";
                sGuidOrEntry = "entry";
            }
            // iterate through the ListBox, converting each entry to SQL script
            for (int i = 0; i < lbContinentCoordinates.Items.Count; i++)
            {
                line = lbContinentCoordinates.Items[i].ToString();
                // split the data into its component parts - X, Y, Z, Orientation

                componentpart = line.Split(delimiter);
                // add it to output list
                sqlScript = "INSERT INTO " + sTableName + " (" + sGuidOrEntry + ", `point`, `position_x`, `position_y`, `position_z`, `waittime`, `script_id`, `textid1`, `textid2`, `textid3`, `textid4`, `textid5`, `emote`, `spell`, `orientation`, `model1`, `model2`) VALUES (" + id + ", " + point + ", " + componentpart[0] + ", " + componentpart[1] + ", " + componentpart[2] + ", '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');";

                point++; // next position in the path
                outputFileList.Add(sqlScript);
            }
            
            // if we want a bi-directional path (back and forth along the same route)
            // write coordinates, from the ListBox, in reverse order
            if (bBirDirectionalPath)
                for (int i = lbContinentCoordinates.Items.Count-2; i >= 1; i--)
                {
                    line = lbContinentCoordinates.Items[i].ToString();
                    
                    // split the data into its component parts - X, Y, Z, Orientation
                    componentpart = line.Split(delimiter);

                    // add it to output list
                    sqlScript = "INSERT INTO " + sTableName + " (" + sGuidOrEntry + ", `point`, `position_x`, `position_y`, `position_z`, `waittime`, `script_id`, `textid1`, `textid2`, `textid3`, `textid4`, `textid5`, `emote`, `spell`, `orientation`, `model1`, `model2`) VALUES (" + id + ", " + point + ", " + componentpart[0] + ", " + componentpart[1] + ", " + componentpart[2] + ", '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');";

                    point++; // next position in the path
                    outputFileList.Add(sqlScript);                    
                }                
            
            // now save it to file
            if (txtSQLScriptFileName.Text == "")
                FileHandling.saveData("defaultSqlScriptFileName", outputFileList);
            else
                FileHandling.saveData(txtSQLScriptFileName.Text, outputFileList);

        }

        /*
         * This loads an existing file that contains CSV data
         */
        private void btnLoadCSVFile_Click(object sender, RoutedEventArgs e)
        {
            // clear the message panel
            txtMessagePanel.Text = "";
            string[] lineOfData;
            char delimiter = txtDelimiterForLoadedCSVFile.Text[0];
            // store the data
            ArrayList listOfData = new ArrayList();
            // load it
            if (txtCSVFileNameToLoad.Text == "")
                listOfData = FileHandling.loadFile("defaultCSVFileNameToLoad");
            else
                listOfData = FileHandling.loadFile(txtCSVFileNameToLoad.Text);
            // store list in ListBox
            if (listOfData.Count > 0) // make sure the loading of the CSV data worked
            {
                // check that the data is valid
                foreach (object logLine in listOfData)
                {
                    // split the data
                    lineOfData = logLine.ToString().Split(delimiter);
                    if (lineOfData.Length != 4) // must have 4 fields for this tool (X, Y, Z, Orientation)
                    {
                        // invalid data - number of elements
                        txtMessagePanel.Text = "Invalid make up of CSV file - it must contain exactly 4 values, each separated by the specified delimiter!";
                        break; // exit routine
                    }
                }

                lbContinentCoordinates.Items.Clear(); // make sure the ListBox is empty
                foreach (object logLine in listOfData)
                    lbContinentCoordinates.Items.Add(logLine);
            }
            else
                txtMessagePanel.Text = "CSV file was empty or does not exist!";

        }

        /*
         * save the default settings, so that we can use them each time the tool is run
         */
        private void btnSaveDefaultSettings_Click(object sender, RoutedEventArgs e)
        {
            // assign current settings to the deafult settings data
            defaultConfigSettings.defaultNameOfCharacter = txtGMCharactersName.Text;
            defaultConfigSettings.defaultCreatureGUID = txtId.Text;
            defaultConfigSettings.defaultPoint = txtPoint.Text;
            defaultConfigSettings.defaultCSVFileNameForSaving = txtCsvFileName.Text;
            defaultConfigSettings.defaultCSVFileNameForLoading = txtCSVFileNameToLoad.Text;
            defaultConfigSettings.defaultSQLScriptFileName = txtSQLScriptFileName.Text;
            defaultConfigSettings.defaultBiDirectionalPathCheckedState = (bool)radBiDirectionalPath.IsChecked;
            defaultConfigSettings.defaultOneDirectionPathCheckedState = (bool)radOneDirectionPath.IsChecked;
            defaultConfigSettings.defaultEntryCheckedState = (bool)radEntry.IsChecked;
            defaultConfigSettings.defaultGuidCheckedState = (bool)radGuid.IsChecked;

            // save - defaultSettings
            FileHandling.saveDefaultSettings("configSettings.txt", defaultConfigSettings);
        }

        /*
         * This opens up a panle on the right hand side of the interface, revealing a "How To" use the tool
         */
        private void btnHelpMe_Click(object sender, RoutedEventArgs e)
        {
            if (expandHelpPanel)
            {
                this.Width = 900;
                btnHelpMe.Content = "< Help";
                expandHelpPanel = false;
            }
            else
            {
                this.Width = 524;
                btnHelpMe.Content = "Help >";
                expandHelpPanel = true;
            }
        }

        /*
         * This will display the brief How-To guide in the meesage panel (below the results window)
         */
        private void btnWTF_Click(object sender, RoutedEventArgs e)
        {
            txtMessagePanel.Text = "(1) Use the WoW addon to acquire the waypoint data (GM mode and enter  /chatlog in chat to activate chat logging).\r\n";
            txtMessagePanel.Text += "(2) When finished plotting waypoints (log out of WoW!) - Copy the WoWChatLog.txt from WoW Logs to this tool's folder.\r\n";
            txtMessagePanel.Text += "(3) Enter your GM character's name, Enter GUID/id of the creature, Enter the point (1 = 1st waypoint in creature_movement table).\r\n";
            txtMessagePanel.Text += "(4) Choose the type of path the creature will walk.\r\n";
            txtMessagePanel.Text += "(5) THEN hit the Grab Data button!!!.\r\n";
            txtMessagePanel.Text += "(6) AND FINALLY convert it all to SQL script (Convert to SQL button)";
        }

        /*
         * This is used to make sure the Point field (TextBox) receives a numeric value.
         */
        private void txtPoint_LostFocus(object sender, RoutedEventArgs e)
        {
            // make sure the input is numeric
            try
            {
                int testContents = Convert.ToInt32(txtPoint.Text);
            }
            catch
            {
                // set the value to 1
                txtPoint.Text = "1"; // set to default value
                txtMessagePanel.Text = "ERROR: Point must have a numeric value!!";
            }
        }

        private void radGuid_Checked(object sender, RoutedEventArgs e)
        {
            // we are to create SQL script for the creature_movement table
            bGUID = true;
        }

        private void radEntry_Checked(object sender, RoutedEventArgs e)
        {
            // we are to create SQL script for the creature_movement_template table
            bGUID = false;
        }

        private void radOneDirectionPath_Checked(object sender, RoutedEventArgs e)
        {
            // the creature can only walk in one direction on the path
            bBirDirectionalPath = false;
        }

        private void radBiDirectionalPath_Checked(object sender, RoutedEventArgs e)
        {
            // the creature walks back and forth along the path
            bBirDirectionalPath = true;
        }

        private void btnDelimiterConversion_Click(object sender, RoutedEventArgs e)
        {
            if (expandDelimiterToolPanel)
            {
                this.Height = 600;
                btnDelimiterConversion.Content = "/\\ Close /\\";
                expandDelimiterToolPanel = false;
            }
            else
            {
                this.Height = 490;
                btnDelimiterConversion.Content = "\\/ Delimiter conversion \\/";
                expandDelimiterToolPanel = true;
            }
        }
    }
}
