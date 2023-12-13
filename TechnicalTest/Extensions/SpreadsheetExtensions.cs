namespace TechnicalTest.Extensions;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

public static class SpreadsheetExtensions
{
    public static object GetCellValue(this SpreadsheetDocument spreadsheetDocument, Row row, int cellIndex)
    {
        ArgumentNullException.ThrowIfNull(spreadsheetDocument);
        ArgumentNullException.ThrowIfNull(row);

        var sharedStringTablePart = spreadsheetDocument.WorkbookPart?.SharedStringTablePart;
        var cell = row.Descendants<Cell>().ElementAt(cellIndex);
        var cellValue = cell.CellValue?.InnerXml ?? string.Empty;

        if (sharedStringTablePart == null || cell.DataType?.Value == null)
        {
            return cellValue;
        }

        // There could be other types of course, but for the purpose of this demo I'll stick to the types we need
        return cell.DataType.Value == CellValues.SharedString
            ? sharedStringTablePart.SharedStringTable.ChildElements[int.Parse(cellValue)].InnerText.ToString()
            : (object)cellValue;
    }
}
