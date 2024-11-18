using ManuscriptTableHelper.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManuscriptTableHelper.Interfaces
{
    public interface IMultiViewType
    {
        string Name { get; }
        void Initialize(List<Table> tables, Panel container, RichTextBox displayBox);
        void Cleanup();
    }
}
