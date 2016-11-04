using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


//Based off of this person's(Alex Bennett) code here http://timetocode.tumblr.com/post/27473374451/c-cellular-automata-for-procedural-generation



namespace CellularAutomataGenerator2
{
    public partial class Form1 : Form
    {
        
        //MAKE IT SO USER CAN RESET DURING RUNTIME - DONE
        //IMPROVE RANDOM COLOR GEN - MAKE IT SO USER CAN INPUT COLOUR VALUES - DONE




        //Size of the array.
        public static int sq = 150;
        int[,] cells = new int[sq, sq];

        //Dictionary to store the ruleSet.
        //Tuple is used here instead of say a List as we only need 4 ints (cells + neighbors and result cell)
        Dictionary<Tuple<int, int, int>, int> ruleSet = new Dictionary<Tuple<int, int, int>, int>();

        string initialFilePath = @"C:\temp\cellular-automata.png";
        int red = 0;
        int green = 0;
        int blue = 0;

        //Create the base map which is an array of 0s.
        public void CreateBaseMap()
        {
            for (int i = 0; i < sq; i++)
            {
                for (int j = 0; j < sq; j++)
                {
                    cells[i, j] = 0;
                }                    
            }                
        }
        
        //Create the starting point of the automation by putting 1s or 0s in pseudo random places on the array.
        public void CreateInitialMap()
        {
            Random r = new Random();

            for (int i = 0; i < sq; i++)
            {
                cells[i, 0] = r.Next(0, 2); 
            }
                
        }
        

        //This is the elementary cellular automata ruleset, which makes use of an 8 binary number to generate the cells.
        public void AddRules()
        {
            ruleSet.Add(new Tuple<int, int, int>(0, 0, 0), 1);
            ruleSet.Add(new Tuple<int, int, int>(0, 0, 1), 0);
            ruleSet.Add(new Tuple<int, int, int>(0, 1, 0), 0);
            ruleSet.Add(new Tuple<int, int, int>(0, 1, 1), 1);
            ruleSet.Add(new Tuple<int, int, int>(1, 0, 0), 1);
            ruleSet.Add(new Tuple<int, int, int>(1, 0, 1), 1);
            ruleSet.Add(new Tuple<int, int, int>(1, 1, 0), 1);
            ruleSet.Add(new Tuple<int, int, int>(1, 1, 1), 0);
        }
        

        //generates the cellular automation by going through each cell on the array.
        public void GenerateCells()
        {
            //For each cell in the array... 
            for (int i = 1; i < sq; i++)
            {
                for (int j = 0; j < sq; j++)
                {
                    //Find the left(j - 1), middle(i - 1) and right(j + 1) cells for each cell.
                    int left = (j == 0) ? left = cells[sq - 1, i - 1] : left = cells[j - 1, i - 1];

                    int middle = cells[j, i - 1];

                    int right = (j == sq - 1) ? right = cells[0, i - 1] : right = cells[j + 1, i - 1];

                    int result = ruleSet[new Tuple<int, int, int>(left, middle, right)];

                    cells[j, i] = result;
                }
            }
        }


        //Saves an image of the result to a .png file.
        public void SaveImage()
        {
            Bitmap img = new Bitmap(sq, sq);
            for (int i = 0; i < sq; i++)
            {
                for (int j = 0; j < sq; j++)
                {
                    Color g = (cells[i, j] == 1) ? Color.Black : Color.White;
                    img.SetPixel(i, j, g);
                }
            }
            MessageBox.Show("Save Complete.");
            img.Save(initialFilePath);
        }


        //Shows the image on the picturebox.
        public void ShowImage()
        {
            Image image = Image.FromFile(initialFilePath);
            if (pictureBox2.Image != null)
            {
                MessageBox.Show("Error: Please delete the current image first.");
            }
            else
            {               
                if(image.Width < pictureBox2.Width && image.Height < pictureBox2.Height)
                {
                    pictureBox2.Image = image;
                    pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pictureBox2.Image = image;
                    pictureBox2.Width = image.Width;
                    pictureBox2.Height = image.Height;
                }

            }           
        }


