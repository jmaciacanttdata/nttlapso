using NTTLapso.Models.DataDump;
using OfficeOpenXml;

namespace NTTLapso.Tools
{
    public class ExcelExtractor
    {
        public List<T> GetList<T>(ExcelWorksheet sheet, Dictionary<string, Tuple<string, bool>> columnas)
        {
            List<T> list = new List<T>();
            List<string> columnInfo = Enumerable.Range(1, sheet.Dimension.Columns).Select(n => sheet.Cells[1, n].Value.ToString()).ToList();

            for (int row = 2; row < sheet.Dimension.Rows + 1; row++)
            {
                T obj = (T)Activator.CreateInstance(typeof(T));
                foreach (var column in columnas.Where(c => c.Value.Item2))
                {
                    string columnName = column.Key;
                    string propertyName = column.Value.Item1;

                    int col = columnInfo.IndexOf(columnName) + 1;
                    // Console.WriteLine(columnName + " - " + propertyName);
                    var val = sheet.Cells[row, col].Value;
                    Type t = obj.GetType();
                    var prop = t.GetProperty(propertyName);
                    if (val != null)
                    {
                        prop.SetValue(obj, val.ToString());
                    }

                }
                list.Add(obj);
            }

            return list;
        }
    }
}
