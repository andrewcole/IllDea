using iTextSharp.text.pdf;

namespace Illallangi.IllDea.Pdf
{
    public static class PdfPTableExtensions
    {   
        public static PdfCellOperation AddPageHeaderCell(this PdfPTable table, params string[] args)
        {
            return new PdfCellOperation(table).AddPageHeaderCell(args);
        }

        public static PdfCellOperation AddColumnHeaderCell(this PdfPTable table, params string[] args)
        {
            return new PdfCellOperation(table).AddColumnHeaderCell(args);
        }

        public static PdfCellOperation AddHeaderCell(this PdfPTable table, params string[] args)
        {
            return new PdfCellOperation(table).AddHeaderCell(args);
        }

        public static PdfCellOperation AddBodyCell(this PdfPTable table, params string[] args)
        {
            return new PdfCellOperation(table).AddBodyCell(args);
        }
    }
}
