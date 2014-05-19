using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace Illallangi.IllDea.Pdf
{
    public sealed class PdfCellOperation
    {
        #region Fields

        /// <summary>
        /// Holds the current value of the FontSelection property.
        /// </summary>
        private static FontSelection staticFontSelection;

        /// <summary>
        /// Holds the current value of the Table property.
        /// </summary>
        private readonly PdfPTable currentTable;

        /// <summary>
        /// Holds the current value of the Args property.
        /// </summary>
        private IList<string> currentArgs;

        #endregion

        #region Constructor

        public PdfCellOperation(PdfPTable table)
        {
            this.currentTable = table;
        }

        #endregion

        #region Properties

        private static FontSelection FontSelection
        {
            get
            {
                return PdfCellOperation.staticFontSelection ?? (PdfCellOperation.staticFontSelection = new FontSelection());
            }
        }

        private int Alignment { get; set; }
        
        private ICollection<string> Args
        {
            get { return this.currentArgs ?? (this.currentArgs = new List<string>()); }
        }
        
        private BaseColor BackgroundColor { get; set; }

        private int Border { get; set; }

        private int Colspan { get; set; }

        private Font Font { get; set; }

        private float MinimumHeight { get; set; }

        private bool TabStops { get; set; }

        private PdfPTable Table
        {
            get { return this.currentTable; }
        }

        #endregion

        #region Methods

        #region Add Cell Methods

        public PdfCellOperation AddPageHeaderCell(params string[] args)
        {
            return this
                .AddBodyCell(args)
                .WithFont(PdfCellOperation.FontSelection.Header1)
                .WithColspan(this.Table.NumberOfColumns)
                .WithBorder(Rectangle.BOTTOM_BORDER);
        }

        public PdfCellOperation AddColumnHeaderCell(params string[] args)
        {
            return this
                .AddHeaderCell(args)
                .CenterAligned();
        }

        public PdfCellOperation AddHeaderCell(params string[] args)
        {
            return this
                .AddBodyCell(args)
                .WithFont(PdfCellOperation.FontSelection.Bold);
        }

        public PdfCellOperation AddBodyCell(params string[] args)
        {
            foreach (var arg in args)
            {
                this.Args.Add(arg);
            }

            return this
                .LeftAligned()
                .WithBackgroundColor(BaseColor.WHITE)
                .WithBorder(Rectangle.NO_BORDER)
                .WithColspan(1)
                .WithFont(PdfCellOperation.FontSelection.Body)
                .WithMinimumHeight(13f)
                .WithTabStops(false);
        }
        
        #endregion
        
        #region Modifier Methods

        public PdfCellOperation Inverted()
        {
            return this
                .WithBackgroundColor(BaseColor.LIGHT_GRAY);
        }

        #endregion

        #region Go Method

        public PdfPTable Go()
        {
            var paragraph = new Paragraph();

            paragraph.AddAll(this.Args.Select(a => new Chunk(a, this.Font)).ToList());

            if (this.TabStops)
            {
                paragraph.Add(new Chunk(new DottedLineSeparator { Alignment = Element.ALIGN_RIGHT }));
            }

            this.Table.AddCell(new PdfPCell(paragraph)
            {
                Colspan = this.Colspan,
                MinimumHeight = this.MinimumHeight,
                BackgroundColor = this.BackgroundColor,
                Border = this.Border,
                HorizontalAlignment = this.Alignment,
            });

            return this.Table;
        }

        #endregion

        #region Property Methods

        public PdfCellOperation LeftAligned()
        {
            this.Alignment = Element.ALIGN_LEFT;
            return this;
        }
        public PdfCellOperation CenterAligned()
        {
            this.Alignment = Element.ALIGN_CENTER;
            return this;
        }
        
        private PdfCellOperation WithBackgroundColor(BaseColor backgroundColor)
        {
            this.BackgroundColor = backgroundColor;
            return this;
        }

        private PdfCellOperation WithBorder(int border)
        {
            this.Border = border;
            return this;
        }

        private PdfCellOperation WithColspan(int colspan)
        {
            this.Colspan = colspan;
            return this;
        }

        private PdfCellOperation WithFont(Font font)
        {
            this.Font = font;
            return this;
        }

        private PdfCellOperation WithMinimumHeight(float minimumHeight)
        {
            this.MinimumHeight = minimumHeight;
            return this;
        }

        public PdfCellOperation WithTabStops(bool withTabStops = true)
        {
            this.TabStops = withTabStops;
            return this;
        }

        #endregion

        #endregion
    }
}
