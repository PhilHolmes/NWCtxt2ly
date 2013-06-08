namespace NWCTXT2Ly
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.btnBrowse = new System.Windows.Forms.Button();
			this.txtFile = new System.Windows.Forms.TextBox();
			this.lblFile = new System.Windows.Forms.Label();
			this.btnGo = new System.Windows.Forms.Button();
			this.txtResult = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.chkRemove = new System.Windows.Forms.CheckBox();
			this.udFontSize = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.chkFirstStave = new System.Windows.Forms.CheckBox();
			this.chkVoice = new System.Windows.Forms.CheckBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.grpGrace = new System.Windows.Forms.GroupBox();
			this.radGrace = new System.Windows.Forms.RadioButton();
			this.radAcc = new System.Windows.Forms.RadioButton();
			this.chkForcePage = new System.Windows.Forms.CheckBox();
			this.chkSmall = new System.Windows.Forms.CheckBox();
			this.chkCompress = new System.Windows.Forms.CheckBox();
			this.grpPaper = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.udLineWidth = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.udLeftMargin = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.udBottomMargin = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.udTopMargin = new System.Windows.Forms.NumericUpDown();
			this.chkLabel = new System.Windows.Forms.CheckBox();
			this.chkStanza = new System.Windows.Forms.CheckBox();
			this.chkPiano = new System.Windows.Forms.CheckBox();
			this.chkLast = new System.Windows.Forms.CheckBox();
			this.chkMelody = new System.Windows.Forms.CheckBox();
			this.chkShort = new System.Windows.Forms.CheckBox();
			this.chkAutobeam = new System.Windows.Forms.CheckBox();
			this.udDynSpace = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.udFontSize)).BeginInit();
			this.grpGrace.SuspendLayout();
			this.grpPaper.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udLineWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udLeftMargin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udBottomMargin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udTopMargin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udDynSpace)).BeginInit();
			this.SuspendLayout();
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(418, 8);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 5;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// txtFile
			// 
			this.txtFile.Location = new System.Drawing.Point(81, 11);
			this.txtFile.Name = "txtFile";
			this.txtFile.ReadOnly = true;
			this.txtFile.Size = new System.Drawing.Size(331, 20);
			this.txtFile.TabIndex = 4;
			this.txtFile.TextChanged += new System.EventHandler(this.FileChanged);
			// 
			// lblFile
			// 
			this.lblFile.AutoSize = true;
			this.lblFile.Location = new System.Drawing.Point(28, 14);
			this.lblFile.Name = "lblFile";
			this.lblFile.Size = new System.Drawing.Size(47, 13);
			this.lblFile.TabIndex = 3;
			this.lblFile.Text = "Input file";
			// 
			// btnGo
			// 
			this.btnGo.Enabled = false;
			this.btnGo.Location = new System.Drawing.Point(418, 37);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 6;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// txtResult
			// 
			this.txtResult.Location = new System.Drawing.Point(81, 40);
			this.txtResult.Name = "txtResult";
			this.txtResult.ReadOnly = true;
			this.txtResult.Size = new System.Drawing.Size(331, 20);
			this.txtResult.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(29, 87);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 13);
			this.label1.TabIndex = 8;
			this.label1.Text = "Options:";
			// 
			// chkRemove
			// 
			this.chkRemove.AutoSize = true;
			this.chkRemove.Location = new System.Drawing.Point(81, 86);
			this.chkRemove.Name = "chkRemove";
			this.chkRemove.Size = new System.Drawing.Size(131, 17);
			this.chkRemove.TabIndex = 9;
			this.chkRemove.Text = "Remove empty staves";
			this.chkRemove.UseVisualStyleBackColor = true;
			this.chkRemove.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// udFontSize
			// 
			this.udFontSize.Location = new System.Drawing.Point(81, 109);
			this.udFontSize.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.udFontSize.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.udFontSize.Name = "udFontSize";
			this.udFontSize.Size = new System.Drawing.Size(35, 20);
			this.udFontSize.TabIndex = 10;
			this.udFontSize.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
			this.udFontSize.ValueChanged += new System.EventHandler(this.ControlChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(122, 111);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(51, 13);
			this.label2.TabIndex = 11;
			this.label2.Text = "Font Size";
			// 
			// chkFirstStave
			// 
			this.chkFirstStave.AutoSize = true;
			this.chkFirstStave.Enabled = false;
			this.chkFirstStave.Location = new System.Drawing.Point(218, 87);
			this.chkFirstStave.Name = "chkFirstStave";
			this.chkFirstStave.Size = new System.Drawing.Size(163, 17);
			this.chkFirstStave.TabIndex = 12;
			this.chkFirstStave.Text = "Also remove empty first stave";
			this.chkFirstStave.UseVisualStyleBackColor = true;
			this.chkFirstStave.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// chkVoice
			// 
			this.chkVoice.AutoSize = true;
			this.chkVoice.Location = new System.Drawing.Point(81, 135);
			this.chkVoice.Name = "chkVoice";
			this.chkVoice.Size = new System.Drawing.Size(335, 17);
			this.chkVoice.TabIndex = 13;
			this.chkVoice.Text = "Assign voice names (voiceOne, voiceTwo, etc.) to layered staves";
			this.chkVoice.UseVisualStyleBackColor = true;
			this.chkVoice.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(81, 158);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(100, 20);
			this.txtName.TabIndex = 14;
			this.txtName.TextChanged += new System.EventHandler(this.ControlChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(17, 161);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(58, 13);
			this.label3.TabIndex = 15;
			this.label3.Text = "Part name:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(187, 161);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(306, 32);
			this.label4.TabIndex = 16;
			this.label4.Text = "Use this when you will need to combine LilyPond \"scores\" into a \"book\" and need e" +
				"ach score identified";
			// 
			// grpGrace
			// 
			this.grpGrace.Controls.Add(this.radGrace);
			this.grpGrace.Controls.Add(this.radAcc);
			this.grpGrace.Location = new System.Drawing.Point(81, 198);
			this.grpGrace.Name = "grpGrace";
			this.grpGrace.Size = new System.Drawing.Size(100, 67);
			this.grpGrace.TabIndex = 18;
			this.grpGrace.TabStop = false;
			this.grpGrace.Text = "Grace note:";
			// 
			// radGrace
			// 
			this.radGrace.AutoSize = true;
			this.radGrace.Location = new System.Drawing.Point(6, 43);
			this.radGrace.Name = "radGrace";
			this.radGrace.Size = new System.Drawing.Size(84, 17);
			this.radGrace.TabIndex = 1;
			this.radGrace.TabStop = true;
			this.radGrace.Text = "simple grace";
			this.radGrace.UseVisualStyleBackColor = true;
			this.radGrace.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// radAcc
			// 
			this.radAcc.AutoSize = true;
			this.radAcc.Checked = true;
			this.radAcc.Location = new System.Drawing.Point(6, 20);
			this.radAcc.Name = "radAcc";
			this.radAcc.Size = new System.Drawing.Size(87, 17);
			this.radAcc.TabIndex = 0;
			this.radAcc.TabStop = true;
			this.radAcc.Text = "acciaccatura";
			this.radAcc.UseVisualStyleBackColor = true;
			this.radAcc.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// chkForcePage
			// 
			this.chkForcePage.AutoSize = true;
			this.chkForcePage.Location = new System.Drawing.Point(81, 271);
			this.chkForcePage.Name = "chkForcePage";
			this.chkForcePage.Size = new System.Drawing.Size(153, 17);
			this.chkForcePage.TabIndex = 19;
			this.chkForcePage.Text = "Use manual page breaking";
			this.chkForcePage.UseVisualStyleBackColor = true;
			this.chkForcePage.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// chkSmall
			// 
			this.chkSmall.AutoSize = true;
			this.chkSmall.Location = new System.Drawing.Point(81, 294);
			this.chkSmall.Name = "chkSmall";
			this.chkSmall.Size = new System.Drawing.Size(160, 17);
			this.chkSmall.TabIndex = 20;
			this.chkSmall.Text = "Use small fonts for solo parts";
			this.chkSmall.UseVisualStyleBackColor = true;
			this.chkSmall.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// chkCompress
			// 
			this.chkCompress.AutoSize = true;
			this.chkCompress.Location = new System.Drawing.Point(81, 317);
			this.chkCompress.Name = "chkCompress";
			this.chkCompress.Size = new System.Drawing.Size(156, 17);
			this.chkCompress.TabIndex = 21;
			this.chkCompress.Text = "Compress full measure rests";
			this.chkCompress.UseVisualStyleBackColor = true;
			this.chkCompress.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// grpPaper
			// 
			this.grpPaper.Controls.Add(this.label8);
			this.grpPaper.Controls.Add(this.udLineWidth);
			this.grpPaper.Controls.Add(this.label7);
			this.grpPaper.Controls.Add(this.udLeftMargin);
			this.grpPaper.Controls.Add(this.label6);
			this.grpPaper.Controls.Add(this.udBottomMargin);
			this.grpPaper.Controls.Add(this.label5);
			this.grpPaper.Controls.Add(this.udTopMargin);
			this.grpPaper.Location = new System.Drawing.Point(263, 199);
			this.grpPaper.Name = "grpPaper";
			this.grpPaper.Size = new System.Drawing.Size(229, 89);
			this.grpPaper.TabIndex = 22;
			this.grpPaper.TabStop = false;
			this.grpPaper.Text = "Margins (mm)";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(160, 49);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(58, 13);
			this.label8.TabIndex = 19;
			this.label8.Text = "Line Width";
			// 
			// udLineWidth
			// 
			this.udLineWidth.Location = new System.Drawing.Point(114, 47);
			this.udLineWidth.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.udLineWidth.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.udLineWidth.Name = "udLineWidth";
			this.udLineWidth.Size = new System.Drawing.Size(40, 20);
			this.udLineWidth.TabIndex = 18;
			this.udLineWidth.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.udLineWidth.ValueChanged += new System.EventHandler(this.ControlChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(160, 23);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(25, 13);
			this.label7.TabIndex = 17;
			this.label7.Text = "Left";
			// 
			// udLeftMargin
			// 
			this.udLeftMargin.Location = new System.Drawing.Point(114, 21);
			this.udLeftMargin.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.udLeftMargin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udLeftMargin.Name = "udLeftMargin";
			this.udLeftMargin.Size = new System.Drawing.Size(35, 20);
			this.udLeftMargin.TabIndex = 16;
			this.udLeftMargin.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.udLeftMargin.ValueChanged += new System.EventHandler(this.ControlChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(64, 47);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(40, 13);
			this.label6.TabIndex = 15;
			this.label6.Text = "Bottom";
			// 
			// udBottomMargin
			// 
			this.udBottomMargin.Location = new System.Drawing.Point(23, 45);
			this.udBottomMargin.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.udBottomMargin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udBottomMargin.Name = "udBottomMargin";
			this.udBottomMargin.Size = new System.Drawing.Size(35, 20);
			this.udBottomMargin.TabIndex = 14;
			this.udBottomMargin.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.udBottomMargin.ValueChanged += new System.EventHandler(this.ControlChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(64, 21);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(26, 13);
			this.label5.TabIndex = 13;
			this.label5.Text = "Top";
			// 
			// udTopMargin
			// 
			this.udTopMargin.Location = new System.Drawing.Point(23, 19);
			this.udTopMargin.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.udTopMargin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udTopMargin.Name = "udTopMargin";
			this.udTopMargin.Size = new System.Drawing.Size(35, 20);
			this.udTopMargin.TabIndex = 12;
			this.udTopMargin.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.udTopMargin.ValueChanged += new System.EventHandler(this.ControlChanged);
			// 
			// chkLabel
			// 
			this.chkLabel.AutoSize = true;
			this.chkLabel.Location = new System.Drawing.Point(81, 340);
			this.chkLabel.Name = "chkLabel";
			this.chkLabel.Size = new System.Drawing.Size(98, 17);
			this.chkLabel.TabIndex = 23;
			this.chkLabel.Text = "Use staff labels";
			this.chkLabel.UseVisualStyleBackColor = true;
			this.chkLabel.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// chkStanza
			// 
			this.chkStanza.AutoSize = true;
			this.chkStanza.Location = new System.Drawing.Point(81, 363);
			this.chkStanza.Name = "chkStanza";
			this.chkStanza.Size = new System.Drawing.Size(122, 17);
			this.chkStanza.TabIndex = 24;
			this.chkStanza.Text = "Use stanza numbers";
			this.chkStanza.UseVisualStyleBackColor = true;
			this.chkStanza.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// chkPiano
			// 
			this.chkPiano.AutoSize = true;
			this.chkPiano.Location = new System.Drawing.Point(81, 386);
			this.chkPiano.Name = "chkPiano";
			this.chkPiano.Size = new System.Drawing.Size(206, 17);
			this.chkPiano.TabIndex = 25;
			this.chkPiano.Text = "Allow empty piano staves to disappear";
			this.chkPiano.UseVisualStyleBackColor = true;
			this.chkPiano.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// chkLast
			// 
			this.chkLast.AutoSize = true;
			this.chkLast.Location = new System.Drawing.Point(81, 409);
			this.chkLast.Name = "chkLast";
			this.chkLast.Size = new System.Drawing.Size(145, 17);
			this.chkLast.TabIndex = 26;
			this.chkLast.Text = "Last page flush to bottom";
			this.chkLast.UseVisualStyleBackColor = true;
			this.chkLast.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// chkMelody
			// 
			this.chkMelody.AutoSize = true;
			this.chkMelody.Location = new System.Drawing.Point(81, 432);
			this.chkMelody.Name = "chkMelody";
			this.chkMelody.Size = new System.Drawing.Size(266, 17);
			this.chkMelody.TabIndex = 27;
			this.chkMelody.Text = "Use surrounding notes for stem direction for singers";
			this.chkMelody.UseVisualStyleBackColor = true;
			this.chkMelody.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// chkShort
			// 
			this.chkShort.AutoSize = true;
			this.chkShort.Location = new System.Drawing.Point(189, 340);
			this.chkShort.Name = "chkShort";
			this.chkShort.Size = new System.Drawing.Size(105, 17);
			this.chkShort.TabIndex = 28;
			this.chkShort.Text = "And short names";
			this.chkShort.UseVisualStyleBackColor = true;
			this.chkShort.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// chkAutobeam
			// 
			this.chkAutobeam.AutoSize = true;
			this.chkAutobeam.Location = new System.Drawing.Point(81, 455);
			this.chkAutobeam.Name = "chkAutobeam";
			this.chkAutobeam.Size = new System.Drawing.Size(74, 17);
			this.chkAutobeam.TabIndex = 29;
			this.chkAutobeam.Text = "Autobeam";
			this.chkAutobeam.UseVisualStyleBackColor = true;
			this.chkAutobeam.CheckedChanged += new System.EventHandler(this.ControlChanged);
			// 
			// udDynSpace
			// 
			this.udDynSpace.Location = new System.Drawing.Point(286, 294);
			this.udDynSpace.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
			this.udDynSpace.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udDynSpace.Name = "udDynSpace";
			this.udDynSpace.Size = new System.Drawing.Size(37, 20);
			this.udDynSpace.TabIndex = 30;
			this.udDynSpace.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
			this.udDynSpace.ValueChanged += new System.EventHandler(this.ControlChanged);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(329, 296);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(93, 13);
			this.label9.TabIndex = 31;
			this.label9.Text = "Dynamics spacing";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(521, 480);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.udDynSpace);
			this.Controls.Add(this.chkAutobeam);
			this.Controls.Add(this.chkShort);
			this.Controls.Add(this.chkMelody);
			this.Controls.Add(this.chkLast);
			this.Controls.Add(this.chkPiano);
			this.Controls.Add(this.chkStanza);
			this.Controls.Add(this.chkLabel);
			this.Controls.Add(this.grpPaper);
			this.Controls.Add(this.chkCompress);
			this.Controls.Add(this.chkSmall);
			this.Controls.Add(this.chkForcePage);
			this.Controls.Add(this.grpGrace);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.chkVoice);
			this.Controls.Add(this.chkFirstStave);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.udFontSize);
			this.Controls.Add(this.chkRemove);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtResult);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtFile);
			this.Controls.Add(this.lblFile);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Noteworthy Text to LilyPond Converter";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.udFontSize)).EndInit();
			this.grpGrace.ResumeLayout(false);
			this.grpGrace.PerformLayout();
			this.grpPaper.ResumeLayout(false);
			this.grpPaper.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.udLineWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udLeftMargin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udBottomMargin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udTopMargin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udDynSpace)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox txtFile;
		private System.Windows.Forms.Label lblFile;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.TextBox txtResult;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkRemove;
		private System.Windows.Forms.NumericUpDown udFontSize;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkFirstStave;
		private System.Windows.Forms.CheckBox chkVoice;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox grpGrace;
		private System.Windows.Forms.RadioButton radGrace;
		private System.Windows.Forms.RadioButton radAcc;
		private System.Windows.Forms.CheckBox chkForcePage;
		private System.Windows.Forms.CheckBox chkSmall;
		private System.Windows.Forms.CheckBox chkCompress;
		private System.Windows.Forms.GroupBox grpPaper;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown udLineWidth;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown udLeftMargin;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown udBottomMargin;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown udTopMargin;
		private System.Windows.Forms.CheckBox chkLabel;
		private System.Windows.Forms.CheckBox chkStanza;
		private System.Windows.Forms.CheckBox chkPiano;
		private System.Windows.Forms.CheckBox chkLast;
		private System.Windows.Forms.CheckBox chkMelody;
		private System.Windows.Forms.CheckBox chkShort;
		private System.Windows.Forms.CheckBox chkAutobeam;
		private System.Windows.Forms.NumericUpDown udDynSpace;
		private System.Windows.Forms.Label label9;
	}
}

