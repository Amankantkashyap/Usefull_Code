using System;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.IO;
using iTextSharp.text;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void upload_bill_Click(object sender, EventArgs e)
        {
            OpenFileDialog bill = new OpenFileDialog();

            bill.Filter = "allfiles|*.pdf";

            if (bill.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // get full path.
                bill_choosen.Text = bill.FileName;
                // get file name.
                fillname.Text = bill.SafeFileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get path of current directory where header file is saved.
            string headerpdf = Directory.GetCurrentDirectory()+"\\one.pdf";

            string FileName="Bill_"+DateTime.Now.Ticks.ToString()+"_.pdf";

            string generatedbill ="D:\\Generated Bill\\"+FileName;

            // if forlder does not exist create new one. 

            System.IO.Directory.CreateDirectory("D:\\Generated Bill");

            // if no file choosen 

            if (bill_choosen.Text == "NO FILE CHOOSEN..!")
            {
                
                MessageBox.Show("Oops No Bill Selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    // Generate and then Print bill
                    Printbill(headerpdf, bill_choosen.Text, generatedbill);
                }
                catch(Exception exc)
                {
                    MessageBox.Show("Something went Wrong"+exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                
                bill_choosen.Text = "NO FILE CHOOSEN..!";
                fillname.Text = "NO FILE CHOOSEN..!";
            }
        }

        static void Printbill(String headerpdf, String billpdf, String generatedbill)
        {
            // Create readers
            using (PdfReader reader = new PdfReader(billpdf))
            {
                using (PdfReader sReader = new PdfReader(headerpdf))
                {
                    // Create the stamper
                    using (PdfStamper stamper = new PdfStamper(reader, new FileStream(generatedbill, FileMode.Create)))
                    {
                        // Add the stationery to each page
                        PdfImportedPage page = stamper.GetImportedPage(sReader, 1);
                        int n = reader.NumberOfPages;
                        PdfContentByte background;
                        for (int i = 1; i <= n; i++)
                        {
                            background = stamper.GetUnderContent(i);
                            background.AddTemplate(page, 0, 0);
                        }
                        // CLose the stamper
                        stamper.Close();

                        // provide printer name
                        string printerame="\\\\DEV101-PC\\HP LaserJet 1020";

                        using (PrintDialog printDialog1 = new PrintDialog())
                        {
                            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(generatedbill);
                            // Set printer name
                            //info.Arguments = "\"" + printDialog1.PrinterSettings.PrinterName + "\""; --> for Choosing printer Manualy
                            info.Arguments = "\"" + printerame + "\"";  // --> Hardcode Printer name
                            info.CreateNoWindow = true;
                            info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            info.UseShellExecute = true;

                            // Assign Verb what to do
                            info.Verb = "PrintTo";
                            // Start printing
                            System.Diagnostics.Process.Start(info);
                        }
                    }
                }
            }
            MessageBox.Show("Bill Printed", "Printing Bill", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }


    }
}
