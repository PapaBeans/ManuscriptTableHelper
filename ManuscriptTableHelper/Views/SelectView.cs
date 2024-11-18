using ManuscriptTableHelper.DataTypes;
using ManuscriptTableHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManuscriptTableHelper.Views
{
    public class SelectView : IViewType
    {
        public string Name => "SelectView";

        private ComboBox keyComboBox;
        private RichTextBox displayBox;

        public void Initialize(Table table, Panel container, RichTextBox displayBox)
        {
            this.displayBox = displayBox;

            // Key ComboBox
            keyComboBox = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            container.Controls.Add(keyComboBox);

            foreach (var row in table.Rows)
            {
                keyComboBox.Items.Add(row.Key.Value);
            }

            keyComboBox.SelectedIndexChanged += (s, e) =>
            {
                if (keyComboBox.SelectedIndex >= 0)
                {
                    string selectedKey = keyComboBox.SelectedItem.ToString();
                    var matchingRow = table.Rows.Find(row => row.Key.Value == selectedKey);
                    displayBox.Text = matchingRow.Value;
                }
            };

            if (keyComboBox.Items.Count > 0)
            {
                keyComboBox.SelectedIndex = 0;
            }
        }

        public void Cleanup()
        {
            keyComboBox?.Dispose();
        }
    }
}
