using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.QrCode.Internal;

namespace PrintSolution
{
    public class PrintClass : IDisposable
    {
        public PrinterSettings printerSetting;
        public PageSettings pageSettings;
        //public PrintDocument pd;

        public Font printFont;

        
        ~PrintClass()
        {
            Dispose();
        }

        
        public struct PrintDetail
        {
            public string Name;
            public Point Location;
            public Size Size;
            public StringFormat NameFormat;
            //NameFormat.Alignment = System.Drawing.StringAlignment.Center;
            //NameFormat.LineAlignment = System.Drawing.StringAlignment.Center;
            public Font font;
            public Type type;
            public Image image;
            public string Text;
            public bool PrintBorder;
        }

        public enum Type
        {
            Text = 0,
            Image = 1
        }


        public PrintClass(PrintDetail printDetail)
        {
            printDetails.Add(printDetail);
        }
        public PrintClass(PrintDetail[] printDetail)
        {
            printDetails.AddRange(printDetail);
        }

        public List<PrintDetail> printDetails = new List<PrintDetail>();

        public void Dispose()
        {
            if (printerSetting != null)
            {
                printerSetting = null;
            }

            if (pageSettings != null)
            {
                pageSettings = null;
            }
            
            if (printDetails != null)
            {
                for (int i = 0; i < printDetails.Count; i++)
                {
                    if (printDetails[i].image != null)
                        printDetails[i].image.Dispose();
                }
            }

            //if (TemplateImage != null)
            //{
            //    TemplateImage.Dispose();
            //}
            /*
            if (rptID1 != null)
            {
                rptID1.Dispose();
            }*/

        }
        
        private void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            string line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            //A4 paper are 210mm × 297mm or 8.27in × 11.69in
            //4960 x 7016 pixels
            
            //ev.Graphics.DrawRectangle(new System.Drawing.Pen(Color.Black), PageStart.X, PageStart.Y, PagesSize.Width, PagesSize.Height);
            /*
            //Template
            if ((!withHeaderFooter))
                ev.Graphics.DrawImage(TemplateImage, RectangleF.FromLTRB(0, 0, 300f, 187.5f));

            //Header
            if (withHeaderFooter)
                ev.Graphics.DrawImage(Image.FromFile(Application.StartupPath + "\\jH.jpg"), RectangleF.FromLTRB(0, 0, 300, 65));
            */

            //QR.Code
            
            foreach(var pd in printDetails)
            {
                if (pd.type == Type.Image)
                {
                    Point Image_Location = pd.Location;
                    Size Image_Size = pd.Size;

                    if (pd.image == null)
                    using (var image = Image.FromFile(pd.Text))
                    {
                        if (pd.PrintBorder)
                            ev.Graphics.DrawRectangle(new System.Drawing.Pen(Color.Black), Image_Location.X, Image_Location.Y, Image_Size.Width, Image_Size.Height);
                        ev.Graphics.DrawImage(image, RectangleF.FromLTRB(Image_Location.X, Image_Location.Y, Image_Location.X + Image_Size.Width, Image_Location.Y + Image_Size.Height));
                    }
                    else
                    {
                        if (pd.PrintBorder)
                            ev.Graphics.DrawRectangle(new System.Drawing.Pen(Color.Black), Image_Location.X, Image_Location.Y, Image_Size.Width, Image_Size.Height);
                        ev.Graphics.DrawImage(pd.image, RectangleF.FromLTRB(Image_Location.X, Image_Location.Y, Image_Location.X + Image_Size.Width, Image_Location.Y + Image_Size.Height));
                    }
                }
                else if (pd.type == Type.Text)
                {
                    //ID
                    Point Text_Location = pd.Location;
                    Size Text_Size = pd.Size;
                    var f = (pd.font != null) ? pd.font : new Font("Arial",7);
                    var rec = RectangleF.FromLTRB(Text_Location.X, Text_Location.Y, Text_Location.X + Text_Size.Width, Text_Location.Y + Text_Size.Height);
                    StringFormat NameFormat = pd.NameFormat;
                    //NameFormat.Alignment = System.Drawing.StringAlignment.Center;
                    //NameFormat.LineAlignment = System.Drawing.StringAlignment.Center;

                    if (pd.PrintBorder)
                        ev.Graphics.DrawRectangle(new System.Drawing.Pen(Color.Black), Text_Location.X, Text_Location.Y, Text_Size.Width, Text_Size.Height);
                    ev.Graphics.DrawString(pd.Text, f, Brushes.Black, rec, NameFormat);
                }
            }            
            
            // If more lines exist, print another page.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }
        
        public async Task<bool> Print()
        {

            try
            {
                PrintDocument pd = new PrintDocument();
                pd.OriginAtMargins = true;

                //System.IO.StreamReader streamToPrint = new System.IO.StreamReader
                //   (Application.StartupPath + "\\MyFile.txt");
                try
                {

                    printFont = new Font("Arial", 10);
                    pd.PrintPage += new PrintPageEventHandler
                       (this.pd_PrintPage);

                    pd.DefaultPageSettings = pageSettings;
                    pd.PrinterSettings = printerSetting;
                    pd.PrintController = new StandardPrintController();
                    pd.Print();
                    await Task.Delay(300);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
                finally
                {
                    //streamToPrint.Close();
                    pd = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public Bitmap EncodeToBitmap(string qrvalue)
        {
            QRCode qrc = new QRCode();
            qrc.Version = ZXing.QrCode.Internal.Version.getVersionForNumber(7);
            var qr = new BarcodeWriter();
            qr.Format = BarcodeFormat.QR_CODE;

            qr.Options.Margin = 1;
            qr.Options.Height = 100;
            qr.Options.Width = 100;
            Bitmap result = new Bitmap(qr.Write(qrvalue));
            return result;
        }

        public static Bitmap EncodeToBitmap_Static(string qrvalue)
        {
            QRCode qrc = new QRCode();
            qrc.Version = ZXing.QrCode.Internal.Version.getVersionForNumber(7);
            var qr = new BarcodeWriter();
            qr.Format = BarcodeFormat.QR_CODE;

            qr.Options.Margin = 1;
            qr.Options.Height = 100;
            qr.Options.Width = 100;
            Bitmap result = new Bitmap(qr.Write(qrvalue));
            return result;
        }

        public async Task<bool> PrintPDF(string FileName)
        {

            try
            {
                PrintDocument pd = new PrintDocument();
                pd.OriginAtMargins = true;

                //System.IO.StreamReader streamToPrint = new System.IO.StreamReader
                //   (Application.StartupPath + "\\MyFile.txt");
                try
                {

                    printFont = new Font("Arial", 10);


                    pd.PrinterSettings.PrintToFile = true;
                    pd.PrinterSettings.PrintFileName = FileName;
                    pd.PrinterSettings.PrinterName = "Microsoft XPS Document Writer";

                    pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

                    pd.PrintPage += new PrintPageEventHandler
                       (this.pd_PrintPage);

                    //pd.DefaultPageSettings = pageSettings;
                    //pd.PrinterSettings = printerSetting;
                    pd.PrintController = new StandardPrintController();
                    pd.Print();
                    await Task.Delay(300);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
                finally
                {
                    //streamToPrint.Close();
                    pd = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private int _delayPrinterTime = 1000;
        private bool _Active = true;
        
    }
}
