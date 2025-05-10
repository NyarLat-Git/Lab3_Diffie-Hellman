using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using static WindowsFormsApp1.Utils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Считываем количество знаков
            int digits = 20;
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
                digits = int.Parse(textBox1.Text);

            if (digits <= 1)
            {
                digits = 2;
            }

            // 2. Задаём диапазон для q
            int qDigits = digits - 1;
            BigInteger qMin = BigInteger.Pow(10, qDigits - 1);
            BigInteger qMax = BigInteger.Pow(10, qDigits);
            BigInteger qRange = qMax - qMin;

            // 3. Генерируем безопасную пару p = 2q + 1
            BigInteger q, p;
            while (true)
            {
                q = FindNextPrimeM(RandomBigInteger(qRange) + qMin);
                p = q * 2 + 1;
                if (IsPrimeM(p) && p.ToString().Length == digits)
                    break;
            }

            // Вывод
            textBox2.Text = p.ToString();
            textBox3.Text = q.ToString();

            Random rng = new Random();
            List<BigInteger> candidates = new List<BigInteger>();
            BigInteger g = 2;

            while (g < p - 1)
            {
                if (g > 1)
                {
                    BigInteger gMod = g % p;
                    if (BigInteger.ModPow(gMod, q, p) != 1)
                    {
                        candidates.Add(gMod);
                        if (candidates.Count == 10) // остановимся после 10 подходящих
                            break;
                    }
                }
                g += 1;
            }

            if (candidates.Count > 1)
                g = candidates[rng.Next(1, candidates.Count)]; // исключаем первый (индекс 0)
            else
                throw new Exception("Недостаточно подходящих g найдено");

            textBox4.Text = g.ToString();

            // 5. Вычисляем половину с округлением вверх
            int halfDigits = (digits + 1) / 2;
            BigInteger min = BigInteger.Pow(10, halfDigits - 1);
            BigInteger max = BigInteger.Pow(10, halfDigits);
            BigInteger range = max - min;

            BigInteger smallNumber = RandomBigInteger(range) + min;
            BigInteger smallNumber1 = RandomBigInteger(range) + min;

            textBox5.Text = smallNumber.ToString();
            textBox7.Text = smallNumber1.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
   
            textBox6.Clear();
            textBox8.Clear();
            BigInteger p;
            BigInteger q;

            if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Введите или сгенерируйте p и q!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                p = BigInteger.Parse(textBox2.Text);
                q = BigInteger.Parse(textBox3.Text);

                if (p == q)
                {
                    MessageBox.Show("Числа p и q должны быть разными!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("p и q должны быть целыми числами!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            p = BigInteger.Parse(textBox2.Text);


            BigInteger x1 = BigInteger.Parse(textBox5.Text);

            BigInteger x2 = BigInteger.Parse(textBox7.Text);

            BigInteger g = BigInteger.Parse(textBox4.Text);

            BigInteger y1 = BigInteger.ModPow(g, x1, p);
            BigInteger y2 = BigInteger.ModPow(g, x2, p);

            BigInteger z1 = BigInteger.ModPow(y2, x1, p);
            BigInteger z2 = BigInteger.ModPow(y1, x2, p);


            textBox6.Text = z1.ToString();
            textBox8.Text = z2.ToString();


        }
}
}
