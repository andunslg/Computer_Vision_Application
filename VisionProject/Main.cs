using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace VisionProject
{
    public partial class Main : Form
    {
        
        Bitmap originalImage;
        Bitmap grayImage;

        Bitmap secondOrginalImage;
        Bitmap secondGrayImage;

        Bitmap comparisonImage;

        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Files (*.bmp.*.jpeg,*.png,*.jpg)|*.bmp;*.jpeg;*.png;*.jpg";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    PictureBox pictureBox1 = this.pictureBox1;
                    originalImage = new Bitmap(dlg.FileName);
                    pictureBox1.Image = originalImage;

                    this.button2.Enabled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //make an empty bitmap the same size as original
            grayImage = new Bitmap(originalImage.Width, originalImage.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(grayImage);
            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix=new ColorMatrix(new float[][]{
                new float[]{.3f, .3f, .3f,0,0},
                new float[]{.59f, .59f, .59f,0,0},
                new float[]{.11f, .11f, .11f,0,0},
                new float[]{0,0,0,1,0},
                new float[]{0,0,0,0,1}});
            //create some image attributes
            ImageAttributes attributes =new ImageAttributes();
            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);
            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(originalImage,new Rectangle(0,0, originalImage.Width, originalImage.Height),0,0, originalImage.Width, originalImage.Height,GraphicsUnit.Pixel, attributes);
            //dispose the Graphics object
            g.Dispose();
            pictureBox2.Image = grayImage;

            this.button3.Enabled = true;
            this.button4.Enabled = true;

            this.label1.Enabled = true;
            this.label2.Enabled = true;
            this.comboBox1.Enabled = true;
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.Enabled = true;
            this.comboBox2.SelectedIndex = 0;
            this.button6.Enabled = true;

            this.comboBox4.Enabled = true;
            this.comboBox4.SelectedIndex = 0;
            this.label4.Enabled = true;
            this.button7.Enabled = true;
            this.button11.Enabled = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            
            long[] myHistogram = new long[256];
            for (int i = 0 ; i < grayImage.Size.Width; i++)
                for (int j = 0 ; j < grayImage.Size.Height; j++)
                {
                    System.Drawing.Color c = grayImage.GetPixel(i, j);
                    long Temp = 0;
                    Temp += c.R;
                    myHistogram[Temp]++;
                }
            this.histogramaDesenat1.DrawHistogram(myHistogram);
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image.Save(dialog.FileName);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox3.Image.Save(dialog.FileName);
            }
            this.button9.Enabled = true;
        }


        private void button6_Click(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(grayImage);

            if (this.comboBox1.SelectedIndex == 0)
            {
                if (this.comboBox2.SelectedIndex == 0)
                {                  
                    this.pictureBox3.Image = MeanFilter(img, 3);
                }
                else if (this.comboBox2.SelectedIndex == 1)
                {
                    this.pictureBox3.Image = MeanFilter(img, 5);
                }
                else if (this.comboBox2.SelectedIndex == 2)
                {
                    this.pictureBox3.Image = MeanFilter(img, 7);
                }
                else if (this.comboBox2.SelectedIndex == 3)
                {
                    this.pictureBox3.Image = MeanFilter(img, 9);
                }
            }
            else if (this.comboBox1.SelectedIndex == 1)
            {
                if (this.comboBox2.SelectedIndex == 0)
                {
                    this.pictureBox3.Image = MedianFilter(img, 3);
                }
                else if (this.comboBox2.SelectedIndex == 1)
                {
                    this.pictureBox3.Image = MedianFilter(img, 5);
                }
                else if (this.comboBox2.SelectedIndex == 2)
                {
                    this.pictureBox3.Image = MedianFilter(img, 7);
                }
                else if (this.comboBox2.SelectedIndex == 3)
                {
                    this.pictureBox3.Image = MedianFilter(img, 9);
                }
            }

            this.button9.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Bitmap sourceBitmap = new Bitmap(grayImage);

            if (this.comboBox4.SelectedIndex == 0)
            {
                this.pictureBox3.Image = ConvolutionFilter(sourceBitmap,
                                                   Matrix.Sobel3x3Horizontal,
                                                     Matrix.Sobel3x3Vertical,
                                                          1.0, 0, true);
            }
            else if (this.comboBox4.SelectedIndex == 1)
            {
                this.pictureBox3.Image = RobertsEdgeDetection(sourceBitmap);
            }
            else if (this.comboBox4.SelectedIndex == 2)
            {
                this.pictureBox3.Image = ConvolutionFilter(sourceBitmap,
                                               Matrix.Prewitt3x3Horizontal,
                                                 Matrix.Prewitt3x3Vertical,
                                                        1.0, 0, true);
            }
            else if (this.comboBox4.SelectedIndex == 3)
            {

                float TH, TL, Sigma;
                int MaskSize;

                TH = (float)Convert.ToDouble(this.textBox1.Text);
                TL = (float)Convert.ToDouble(this.textBox2.Text);

                MaskSize = Convert.ToInt32(this.textBox3.Text);
                Sigma = (float)Convert.ToDouble(this.textBox4.Text);

                Canny CannyData = new Canny(grayImage, TH, TL, MaskSize, Sigma);

                CannyData.DisplayImage(CannyData.NonMax);

                CannyData.DisplayImage(CannyData.FilteredImage);

                CannyData.DisplayImage(CannyData.GNL);

                CannyData.DisplayImage(CannyData.GNH);

                this.pictureBox3.Image = CannyData.DisplayImage(CannyData.EdgeMap);

            }
            this.button9.Enabled = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image = grayImage;

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Files (*.bmp.*.jpeg,*.png,*.jpg)|*.bmp;*.jpeg;*.png;*.jpg";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    secondOrginalImage = new Bitmap(dlg.FileName);
                }
            }

            //make an empty bitmap the same size as original
            secondGrayImage = new Bitmap(secondOrginalImage.Width, secondOrginalImage.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(secondGrayImage);
            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]{
                new float[]{.3f, .3f, .3f,0,0},
                new float[]{.59f, .59f, .59f,0,0},
                new float[]{.11f, .11f, .11f,0,0},
                new float[]{0,0,0,1,0},
                new float[]{0,0,0,0,1}});
            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();
            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);
            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(secondOrginalImage, new Rectangle(0, 0, secondOrginalImage.Width, secondOrginalImage.Height), 0, 0, secondOrginalImage.Width, secondOrginalImage.Height, GraphicsUnit.Pixel, attributes);
            //dispose the Graphics object
            g.Dispose();

            this.pictureBox2.Image = secondGrayImage;

            this.button12.Enabled = true;

        }

        private void button12_Click(object sender, EventArgs e)
        {
            comparisonImage = ArithmeticBlend(grayImage, secondGrayImage, ColorCalculator.ColorCalculationType.Difference);
            this.pictureBox3.Image = comparisonImage;
            this.button9.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox4.SelectedIndex == 3)
            {
                this.label3.Enabled = true;
                this.label5.Enabled = true;
                this.label6.Enabled = true;
                this.label7.Enabled = true;

                this.textBox1.Enabled = true;
                this.textBox2.Enabled = true;
                this.textBox3.Enabled = true;
                this.textBox4.Enabled = true;
            }
            else
            {
                this.label3.Enabled = false;
                this.label5.Enabled = false;
                this.label6.Enabled = false;
                this.label7.Enabled = false;

                this.textBox1.Enabled = false;
                this.textBox2.Enabled = false;
                this.textBox3.Enabled = false;
                this.textBox4.Enabled = false;
            }

        }


      /*  public Bitmap AlphaTrimmedMeanFilter(Bitmap sourceBitmap,int matrixSize,int alpha)
        {
            Bitmap result = new Bitmap(sourceBitmap);
           
            //   Start of the trimmed ordered set
           int start = alpha;
           //   End of the trimmed ordered set
           int end = 9 - alpha;
           //   Move window through all elements of the image
           for (int m = 1; m < sourceBitmap.Width; ++m)
           {
              for (int n = 1; n < sourceBitmap.Height; ++n)
              {
                 //   Pick up window elements
                 int k = 0;
                 int [] window= new int[9];
                 for (int j = m - 1; j < m + 2; ++j)
                 {
                     for (int i = n - 1; i < n + 2; ++i)
                     {
                         if (j > 0 && j < sourceBitmap.Width && i > 0 && i < sourceBitmap.Height)
                         {
                             Color pixelColor = sourceBitmap.GetPixel(j, i);
                             window[k++] = Convert.ToInt16(pixelColor.R);
                         }
                     }
                 }
                 //   Order elements (only necessary part of them)
                 for (int j = 0; j < end; ++j)
                 {
                    //   Find position of minimum element
                    int min = j;
                    for (int l = j + 1; l < 9; ++l)
                    {
                        if (window[l] < window[min])
                            min = l;
                    }
                    //   Put found minimum element in its place
                    int temp = window[j];
                    window[j] = window[min];
                    window[min] = temp;
                 }

                 //   Get result - the mean value of the elements in trimmed set
                 result.SetPixel(n - 1, m - 1, Color.FromArgb(window[start], window[start], window[start]));
                 for (int j = start + 1; j < end; ++j)
                 {
                     result.SetPixel(n - 1, m - 1, Color.FromArgb(window[j], window[j], window[j]));
                 }
                 Color pixelColor1 = sourceBitmap.GetPixel(n - 1, m - 1);
                 int tempColor = Convert.ToInt16(pixelColor1.R)/9;
                 result.SetPixel(n - 1, m - 1, Color.FromArgb(tempColor,tempColor,tempColor));
         
                }
           }
             return result;
        }*/
        
        public  Bitmap MedianFilter(Bitmap sourceBitmap,int matrixSize,int bias = 0, bool grayscale = true) 
        {
            BitmapData sourceData = 
                       sourceBitmap.LockBits(new Rectangle(0, 0,
                       sourceBitmap.Width, sourceBitmap.Height),
                       ImageLockMode.ReadOnly, 
                       PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * 
                                          sourceData.Height];

            byte[] resultBuffer = new byte[sourceData.Stride * 
                                           sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, 
                                       pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            if (grayscale == true)
            {
                float rgb = 0;

                for (int k = 0; k < pixelBuffer.Length; k += 4)
                {
                    rgb = pixelBuffer[k] * 0.11f;
                    rgb += pixelBuffer[k + 1] * 0.59f;
                    rgb += pixelBuffer[k + 2] * 0.3f;


                    pixelBuffer[k] = (byte)rgb;
                    pixelBuffer[k + 1] = pixelBuffer[k];
                    pixelBuffer[k + 2] = pixelBuffer[k];
                    pixelBuffer[k + 3] = 255;
                }
            }

            int filterOffset = (matrixSize - 1) / 2;
            int calcOffset = 0;

            int byteOffset = 0;

            List<int> neighbourPixels = new List<int>();
            byte[] middlePixel;

            for (int offsetY = filterOffset; offsetY < 
                sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < 
                    sourceBitmap.Width - filterOffset; offsetX++)
                {
                    byteOffset = offsetY * 
                                 sourceData.Stride + 
                                 offsetX * 4;

                    neighbourPixels.Clear();

                    for (int filterY = -filterOffset; 
                        filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset;
                            filterX <= filterOffset; filterX++)
                        {

                            calcOffset = byteOffset + 
                                         (filterX * 4) + 
                                         (filterY * sourceData.Stride);

                            neighbourPixels.Add(BitConverter.ToInt32(
                                             pixelBuffer, calcOffset));
                        }
                    }

                    neighbourPixels.Sort();

                    middlePixel = BitConverter.GetBytes(
                                       neighbourPixels[filterOffset]);

                    resultBuffer[byteOffset] = middlePixel[0];
                    resultBuffer[byteOffset + 1] = middlePixel[1];
                    resultBuffer[byteOffset + 2] = middlePixel[2];
                    resultBuffer[byteOffset + 3] = middlePixel[3];
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, 
                                             sourceBitmap.Height);

            BitmapData resultData = 
                       resultBitmap.LockBits(new Rectangle(0, 0,
                       resultBitmap.Width, resultBitmap.Height),
                       ImageLockMode.WriteOnly,
                       PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, 
                                       resultBuffer.Length);

            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }

        public  Bitmap MeanFilter(Bitmap img,int matrixSize) { 

         Color pixelColor;
         for (int i = 0; i < img.Width; i++)
                {
                    for (int j = 0; j < img.Height; j++)
                    {
                        int total = 0;

                        if (matrixSize == 3)
                        {

                            if (i - 1 >= 0 && j - 1 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 1, j - 1);
                                total += Convert.ToInt16(pixelColor.R);

                            }
                            if (j - 1 >= 0 && i + 1 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 1, j - 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (j - 1 >= 0)
                            {
                                pixelColor = img.GetPixel(i, j - 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 1 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 1, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 1 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 1, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 1 >= 0 && j + 1 < img.Height)
                            {
                                pixelColor = img.GetPixel(i - 1, j + 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (j + 1 < img.Height)
                            {
                                pixelColor = img.GetPixel(i, j + 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 1 < img.Width && j+1 < img.Height)
                            {
                                pixelColor = img.GetPixel(i + 1, j + 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            pixelColor = img.GetPixel(i, j);
                            total += Convert.ToInt16(pixelColor.R);

                            img.SetPixel(i, j, Color.FromArgb(total / 9, total / 9, total / 9));
                        }

                        if (matrixSize == 5)
                        {

                            if (i - 1 >= 0 && j - 1 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 1, j - 1);
                                total += Convert.ToInt16(pixelColor.R);

                            }
                            if (j - 1 >= 0 && i + 1 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 1, j - 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (j - 1 >= 0)
                            {
                                pixelColor = img.GetPixel(i, j - 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 1 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 1, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 1 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 1, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 1 >= 0 && j + 1 < img.Height)
                            {
                                pixelColor = img.GetPixel(i - 1, j + 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (j + 1 < img.Height)
                            {
                                pixelColor = img.GetPixel(i, j + 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 1 < img.Width && j + 1 < img.Height)
                            {
                                pixelColor = img.GetPixel(i + 1, j + 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (i - 2 >= 0 && j - 2 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 2, j - 2);
                                total += Convert.ToInt16(pixelColor.R);

                            }
                            if (j - 2 >= 0 && i + 2 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 2, j - 2);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (j - 2 >= 0)
                            {
                                pixelColor = img.GetPixel(i, j -2);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 2 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 2, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 2 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 2, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 2 >= 0 && j + 2 < img.Height)
                            {
                                pixelColor = img.GetPixel(i - 2, j + 2);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (j + 2 < img.Height)
                            {
                                pixelColor = img.GetPixel(i , j + 2);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 2 < img.Width && j + 2 < img.Height)
                            {
                                pixelColor = img.GetPixel(i + 2, j + 2);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            pixelColor = img.GetPixel(i, j);
                            total += Convert.ToInt16(pixelColor.R);

                            img.SetPixel(i, j, Color.FromArgb(total / 25, total / 25, total / 25));
                        }


                        if (matrixSize == 7)
                        {

                            if (i - 1 >= 0 && j - 1 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 1, j - 1);
                                total += Convert.ToInt16(pixelColor.R);

                            }
                            if (j - 1 >= 0 && i + 1 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 1, j - 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (j - 1 >= 0)
                            {
                                pixelColor = img.GetPixel(i, j - 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 1 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 1, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 1 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 1, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 1 >= 0 && j + 1 < img.Height)
                            {
                                pixelColor = img.GetPixel(i - 1, j + 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (j + 1 < img.Height)
                            {
                                pixelColor = img.GetPixel(i, j + 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 1 < img.Width && j + 1 < img.Height)
                            {
                                pixelColor = img.GetPixel(i + 1, j + 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (i - 2 >= 0 && j - 2 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 2, j - 2);
                                total += Convert.ToInt16(pixelColor.R);

                            }
                            if (j - 2 >= 0 && i + 2 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 2, j - 2);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (j - 2 >= 0)
                            {
                                pixelColor = img.GetPixel(i, j - 2);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 2 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 2, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 2 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 2, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 2 >= 0 && j + 2 < img.Height)
                            {
                                pixelColor = img.GetPixel(i - 2, j + 2);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (j + 2 < img.Height)
                            {
                                pixelColor = img.GetPixel(i, j + 2);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 2 < img.Width && j + 2 < img.Height)
                            {
                                pixelColor = img.GetPixel(i + 2, j + 2);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (i - 3 >= 0 && j - 3 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 3, j - 3);
                                total += Convert.ToInt16(pixelColor.R);

                            }
                            if (j - 3 >= 0 && i + 3 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 3, j - 3);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (j - 3 >= 0)
                            {
                                pixelColor = img.GetPixel(i, j - 3);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 3 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 3, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 3 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 3, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 3 >= 0 && j + 3 < img.Height)
                            {
                                pixelColor = img.GetPixel(i - 3, j + 3);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (j + 3 < img.Height)
                            {
                                pixelColor = img.GetPixel(i, j + 3);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 3 < img.Width && j + 3 < img.Height)
                            {
                                pixelColor = img.GetPixel(i + 3, j + 3);
                                total += Convert.ToInt16(pixelColor.R);
                            }


                            pixelColor = img.GetPixel(i, j);
                            total += Convert.ToInt16(pixelColor.R);

                            img.SetPixel(i, j, Color.FromArgb(total / 49, total / 49, total / 49));
                        }

                        if (matrixSize == 9)
                        {

                            if (i - 1 >= 0 && j - 1 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 1, j - 1);
                                total += Convert.ToInt16(pixelColor.R);

                            }
                            if (j - 1 >= 0 && i + 1 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 1, j - 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (j - 1 >= 0)
                            {
                                pixelColor = img.GetPixel(i, j - 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 1 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 1, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 1 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 1, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 1 >= 0 && j + 1 < img.Height)
                            {
                                pixelColor = img.GetPixel(i - 1, j + 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (j + 1 < img.Height)
                            {
                                pixelColor = img.GetPixel(i, j + 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 1 < img.Width && j + 1 < img.Height)
                            {
                                pixelColor = img.GetPixel(i + 1, j + 1);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (i - 2 >= 0 && j - 2 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 2, j - 2);
                                total += Convert.ToInt16(pixelColor.R);

                            }
                            if (j - 2 >= 0 && i + 2 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 2, j - 2);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (j - 2 >= 0)
                            {
                                pixelColor = img.GetPixel(i, j - 2);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 2 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 2, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 2 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 2, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 2 >= 0 && j + 2 < img.Height)
                            {
                                pixelColor = img.GetPixel(i - 2, j + 2);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (j + 2 < img.Height)
                            {
                                pixelColor = img.GetPixel(i, j + 2);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 2 < img.Width && j + 2 < img.Height)
                            {
                                pixelColor = img.GetPixel(i + 2, j + 2);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (i - 3 >= 0 && j - 3 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 3, j - 3);
                                total += Convert.ToInt16(pixelColor.R);

                            }
                            if (j - 3 >= 0 && i + 3 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 3, j - 3);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (j - 3 >= 0)
                            {
                                pixelColor = img.GetPixel(i, j - 3);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 3 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 3, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 3 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 3, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 3 >= 0 && j + 3 < img.Height)
                            {
                                pixelColor = img.GetPixel(i - 3, j + 3);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (j + 3 < img.Height)
                            {
                                pixelColor = img.GetPixel(i, j + 3);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 3 < img.Width && j + 3 < img.Height)
                            {
                                pixelColor = img.GetPixel(i + 3, j + 3);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (i - 4 >= 0 && j - 4 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 4, j - 4);
                                total += Convert.ToInt16(pixelColor.R);

                            }
                            if (j - 4 >= 0 && i +4 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 4, j - 4);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            if (j - 4 >= 0)
                            {
                                pixelColor = img.GetPixel(i, j - 4);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 4 < img.Width)
                            {
                                pixelColor = img.GetPixel(i + 4, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 4 >= 0)
                            {
                                pixelColor = img.GetPixel(i - 4, j);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i - 4 >= 0 && j + 4 < img.Height)
                            {
                                pixelColor = img.GetPixel(i - 4, j + 4);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (j + 4 < img.Height)
                            {
                                pixelColor = img.GetPixel(i, j + 4);
                                total += Convert.ToInt16(pixelColor.R);
                            }
                            if (i + 4 < img.Width && j + 4 < img.Height)
                            {
                                pixelColor = img.GetPixel(i + 4, j + 4);
                                total += Convert.ToInt16(pixelColor.R);
                            }

                            pixelColor = img.GetPixel(i, j);
                            total += Convert.ToInt16(pixelColor.R);

                            img.SetPixel(i, j, Color.FromArgb(total / 81, total / 81, total / 81));
                        }
                    }
                }
             return img;
            }      

        public Bitmap ConvolutionFilter(Bitmap sourceBitmap,
                                              double[,] xFilterMatrix,
                                              double[,] yFilterMatrix,
                                                    double factor = 1,
                                                         int bias = 0,
                                               bool grayscale = false)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                     sourceBitmap.Width, sourceBitmap.Height),
                                                       ImageLockMode.ReadOnly,
                                                  PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            if (grayscale == true)
            {
                float rgb = 0;

                for (int k = 0; k < pixelBuffer.Length; k += 4)
                {
                    rgb = pixelBuffer[k] * 0.11f;
                    rgb += pixelBuffer[k + 1] * 0.59f;
                    rgb += pixelBuffer[k + 2] * 0.3f;

                    pixelBuffer[k] = (byte)rgb;
                    pixelBuffer[k + 1] = pixelBuffer[k];
                    pixelBuffer[k + 2] = pixelBuffer[k];
                    pixelBuffer[k + 3] = 255;
                }
            }

            double blueX = 0.0;
            double greenX = 0.0;
            double redX = 0.0;

            double blueY = 0.0;
            double greenY = 0.0;
            double redY = 0.0;

            double blueTotal = 0.0;
            double greenTotal = 0.0;
            double redTotal = 0.0;

            int filterOffset = 1;
            int calcOffset = 0;

            int byteOffset = 0;

            for (int offsetY = filterOffset; offsetY <
                sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX <
                    sourceBitmap.Width - filterOffset; offsetX++)
                {
                    blueX = greenX = redX = 0;
                    blueY = greenY = redY = 0;

                    blueTotal = greenTotal = redTotal = 0.0;

                    byteOffset = offsetY *
                                 sourceData.Stride +
                                 offsetX * 4;

                    for (int filterY = -filterOffset;
                        filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset;
                            filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset +
                                         (filterX * 4) +
                                         (filterY * sourceData.Stride);

                            blueX += (double)(pixelBuffer[calcOffset]) *
                                      xFilterMatrix[filterY + filterOffset,
                                              filterX + filterOffset];

                            greenX += (double)(pixelBuffer[calcOffset + 1]) *
                                      xFilterMatrix[filterY + filterOffset,
                                              filterX + filterOffset];

                            redX += (double)(pixelBuffer[calcOffset + 2]) *
                                      xFilterMatrix[filterY + filterOffset,
                                              filterX + filterOffset];

                            blueY += (double)(pixelBuffer[calcOffset]) *
                                      yFilterMatrix[filterY + filterOffset,
                                              filterX + filterOffset];

                            greenY += (double)(pixelBuffer[calcOffset + 1]) *
                                      yFilterMatrix[filterY + filterOffset,
                                              filterX + filterOffset];

                            redY += (double)(pixelBuffer[calcOffset + 2]) *
                                      yFilterMatrix[filterY + filterOffset,
                                              filterX + filterOffset];
                        }
                    }

                    blueTotal = Math.Sqrt((blueX * blueX) + (blueY * blueY));
                    greenTotal = Math.Sqrt((greenX * greenX) + (greenY * greenY));
                    redTotal = Math.Sqrt((redX * redX) + (redY * redY));

                    if (blueTotal > 255)
                    { blueTotal = 255; }
                    else if (blueTotal < 0)
                    { blueTotal = 0; }

                    if (greenTotal > 255)
                    { greenTotal = 255; }
                    else if (greenTotal < 0)
                    { greenTotal = 0; }

                    if (redTotal > 255)
                    { redTotal = 255; }
                    else if (redTotal < 0)
                    { redTotal = 0; }

                    resultBuffer[byteOffset] = (byte)(blueTotal);
                    resultBuffer[byteOffset + 1] = (byte)(greenTotal);
                    resultBuffer[byteOffset + 2] = (byte)(redTotal);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                     resultBitmap.Width, resultBitmap.Height),
                                                      ImageLockMode.WriteOnly,
                                                  PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }
        public Bitmap ArithmeticBlend(Bitmap sourceBitmap, Bitmap blendBitmap,
                                     ColorCalculator.ColorCalculationType calculationType)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                    sourceBitmap.Width, sourceBitmap.Height),
                                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            BitmapData blendData = blendBitmap.LockBits(new Rectangle(0, 0,
                                    blendBitmap.Width, blendBitmap.Height),
                                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] blendBuffer = new byte[blendData.Stride * blendData.Height];
            Marshal.Copy(blendData.Scan0, blendBuffer, 0, blendBuffer.Length);
            blendBitmap.UnlockBits(blendData);

            for (int k = 0; (k + 4 < pixelBuffer.Length) &&
                            (k + 4 < blendBuffer.Length); k += 4)
            {
                pixelBuffer[k] = ColorCalculator.Calculate(pixelBuffer[k],
                                 blendBuffer[k], calculationType);

                pixelBuffer[k + 1] = ColorCalculator.Calculate(pixelBuffer[k + 1],
                                     blendBuffer[k + 1], calculationType);

                pixelBuffer[k + 2] = ColorCalculator.Calculate(pixelBuffer[k + 2],
                                     blendBuffer[k + 2], calculationType);
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }

        public Bitmap RobertsEdgeDetection(Bitmap img)
        {
            Bitmap result = new Bitmap(img);

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    if (i < img.Width - 1 && j < img.Height - 1)
                    {
                        Color pixelColor1 = img.GetPixel(i, j);
                        Color pixelColor2 = img.GetPixel(i + 1, j);
                        Color pixelColor4 = img.GetPixel(i + 1, j + 1);
                        Color pixelColor3 = img.GetPixel(i, j + 1);

                        int pixVal1 = Convert.ToInt16(pixelColor1.R);
                        int pixVal2 = Convert.ToInt16(pixelColor2.R);
                        int pixVal3 = Convert.ToInt16(pixelColor3.R);
                        int pixVal4 = Convert.ToInt16(pixelColor4.R);

                        int resultVal = Math.Abs(pixVal1 - pixVal4) + Math.Abs(pixVal2 - pixVal3);

                        if (resultVal > 255)
                        {
                            resultVal = 255;
                        }
                        else if(resultVal < 0){
                            resultVal = 0;
                        }

                        result.SetPixel(i, j, Color.FromArgb(resultVal, resultVal, resultVal));
                    }

                }
            }
            return result;
        }
    }
}
