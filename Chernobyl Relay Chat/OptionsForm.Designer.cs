namespace Chernobyl_Relay_Chat
{
    partial class OptionsForm
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.buttonRandom = new System.Windows.Forms.Button();
            this.comboBoxFaction = new System.Windows.Forms.ComboBox();
            this.checkBoxDeathSend = new System.Windows.Forms.CheckBox();
            this.checkBoxDeathReceive = new System.Windows.Forms.CheckBox();
            this.radioButtonFactionAuto = new System.Windows.Forms.RadioButton();
            this.radioButtonFactionManual = new System.Windows.Forms.RadioButton();
            this.numericUpDownDeath = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDeath)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(72, 207);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(153, 207);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(12, 75);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(135, 20);
            this.textBoxName.TabIndex = 3;
            // 
            // buttonRandom
            // 
            this.buttonRandom.Location = new System.Drawing.Point(153, 73);
            this.buttonRandom.Name = "buttonRandom";
            this.buttonRandom.Size = new System.Drawing.Size(75, 23);
            this.buttonRandom.TabIndex = 4;
            this.buttonRandom.Text = "Random";
            this.buttonRandom.UseVisualStyleBackColor = true;
            this.buttonRandom.Click += new System.EventHandler(this.buttonRandom_Click);
            // 
            // comboBoxFaction
            // 
            this.comboBoxFaction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFaction.Enabled = false;
            this.comboBoxFaction.FormattingEnabled = true;
            this.comboBoxFaction.Items.AddRange(new object[] {
            "Bandit",
            "Clear Sky",
            "Duty",
            "Ecologist",
            "Freedom",
            "Loner",
            "Mercenary",
            "Military",
            "Monolith"});
            this.comboBoxFaction.Location = new System.Drawing.Point(12, 35);
            this.comboBoxFaction.Name = "comboBoxFaction";
            this.comboBoxFaction.Size = new System.Drawing.Size(216, 21);
            this.comboBoxFaction.TabIndex = 6;
            // 
            // checkBoxDeathSend
            // 
            this.checkBoxDeathSend.AutoSize = true;
            this.checkBoxDeathSend.Checked = true;
            this.checkBoxDeathSend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeathSend.Location = new System.Drawing.Point(12, 122);
            this.checkBoxDeathSend.Name = "checkBoxDeathSend";
            this.checkBoxDeathSend.Size = new System.Drawing.Size(131, 17);
            this.checkBoxDeathSend.TabIndex = 7;
            this.checkBoxDeathSend.Text = "Send death messages";
            this.checkBoxDeathSend.UseVisualStyleBackColor = true;
            // 
            // checkBoxDeathReceive
            // 
            this.checkBoxDeathReceive.AutoSize = true;
            this.checkBoxDeathReceive.Checked = true;
            this.checkBoxDeathReceive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeathReceive.Location = new System.Drawing.Point(12, 145);
            this.checkBoxDeathReceive.Name = "checkBoxDeathReceive";
            this.checkBoxDeathReceive.Size = new System.Drawing.Size(146, 17);
            this.checkBoxDeathReceive.TabIndex = 8;
            this.checkBoxDeathReceive.Text = "Receive death messages";
            this.checkBoxDeathReceive.UseVisualStyleBackColor = true;
            this.checkBoxDeathReceive.CheckedChanged += new System.EventHandler(this.checkBoxDeathReceive_CheckedChanged);
            // 
            // radioButtonFactionAuto
            // 
            this.radioButtonFactionAuto.AutoSize = true;
            this.radioButtonFactionAuto.Checked = true;
            this.radioButtonFactionAuto.Location = new System.Drawing.Point(12, 12);
            this.radioButtonFactionAuto.Name = "radioButtonFactionAuto";
            this.radioButtonFactionAuto.Size = new System.Drawing.Size(107, 17);
            this.radioButtonFactionAuto.TabIndex = 9;
            this.radioButtonFactionAuto.TabStop = true;
            this.radioButtonFactionAuto.Text = "Automatic faction";
            this.radioButtonFactionAuto.UseVisualStyleBackColor = true;
            // 
            // radioButtonFactionManual
            // 
            this.radioButtonFactionManual.AutoSize = true;
            this.radioButtonFactionManual.Location = new System.Drawing.Point(133, 12);
            this.radioButtonFactionManual.Name = "radioButtonFactionManual";
            this.radioButtonFactionManual.Size = new System.Drawing.Size(95, 17);
            this.radioButtonFactionManual.TabIndex = 10;
            this.radioButtonFactionManual.Text = "Manual faction";
            this.radioButtonFactionManual.UseVisualStyleBackColor = true;
            this.radioButtonFactionManual.CheckedChanged += new System.EventHandler(this.radioButtonFactionManual_CheckedChanged);
            // 
            // numericUpDownDeath
            // 
            this.numericUpDownDeath.Location = new System.Drawing.Point(12, 181);
            this.numericUpDownDeath.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numericUpDownDeath.Name = "numericUpDownDeath";
            this.numericUpDownDeath.Size = new System.Drawing.Size(46, 20);
            this.numericUpDownDeath.TabIndex = 11;
            this.numericUpDownDeath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownDeath.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Minimum time between death messages";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "seconds";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(12, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(216, 2);
            this.label4.TabIndex = 14;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(240, 242);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownDeath);
            this.Controls.Add(this.radioButtonFactionManual);
            this.Controls.Add(this.radioButtonFactionAuto);
            this.Controls.Add(this.checkBoxDeathReceive);
            this.Controls.Add(this.checkBoxDeathSend);
            this.Controls.Add(this.comboBoxFaction);
            this.Controls.Add(this.buttonRandom);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Name = "OptionsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDeath)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button buttonRandom;
        private System.Windows.Forms.ComboBox comboBoxFaction;
        private System.Windows.Forms.CheckBox checkBoxDeathSend;
        private System.Windows.Forms.CheckBox checkBoxDeathReceive;
        private System.Windows.Forms.RadioButton radioButtonFactionAuto;
        private System.Windows.Forms.RadioButton radioButtonFactionManual;
        private System.Windows.Forms.NumericUpDown numericUpDownDeath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}