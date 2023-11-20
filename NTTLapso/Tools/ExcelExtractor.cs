using NTTLapso.Models.DataDump;
using OfficeOpenXml;

namespace NTTLapso.Tools
{
    public class ExcelExtractorFilters
    {
        public static string FilterDate(string value)
        {
            DateTime dateFilt;
            return (DateTime.TryParse(value, out dateFilt)) ? dateFilt.ToShortDateString() : value;
        }

        public static string FilterText(string value)
        {
            var filt = value.Replace("\'", "\'\'");
            return filt;
        }
    }

    public class ExcelExtractor
    {
        private string GetRowData(int row, ExcelWorksheet sheet, Dictionary<string, Tuple<bool, Func<string, string>?>> columnas)
        {
            List<string> columnInfo = Enumerable.Range(1, sheet.Dimension.Columns).Select(n => sheet.Cells[1, n].Value.ToString()).ToList();

            string rowInsert = "(";

            foreach (var column in columnas.Where(c => c.Value.Item1))
            {
                string columnName = column.Key;

                int col = columnInfo.IndexOf(columnName) + 1;

                var val = sheet.Cells[row, col].Value;

                rowInsert += "'";

                if (val != null)
                {
                    rowInsert += column.Value.Item2 == null ? val : column.Value.Item2(val.ToString());
                }
                else
                {
                    rowInsert += "";
                }

                rowInsert += "'" + ((column.Key != columnas.Last().Key) ? "," : "");
            }
            rowInsert += "),";

            return rowInsert;
        }

        public string GetDataAsInsertQuery(ExcelWorksheet sheet, Dictionary<string, Tuple<bool, Func<string, string>?>> columnas)
        {
            
            string data = "";
            

            for (int row = 2; row < sheet.Dimension.Rows + 1; row++)
            {
                if(sheet.Cells[row, 1] == null || sheet.Cells[row, 1].Value == null)
                {
                    continue;
                }
                else
                {
                    data += GetRowData(row, sheet, columnas);
                }
            }

            return (data.ElementAt(data.Length - 1) == ',') ? data.Remove(data.Length - 1): data;
        }
    }
}
