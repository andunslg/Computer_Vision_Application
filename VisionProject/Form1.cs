using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace VisionProject
{
    public partial class Form1 : Form
    {
        
        Bitmap image;
        Bitmap grayImage;
        public Form1()
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

                    this.Controls.Add(pictureBox1);
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
            pictureBox1.Image = grayImage;
        }
    }
}
