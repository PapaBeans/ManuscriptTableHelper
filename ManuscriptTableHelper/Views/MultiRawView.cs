using ManuscriptTableHelper.DataTypes;
using ManuscriptTableHelper.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ManuscriptTableHelper.Views
{
    public class MultiRawView : IMultiViewType
    {
        public string Name => "MultiRaw";

        public void Initialize(List<Table> tables, Panel container, RichTextBox displayBox)
        {
            // Display raw representations of all tables concatenated
            StringBuilder rawDisplay = new StringBuilder();
            foreach (var table in tables)
            {
                rawDisplay.AppendLine(table.ToRawString());
                rawDisplay.AppendLine(new string('-', 40)); // Separator
            }

            displayBox.Text = rawDisplay.ToString();
        }

        public void Cleanup()
        {
            // No specific cleanup required for MultiRaw
        }
    }
}