        //Resets the picturebox and the Dictionary.
        public void ResetImage()
        {
            pictureBox2.Image = null;
            ruleSet.Clear();
            
            MessageBox.Show("Image Reset.");
        }

        public Form1()
        {
            InitializeComponent();
        }


        //Change file path
        private void button2_Click(object sender, EventArgs e)
        {
            initialFilePath = textBox1.Text;
        }


        //Generate during runtime - Black and White colours.
        private void button1_Click(object sender, EventArgs e)
        {
            CreateBaseMap();
            AddRules();
            CreateInitialMap();
            GenerateCells();
            SaveImage();
            ShowImage();
            button1.Enabled = false;
            button4.Enabled = false;
            button6.Enabled = false;
        }


        //Reset the picturebox.
        private void button3_Click(object sender, EventArgs e)
        {
            ResetImage();
            button1.Enabled = true;
            button4.Enabled = true;
            button6.Enabled = true;
        }

        //Save Image with Psuedo Random pixel colours.
        public void SaveImageRandom()
        {
            Bitmap img = new Bitmap(sq, sq);
            Random rand = new Random();
            for (int i = 0; i < sq; i++)
            {
                for (int j = 0; j < sq; j++)
                {
                    Color test = (cells[i, j] == 1) ? Color.FromArgb(rand.Next(70,200),rand.Next(70,200),rand.Next(70,100)) : Color.White;
                    img.SetPixel(i, j, test);
                }
            }
            MessageBox.Show("Save Complete.");
            img.Save(initialFilePath);
        }


        //Save image with User inputted colour values.
        public void SaveImageUserInput()
        {
            Bitmap img = new Bitmap(sq, sq);
            Random rand = new Random();
            for (int i = 0; i < sq; i++)
            {
                for (int j = 0; j < sq; j++)
                {
                    Color test = (cells[i, j] == 1) ? Color.FromArgb(red, green, blue) : Color.White;
                    img.SetPixel(i, j, test);
                }
            }
            MessageBox.Show("Save Complete.");
            img.Save(initialFilePath);
        }


        //generate an image with pseudo random colours.
        private void button4_Click(object sender, EventArgs e)
        {
            CreateBaseMap();
            AddRules();
            CreateInitialMap();
            GenerateCells();
            SaveImageRandom();
            ShowImage();
            button4.Enabled = false;
            button1.Enabled = false;
            button6.Enabled = false;
        }


        //Save user inputted colour values.
        private void button5_Click(object sender, EventArgs e)
        {
            int redValue;
            int greenValue;
            int blueValue;

            if(int.TryParse(textBox2.Text, out redValue))
            {
                red = redValue;
                MessageBox.Show("Values Saved.");
            }
            else if(redValue > 255)
            {
                MessageBox.Show("Invalid Number.");
            }
            else
            {
                MessageBox.Show("Numbers Only!");
                return;
            }

            if (int.TryParse(textBox3.Text, out greenValue))
            {
                green = greenValue;
            }
            else if (greenValue > 255)
            {
                MessageBox.Show("Invalid Number.");
            }
            else
            {
                MessageBox.Show("Numbers Only!");
                return;
            }

            if (int.TryParse(textBox4.Text, out blueValue))
            {
                blue = blueValue;
            }
            else if (blueValue > 255)
            {
                MessageBox.Show("Invalid Number.");
            }
            else
            {
                MessageBox.Show("Numbers Only!");
                return;
            }
        }

        //Show image with user inputted colour values.
        private void button6_Click(object sender, EventArgs e)
        {
            CreateBaseMap();
            AddRules();
            CreateInitialMap();
            GenerateCells();
            SaveImageUserInput();
            ShowImage();
            button1.Enabled = false;
            button4.Enabled = false;
            button6.Enabled = false;
        }

        //Show help.
        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To change the range for the pseudo-random colour values, change them from within 'SaveImageRandom' method. \n");
        }
    }
}
