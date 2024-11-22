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
            // 
            // txtTrim
            // 
            this.txtTrim.Location = new System.Drawing.Point(13, 56);
            this.txtTrim.Name = "txtTrim";
            this.txtTrim.Size = new System.Drawing.Size(100, 22);
            this.txtTrim.TabIndex = 1;
            // 
            // txtDraft
            // 
            this.txtDraft.Location = new System.Drawing.Point(13, 96);
            this.txtDraft.Name = "txtDraft";
            this.txtDraft.Size = new System.Drawing.Size(100, 22);
            this.txtDraft.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(13, 138);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(240, 150);
            this.dataGridView1.TabIndex = 3;
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(13, 307);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(75, 23);
            this.btnCalculate.TabIndex = 4;
            this.btnCalculate.Text = "Hesapla";
            this.btnCalculate.UseVisualStyleBackColor = true;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(13, 351);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(173, 16);
            this.lblResult.TabIndex = 5;
            this.lblResult.Text = "Sonuçlar burada görünecek";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(683, 487);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtDraft);
            this.Controls.Add(this.txtTrim);
            this.Controls.Add(this.cmbTrimValues);
            this.Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
