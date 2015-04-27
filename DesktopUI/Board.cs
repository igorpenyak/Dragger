
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using GameEngine;
using Handlers;
using GameEngine.Enums;

namespace DesktopUI
{
    // This class draws everything on screen. It also handles user input.
    public class Board : System.Windows.Forms.Form
    {
        #region Private Fields
        private System.ComponentModel.Container components = null;

        // Initialize the required objects
        // private PlayerData playerData;
        private LevelSet levelSet;        
        private Level level;

        // Objects for drawing graphics on screen
        private FormDrawing draw;
        private PictureBox screen;
        private Label lblPushes;
        private Label lblMoves;
        private Label lblMvs;
        private Label lblPshs;
        private GroupBox grpMoves;
        private Label lblLevelNr;
        private Image img;
        #endregion

        #region Constructor
        public Board()
        {
            InitializeComponent();
            screen = new PictureBox();
            Controls.AddRange(new Control[] { screen });

            InitializeGame();
        }
        #endregion

        #region Drawing UI
        // Sets the data for LevelSet
        private void InitializeGame()
        {
            levelSet = new LevelSet();

            FormLevels formLevels = new FormLevels();
            formLevels.ShowDialog();
            levelSet.SetLevelSet(formLevels.FilenameLevelSet);
            levelSet.CurrentLevel = 1;

            levelSet.SetLevelsInLevelSet(levelSet.Filename);
            level = (Level)levelSet[levelSet.CurrentLevel - 1];
            draw = new FormDrawing(level);
            // Draw the level on the screen
            DrawLevel();
        }

        //sets the width and the height of the screen,
        // according to the level width and height. It then lets the level
        // itself.we set the labels to display the level info.
        private void DrawLevel()
        {
            int levelWidth = (level.Width + 2) * Level.ItemSize;
            int levelHeight = (level.Height + 2) * Level.ItemSize;

            this.ClientSize = new Size(levelWidth + 150, levelHeight);
            screen.Size = new System.Drawing.Size(levelWidth, levelHeight);

            img = draw.DrawLevel();
            screen.Image = img;

            lblLevelNr.Location = new Point(levelWidth, 65);

            grpMoves.Location = new Point(levelWidth + 15, 90);
            lblMvs.Location = new Point(15, 20);
            lblPshs.Location = new Point(15, 36);
            lblMoves.Location = new Point(70, 20);
            lblPushes.Location = new Point(70, 36);

            lblMoves.Text = "0";
            lblPushes.Text = "0";
            lblLevelNr.Text = "Level: " + level.LevelNr;
        }

        // After moving Dragger we only draw the changes on the level, and not
        // redraw the whole level
        private void DrawChanges()
        {
            img = draw.DrawChanges();
            screen.Image = img;

            // Update labels with number of moves and pushes
            lblMoves.Text = level.Moves.ToString();
            lblPushes.Text = level.Pushes.ToString();
        }

        private void DrawUndo()
        {
            if (level.IsUndoable)
            {
                img = draw.Undo();
                screen.Image = img;

                // Update labels with number of moves and pushes
                lblMoves.Text = level.Moves.ToString();
                lblPushes.Text = level.Pushes.ToString();
            }
        }
#endregion

        #region Drager movement
        // Reads input from the keyboard and does something depending on what
        // key is pressed.
        private void AKeyDown(object sender, KeyEventArgs e)
        {
            string result = e.KeyData.ToString();

            switch (result)
            {
                case "Up":
                    MoveDragger(MoveDirection.Up);
                    break;
                case "Down":
                    MoveDragger(MoveDirection.Down);
                    break;
                case "Right":
                    MoveDragger(MoveDirection.Right);
                    break;
                case "Left":
                    MoveDragger(MoveDirection.Left);
                    break;
                case "U":
                    DrawUndo();
                    break;
            }
        }
        private void MoveDragger(MoveDirection direction)
        {
            if (direction == MoveDirection.Up)
                level.MoveDragger(MoveDirection.Up);
            else if (direction == MoveDirection.Down)
                level.MoveDragger(MoveDirection.Down);
            else if (direction == MoveDirection.Right)
                level.MoveDragger(MoveDirection.Right);
            else if (direction == MoveDirection.Left)
                level.MoveDragger(MoveDirection.Left);

            // Draw the changes of the level
            DrawChanges();

            //TODO: If the level is finished we save the number of moves and pushes
            // and the last finished level to the savegame.
            if (level.IsFinished())
            {
                //levelSet.LastFinishedLevel = levelSet.CurrentLevel;
                MessageBox.Show("You did it in  moves " + level.Moves.ToString() + " and " + level.Pushes.ToString() +  " pushes " );

                if (levelSet.CurrentLevel < levelSet.NrOfLevelsInSet)
                {

                    MessageBox.Show("Good job!");
                    levelSet.CurrentLevel++;
                    level = (Level)levelSet[levelSet.CurrentLevel - 1];
                    draw = new FormDrawing(level);
                    DrawLevel();
                }
                else
                {
                    MessageBox.Show("That was the last level!");
                    this.Close();
                }
            }
        }
        #endregion

