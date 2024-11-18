using ManuscriptTableHelper.Interfaces;
using System;
using System.Collections.Generic;

namespace ManuscriptTableHelper.Views
{
    public class ViewManager
    {
        private readonly Dictionary<string, IViewType> singleViewTypes = new Dictionary<string, IViewType>();
        private readonly Dictionary<string, IMultiViewType> multiViewTypes = new Dictionary<string, IMultiViewType>();

        public void RegisterViewType(IViewType viewType)
        {
            if (!singleViewTypes.ContainsKey(viewType.Name))
            {
                singleViewTypes.Add(viewType.Name, viewType);
            }
        }

        public void RegisterMultiViewType(IMultiViewType viewType)
        {
            if (!multiViewTypes.ContainsKey(viewType.Name))
            {
                multiViewTypes.Add(viewType.Name, viewType);
            }
        }

        public bool IsSingleViewType(string name)
        {
            return singleViewTypes.ContainsKey(name);
        }

        public bool IsMultiViewType(string name)
        {
            return multiViewTypes.ContainsKey(name);
        }

        public IViewType GetSingleViewType(string name)
        {
            if (singleViewTypes.TryGetValue(name, out var viewType))
            {
                return viewType;
            }
            throw new ArgumentException($"Single-table view type '{name}' not found.");
        }

        public IMultiViewType GetMultiViewType(string name)
        {
            if (multiViewTypes.TryGetValue(name, out var viewType))
            {
                return viewType;
            }
            throw new ArgumentException($"Multi-table view type '{name}' not found.");
        }

        public IEnumerable<string> GetViewTypeNames()
        {
            var allNames = new HashSet<string>(singleViewTypes.Keys);
            allNames.UnionWith(multiViewTypes.Keys);
            return allNames;
        }
    }

}
