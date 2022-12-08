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

            // ���� ��������� "���������� ����" � "���������� �����"
            if(words != 0  & cells != 0)
            {
                // ������������ �� �������, � ������� ����������
                FormulaCount();
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
            int total = Enumerable.Range(1, N).Aggregate(1, (p, item) => p * item);

            Console.WriteLine("����� ����������: " + total);
            totalOptions.Text += total.ToString();
        }
    }
}