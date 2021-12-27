using MemoryGame.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryGame
{
    public partial class Form1 : Form
    {
        private int m_CountTrue = 0;
        private Image[] m_Images = new Image[8];
        private bool m_isFirst = true;
        private PictureBox m_firstPictureBox;
        private PictureBox m_secondPictureBox;
        public Form1()
        {
            InitializeComponent();
            setImagesArray();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Swap(int i, int j)
        {
            Image image = m_Images[i];
            m_Images[i] = m_Images[j];
            m_Images[j] = image;
        }
        private void setImagesArray()
        {
            m_Images[0] = Resources.Airplane1;
            m_Images[1] = Resources.Airplane2;
            m_Images[2] = Resources.Airplane3;
            m_Images[3] = Resources.Airplane4;
            m_Images[4] = Resources.Airplane4;
            m_Images[5] = Resources.Airplane3;
            m_Images[6] = Resources.Airplane2;
            m_Images[7] = Resources.Airplane1;


            Random rnd = new Random();
            for(int i = 0; i < m_Images.Length; i++)
            {
                Swap(i, rnd.Next(m_Images.Length));
            }

        }
        public bool IsImagesMatch(Image image1, Image image2)
        {
            try
            {
                //create instance or System.Drawing.ImageConverter to convert
                //each image to a byte array
                ImageConverter converter = new ImageConverter();
                //create 2 byte arrays, one for each image
                byte[] imgBytes1 = new byte[1];
                byte[] imgBytes2 = new byte[1];

                //convert images to byte array
                imgBytes1 = (byte[])converter.ConvertTo(image1, imgBytes2.GetType());
                imgBytes2 = (byte[])converter.ConvertTo(image2, imgBytes1.GetType());

                //now compute a hash for each image from the byte arrays
                System.Security.Cryptography.SHA256Managed sha = new System.Security.Cryptography.SHA256Managed();
                byte[] imgHash1 = sha.ComputeHash(imgBytes1);
                byte[] imgHash2 = sha.ComputeHash(imgBytes2);

                //now let's compare the hashes
                for (int i = 0; i < imgHash1.Length && i < imgHash2.Length; i++)
                {
                    //whoops, found a non-match, exit the loop
                    //with a false value
                    if (!(imgHash1[i] == imgHash2[i]))
                        return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            //we made it this far so the images must match
            return true;
        }

        private void pictureBox_Card_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;

            string picName = pictureBox.Name;

            int k = int.Parse(picName.Substring(picName.Length - 1));

            k--;
            if(!IsImagesMatch(pictureBox.Image, Resources.Back))
            {
                pictureBox.Image = Resources.Back;
            }else
            {
                pictureBox.Image = m_Images[k]; 
            }

            if (!m_isFirst)
            {
                m_secondPictureBox = pictureBox;
                timer1.Start();
            }
            else
            {
                m_firstPictureBox = pictureBox;
            }
            m_isFirst = !m_isFirst;
        }

        private void ToogleImages()
        {
            pictureBox1.Visible = !pictureBox1.Visible;
            pictureBox2.Visible = !pictureBox2.Visible;
            pictureBox3.Visible = !pictureBox3.Visible;
            pictureBox4.Visible = !pictureBox4.Visible;
            pictureBox5.Visible = !pictureBox5.Visible;
            pictureBox6.Visible = !pictureBox6.Visible;
            pictureBox7.Visible = !pictureBox7.Visible;
            pictureBox8.Visible = !pictureBox8.Visible;
            pictureBox1.Image = Resources.Back;
            pictureBox2.Image = Resources.Back;
            pictureBox3.Image = Resources.Back;
            pictureBox4.Image = Resources.Back;
            pictureBox5.Image = Resources.Back;
            pictureBox6.Image = Resources.Back;
            pictureBox7.Image = Resources.Back;
            pictureBox8.Image = Resources.Back;
            pictureBox1.Enabled = true;
            pictureBox2.Enabled = true;
            pictureBox3.Enabled = true;
            pictureBox4.Enabled = true;
            pictureBox5.Enabled = true;
            pictureBox6.Enabled = true;
            pictureBox7.Enabled = true;
            pictureBox8.Enabled = true;
        }

        private void timer1_tick(object sender, EventArgs e)
        {
            if (IsImagesMatch(m_firstPictureBox.Image, m_secondPictureBox.Image))
            {
                m_firstPictureBox.Enabled = false;
                m_secondPictureBox.Enabled = false;

                m_CountTrue += 2;
            }
            else
            {
                m_firstPictureBox.Image = Resources.Back;
                m_secondPictureBox.Image = Resources.Back;
            }
            

            if (m_CountTrue == m_Images.Length)
            {
                ToogleImages();
                button1.Visible = true;
                label1.Visible = true;
                this.BackColor = Color.MediumTurquoise;
            }

            timer1.Stop();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            setImagesArray();
            ToogleImages();
            m_CountTrue = 0;
            button1.Visible = false;
            label1.Visible = false;
            this.BackColor = Color.Linen;
        }
    }
}