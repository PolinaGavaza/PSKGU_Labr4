using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabR4
{
    [Serializable]
    public partial class Form2 : Form
    {
        public bool IsModified = false;
        public string SavingPath = "";
        public List<object> array = new List<object>();        
        Graphics g;
        bool IsDraw = false;

        public int LineSize;
        public Color LineColor;
        public Color SolidColor;

        int X1, Y1;
        Figure.Rect rect;
        Figure.Elips ellips;

        public Form2()
        {
            InitializeComponent();            
        }
        private void CheckSettings() 
        {
            Form1 recieveSettings = (Form1)MdiParent;
            this.LineSize = recieveSettings.LS;
            this.LineColor = recieveSettings.LC;
            this.SolidColor = recieveSettings.SC;
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            CheckSettings();
            g = CreateGraphics();
            IsDraw = true;
            X1 = e.X;
            Y1 = e.Y;
            if (IsDraw == true)
            {
                if (e.Button == MouseButtons.Left)
                {
                    rect = new Figure.Rect(new Point(X1, Y1), new Point(e.X, e.Y), LineSize, LineColor, SolidColor);
                }
                if (e.Button == MouseButtons.Right)
                {
                    ellips = new Figure.Elips(new Point(X1, Y1), new Point(e.X, e.Y), LineSize, LineColor, SolidColor);
                }
            }
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDraw == true)
            {
                if (e.Button == MouseButtons.Left)
                {
                    rect.Hide(g);
                    rect.SetPoint(e.X, e.Y);
                    rect.Dash(g);
                }
                if (e.Button == MouseButtons.Right)
                {
                    ellips.Hide(g);
                    ellips.SetPoint(e.X, e.Y);
                    ellips.Dash(g);
                }
            }

        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsDraw == true)
            {
                if (e.Button == MouseButtons.Left)
                {
                    rect.Draw(g);
                    array.Add(rect);
                    Invalidate();
                }
                if (e.Button == MouseButtons.Right)
                {
                    ellips.Draw(g);
                    array.Add(ellips);
                    Invalidate();
                }
                g.Dispose();
                IsDraw = false;
                IsModified = true;
            }
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            foreach (Figure t in array)
            {
                t.Draw(e.Graphics);
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsModified)
            { 
                DialogResult result = MessageBox.Show("Сохранить?", "Сохранение", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }

                if (result == DialogResult.Yes)
                {
                    ((Form1)ParentForm).SaveFile();
                }
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 f1 = (Form1)MdiParent;
            f1.CheckForm(this);
        }
    }
}
