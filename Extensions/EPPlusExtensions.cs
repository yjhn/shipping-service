using OfficeOpenXml;

namespace shipping_service.Extensions
{
    public static class EPPlusExtensions
    {

        public static int GetColumnByName(this ExcelWorksheet ws, string columnName)
        {
            if (ws == null) throw new ArgumentNullException(nameof(ws));
            return ws.Cells["1:1"].First(c => c.Value.ToString() == columnName).Start.Column;
        }

    }
}
