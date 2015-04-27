using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using GameEngine;

namespace DesktopUI
{
    public class FormLevels : System.Windows.Forms.Form
    {
        #region Private fields
        private System.Windows.Forms.ListBox lstLevelSets;
        private System.Windows.Forms.Button btnSelect;

        private ArrayList levelSets = new ArrayList();
        private string filenameLevelSet = string.Empty;
        private Label lblChoose;
        private string nameLevelSet = string.Empty;
        #endregion

        #region Constructor
        public FormLevels()
        {
            InitializeComponent();

            // Loads the information of all level sets
            levelSets = LevelSet.GetAllLevelSetInfos();

            // Adds the title of each level set to the listbox
            foreach (LevelSet levelSet in levelSets)
                lstLevelSets.Items.Add(levelSet.Title);

            lstLevelSets.SelectedIndex = 0;
        }

        private void btnSelect_Click(object sender, System.EventArgs e)
        {
            nameLevelSet = lstLevelSets.SelectedItem.ToString();

            foreach (LevelSet levelSet in levelSets)
            {
                if (levelSet.Title == nameLevelSet)
                {
                    filenameLevelSet = levelSet.Filename;
                    break;
                }
            }
            this.Close();
        }
        #endregion

        #region Properties
        public string FilenameLevelSet
        {
            get { return filenameLevelSet; }
        }
        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lstLevelSets = new System.Windows.Forms.ListBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.lblChoose = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstLevelSets
            // 
            this.lstLevelSets.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLevelSets.Location = new System.Drawing.Point(12, 71);
            this.lstLevelSets.Name = "lstLevelSets";
            this.lstLevelSets.Size = new System.Drawing.Size(193, 69);
            this.lstLevelSets.TabIndex = 0;
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.Location = new System.Drawing.Point(12, 186);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(193, 79);
            this.btnSelect.TabIndex = 12;
            this.btnSelect.Text = "Start game";
            this.btnSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lblChoose
            // 
            this.lblChoose.AutoSize = true;
            this.lblChoose.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChoose.Location = new System.Drawing.Point(9, 20);
            this.lblChoose.Name = "lblChoose";
            this.lblChoose.Size = new System.Drawing.Size(196, 18);
            this.lblChoose.TabIndex = 13;
            this.lblChoose.Text = "Please,choose Level set!";
            // 
            // FormLevels
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(338, 277);
            this.ControlBox = false;
            this.Controls.Add(this.lblChoose);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lstLevelSets);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormLevels";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Levels";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

    }
}

