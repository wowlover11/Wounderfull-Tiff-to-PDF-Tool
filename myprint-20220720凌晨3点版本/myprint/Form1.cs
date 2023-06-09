using System;

using System.Collections.Generic;

using System.ComponentModel;

using System.Data;

using System.Drawing;

using System.Windows.Forms;

using System.Drawing.Imaging;

using System.Drawing.Printing;

using System.IO;
namespace myprint
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Button btnOpen;
       
        public Form1()
        {
            InitializeComponent();





        }

        int TifFrame;
        string newpath;


        private List<Image> TifImages = new List<Image>();
        String path;

        protected void btnOpen_Click(object sender, EventArgs e)
        {


            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "TIFF|*.tif|JPGE|*.jpg|所有文件(*.*)|*.*", ValidateNames = false })
            // using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "TIFF|*.tif", ValidateNames = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                    textBox1.Text = ofd.FileName;
            }



        }



        private void btnPrint(object sender, EventArgs e,String filepath)
        {
            PrintDocument pd = new PrintDocument();
            pd.DefaultPageSettings.PrinterSettings.PrinterName = "Microsoft Print to PDF";

            pd.PrinterSettings.PrintToFile = true;
            //pd.PrinterSettings.PrintFileName = @"H:\test.pdf";


            TifFrame = 0;
            Image i;


            // String cpath = path;
            //Image img = Image.FromFile(@"h:\test.tif");
            try
            {
                label9.Text = path;
                Image img = Image.FromFile(path);
                
            newpath = path.Substring(0, path.Length - 3) + "pdf";




            /*
                        if (textBox3.Text != "") { 


                            newpath = textBox3.Text + newpath.Substring(1, newpath.Length - 1);

                        label9.Text = newpath;
                    }*/
            pd.PrinterSettings.PrintFileName = newpath;





            i = img;
            int mpageCount = i.GetFrameCount(FrameDimension.Page);


            FrameDimension TifDimension = new FrameDimension(img.FrameDimensionsList[0]);

            int TifFrames = img.GetFrameCount(TifDimension);

            pd.PrintPage += (sender1, args) =>
            {

                Rectangle m = args.MarginBounds;



                if (path.Substring(path.Length - 1, 1) == "f")
                {





                    i.SelectActiveFrame(FrameDimension.Page, TifFrame);


                    if ((double)i.Width / (double)i.Height > (double)m.Width / (double)m.Height) // image is wider
                    {
                        m.Height = (int)((double)i.Height / (double)i.Width * (double)m.Width);
                        pd.DefaultPageSettings.Landscape = true;
                    }
                    else
                    {
                        m.Width = (int)((double)i.Width / (double)i.Height * (double)m.Height);
                        pd.DefaultPageSettings.Landscape = false;
                    }

                    args.Graphics.DrawImage(i, m);

                    Application.DoEvents();


                    TifFrame += 1;


                    if (TifFrame < TifFrames)
                    {
                        args.HasMorePages = true;
                    }


                }
                else
                {


                    if ((double)img.Width / (double)img.Height > (double)m.Width / (double)m.Height) // image is wider
                    {
                        m.Height = (int)((double)img.Height / (double)img.Width * (double)m.Width);
                        pd.DefaultPageSettings.Landscape = true;
                    }
                    else
                    {
                        m.Width = (int)((double)img.Width / (double)img.Height * (double)m.Height);
                        pd.DefaultPageSettings.Landscape = false;
                    }


                    args.Graphics.DrawImage(img, m);
                    Application.DoEvents();
                }




            };
            pd.Print();
                Application.DoEvents();
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("There was an error opening the image." +"Please check the path:"+ path);

            }
            //System.Diagnostics.Process.Start(newpath);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            path = textBox1.Text;
            btnPrint(sender, e);
            System.Diagnostics.Process.Start(newpath);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = fbd.SelectedPath;
                DirectoryInfo TheFolder = new DirectoryInfo(fbd.SelectedPath);
                var fileall = TheFolder.GetFiles("*.tif");
                int i = 0;

                foreach (FileInfo NextFile in fileall)
                {

                    if ((NextFile.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        i++;
                        label1.Text = i.ToString();
                    }
                }






            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            label6.Text = label1.Text;
            int i = 0;
            DirectoryInfo TheFolder = new DirectoryInfo(textBox2.Text);
            var fileall = TheFolder.GetFiles("*.tif");
            /*
                        if (textBox3.Text != "") { 
                        string myDir =textBox3+ textBox2.Text.Substring(1,textBox2.Text.Length-1);
                        if (!Directory.Exists(myDir))
                        {
                            Directory.CreateDirectory(myDir);
                        }
                        }
            */

            foreach (FileInfo NextFile in fileall)
            {
                if ((NextFile.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) { 
                    i++;
                label5.Text = i.ToString();

                Application.DoEvents();
                //System.Threading.Thread.Sleep(300);




                path = textBox2.Text + "\\" + NextFile.Name;

                
                    btnPrint(sender, e);

                }
            }
            MessageBox.Show("Tansform Done.");


        }






    }
}