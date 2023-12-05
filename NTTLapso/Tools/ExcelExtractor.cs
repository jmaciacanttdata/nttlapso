using NTTLapso.Models.DataDump;
using OfficeOpenXml;


namespace NTTLapso.Tools
{

    public class ExcelExtractorParsers
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
        private T GetRowData<T>(int row, ExcelWorksheet sheet, List<Tuple<bool, string, string, Func<string, string>?>> columnas)
        {
            List<string> columnInfo = Enumerable.Range(1, sheet.Dimension.Columns).Select(n => sheet.Cells[1, n].Value.ToString()).ToList();

            T obj = (T)Activator.CreateInstance(typeof(T));
            var validColumns = columnas.Where(c => c.Item1);

            foreach (var column in validColumns)
            {
                string columnName = column.Item2;
                string propName = column.Item3;

                int col = columnInfo.IndexOf(columnName) + 1;

                var val = sheet.Cells[row, col].Value;

                if (val != null)
                {
                    Type t = obj.GetType();
                    var prop = t.GetProperty(propName);
                    if(prop != null)
                    {
                        string valString = val.ToString();
                        string parsedVal = (column.Item4 == null) ? valString : column.Item4(val.ToString());
                        prop.SetValue(obj, parsedVal.ToString());
                    }
                }
            }

            return obj;
        }

        public List<T> GetDataAsList<T>(ExcelWorksheet sheet, List<Tuple<bool, string, string, Func<string, string>?>> columnas)
        {
            List<T> data = new List<T>();
            for (int row = 2; row < sheet.Dimension.Rows + 1; row++)
            {
                if(sheet.Cells[row, 1] == null || sheet.Cells[row, 1].Value == null)
                {
                    continue;
                }
                else
                {
                    data.Add(GetRowData<T>(row, sheet, columnas));
                }
            }

            return data;
        }
    }
}
