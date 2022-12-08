using System.Numerics;

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
        }

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
                // ���������� �� �������, � ������� ����������
                FormulaCount();
            }
        }

        private void FormulaCount()
        {
            try
            {
                int N = (int)words;
                int K = (int)cells;

                // 1) ���������� ���������� ��������

                //var nFac = Enumerable.Range(1, N).Aggregate(1, (p, item) => p * item);

                BigInteger nFac = Enumerable.Range(1, N).Aggregate(1, (p, item) => p * item);
                BigInteger nkFac = Enumerable.Range(1, N - K).Aggregate(1, (p, item) => p * item);

                //ulong nFac = 1UL;
                //for (ulong i = 1; i <= N; i++) nFac *= i;

                //ulong nkFac = 1UL;
                //for (ulong i = 1; i <= N - K; i++) nkFac *= i;

                var result = nFac / nkFac;

                //var nkFac = Enumerable.Range(1, N - K).Aggregate(1, (p, item) => p * item);

                //var result = nFac / nkFac;
                //Console.WriteLine("���������� ����������: " + result);
                resultBox.Text += result.ToString();

                // 2) ����� ��������

                //int total = Enumerable.Range(1, N).Aggregate(1, (p, item) => p * item);

                //ulong total = (ulong)Math.Pow(N, K);

                BigInteger total = BigInteger.Pow(N, K);

                //Console.WriteLine("����� ����������: " + total);
                totalOptions.Text += total.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}