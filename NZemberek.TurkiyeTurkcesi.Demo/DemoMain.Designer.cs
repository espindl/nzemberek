namespace NZemberek.TurkiyeTurkcesi.Demo
{
    partial class DemoMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.btnToAscii = new System.Windows.Forms.Button();
            this.txtEntry = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAnalyseByAsciiTolerance = new System.Windows.Forms.Button();
            this.btnAnalyse = new System.Windows.Forms.Button();
            this.btnControl = new System.Windows.Forms.Button();
            this.btnFindPossibilities = new System.Windows.Forms.Button();
            this.btnSuggest = new System.Windows.Forms.Button();
            this.btnConvertToTurkishChars = new System.Windows.Forms.Button();
            this.btnIsThisTurkish = new System.Windows.Forms.Button();
            this.lstResults = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(867, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Kök Dosyası Oluştur";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnToAscii
            // 
            this.btnToAscii.Location = new System.Drawing.Point(12, 41);
            this.btnToAscii.Name = "btnToAscii";
            this.btnToAscii.Size = new System.Drawing.Size(156, 23);
            this.btnToAscii.TabIndex = 1;
            this.btnToAscii.Text = "ASCII Karakterlere Dönüştür";
            this.btnToAscii.UseVisualStyleBackColor = true;
            this.btnToAscii.Click += new System.EventHandler(this.btnToAscii_Click);
            // 
            // txtEntry
            // 
            this.txtEntry.Location = new System.Drawing.Point(41, 6);
            this.txtEntry.Name = "txtEntry";
            this.txtEntry.Size = new System.Drawing.Size(161, 20);
            this.txtEntry.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Giriş:";
            // 
            // btnAnalyseByAsciiTolerance
            // 
            this.btnAnalyseByAsciiTolerance.Location = new System.Drawing.Point(12, 70);
            this.btnAnalyseByAsciiTolerance.Name = "btnAnalyseByAsciiTolerance";
            this.btnAnalyseByAsciiTolerance.Size = new System.Drawing.Size(156, 23);
            this.btnAnalyseByAsciiTolerance.TabIndex = 5;
            this.btnAnalyseByAsciiTolerance.Text = "ASCII Toleranslı Çözümle";
            this.btnAnalyseByAsciiTolerance.UseVisualStyleBackColor = true;
            this.btnAnalyseByAsciiTolerance.Click += new System.EventHandler(this.btnAnalyseByAsciiTolerance_Click);
            // 
            // btnAnalyse
            // 
            this.btnAnalyse.Location = new System.Drawing.Point(12, 99);
            this.btnAnalyse.Name = "btnAnalyse";
            this.btnAnalyse.Size = new System.Drawing.Size(156, 23);
            this.btnAnalyse.TabIndex = 7;
            this.btnAnalyse.Text = "Kelime Çözümle";
            this.btnAnalyse.UseVisualStyleBackColor = true;
            this.btnAnalyse.Click += new System.EventHandler(this.btnAnalyse_Click);
            // 
            // btnControl
            // 
            this.btnControl.Location = new System.Drawing.Point(12, 128);
            this.btnControl.Name = "btnControl";
            this.btnControl.Size = new System.Drawing.Size(156, 23);
            this.btnControl.TabIndex = 8;
            this.btnControl.Text = "Kelime Denetle";
            this.btnControl.UseVisualStyleBackColor = true;
            this.btnControl.Click += new System.EventHandler(this.btnControl_Click);
            // 
            // btnFindPossibilities
            // 
            this.btnFindPossibilities.Location = new System.Drawing.Point(12, 157);
            this.btnFindPossibilities.Name = "btnFindPossibilities";
            this.btnFindPossibilities.Size = new System.Drawing.Size(156, 23);
            this.btnFindPossibilities.TabIndex = 9;
            this.btnFindPossibilities.Text = "Olası Açılımları Bul";
            this.btnFindPossibilities.UseVisualStyleBackColor = true;
            this.btnFindPossibilities.Click += new System.EventHandler(this.btnFindPossibilities_Click);
            // 
            // btnSuggest
            // 
            this.btnSuggest.Location = new System.Drawing.Point(12, 186);
            this.btnSuggest.Name = "btnSuggest";
            this.btnSuggest.Size = new System.Drawing.Size(156, 23);
            this.btnSuggest.TabIndex = 10;
            this.btnSuggest.Text = "Kelime Öner";
            this.btnSuggest.UseVisualStyleBackColor = true;
            this.btnSuggest.Click += new System.EventHandler(this.btnSuggest_Click);
            // 
            // btnConvertToTurkishChars
            // 
            this.btnConvertToTurkishChars.Location = new System.Drawing.Point(12, 215);
            this.btnConvertToTurkishChars.Name = "btnConvertToTurkishChars";
            this.btnConvertToTurkishChars.Size = new System.Drawing.Size(156, 23);
            this.btnConvertToTurkishChars.TabIndex = 11;
            this.btnConvertToTurkishChars.Text = "Türkçe Karakterlere Dönüştür ";
            this.btnConvertToTurkishChars.UseVisualStyleBackColor = true;
            this.btnConvertToTurkishChars.Click += new System.EventHandler(this.btnConvertToTurkishChars_Click);
            // 
            // btnIsThisTurkish
            // 
            this.btnIsThisTurkish.Location = new System.Drawing.Point(12, 244);
            this.btnIsThisTurkish.Name = "btnIsThisTurkish";
            this.btnIsThisTurkish.Size = new System.Drawing.Size(156, 23);
            this.btnIsThisTurkish.TabIndex = 12;
            this.btnIsThisTurkish.Text = "Türkçe Mi?";
            this.btnIsThisTurkish.UseVisualStyleBackColor = true;
            this.btnIsThisTurkish.Click += new System.EventHandler(this.btnIsThisTurkish_Click);
            // 
            // lstResults
            // 
            this.lstResults.FormattingEnabled = true;
            this.lstResults.Location = new System.Drawing.Point(183, 41);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new System.Drawing.Size(800, 277);
            this.lstResults.TabIndex = 14;
            // 
            // DemoMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 330);
            this.Controls.Add(this.lstResults);
            this.Controls.Add(this.btnIsThisTurkish);
            this.Controls.Add(this.btnConvertToTurkishChars);
            this.Controls.Add(this.btnSuggest);
            this.Controls.Add(this.btnFindPossibilities);
            this.Controls.Add(this.btnControl);
            this.Controls.Add(this.btnAnalyse);
            this.Controls.Add(this.btnAnalyseByAsciiTolerance);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEntry);
            this.Controls.Add(this.btnToAscii);
            this.Controls.Add(this.button1);
            this.Name = "DemoMain";
            this.Text = "Demo";
            this.Load += new System.EventHandler(this.DemoMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnToAscii;
        private System.Windows.Forms.TextBox txtEntry;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAnalyseByAsciiTolerance;
        private System.Windows.Forms.Button btnAnalyse;
        private System.Windows.Forms.Button btnControl;
        private System.Windows.Forms.Button btnFindPossibilities;
        private System.Windows.Forms.Button btnSuggest;
        private System.Windows.Forms.Button btnConvertToTurkishChars;
        private System.Windows.Forms.Button btnIsThisTurkish;
        private System.Windows.Forms.ListBox lstResults;

    }
}

