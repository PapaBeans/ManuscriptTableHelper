using System.Collections.Generic;

namespace ManuscriptTableHelper.DataTypes
{
    public class Table
    {
        public string Id { get; set; }
        public string TableType { get; set; }
        public string Separator { get; set; }
        public List<Field> Fields { get; set; } = new List<Field>();
        public List<KeyValuePair<Key, string>> Rows { get; set; } = new List<KeyValuePair<Key, string>>();

        public string ToRawString()
        {
            string raw = $"<table id=\"{Id}\" tableType=\"{TableType}\" separator=\"{Separator}\">\n";
            raw += "  <fields>\n";
            foreach (var field in Fields)
            {
                raw += $"    <field type=\"{field.Type}\" name=\"{field.Name}\" />\n";
            }
            raw += "  </fields>\n";

            raw += "  <rowKeys>\n";
            foreach (var row in Rows)
            {
                var key = row.Key;
                string defaultAttr = key.IsDefault ? " default=\"1\"" : "";
                string captionAttr = string.IsNullOrEmpty(key.Caption) ? "" : $" caption=\"{key.Caption}\"";
                raw += $"    <key value=\"{key.Value}\"{defaultAttr}{captionAttr} />\n";
            }
            raw += "  </rowKeys>\n";

            raw += "  <data>\n";
            foreach (var row in Rows)
            {
                raw += $"    <row value=\"{row.Value}\" />\n";
            }
            raw += "  </data>\n</table>\n";

            return raw;
        }
    }
}
