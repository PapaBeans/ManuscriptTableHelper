using ManuscriptTableHelper.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ManuscriptTableHelper
{
    public static class TableParser
    {
        public static List<Table> ParseTables(string input)
        {
            List<Table> tables = new List<Table>();

            // Use regex to extract each <table> block, handling tables with or without trailing spaces/newlines
            var tableMatches = Regex.Matches(input, @"<table.*?>.*?<\/table>", RegexOptions.Singleline);

            foreach (Match match in tableMatches)
            {
                string tableContent = match.Value;
                tables.Add(ParseTable(tableContent));
            }

            return tables;
        }

        public static Table ParseTable(string input)
        {
            Table table = new Table();

            // Match table attributes
            var tableMatch = Regex.Match(input, @"<table id=""(.*?)"" tableType=""(.*?)"" separator=""(.*?)"">");
            if (!tableMatch.Success)
                throw new FormatException("Invalid or missing <table> tag.");

            table.Id = tableMatch.Groups[1].Value;
            table.TableType = tableMatch.Groups[2].Value;
            table.Separator = tableMatch.Groups[3].Value;

            // Match fields
            var fieldMatches = Regex.Matches(input, @"<field type=""(.*?)"" name=""(.*?)"" />");
            foreach (Match match in fieldMatches)
            {
                table.Fields.Add(new Field
                {
                    Type = match.Groups[1].Value,
                    Name = match.Groups[2].Value
                });
            }

            // Match row keys
            var keyMatches = Regex.Matches(input, @"<key value=""(.*?)""( default=""1"")?( caption=""(.*?)"")? />");
            List<Key> keys = new List<Key>();
            foreach (Match match in keyMatches)
            {
                keys.Add(new Key
                {
                    Value = match.Groups[1].Value,
                    IsDefault = !string.IsNullOrEmpty(match.Groups[2].Value),
                    Caption = match.Groups[4].Value
                });
            }

            // Match data rows
            var rowMatches = Regex.Matches(input, @"<row value=""(.*?)"" />");
            if (keys.Count != rowMatches.Count)
                throw new FormatException("Mismatch between the number of keys and rows.");

            for (int i = 0; i < keys.Count; i++)
            {
                table.Rows.Add(new KeyValuePair<Key, string>(keys[i], rowMatches[i].Groups[1].Value));
            }

            return table;
        }
    }
}
