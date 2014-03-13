using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace VisionProject
{
    public partial class Main : Form
    {
        
        Bitmap image;
        Bitmap grayImage;


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
                    image = new Bitmap(dlg.FileName);
                    pictureBox1.Image = image;

                    this.button2.Visible = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //make an empty bitmap the same size as original
            grayImage = new Bitmap(image.Width, image.Height);

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
            g.DrawImage(image,new Rectangle(0,0, image.Width, image.Height),0,0, image.Width, image.Height,GraphicsUnit.Pixel, attributes);
            //dispose the Graphics object
            g.Dispose();
            pictureBox2.Image = grayImage;

            this.button3.Visible = true;
            this.button4.Visible = true;
            this.button5.Visible = true;
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
                grayImage.Save(dialog.FileName);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.label1.Visible = true;
            this.label2.Visible = true;
            this.comboBox1.Visible = true;
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.Visible = true;
            this.comboBox2.SelectedIndex = 0;
            this.button6.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Bitmap img = grayImage;
            Color pixelColor;

            if (this.comboBox1.SelectedIndex == 0)
            {
                
                for (int i = 0; i < img.Width; i++)
                {
                    for (int j = 0; j < img.Height; j++)
                    {
                        int total = 0;

                        if (this.comboBox2.SelectedIndex == 0)
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

                        if (this.comboBox2.SelectedIndex == 1)
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


                        if (this.comboBox2.SelectedIndex == 2)
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

                        if (this.comboBox2.SelectedIndex == 3)
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
            }
            this.pictureBox3.Image = img;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

       
    }
}
