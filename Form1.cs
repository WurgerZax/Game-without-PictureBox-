using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game
{
    



    public partial class Form1 : Form
    {

        public class CAnimal
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public Image BackgroundImage { get; set; }

        }

        public List<CAnimal> sheeps = new List<CAnimal>();
        public List<CAnimal> wolfs = new List<CAnimal>();
        public Random rnd = new Random();
        public int step = 96;
        public const int sizeAnimal = 48;
        public int countAllSheeps = 0;
        public int countAllWolfs = 0;

        public int sheepTimePeriod = 300;
        public int wolfTimePeriod = 400;

        public Bitmap bitmap;
      
        public Form1()
        {
            InitializeComponent();
          
        }



        public void newSheep()
        {
            int x = rnd.Next(0, field.Width);
            int y = rnd.Next(0, field.Height);

            if (x <= 0) x = 0;
            if (x >= field.Width - sizeAnimal) x = field.Width - sizeAnimal;
            if (y <= 0) y = 0;
            if (y >= field.Height - sizeAnimal) y = field.Height - sizeAnimal;

            var sheep = new CAnimal();
            sheep.BackgroundImage = Properties.Resources.sheep;
            
            sheep.Width = 48;
            sheep.Height = 48;
            sheep.Left = x;
            sheep.Top = y;
            sheeps.Add(sheep);
            //field.Controls.Add(sheep);
            countAllSheeps++;
            sheepCount.Text = "x" + countAllSheeps.ToString();
        }

        public void newWolf()
        {
            int x = rnd.Next(0, field.Width);
            int y = rnd.Next(0, field.Height);

            if (x <= 0) x = 0;
            if (x >= field.Width - sizeAnimal) x = field.Width - sizeAnimal;
            if (y <= 0) y = 0;
            if (y >= field.Height - sizeAnimal) y = field.Height - sizeAnimal;

            var wolf = new CAnimal();
            wolf.BackgroundImage = Properties.Resources.wolf;
           
            wolf.Width = 48;
            wolf.Height = 48;
            wolf.Left = x;
            wolf.Top = y;
            wolfs.Add(wolf);
           // field.Controls.Add(wolf);
            countAllWolfs++;
            wolfCount.Text = "x" + countAllWolfs.ToString();
        }

        public void calcPosition(CAnimal animal, int dir, int step)
        {
            switch(dir)
            {
                case 1:
                    {
                        animal.Top -= step;
                        if (animal.Top <= 0) animal.Top = 0;
                        break;
                    }
                case 2:
                    {
                        animal.Top -= step;
                        if (animal.Top <= 0) animal.Top = 0;
                        animal.Left += step;
                        if (animal.Left >= field.Width-sizeAnimal) animal.Left = field.Width - sizeAnimal;
                        break;
                    }
                case 3:
                    {
                        animal.Left += step;
                        if (animal.Left >= field.Width - sizeAnimal) animal.Left = field.Width - sizeAnimal;
                        break;
                    }
                case 4:
                    {
                        animal.Top += step;
                        if (animal.Top >= field.Height-sizeAnimal) animal.Top = field.Height - sizeAnimal;
                        animal.Left += step;
                        if (animal.Left >= field.Width - sizeAnimal) animal.Left = field.Width - sizeAnimal;
                        break;
                    }
                case 5:
                    {
                        animal.Top += step;
                        if (animal.Top >= field.Height - sizeAnimal) animal.Top = field.Height - sizeAnimal;
                        break;
                    }
                case 6:
                    {
                        animal.Top += step;
                        if (animal.Top >= field.Height - sizeAnimal) animal.Top = field.Height - sizeAnimal;
                        animal.Left -= step;
                        if (animal.Left <= 0) animal.Left = 0;
                        break;
                    }
                case 7:
                    {
                        animal.Left -= step;
                        if (animal.Left <= 0) animal.Left = 0;
                        break;
                    }
                case 8:
                    {
                        animal.Top -= step;
                        if (animal.Top <= 0) animal.Top = 0;
                        animal.Left -= step;
                        if (animal.Left <= 0) animal.Left = 0;
                        break;
                    }
            }
        }
        public void updateSheepsPosition()
        {

            for (int i = 0; i < sheeps.Count; i++)
            {
                int dir = rnd.Next(1, 9);
                calcPosition(sheeps[i], dir, step);
                Graphics gr = Graphics.FromImage(bitmap);
                gr.DrawImage(sheeps[i].BackgroundImage, new Point(sheeps[i].Left, sheeps[i].Top));
                field.Invalidate();
            }
        }

        public void updateWolfsPosition()
        {
           
            for (int i = 0; i < wolfs.Count; i++)
            {
                int dir = rnd.Next(1, 9);
                calcPosition(wolfs[i], dir, step);
 
                Graphics gr = Graphics.FromImage(bitmap);
                gr.DrawImage(wolfs[i].BackgroundImage, new Point(wolfs[i].Left, wolfs[i].Top));
                field.Invalidate();
            }
        }

        public bool isIntersect(CAnimal a1, CAnimal a2)
        {
            return (a2.Left < a1.Left + a1.Width) &&
            (a1.Left < (a2.Left + a2.Width)) &&
            (a2.Top < a1.Top + a1.Height) &&
            (a1.Top < a2.Top + a2.Height);
        }
            public void intersectSheepsAndSheeps()
        {
            int countAllSheeps = sheeps.Count;

            for (int i = 0; i<countAllSheeps-1; i++)
            {
                
                for (int j=i+1; j <countAllSheeps; j++)
                {
        
                    
                    if (isIntersect(sheeps[i], sheeps[j]))
                    {
                        newSheep();
                    }
                }
            }
        }

        public void intersectWolfsAndSheeps()
        {

            foreach (var wolf in wolfs)
            {
              for (int i = 0; i < countAllSheeps; i++)
                {
                    if (isIntersect(wolf, sheeps[i]))
                    {
                        sheeps[i].BackgroundImage = Properties.Resources.icons8_crossbones_64; // когда овечка умирает- появляется картинка костей
                        sheeps.Remove(sheeps[i]);
                        countAllSheeps--;
                        sheepCount.Text = "x" + countAllSheeps.ToString();
                        break;
                    }
               }
            }
        }

        public void updateSheepSituation()
        {
                updateSheepsPosition();
                intersectSheepsAndSheeps();

        }
        public void updateWolfSituation()
        {
            updateWolfsPosition();
            intersectWolfsAndSheeps();
        }

        public void initAnimals()
        {
            wolfs.Clear();
            sheeps.Clear();
            countAllSheeps = 0;
            countAllWolfs = 0;
           
            newWolf();
            newSheep();
            newSheep();

            updateSheepsPosition();
            updateWolfsPosition();
            field.Invalidate();


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, field, new object[] { true });

            bitmap = new Bitmap(field.Width, field.Height);
            initAnimals();
/*
            TimerCallback tmSheep = new TimerCallback(updateSheepSituation);
            System.Threading.Timer timerSheep = new System.Threading.Timer(tmSheep, 0, 0, 500);
           
            TimerCallback tmWolf = new TimerCallback(updateWolfSituation);
            System.Threading.Timer timerWolf = new System.Threading.Timer(tmWolf, 0, 0, 500);
*/
        }

        private void timerSheep_Tick(object sender, EventArgs e)
        {
            updateSheepSituation();
         }

        private void timerWolf_Tick(object sender, EventArgs e)
        {
            updateWolfSituation();
 
            if (countAllSheeps <= 0)
            {
                timerSheep.Enabled = false;
                timerWolf.Enabled = false;
                DialogResult result = MessageBox.Show("Do you want restart game?", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    initAnimals();
                    field.Invalidate();
                }
            }

            if (countAllSheeps >= 500)
            {
                timerSheep.Enabled = false;
                timerWolf.Enabled = false;
                DialogResult result = MessageBox.Show("Wolves can't eat so many sheep! \n Do you want restart game?", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    initAnimals();
                    field.Invalidate();
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            newWolf();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            newSheep();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timerSheep.Enabled = true;
            timerWolf.Enabled = true;
        }

        private void field_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;

           gr.DrawImage(bitmap, 0, 0, field.Width, field.Height);
            //bitmap = new Bitmap(field.Width, field.Height);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bitmap = new Bitmap(field.Width, field.Height);
        }

        private void scroolSheepSpeed_Scroll(object sender, ScrollEventArgs e)
        {

        }
    }
}
