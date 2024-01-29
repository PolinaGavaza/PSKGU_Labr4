using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace LabR4
{


    [Serializable]
    public partial class Form1 : Form
    {

        public int LS = 1; 
        public Color LC = Color.Black, SC;
        public List<Form2> CTR;
        
        public Form1()
        {
            InitializeComponent();
            CTR = new List<Form2>();
            HideElements();
        }
        public void CheckForm(Form2 f2)
        {
            CTR.Remove(f2);
            ControlForms();
        }
        private void ControlForms()
        {
            if (CTR.Count > 0)
            { ShowElements(); }
            else
            { HideElements(); }
        }
        private void HideElements()
        {
            сохранитьToolStripMenuItem.Enabled = false;
            сохранитьКакToolStripMenuItem.Enabled = false;
            толщинаПераToolStripMenuItem.Enabled = false;
            цветЛинииToolStripMenuItem.Enabled = false;
            цветФонаToolStripMenuItem.Enabled = false;
        }

        private void ShowElements()
        {
            сохранитьToolStripMenuItem.Enabled = true;
            сохранитьКакToolStripMenuItem.Enabled = true;
            толщинаПераToolStripMenuItem.Enabled = true;
            цветЛинииToolStripMenuItem.Enabled = true;
            цветФонаToolStripMenuItem.Enabled = true;
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.MdiParent = this;
            f.Text = "Рисунок " + MdiChildren.Length.ToString();            
            f.Show();
            CTR.Add(f);
            ControlForms();
        }

        private void окноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2
            {
                MdiParent = this,
                Text = "Рисунок " + MdiChildren.Length.ToString()
            };
            f.Show();
            CTR.Add(f);
            ControlForms();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Все файлы изображений(*.BMP;*.PNG;*.JPEG;*.GIF;*.TIFF)|*.BMP;*.PNG;*.JPG;*.GIF;*.TIFF|PNG|*.png|JPEG|*.jpg;*.jpeg;*.jpe;*.jfif|GIF|*.gif|TIFF|*.tiff";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Form2 f = new Form2
                                {
                    MdiParent = this,
                    Text = openFileDialog.SafeFileName,
                    SavingPath = openFileDialog.FileName
                };
                f.Show();
                CTR.Add(f);
                BinaryFormatter formatter = new BinaryFormatter();               

                List<object> list;
                using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    list = (List<object>)formatter.Deserialize(fs);
                    f.array = list;
                }                
            }
            ControlForms();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
            ControlForms();
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAs();
        }
        public void SaveFile()
        {
            if (((Form2)ActiveMdiChild).SavingPath == "")
            {
                SaveFileAs();
            }
            else
            {
                SaveSerializedData(((Form2)ActiveMdiChild).SavingPath);
            }
        }
        private void SaveFileAs()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Все файлы изображений(*.BMP;*.PNG;*.JPEG;*.GIF;*.TIFF)|*.BMP;*.PNG;*.JPG;*.GIF;*.TIFF|PNG|*.png|JPEG|*.jpg;*.jpeg;*.jpe;*.jfif|GIF|*.gif|TIFF|*.tiff";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ((Form2)ActiveMdiChild).SavingPath = saveFileDialog.FileName;
                SaveSerializedData(saveFileDialog.FileName);
                ActiveMdiChild.Text = Path.GetFileName(saveFileDialog.FileName);
            }
        }

        private void SaveSerializedData(string path)
        {
            Form2 f2 = (Form2)ActiveMdiChild;
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, f2.array);
            stream.Close();
            f2.IsModified = false;
        }

        private void толщинаПераToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PenSize f = new PenSize();
            f.ShowDialog(this);
            LS = f.PW;            
        }
                
        private void цветЛинииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                LC = colorDialog1.Color;
            }
        }

        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                SC = colorDialog1.Color;
            }        
        }
    }
}