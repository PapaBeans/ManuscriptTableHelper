using ManuscriptTableHelper.DataTypes;
using ManuscriptTableHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ManuscriptTableHelper.Views
{
    public class MultiSelectView : IMultiViewType
    {
        public string Name => "MultiSelect";

        private ComboBox keyComboBox;
        private RichTextBox displayBox;

        public void Initialize(List<Table> tables, Panel container, RichTextBox displayBox)
        {
            this.displayBox = displayBox;

            // Create a ComboBox for selecting keys
            keyComboBox = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            container.Controls.Add(keyComboBox);

            // Get all unique keys across tables
            var allKeys = new HashSet<string>();
            foreach (var table in tables)
            {
                foreach (var row in table.Rows)
                {
                    allKeys.Add(row.Key.Value);
                }
            }

            // Populate the ComboBox with sorted keys
            foreach (var key in allKeys.OrderBy(k => k))
            {
                keyComboBox.Items.Add(key);
            }

            // Handle ComboBox selection change
            keyComboBox.SelectedIndexChanged += (s, e) =>
            {
                if (keyComboBox.SelectedIndex >= 0)
                {
                    string selectedKey = keyComboBox.SelectedItem.ToString();
                    DisplayRowsForKey(tables, selectedKey);
                }
            };

            // Select the first key if available
            if (keyComboBox.Items.Count > 0)
            {
                keyComboBox.SelectedIndex = 0;
            }
        }

        private void DisplayRowsForKey(List<Table> tables, string key)
        {
            var result = new List<string>();

            foreach (var table in tables)
            {
                var matchingRow = table.Rows.FirstOrDefault(row => row.Key.Value == key);

                if (matchingRow.Key != null)
                {
                    result.Add($"Table: {table.Id} -> {matchingRow.Value}");
                }
                else
                {
                    result.Add($"Table: {table.Id} -> Key '{key}' not found");
                }
            }

            // Update the display box
            displayBox.Text = string.Join(Environment.NewLine, result);
        }

        public void Cleanup()
        {
            keyComboBox?.Dispose();
        }
    }
}
