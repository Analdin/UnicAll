using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace UnicAll
{
    public partial class Form1 : Form
    {
        public static ulong words { get; set; }
        public static ulong cells { get; set; }
        public static int unicResults { get; set; }
        public static int totalVariants { get; set; }

        public Form1()
        {
            InitializeComponent();
            resultBox.TextChanged += resultBox_TextChanged;
            // ��������� ����� � ��������.
            Task.Factory.StartNew(ConsoleOpen);
        }
        private void ConsoleOpen()
        {
            // ��������� �������.
            if (AllocConsole())
            {
                System.Console.WriteLine("��� ������ �������� exit.");
                while (true)
                {
                    // ��������� ������.
                    string output = Console.ReadLine();
                    if (output == "exit")
                        break;
                    // ������� ������ � textBox
                    Action action = () => totalOptions.Text += output + Environment.NewLine;
                    if (InvokeRequired)
                        Invoke(action);
                    else
                        action();
                }
                // ��������� �������.
                //FreeConsole();
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeConsole();

        private void countOfWords_TextChanged(object sender, EventArgs e)
        {
            // �����
            ulong words1 = 0;
            if(ulong.TryParse(countOfWords.Text, out words1))
            {
                words = words1;
            }
        }

        private void countOfNums_TextChanged(object sender, EventArgs e)
        {
            // ������
            ulong cells1 = 0;
            if(ulong.TryParse(countOfNums.Text, out cells1))
            {
                cells = cells1;
            }
        }

        private void resultBox_TextChanged(object sender, EventArgs e)
        {
            int unicResults1 = 0;
            if(Int32.TryParse(resultBox.Text, out unicResults1))
            {
                unicResults = unicResults1;
            }
        }

        private void totalOptions_TextChanged(object sender, EventArgs e)
        {
            int totalVariants1 = 0;
            if (Int32.TryParse(totalOptions.Text, out totalVariants1))
            {
                totalVariants = totalVariants1;
            }
        }

        private void calcBtn_Click(object sender, EventArgs e)
        {
            // ������ ������ ����� ��������� ��������
            totalOptions.Text = "";
            resultBox.Text = "";

            string mainFolder = Directory.GetCurrentDirectory() + @"\Files";
            string pattern = @"\b(ArtId=[0-9]*)\b";

            // ���� ��������� "���������� ����" � "���������� �����"
            if(words != 0  & cells != 0)
            {
                // 0 - ������� ������ �������� �������
                CountOfVariants(0);

                // ������������ �� �������, � ������� ����������
                FormulaCount();

            }
            else
            {
                int count = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\Files\Words.txt").Length;

                using (StreamWriter sw = File.CreateText(Directory.GetCurrentDirectory() + @"\Files\target.txt"))
                {
                    List<string> linesToWrite = new List<string>();
                    List<string> linesToWrite2 = new List<string>();

                    foreach (var file in Directory.EnumerateFiles(mainFolder, "*.txt", SearchOption.AllDirectories))
                    {
                        using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + @"\Files\Words.txt", System.Text.Encoding.Default))
                        {
                            string line;
                            for (int i = 0; i < count; i++)
                            {
                                string newString = sr.ReadLine();
                                linesToWrite.Add(newString);
                            }
                        }
                    }

                    linesToWrite2 = linesToWrite.Distinct().ToList();

                    unicResults = linesToWrite2.Count;

                    // ������� ���������� ��������� - ���������� � �����
                    resultBox.Text += unicResults;
                    totalOptions.Text += count.ToString();

                    MessageBox.Show("������ ���������");
                }
            }
        }

        private void FormulaCount()
        {
            int N = (int)words;
            int K = (int)cells;

            // 1) ���������� ���������� ��������
            var nFac = Enumerable.Range(1, N).Aggregate(1, (p, item) => p * item);
            var nkFac = Enumerable.Range(1, N - K).Aggregate(1, (p, item) => p * item);
            var result = nFac / nkFac;
            Console.WriteLine("���������� ����������: " + result);
            resultBox.Text += result.ToString();

            // 2) ����� ��������

            // �������
            //ulong diff = words - cells;
            // ��������� ��������� ������� ����� - 300
            //ulong firstNum = Factorial(words);
            // ��������� ��������� ������� ����� - 292 (diff)
            //ulong secondNum = Factorial(cells);
            // ���������� ��������� - �����
            //ulong total = firstNum / secondNum;

            int total = Enumerable.Range(1, N).Aggregate(1, (p, item) => p * item);

            Console.WriteLine("����� ����������: " + total);
            totalOptions.Text += total.ToString();
        }

        //ulong Factorial(ulong n)
        //{
        //    if (n == 1) return 1;

        //    return n * Factorial(n - 1);
        //}

        int N = 0;
        int[,] a = new int[5, 7]; // ����������� �������, �� �� 300 � 8

        private void CountOfVariants(int n)
        {
            int x = n % 7;
            int y = n / 7;
            for (int k, j, i = 1; i <= 7 && N < 10; i++)
            {
                for (j = 0; j < x; j++)
                    if (a[y, j] == i) break;
                if (j < x) continue;

                for (j = 0; j < y; j++)
                    if (a[j, x] == i) break;
                if (j < y) continue;
                if (y > 0 && x > 0)
                {
                    int b = a[y, x - 1];
                    for (j = 0; j < y; j++)
                    {
                        for (k = 0; k < 6; k++)
                            if (a[j, k] == b && a[j, k + 1] == i) break;
                        if (k < 6) break;
                    }
                    if (j < y) continue;
                }
                a[y, x] = i;
                if (n < 34) CountOfVariants(n + 1);
                else
                {
                    for (j = 0; j < 5; j++)
                    {
                        for (k = 0; k < 7; k++)
                            Console.Write(" " + a[j, k]);
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                    N++;
                }
            }
        }
    }
}