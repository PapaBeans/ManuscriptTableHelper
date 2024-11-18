using ManuscriptTableHelper.DataTypes;
using System.Windows.Forms;

namespace ManuscriptTableHelper.Interfaces
{
    public interface IViewType
    {
        string Name { get; }
        void Initialize(Table table, Panel container, RichTextBox displayBox);
        void Cleanup();
    }
}
