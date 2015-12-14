using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWCoordsToSQLScript
{
    class DefaultSettings
    {
        public string defaultNameOfCharacter;
        public string defaultCreatureGUID;
        public string defaultPoint;
        public string defaultCSVFileNameForSaving;
        public string defaultDelimiterForCSVFileSaving;
        public string defaultDelimiterForCSVFileLoading;
        public string defaultCSVFileNameForLoading;
        public string defaultSQLScriptFileName;
        public bool defaultBiDirectionalPathCheckedState;
        public bool defaultOneDirectionPathCheckedState;
        public bool defaultEntryCheckedState;
        public bool defaultGuidCheckedState;

        // constructor
        public DefaultSettings()
        {
            // initialise settings
            defaultNameOfCharacter = "";
            defaultCreatureGUID = "";
            defaultPoint = "1";
            defaultCSVFileNameForSaving = "";
            defaultDelimiterForCSVFileSaving = ",";
            defaultCSVFileNameForLoading = "";
            defaultDelimiterForCSVFileLoading = ",";
            defaultSQLScriptFileName = "";
            defaultBiDirectionalPathCheckedState =  true;
            defaultOneDirectionPathCheckedState = false;
            defaultEntryCheckedState = true;
            defaultGuidCheckedState = false;
        }
    }
}
