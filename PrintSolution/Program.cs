using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintSolution
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            var page = new System.Drawing.Printing.PageSettings();
            var printer = new System.Drawing.Printing.PrinterSettings();
            page.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);

            var printDetails = new List<PrintClass.PrintDetail>();
            var nf = new StringFormat();
            nf.Alignment = StringAlignment.Center;
            nf.LineAlignment = StringAlignment.Center;

            var p = new PrintClass.PrintDetail();

            //var p = new PrintClass.PrintDetail();

            p.Name = "SerialCode";
            //p.Text = "Resources/PHILIPPINE INSTITUTE-1.png";
            p.Text = "Resources/tempCert.png";
            p.NameFormat = nf;
            p.type = PrintClass.Type.Image;
            p.Location = new Point(0, 0);
            p.Size = new Size(page.Bounds.Width, page.Bounds.Height);
            printDetails.Add(p);
            
            p.Name = "name";
            p.Text = "Bernard Casil Ponce";
            p.NameFormat = nf;
            p.type = PrintClass.Type.Text;
            p.font = new Font("Tahoma", 25, FontStyle.Bold);
            p.Location = new Point(0, 425);
            p.Size = new Size(page.Bounds.Width - page.Margins.Left - page.Margins.Right, 50);
            p.PrintBorder = false;
            printDetails.Add(p);

            p.Name = "SerialCode";
            p.Text = "RTC-2019-00001";
            p.NameFormat = nf;
            p.type = PrintClass.Type.Text;
            p.font = new Font("Tahoma", 15, FontStyle.Bold);
            p.Location = new Point(0, 475);
            p.Size = new Size(page.Bounds.Width - page.Margins.Left - page.Margins.Right, 40);
            p.PrintBorder = false;
            printDetails.Add(p);

            //p.Name = "details";
            //p.Text = "1. Bla Bla Bla Bla Bla Bla Bla Bla Bla \n2. Bla Bla Bla Bla Bla Bla Bla Bla Bla Bla Bla Bla\n3. Bla Bla Bla Bla Bla Bla Bla Bla Bla \n4. Bla Bla Bla Bla Bla Bla Bla Bla Bla Bla Bla Bla Bla Bla\n";
            //var nf1 = new StringFormat();
            //nf1.Alignment = StringAlignment.Near;
            //nf1.LineAlignment = StringAlignment.Center;
            //p.NameFormat = nf1;
            //p.type = PrintClass.Type.Text;
            //p.font = new Font("Calibre", 10, FontStyle.Regular);
            //p.Location = new Point((int)(page.Bounds.Width * 0.13f), 400);
            //p.PrintBorder = false;
            //p.Size = new Size((int)(page.Bounds.Width * 0.75f), 400);
            //printDetails.Add(p);

            p.Name = "TotalPoint";
            p.Text = "10";
            p.NameFormat = nf;
            p.type = PrintClass.Type.Text;
            p.font = new Font("Tahoma", 10, FontStyle.Bold);
            p.Location = new Point((int)(page.Bounds.Width * 0.485f), 557);
            p.PrintBorder = false;
            p.Size = new Size((int)(page.Bounds.Width * 0.03f), 15);
            printDetails.Add(p);

            PrintClass printClass = new PrintClass(printDetails.ToArray());
            printClass.printerSetting = printer;
            printClass.pageSettings = page;

            printClass.Print();
        }

    }
}
