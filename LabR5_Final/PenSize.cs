using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabR4
{
    
    public partial class PenSize : Form
    {
        public int PW;
        public PenSize()
        {
            InitializeComponent();
        }
    
        private void button_OK_Click(object sender, EventArgs e)
        {
            
            if (comboBox1.SelectedItem == null)
            {
                if(comboBox1.Text != "") 
                { 
                    PW = Int32.Parse(comboBox1.Text); 
                }
                else
                    PW = Convert.ToInt32(comboBox1.SelectedItem);                                
            }
            else
                PW = Int32.Parse(comboBox1.Text); 
                         
            if (PW <= 0)
            {
                MessageBox.Show(" Значение не корректно! \n Будет установлено минимальное значение!", " Ошибка!"); ;
                PW = 1;
            }

            if (PW > 15)
            {
                MessageBox.Show(" Значение не корректно! \n Будет установлено максимальное значение!", " Ошибка!"); ;
                PW = 15;            
            }

            Close();
        }

        private void button_CANCEL_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
