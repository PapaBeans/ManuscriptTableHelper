using ManuscriptTableHelper.DataTypes;
using ManuscriptTableHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManuscriptTableHelper.Views
{
    public class MultiSignatureView : IMultiViewType
    {
        public string Name => "SignatureBoxView";

        private ComboBox keyComboBox;
        private CheckBox decodeCheckbox;
        private TextBox editorBox;
        private RichTextBox displayBox;

        private List<Table> tables;
        private bool decodeText = false;

        public void Initialize(List<Table> tables, Panel container, RichTextBox displayBox)
        {
            this.tables = tables;
            this.displayBox = displayBox;

            // ComboBox for selecting keys
            keyComboBox = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            container.Controls.Add(keyComboBox);

            // Populate ComboBox with unique keys
            var allKeys = new HashSet<string>();
            foreach (var table in tables)
            {
                foreach (var row in table.Rows)
                {
                    allKeys.Add(row.Key.Value);
                }
            }

            foreach (var key in allKeys.OrderBy(k => k))
            {
                keyComboBox.Items.Add(key);
            }

            // Checkbox for decoding text
            decodeCheckbox = new CheckBox
            {
                Dock = DockStyle.Top,
                Text = "Decode Text",
                Checked = false
            };
            decodeCheckbox.CheckedChanged += (s, e) =>
            {
                decodeText = decodeCheckbox.Checked;
                if (keyComboBox.SelectedIndex >= 0)
                {
                    DisplayRowsForKey(keyComboBox.SelectedItem.ToString());
                }
            };
            container.Controls.Add(decodeCheckbox);

            // TextBox for displaying and editing row values
            editorBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            container.Controls.Add(editorBox);

            // Handle ComboBox selection change
            keyComboBox.SelectedIndexChanged += (s, e) =>
            {
                if (keyComboBox.SelectedIndex >= 0)
                {
                    DisplayRowsForKey(keyComboBox.SelectedItem.ToString());
                }
            };

            // Select the first key if available
            if (keyComboBox.Items.Count > 0)
            {
                keyComboBox.SelectedIndex = 0;
            }
        }

        private void DisplayRowsForKey(string key)
        {
            var result = new StringBuilder();

            foreach (var table in tables)
            {
                var matchingRow = table.Rows.FirstOrDefault(row => row.Key.Value == key);

                if (matchingRow.Key != null)
                {
                    var value = decodeText ? DecodeText(matchingRow.Value) : matchingRow.Value;
                    result.AppendLine($"Table: {table.Id} -> {value}");
                }
                else
                {
                    result.AppendLine($"Table: {table.Id} -> Key '{key}' not found");
                }
            }

            displayBox.Text = result.ToString();

            // Show the decoded or encoded value of the first table for editing
            var firstMatchingRow = tables
                .SelectMany(t => t.Rows)
                .FirstOrDefault(row => row.Key.Value == key);

            if (firstMatchingRow.Key != null)
            {
                editorBox.Text = decodeText ? DecodeText(firstMatchingRow.Value) : firstMatchingRow.Value;
            }
            else
            {
                editorBox.Text = "Key not found in selected tables.";
            }
        }

        private string DecodeText(string text)
        {
            return Regex.Replace(text, @"&#(\d+);|&#x([A-Fa-f0-9]+);", match =>
            {
                if (match.Groups[1].Success) // Decimal entity
                {
                    return char.ConvertFromUtf32(int.Parse(match.Groups[1].Value));
                }
                else if (match.Groups[2].Success) // Hexadecimal entity
                {
                    return char.ConvertFromUtf32(Convert.ToInt32(match.Groups[2].Value, 16));
                }
                return match.Value;
            });
        }

        private string EncodeText(string text)
        {
            var encoded = new StringBuilder();
            foreach (var c in text)
            {
                switch (c)
                {
                    case ' ':
                        encoded.Append("&#32;");
                        break;
                    case '\n':
                        encoded.Append("&#xA;");
                        break;
                    case '\r':
                        encoded.Append("&#xD;");
                        break;
                    default:
                        // Encode non-ASCII and special characters
                        if (char.IsControl(c) || c == '<' || c == '>' || c == '&' || c > 127)
                        {
                            encoded.Append($"&#{(int)c};");
                        }
                        else
                        {
                            encoded.Append(c);
                        }
                        break;
                }
            }
            return encoded.ToString();
        }

        public void Cleanup()
        {
            keyComboBox?.Dispose();
            decodeCheckbox?.Dispose();
            editorBox?.Dispose();
        }
    }
}
