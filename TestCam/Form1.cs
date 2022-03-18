using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using System.IO.Ports;//we are working with serial ports so we added this library

namespace TestCam
{
    public partial class Form1 : Form
    {
        private Capture capture; //Capture is the class name and capture is pointer to the camera. It is object
        private Image<Bgr, Byte> IMG; //Image is class type, IMG is containing the bgr image in bytes. Blue is most significant byte, red is least  <> means "List"
        private Image<Gray, Byte> GrayImg;//image list that includes gray image. GrayScale conversion = (R+B+G)/3. For 8-bit Grayscale images. 0-255 Gray levels. Bgr 3 byte. Gray 1 byte
        private Image<Gray, Byte> BWImg; //to create a black&white image

        private double myScale = 0; //640=width of the image cm/px
        private int Xpx, Ypx, N; //center point in pixels. N = number of pixels 
        private double Xcm, Ycm, Xcm1, Ycm1; //Px and Py;
        public double Zcm;
        public double d1 = 4.0;

        static SerialPort _serialPort; //object identifier
        public byte[] Buff = new byte[2];//buffer that contains the values to be used(from c# to arduino)
        //we use two values so its byte[2]
        //we have 2 variables(th1 and th2)

        public Form1() //this is the constructor, name of the class, no return datatype
        {
            InitializeComponent();


            _serialPort = new SerialPort();
            _serialPort.PortName = "COM8"; //Set your board COM
            _serialPort.BaudRate = 9600; //Set your board BaudRate
            _serialPort.Open(); //means open the serial port
        }

        private void processFrame(object sender, EventArgs e)//the most important function is processFrame in this program
        {
            if (capture == null)//very important to handel exception, if its null, it is not connected to the camera yet. there is no object
            {
                try //so we will try to connect it to the camera
                {
                    capture = new Capture(); //now we created a new object and connected it to the camera
                    //parantezin içi boş veya 0 yazıyorsa default kamera bağlı demektir
                    //ikinci kamera için parantezin içine 1 yazmamız gerekir
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message); //show error message
                }
            }

            IMG = capture.QueryFrame(); //QueryFrame means get an image from the camera, each secon it will take 15 pictures, when you run the program
                                        //and reach this line it will put -the last image that it gets* inside the object 'IMG'
                                        

            GrayImg = IMG.Convert<Gray, Byte>(); //Convert image from colored image to the gray image
            BWImg = GrayImg.ThresholdBinaryInv(new Gray(50), new Gray(255)); //part of the thing that i will do is changing first value to find the object

            Xpx = 0;//this helps how to specify the center of the object
            Ypx = 0;
            N = 0; //N is number of pixels
            for (int i = 0; i < BWImg.Width; i++) // for scaning the image
                for (int j = 0; j < BWImg.Height; j++)
                {
                    if (BWImg[j, i].Intensity > 128)//if intensity of the image is greater than 128 // Threshold maksimum 255 olduğu için onun yarısını 127 aldık
                    {
                        N++;
                        Xpx += i; //add its location to the px and py in pixels
                        Ypx += j;
                    }
                }

            if (N > 0)//if N>0 there is an object in the image
            {
                myScale = 100.0 / BWImg.Width;
                Xpx = Xpx / N; //X center point of the foreground object
                Ypx = Ypx / N;

                Xpx = Xpx - BWImg.Width / 2; //Xpx-640
                Ypx = BWImg.Height / 2 - Ypx; //360-Ypx

                Xcm = Xpx * myScale; //location in cantimeters 
                Ycm = Ypx * myScale;

                textBox1.Text = Xcm.ToString();
                textBox2.Text = Ycm.ToString();
                textBox3.Text = N.ToString();

                //invers kinematics model

                Zcm = Ycm + 15; //robotla kamera arası uzaklık
                Ycm1 = -Xcm;
                Xcm1 = 100;
                double Th1 = Math.Atan2(Ycm1, Xcm1);
                double Th2 = Math.Atan(Math.Sin(Th1) * (Zcm-d1) / (Ycm1));

                Th1 = (Th1 * (180 / Math.PI));
                Th2 = (Th2 * (180 / Math.PI));


                Th1 = Th1 + 100; //robottan kaynaklı 
                Th2 = Th2 + 80;




                Buff[0] = (byte)Th1; //Th1 
                Buff[1] = (byte)Th2; //Th2
                _serialPort.Write(Buff, 0, 2);
            }

            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = N.ToString();
            }

            try
            {

                imageBox1.Image = IMG;//color image
                imageBox2.Image = GrayImg; //gray image
                imageBox3.Image = BWImg;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //Application.Idle += processFrame; //means run this function in the background
            timer1.Enabled = true;
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Application.Idle -= processFrame; //means stop running this function
            timer1.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)//button3 is for saving the image
        {
            GrayImg.Save("\\0000\\Image" + ".jpg"); //IMG is the colored image
            //0000 adlı klasörde sakladım resimleri

        }
        private void Timer1Tick(object sender, EventArgs e)
        {
            processFrame(sender, e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        void Label5Click(object sender, EventArgs e)
        {

        }

    }
}

