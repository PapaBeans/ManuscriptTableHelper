using ManuscriptTableHelper.DataTypes;
using ManuscriptTableHelper.Interfaces;
using ManuscriptTableHelper.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ManuscriptTableHelper
{
    public partial class Form1 : Form
    {
        private Button loadButton;
        private ListView tableListView;
        private ComboBox displayModeComboBox;
        private RichTextBox displayTextBox;
        private Panel dynamicControlsPanel;

        private List<Table> tables = new List<Table>();
        private ViewManager viewManager = new ViewManager();
        private IViewType currentViewType;

        public Form1()
        {
            InitializeComponent();
            InitializeViewManager();

            foreach (var viewTypeName in viewManager.GetViewTypeNames())
            {
                this.displayModeComboBox.Items.Add(viewTypeName);
            }
            this.displayModeComboBox.SelectedIndex = 0;
        }

        private void InitializeViewManager()
        {
            viewManager.RegisterViewType(new RawView());
            viewManager.RegisterViewType(new SelectView());

            //Multi views for when we want/need to display more than one table at a time
            viewManager.RegisterMultiViewType(new MultiRawView());
            viewManager.RegisterMultiViewType(new MultiSelectView());
            viewManager.RegisterMultiViewType(new MultiSignatureView());

        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Select a Table File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileContent = File.ReadAllText(openFileDialog.FileName);

                    // Parse multiple tables
                    List<Table> parsedTables = TableParser.ParseTables(fileContent);
                    foreach (var table in parsedTables)
                    {
                        tables.Add(table);
                        tableListView.Items.Add(table.Id);
                    }

                    if (parsedTables.Count == 0)
                    {
                        MessageBox.Show("No tables found in the file.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading tables: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TableListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tableListView.SelectedItems.Count == 0)
            {
                displayTextBox.Clear();
                return;
            }

            string selectedViewType = displayModeComboBox.SelectedItem.ToString();
            var selectedTables = new List<Table>();

            // Retrieve selected tables
            foreach (ListViewItem item in tableListView.SelectedItems)
            {
                string selectedId = item.Text;
                var table = tables.Find(t => t.Id == selectedId);
                if (table != null)
                {
                    selectedTables.Add(table);
                }
            }

            // Check if the current view type is single or multi
            if (viewManager.IsSingleViewType(selectedViewType))
            {
                // Single view: Use only the first selected table
                if (selectedTables.Count > 0)
                {
                    UpdateSingleView(selectedTables[0]);
                }
                else
                {
                    MessageBox.Show("No table selected for the single-table view.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (viewManager.IsMultiViewType(selectedViewType))
            {
                // Multi view: Use all selected tables
                UpdateMultiView(selectedTables);
            }
            else
            {
                MessageBox.Show("Invalid view type selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tableListView.SelectedItems.Count == 0)
            {
                displayTextBox.Clear();
                return;
            }

            string selectedViewType = displayModeComboBox.SelectedItem.ToString();
            var selectedTables = new List<Table>();

            // Retrieve selected tables
            foreach (ListViewItem item in tableListView.SelectedItems)
            {
                string selectedId = item.Text;
                var table = tables.Find(t => t.Id == selectedId);
                if (table != null)
                {
                    selectedTables.Add(table);
                }
            }

            // Check if the selected view type is single or multi
            if (viewManager.IsSingleViewType(selectedViewType))
            {
                // Single view: use only the first selected table
                if (selectedTables.Count > 0)
                {
                    UpdateSingleView(selectedTables[0]);
                }
                else
                {
                    MessageBox.Show("No table selected for the single-table view.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (viewManager.IsMultiViewType(selectedViewType))
            {
                // Multi view: use all selected tables
                UpdateMultiView(selectedTables);
            }
            else
            {
                MessageBox.Show("Invalid view type selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSingleView(Table table)
        {
            if (currentViewType != null)
            {
                currentViewType.Cleanup();
            }

            string selectedViewType = displayModeComboBox.SelectedItem.ToString();
            currentViewType = viewManager.GetSingleViewType(selectedViewType);
            currentViewType.Initialize(table, dynamicControlsPanel, displayTextBox);
        }

        private void UpdateMultiView(List<Table> tables)
        {
            if (currentViewType != null)
            {
                currentViewType.Cleanup();
            }

            string selectedViewType = displayModeComboBox.SelectedItem.ToString();
            var multiViewType = viewManager.GetMultiViewType(selectedViewType);
            if (multiViewType != null)
            {
                multiViewType.Initialize(tables, dynamicControlsPanel, displayTextBox);
            }
            else
            {
                MessageBox.Show("The selected view type does not support multiple tables.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
