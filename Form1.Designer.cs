using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace HydrostaticsCalculator
{
    public partial class Form1 : Form
    {
        public static double[,] HydrostaticsAll { get; set; }
        public static double[] HydrostaticsTrimDegerleri { get; set; }

        private IEnumerable<double[]> ConvertToEnumerable(double[,] array)
        {
            int rows = array.GetLength(0); // Satır sayısı
            int cols = array.GetLength(1); // Sütun sayısı

            for (int i = 0; i < rows; i++)
            {
                double[] row = new double[cols];
                for (int j = 0; j < cols; j++)
                {
                    row[j] = array[i, j];
                }
                yield return row;
            }
        }




        public Form1()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hydrostatics.txt");
            LoadHydrostatics(filePath);
            PopulateTrimValues();
        }

        private void LoadHydrostatics(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var trimValues = new HashSet<double>();
            var dataList = new List<double[]>();

            foreach (var line in lines)
            {
                // Boş satırları, başlıkları ve açıklamaları atla
                if (string.IsNullOrWhiteSpace(line) || line.Any(char.IsLetter))
                    continue;

                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 2 && double.TryParse(parts[0], out double trim) && double.TryParse(parts[1], out double draft))
                {
                    trimValues.Add(trim);

                    var row = parts.Select(value =>
                    {
                        if (double.TryParse(value, out double result)) return result;
                        return double.NaN; // Sayısal olmayan değerleri NaN yap
                    }).ToArray();

                    dataList.Add(row);
                }
            }
            if (dataList.Count == 0)
            {
                MessageBox.Show("TXT dosyasından veri okunamadı. Lütfen dosya formatını kontrol edin.");
                return;
            }

            // Convert dataList to a 2D array (double[,])
            int rowCount = dataList.Count;
            int colCount = dataList[0].Length;

            HydrostaticsAll = new double[rowCount, colCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    HydrostaticsAll[i, j] = dataList[i][j];
                }
            }

            HydrostaticsTrimDegerleri = trimValues.OrderBy(t => t).ToArray();
        }

        private void PopulateTrimValues()
        {
            cmbTrimValues.Items.Clear();
            foreach (var trim in HydrostaticsTrimDegerleri)
            {
                cmbTrimValues.Items.Add(trim);
            }
            cmbTrimValues.SelectedIndex = 0;
        }

        private void cmbTrimValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedTrim = (double)cmbTrimValues.SelectedItem;

            var filteredData = ConvertToEnumerable(HydrostaticsAll)
                .Where(row => Math.Abs(row[0] - selectedTrim) < 0.01)
                .ToArray();

            DisplayTable(filteredData);
        }

        private void DisplayTable(double[][] tableData)
        {
            if (tableData.Length > 0)
            {
                dataGridView1.ColumnCount = tableData[0].Length; // Satırdaki sütun sayısı
            }
            dataGridView1.Rows.Clear();
            foreach (var row in tableData)
            {
                dataGridView1.Rows.Add(row.Select(v => v.ToString("F2")).ToArray());
            }
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (double.TryParse(txtTrim.Text, out double trim) && double.TryParse(txtDraft.Text, out double draft))
            {
                double minTrim = HydrostaticsTrimDegerleri.Min();
                double maxTrim = HydrostaticsTrimDegerleri.Max();
                double minDraft = ConvertToEnumerable(HydrostaticsAll).Min(row => row[1]);
                double maxDraft = ConvertToEnumerable(HydrostaticsAll).Max(row => row[1]);

                if (trim < minTrim || trim > maxTrim)
                {
                    MessageBox.Show($"Trim değeri geçersiz. Lütfen {minTrim} ile {maxTrim} arasında bir değer girin.");
                    return;
                }

                if (draft < minDraft || draft > maxDraft)
                {
                    MessageBox.Show($"Draft değeri geçersiz. Lütfen {minDraft} ile {maxDraft} arasında bir değer girin.");
                    return;
                }
                CalculateInterpolatedValues(trim, draft);
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir trim ve draft değeri girin.");
            }
        }

        private void CalculateInterpolatedValues(double inputTrim, double inputDraft)
        {
            var trims = HydrostaticsTrimDegerleri.OrderBy(t => Math.Abs(t - inputTrim)).Take(2).ToArray();

            if (trims.Length < 2)
            {
                MessageBox.Show("Uygun iki trim değeri bulunamadı.");
                return;
            }

            var lowerTrimTable = ConvertToEnumerable(HydrostaticsAll)
                .Where(row => Math.Abs(row[0] - trims[0]) < 0.01)
                .ToArray();

            var upperTrimTable = ConvertToEnumerable(HydrostaticsAll)
                .Where(row => Math.Abs(row[0] - trims[1]) < 0.01)
                .ToArray();

            if (!lowerTrimTable.Any() || !upperTrimTable.Any())
            {
                MessageBox.Show("Uygun satırlar bulunamadı.");
                return;
            }

            var lowerDraftRows = lowerTrimTable.OrderBy(row => Math.Abs(row[1] - inputDraft)).Take(2).ToArray();
            var upperDraftRows = upperTrimTable.OrderBy(row => Math.Abs(row[1] - inputDraft)).Take(2).ToArray();

            if (lowerDraftRows.Length < 2 || upperDraftRows.Length < 2)
            {
                MessageBox.Show("Interpolasyon için yeterli veriler bulunamadı.");
                return;
            }

            var lowerInterpolatedRow = lowerDraftRows[0]
                .Zip(lowerDraftRows[1], (v1, v2) => Interpolate(inputDraft, lowerDraftRows[0][1], v1, lowerDraftRows[1][1], v2))
                .ToArray();

            var upperInterpolatedRow = upperDraftRows[0]
                .Zip(upperDraftRows[1], (v1, v2) => Interpolate(inputDraft, upperDraftRows[0][1], v1, upperDraftRows[1][1], v2))
                .ToArray();

            var finalRow = lowerInterpolatedRow
                .Zip(upperInterpolatedRow, (v1, v2) => Interpolate(inputTrim, trims[0], v1, trims[1], v2))
                .ToArray();

            DisplayInterpolatedResult(finalRow);
        }

        private double Interpolate(double x, double x1, double y1, double x2, double y2)
        {
            return y1 + (x - x1) * (y2 - y1) / (x2 - x1);
        }

        private void DisplayInterpolatedResult(double[] resultRow)
        {
            if (resultRow == null || resultRow.Length == 0)
            {
                MessageBox.Show("Sonuç boş. Interpolasyon başarısız oldu.");
                return;
            }
            lblResult.Text = string.Join(", ", resultRow.Select(v => v.ToString("F2")));
            dataGridView1.ColumnCount = resultRow.Length;
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(resultRow.Select(v => v.ToString("F2")).ToArray());
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private ComboBox cmbTrimValues;
        private TextBox txtTrim;
        private TextBox txtDraft;
        private DataGridView dataGridView1;
        private Button btnCalculate;
        private Label lblResult;
        private Button btnTest;
    }
}