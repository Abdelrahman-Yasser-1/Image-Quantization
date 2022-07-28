using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;
        RGBPixel[,] ResultImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
            
            #region Image Quantization
            label_test_case_name.Text = "Test Case Name: "+System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
            #endregion
        
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value ;
            //ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            //ImageMatrix = ImageOperations.ImageQuantization(ImageMatrix, int.Parse(txt_k.Text));
            //ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
            
            #region Image Quantization
            List<RGBPixel> Distinct = new List<RGBPixel>();
            Edge[] MSTResult = new Edge[10000*10000];

            long timeBefore = System.Environment.TickCount;
            ResultImageMatrix = ImageQuantization.Quantize_the_image(ImageMatrix, int.Parse(comboBox_k.Text), ref Distinct, ref MSTResult);
            long timeAfter = System.Environment.TickCount;
            TimeSpan t = TimeSpan.FromMilliseconds(timeAfter - timeBefore);
            string time = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
            
            ImageOperations.DisplayImage(ResultImageMatrix, pictureBox2);
            
            label_time.Text = "Time: " + time;
            
            label_dColors.Text = "Number of distinct colors: " + Distinct.Count.ToString();
            
            Console.WriteLine("Total time: " + time);

            //double MSTSum = ImageOperations.MSTSum(Distinct);
            double MSTSum = 0;
            foreach (var i in MSTResult)
            {
                MSTSum += i.weight;
            }
            label_MSTSum.Text = "MST Sum: " + MSTSum.ToString();
            #endregion

        }
    }
}