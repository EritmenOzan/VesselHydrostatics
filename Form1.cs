using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HydrostaticsCalculator
{
    public partial class Form1 : Form
    {
        private void SetupDataGridViewHeaders()
        {
            // Sabit başlıklar
            string[] headers = { "Trim", "Draft", "Displt", "LCB", "TCB", "VCB", "WPA", "LCF", "KML", "KMT", "BML", "BMT", "IL", "IT", "TPC", "MTC", "WSA" };

            // DataGridView'deki mevcut sütunları temizle
            dataGridView1.Columns.Clear();

            // Her başlık için sütun ekle
            foreach (string header in headers)
            {
                dataGridView1.Columns.Add(header, header); // Sütun adı ve başlığı aynı
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.cmbTrimValues = new System.Windows.Forms.ComboBox();
            this.txtTrim = new System.Windows.Forms.TextBox();
            this.txtDraft = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbTrimValues
            // 
            this.cmbTrimValues.FormattingEnabled = true;
            this.cmbTrimValues.Location = new System.Drawing.Point(13, 13);
            this.cmbTrimValues.Name = "cmbTrimValues";
            this.cmbTrimValues.Size = new System.Drawing.Size(121, 24);
            this.cmbTrimValues.TabIndex = 0;
            this.cmbTrimValues.Text = "cmbTrimValues";
            // 
            // txtTrim
            // 
            this.txtTrim.Location = new System.Drawing.Point(13, 56);
            this.txtTrim.Name = "txtTrim";
            this.txtTrim.Size = new System.Drawing.Size(100, 22);
            this.txtTrim.TabIndex = 1;
            this.txtTrim.Text = "txtTrim";
            this.txtTrim.TextChanged += new System.EventHandler(this.txtTrim_TextChanged);
            // 
            // txtDraft
            // 
            this.txtDraft.Location = new System.Drawing.Point(13, 96);
            this.txtDraft.Name = "txtDraft";
            this.txtDraft.Size = new System.Drawing.Size(100, 22);
            this.txtDraft.TabIndex = 2;
            this.txtDraft.Text = "txtDraft";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 208);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(303, 150);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(13, 138);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(75, 23);
            this.btnCalculate.TabIndex = 4;
            this.btnCalculate.Text = "Hesapla";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click_1);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(12, 176);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(173, 16);
            this.lblResult.TabIndex = 5;
            this.lblResult.Text = "Sonuçlar burada görünecek";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(110, 138);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 6;
            this.btnTest.Text = "Test Et";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(683, 487);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtDraft);
            this.Controls.Add(this.txtTrim);
            this.Controls.Add(this.cmbTrimValues);
            this.Name = "Form1";
            this.Text = "dataGridView1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            SetupDataGridViewHeaders();

            double[] testRow = { -3.75, 1.02, 900.00, 28.50, 0.001, 1.00 }; // Örnek sabit veriler

            DisplayInterpolatedResult(testRow);


            MessageBox.Show("Test verileri DataGridView'de gösteriliyor.");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnCalculate_Click_1(object sender, EventArgs e)
        {
            SetupDataGridViewHeaders();

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

        private void txtTrim_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
