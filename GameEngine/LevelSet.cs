using GameEngine.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GameEngine
{
    // LevelSet contains information about the level set we can play. This
    // level set information is stored in an XML file.
    public class LevelSet
    {
        #region Private fields
        // Collection of the levels in a XML level set
        private ArrayList _levels = new ArrayList();

        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _filename = string.Empty;

        private int _currentLevel = 0;
        private int _nrOfLevelsInSet = 0;
        #endregion

        #region Properties

        public string Title
        {
            get { return _title; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string Filename
        {
            get { return _filename; }
        }

        public int NrOfLevelsInSet
        {
            get { return _nrOfLevelsInSet; }
        }

        public int CurrentLevel
        {
            get { return _currentLevel; }
            set { _currentLevel = value; }
        }

        #endregion

        #region Constructors
        public LevelSet(string aTitle, string aDescription, int aNrOfLevels, string aFilename)
        {
            _title = aTitle;
            _description = aDescription;
            _nrOfLevelsInSet = aNrOfLevels;
            _filename = aFilename;
        }

        public LevelSet() { }

        // Indexer for the LevelSet object
        public Level this[int index]
        {
            get { return (Level)_levels[index]; }
        }
        #endregion

        #region Methods
        public void SetLevelSet(string setName)
        {
            // Load XML into memory
            XmlDocument doc = new XmlDocument();
            doc.Load(setName);

            _filename = setName;
            _title = doc.SelectSingleNode("//Title").InnerText;
            _description = doc.SelectSingleNode("//Description").InnerText;


            XmlNode levelCollection = doc.SelectSingleNode("//LevelCollection");
            XmlNodeList levels = doc.SelectNodes("//Level");
            _nrOfLevelsInSet = levels.Count;
        }

        public void SetLevelsInLevelSet(string setName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(setName);

            // Get all Level elements from the level set
            XmlNodeList levelInfoList = doc.SelectNodes("//Level");

            int levelNr = 1;
            foreach (XmlNode levelInfo in levelInfoList)
            {
                LoadLevel(levelInfo, levelNr);
                levelNr++;
            }
        }

        private void LoadLevel(XmlNode levelInfo, int levelNr)
        {
            // Read the attributes from the level element            
            XmlAttributeCollection xac = levelInfo.Attributes;
            string levelName = xac["Id"].Value;
            int levelWidth = int.Parse(xac["Width"].Value);
            int levelHeight = int.Parse(xac["Height"].Value);
            int nrOfGoals = 0;

            // Read the layout of the level
            XmlNodeList levelLayout = levelInfo.SelectNodes("L");

            // Declare the level map
            ItemType[,] levelMap = new ItemType[levelWidth, levelHeight];

            // Read the level line by line
            for (int i = 0; i < levelHeight; i++)
            {
                string line = levelLayout[i].InnerText;
                bool wallEncountered = false;

                // Read the line character by character
                for (int j = 0; j < levelWidth; j++)
                {
                    // If the end of the line is shorter than the width of the
                    // level, then the rest of the line is filled with spaces.
                    if (j >= line.Length)
                        levelMap[j, i] = ItemType.Space;
                    else
                    {
                        switch (line[j].ToString())
                        {
                            case " ":
                                if (wallEncountered)
                                    levelMap[j, i] = ItemType.Floor;
                                else
                                    levelMap[j, i] = ItemType.Space;
                                break;
                            case "#":
                                levelMap[j, i] = ItemType.Wall;
                                wallEncountered = true;
                                break;
                            case "$":
                                levelMap[j, i] = ItemType.Package;
                                break;
                            case ".":
                                levelMap[j, i] = ItemType.Goal;
                                nrOfGoals++;
                                break;
                            case "@":
                                levelMap[j, i] = ItemType.Dragger;
                                break;
                            case "*":
                                levelMap[j, i] = ItemType.PackageOnGoal;
                                nrOfGoals++;
                                break;
                            case "+":
                                levelMap[j, i] = ItemType.DraggerOnGoal;
                                nrOfGoals++;
                                break;
                            case "=":
                                levelMap[j, i] = ItemType.Space;
                                break;
                        }
                    }
                }
            }
            // Add a new level to the collection of levels in the level set.
            _levels.Add(new Level(levelName, levelMap, levelWidth,
                levelHeight, nrOfGoals, levelNr, _title));
        }

        public static ArrayList GetAllLevelSetInfos()
        {
            ArrayList levelSets = new ArrayList();

            // Read current path and remove the 'file:/' from the string
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                .GetName().CodeBase).Substring(6);

            // Read all files from the levels directory
            string[] fileEntries = Directory.GetFiles(path + "/levels");

            // Read the level info from the files with an .xml extension
            foreach (string filename in fileEntries)
            {
                FileInfo fileInfo = new FileInfo(filename);

                if (fileInfo.Extension.Equals(".xml"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filename);

                    string title = doc.SelectSingleNode("//Title").InnerText;
                    string description =
                        doc.SelectSingleNode("//Description").InnerText;
                    XmlNode levelInfo
                        = doc.SelectSingleNode("//LevelCollection");
                    string author = levelInfo.Attributes[0].Value;
                    XmlNodeList levels = doc.SelectNodes("//Level");

                    levelSets.Add(new LevelSet(title, description, levels.Count, filename));
                }
            }

            return levelSets;
        }
        #endregion      
    }
}
