using ManuscriptTableHelper.DataTypes;
using ManuscriptTableHelper.Interfaces;
using System.Windows.Forms;

namespace ManuscriptTableHelper.Views
{
    public class RawView : IViewType
    {
        public string Name => "Raw";

        public void Initialize(Table table, Panel container, RichTextBox displayBox)
        {
            // Display the raw representation of the table in the RichTextBox
            displayBox.Text = table.ToRawString();
        }

        public void Cleanup()
        {
            // No additional cleanup is necessary
        }
    }
}