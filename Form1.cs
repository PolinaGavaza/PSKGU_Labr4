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
    // Создание класса Form1, унаследованного от Form
    [Serializable]
    public partial class Form1 : Form
    {
        // Поля для хранения толщины пера и цветов
        public int LS = 1;
        public Color LC = Color.Black, SC;
        public List<Form2> CTR; // Список MDI-форм

        // Конструктор класса
        public Form1()
        {
            InitializeComponent();
            CTR = new List<Form2>(); // Инициализация списка MDI-форм
            HideElements(); // Метод для скрытия элементов управления
        }

        // Метод для проверки формы и удаления из списка MDI-форм
        public void CheckForm(Form2 f2)
        {
            CTR.Remove(f2); // Удаление формы из списка
            ControlForms(); // Вызов метода контроля форм
        }

        // Метод для контроля состояния элементов управления
        private void ControlForms()
        {
            // Если количество форм больше нуля, показать элементы, иначе скрыть их
            if (CTR.Count > 0)
            {
                ShowElements();
            }
            else
            {
                HideElements();
            }
        }

        // Метод для скрытия элементов управления
        private void HideElements()
        {
            сохранитьToolStripMenuItem.Enabled = false;
            сохранитьКакToolStripMenuItem.Enabled = false;
            толщинаПераToolStripMenuItem.Enabled = false;
            цветЛинииToolStripMenuItem.Enabled = false;
            цветФонаToolStripMenuItem.Enabled = false;
        }

        // Метод для отображения элементов управления
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
            f.MdiParent = this; // Устанавливаем текущую форму в качестве родительской для формы Form2
            f.Text = "Рисунок " + MdiChildren.Length.ToString(); // Устанавливаем заголовок формы
            f.Show(); // Показываем форму
            CTR.Add(f); // Добавляем форму в список MDI-форм
            ControlForms(); // Вызываем метод контроля состояния элементов управления
        }

        private void окноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2 // Создаём новую форму Form2
            {
                MdiParent = this, // Устанавливаем текущую форму в качестве родительской для формы Form2
                Text = "Рисунок " + MdiChildren.Length.ToString() // Устанавливаем заголовок формы
            };
            f.Show(); // Показываем форму
            CTR.Add(f); // Добавляем форму в список MDI-форм
            ControlForms(); // Вызываем метод контроля состояния элементов управления
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog(); // Создаём диалоговое окно открытия файла
            openFileDialog.Filter = "Все файлы изображений(*.BMP;*.PNG;*.JPEG;*.GIF;*.TIFF)|*.BMP;*.PNG;*.JPG;*.GIF;*.TIFF|PNG|*.png|JPEG|*.jpg;*.jpeg;*.jpe;*.jfif|GIF|*.gif|TIFF|*.tiff"; // Устанавливаем фильтр для открытия файлов

            if (openFileDialog.ShowDialog() == DialogResult.OK) // Если пользователь выбрал файл и нажал OK
            {
                Form2 f = new Form2 // Создаём новую форму Form2
                {
                    MdiParent = this, // Устанавливаем текущую форму в качестве родительской для формы Form2
                    Text = openFileDialog.SafeFileName, // Устанавливаем заголовок формы
                    SavingPath = openFileDialog.FileName // Устанавливаем путь сохранения
                };
                f.Show(); // Показываем форму
                CTR.Add(f); // Добавляем форму в список MDI-форм
                BinaryFormatter formatter = new BinaryFormatter(); // Создаём экземпляр класса BinaryFormatter

                List<object> list; // Создаём список объектов
                using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read)) // Открываем файл для чтения
                {
                    list = (List<object>)formatter.Deserialize(fs); // Десериализуем данные из файла в список
                    f.array = list; // Присваиваем полученный список массиву формы
                }
            }
            ControlForms(); // Вызываем метод контроля состояния элементов управления
        }
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(); // Вызов метода сохранения файла
            ControlForms(); // Вызов метода контроля состояния элементов управления
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAs(); // Вызов метода сохранения файла как
        }

        public void SaveFile()
        {
            if (((Form2)ActiveMdiChild).SavingPath == "")
            {
                SaveFileAs(); // Если путь сохранения не установлен, вызываем метод сохранения файла как
            }
            else
            {
                SaveSerializedData(((Form2)ActiveMdiChild).SavingPath); // Иначе вызываем метод сохранения сериализованных данных
            }
        }

        private void SaveFileAs()
        {
            var saveFileDialog = new SaveFileDialog(); // Создаём экземпляр класса SaveFileDialog для выбора пути сохранения файла
            saveFileDialog.Filter = "Все файлы изображений(*.BMP;*.PNG;*.JPEG;*.GIF;*.TIFF)|*.BMP;*.PNG;*.JPG;*.GIF;*.TIFF|PNG|*.png|JPEG|*.jpg;*.jpeg;*.jpe;*.jfif|GIF|*.gif|TIFF|*.tiff"; // Указываем фильтр для выбора файлов
            if (saveFileDialog.ShowDialog() == DialogResult.OK) // Если пользователь выбрал файл и нажал OK
            {
                ((Form2)ActiveMdiChild).SavingPath = saveFileDialog.FileName; // Устанавливаем путь сохранения активной MDI-формы
                SaveSerializedData(saveFileDialog.FileName); // Вызываем метод сохранения сериализованных данных
                ActiveMdiChild.Text = Path.GetFileName(saveFileDialog.FileName); // Устанавливаем имя файла в заголовке активной MDI-формы
            }
        }

        private void SaveSerializedData(string path)
        {
            Form2 f2 = (Form2)ActiveMdiChild; // Получаем активную MDI-форму
            BinaryFormatter formatter = new BinaryFormatter(); // Создаём экземпляр класса BinaryFormatter для сериализации данных
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None); // Создаём поток для записи в файл
            formatter.Serialize(stream, f2.array); // Сериализуем массив данных в поток
            stream.Close(); // Закрываем поток
            f2.IsModified = false; // Устанавливаем флаг модификации в false
        }

        private void толщинаПераToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PenSize f = new PenSize(); // Создаём экземпляр класса PenSize для выбора толщины пера
            f.ShowDialog(this); // Отображаем форму для выбора толщины пера
            LS = f.PW; // Устанавливаем выбранную толщину пера
        }

        private void цветЛинииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK) // Если пользователь выбрал цвет и нажал OK
            {
                LC = colorDialog1.Color; // Устанавливаем выбранный цвет линии
            }
        }

        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK) // Если пользователь выбрал цвет и нажал OK
            {
                SC = colorDialog1.Color; // Устанавливаем выбранный цвет фона
            }
        }
    }
}