        #region Windows Form Designer generated code


        [STAThread]
        static void Main()
        {
            Application.Run(new Board());
        }


        private void InitializeComponent()
        {
            this.lblPushes = new System.Windows.Forms.Label();
            this.lblMoves = new System.Windows.Forms.Label();
            this.lblMvs = new System.Windows.Forms.Label();
            this.lblPshs = new System.Windows.Forms.Label();
            this.grpMoves = new System.Windows.Forms.GroupBox();
            this.lblLevelNr = new System.Windows.Forms.Label();
            this.grpMoves.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPushes
            // 
            this.lblPushes.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPushes.ForeColor = System.Drawing.Color.Orange;
            this.lblPushes.Location = new System.Drawing.Point(64, 40);
            this.lblPushes.Name = "lblPushes";
            this.lblPushes.Size = new System.Drawing.Size(44, 16);
            this.lblPushes.TabIndex = 3;
            // 
            // lblMoves
            // 
            this.lblMoves.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMoves.ForeColor = System.Drawing.Color.Orange;
            this.lblMoves.Location = new System.Drawing.Point(72, 24);
            this.lblMoves.Name = "lblMoves";
            this.lblMoves.Size = new System.Drawing.Size(44, 16);
            this.lblMoves.TabIndex = 2;
            // 
            // lblMvs
            // 
            this.lblMvs.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMvs.ForeColor = System.Drawing.Color.White;
            this.lblMvs.Location = new System.Drawing.Point(16, 24);
            this.lblMvs.Name = "lblMvs";
            this.lblMvs.Size = new System.Drawing.Size(52, 16);
            this.lblMvs.TabIndex = 0;
            this.lblMvs.Text = "Moves:";
            // 
            // lblPshs
            // 
            this.lblPshs.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPshs.ForeColor = System.Drawing.Color.White;
            this.lblPshs.Location = new System.Drawing.Point(16, 40);
            this.lblPshs.Name = "lblPshs";
            this.lblPshs.Size = new System.Drawing.Size(52, 16);
            this.lblPshs.TabIndex = 1;
            this.lblPshs.Text = "Pushes:";
            // 
            // grpMoves
            // 
            this.grpMoves.Controls.Add(this.lblPshs);
            this.grpMoves.Controls.Add(this.lblMvs);
            this.grpMoves.Controls.Add(this.lblMoves);
            this.grpMoves.Controls.Add(this.lblPushes);
            this.grpMoves.Location = new System.Drawing.Point(40, 56);
            this.grpMoves.Name = "grpMoves";
            this.grpMoves.Size = new System.Drawing.Size(120, 64);
            this.grpMoves.TabIndex = 4;
            this.grpMoves.TabStop = false;
            // 
            // lblLevelNr
            // 
            this.lblLevelNr.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevelNr.ForeColor = System.Drawing.Color.White;
            this.lblLevelNr.Location = new System.Drawing.Point(168, 48);
            this.lblLevelNr.Name = "lblLevelNr";
            this.lblLevelNr.Size = new System.Drawing.Size(150, 160);
            this.lblLevelNr.TabIndex = 4;
            this.lblLevelNr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(48)))), ((int)(((byte)(12)))));
            this.ClientSize = new System.Drawing.Size(591, 283);
            this.Controls.Add(this.grpMoves);
            this.Controls.Add(this.lblLevelNr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Board";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Drager";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AKeyDown);
            this.grpMoves.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
