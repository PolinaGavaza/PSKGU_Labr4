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
    // Объявление класса PenSize, который представляет окно выбора размера пера
    public partial class PenSize : Form
    {
        public int PW; // Публичное поле для хранения выбранного размера пера

        // Конструктор класса, который инициализирует компоненты окна
        public PenSize()
        {
            InitializeComponent();
        }

        // Обработчик нажатия кнопки "OK"
        private void button_OK_Click(object sender, EventArgs e)
        {
            // Проверка выбран ли элемент в комбобоксе
            if (comboBox1.SelectedItem == null)
            {
                // Если введенное значение не пустое, сохраняем его в PW, иначе используем выбранный элемент
                if (comboBox1.Text != "")
                {
                    PW = Int32.Parse(comboBox1.Text);
                }
                else
                    PW = Convert.ToInt32(comboBox1.SelectedItem);
            }
            else
                PW = Int32.Parse(comboBox1.Text);

            // Проверка, что значение PW неотрицательно, если отрицательно, устанавливаем минимальное значение
            if (PW <= 0)
            {
                MessageBox.Show(" Значение не корректно! n Будет установлено минимальное значение!", " Ошибка!"); ;
                PW = 1;
            }

            // Проверка, что значение PW не больше 15, если больше, устанавливаем максимальное значение
            if (PW > 15)
            {
                MessageBox.Show(" Значение не корректно! n Будет установлено максимальное значение!", " Ошибка!"); ;
                PW = 15;
            }

            // Закрыть окно
            Close();
        }

        // Обработчик нажатия кнопки "CANCEL"
        private void button_CANCEL_Click(object sender, EventArgs e)
        {
            // Закрыть окно
            Close();
        }
    }
}
