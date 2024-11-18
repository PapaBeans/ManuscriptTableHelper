using System.Windows.Forms;

namespace ManuscriptTableHelper
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadButton = new Button();
            this.tableListView = new ListView();
            this.displayModeComboBox = new ComboBox();
            this.displayTextBox = new RichTextBox();
            this.dynamicControlsPanel = new Panel();

            // Load Button
            this.loadButton.Text = "Load Table";
            this.loadButton.Dock = DockStyle.Top;
            this.loadButton.Click += LoadButton_Click;

            // Table ListView
            this.tableListView.View = View.List;
            this.tableListView.Dock = DockStyle.Left;
            this.tableListView.Width = 200;
            this.tableListView.MultiSelect = true;
            this.tableListView.SelectedIndexChanged += TableListView_SelectedIndexChanged;

            // Display Mode ComboBox
            this.displayModeComboBox.Dock = DockStyle.Top;
            this.displayModeComboBox.SelectedIndexChanged += DisplayModeComboBox_SelectedIndexChanged;

            // Dynamic Controls Panel
            this.dynamicControlsPanel.Dock = DockStyle.Top;
            this.dynamicControlsPanel.Height = 40;

            // Display TextBox
            this.displayTextBox.Dock = DockStyle.Fill;
            this.displayTextBox.Font = new System.Drawing.Font("Consolas", 10);

            // Main Form
            this.Text = "Custom Table Viewer";
            this.Size = new System.Drawing.Size(800, 600);

            // Add Controls
            this.Controls.Add(this.displayTextBox);
            this.Controls.Add(this.dynamicControlsPanel);
            this.Controls.Add(this.displayModeComboBox);
            this.Controls.Add(this.tableListView);
            this.Controls.Add(this.loadButton);
        }

        #endregion
    }
}

