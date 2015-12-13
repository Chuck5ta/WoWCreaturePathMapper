using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO; // for file handling
using System.Windows;

namespace WoWCoordsToSQLScript
{
    class FileHandling
    {


        // Load the contents of the log file
        public static ArrayList loadFile(string fileName)
        {
            ArrayList wordList = new ArrayList();

            try
            {
                string fileLine;
                // this will replace any existing file
                FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (StreamReader fileReader = new StreamReader(fs, Encoding.GetEncoding(1252)))
                {
                    while (!fileReader.EndOfStream)
                    {
                        fileLine = fileReader.ReadLine();
                        wordList.Add(fileLine);
                    }
                }
                fs.Close();
            }
            catch
            {
                MessageBox.Show("ERROR - accessing file! " + fileName);
            }

            return wordList;
        } // END OF loadLogFile(string fileName)

        // Write data as is to file
        public static void saveData(string fileName, ArrayList listOfData)
        {
            // We are saving the configuration
            // Create new file and save the data
            try
            {
                // this will replace any existing file
                FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                using (StreamWriter fileWriter = new StreamWriter(fs, Encoding.GetEncoding(1252)))
                {
                    foreach (object datainList in listOfData)
                    {
                        fileWriter.WriteLine(datainList);
                    }
                }
                fs.Close();
            }
            catch
            {
                MessageBox.Show("ERROR - processing file! " + fileName);
            }
        } // END OF saveData(string fileName)


        // Save the default settings
        public static void saveDefaultSettings(string fileName, DefaultSettings configSettings)
        {
            // We are saving the configuration
            // Create new file and save the data
            try
            {
                // this will replace any existing file
                FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                using (StreamWriter fileWriter = new StreamWriter(fs, Encoding.GetEncoding(1252)))
                {
                    fileWriter.WriteLine(configSettings.defaultNameOfCharacter);
                    fileWriter.WriteLine(configSettings.defaultCreatureGUID);
                    fileWriter.WriteLine(configSettings.defaultPoint);
                    fileWriter.WriteLine(configSettings.defaultCSVFileNameForSaving);
                    fileWriter.WriteLine(configSettings.defaultDelimiterForCSVFileSaving);
                    fileWriter.WriteLine(configSettings.defaultCSVFileNameForLoading);
                    fileWriter.WriteLine(configSettings.defaultDelimiterForCSVFileLoading);
                    fileWriter.WriteLine(configSettings.defaultSQLScriptFileName);
                }
                fs.Close();
            }
            catch
            {
                MessageBox.Show("ERROR - processing file! " + fileName);
            }
        } // END OF saveData(string fileName)

        // Load the contents of the deafultSettings file
        public static DefaultSettings loadDefaultSettings(string fileName)
        {
            DefaultSettings defaultConfigSettings = new DefaultSettings();

            try
            {
                string fileLine;
                // this will replace any existing file
                FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (StreamReader fileReader = new StreamReader(fs, Encoding.GetEncoding(1252)))
                {
                        fileLine = fileReader.ReadLine();
                        defaultConfigSettings.defaultNameOfCharacter = fileLine;
                        fileLine = fileReader.ReadLine();
                        defaultConfigSettings.defaultCreatureGUID = fileLine;
                        fileLine = fileReader.ReadLine();
                        defaultConfigSettings.defaultPoint = fileLine;
                        fileLine = fileReader.ReadLine();
                        defaultConfigSettings.defaultCSVFileNameForSaving = fileLine;
                        fileLine = fileReader.ReadLine();
                        defaultConfigSettings.defaultDelimiterForCSVFileSaving = fileLine;
                        fileLine = fileReader.ReadLine();
                        defaultConfigSettings.defaultCSVFileNameForLoading = fileLine;
                        fileLine = fileReader.ReadLine();
                        defaultConfigSettings.defaultDelimiterForCSVFileLoading = fileLine;
                        fileLine = fileReader.ReadLine();
                        defaultConfigSettings.defaultSQLScriptFileName = fileLine;
                }
                fs.Close();
            }
            catch
            {
                MessageBox.Show("ERROR - accessing file config file (configSettings), default settings used!");
                // apply default settings
                defaultConfigSettings.defaultNameOfCharacter = "";
                defaultConfigSettings.defaultCreatureGUID = "";
                defaultConfigSettings.defaultPoint = "1";
                defaultConfigSettings.defaultCSVFileNameForSaving = "defaultCSVFileName";
                defaultConfigSettings.defaultDelimiterForCSVFileSaving = ",";
                defaultConfigSettings.defaultCSVFileNameForLoading = "defaultCSVFileNameToLoad";
                defaultConfigSettings.defaultDelimiterForCSVFileLoading = ",";
                defaultConfigSettings.defaultSQLScriptFileName = "defaultSqlScriptFileName";

            }

            return defaultConfigSettings;
        } // END OF loadDefaultSettings(string fileName)


    }
}
