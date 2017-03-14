namespace Chernobyl_Relay_Chat
{
    partial class ClientDisplay
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientDisplay));
            this.richTextBoxMessages = new System.Windows.Forms.RichTextBox();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.timerGameCheck = new System.Windows.Forms.Timer(this.components);
            this.timerGameUpdate = new System.Windows.Forms.Timer(this.components);
            this.buttonOptions = new System.Windows.Forms.Button();
            this.textBoxUsers = new System.Windows.Forms.TextBox();
            this.timerCheckUpdate = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // richTextBoxMessages
            // 
            this.richTextBoxMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxMessages.HideSelection = false;
            this.richTextBoxMessages.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxMessages.Name = "richTextBoxMessages";
            this.richTextBoxMessages.ReadOnly = true;
            this.richTextBoxMessages.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxMessages.Size = new System.Drawing.Size(822, 421);
            this.richTextBoxMessages.TabIndex = 3;
            this.richTextBoxMessages.TabStop = false;
            this.richTextBoxMessages.Text = "";
            this.richTextBoxMessages.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxMessages_LinkClicked);
            // 
            // textBoxInput
            // 
            this.textBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInput.Location = new System.Drawing.Point(12, 441);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(822, 20);
            this.textBoxInput.TabIndex = 0;
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.Enabled = false;
            this.buttonSend.Location = new System.Drawing.Point(840, 439);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 1;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // timerGameCheck
            // 
            this.timerGameCheck.Enabled = true;
            this.timerGameCheck.Interval = 10000;
            this.timerGameCheck.Tick += new System.EventHandler(this.timerGameCheck_Tick);
            // 
            // timerGameUpdate
            // 
            this.timerGameUpdate.Enabled = true;
            this.timerGameUpdate.Interval = 500;
            this.timerGameUpdate.Tick += new System.EventHandler(this.timerGameUpdate_Tick);
            // 
            // buttonOptions
            // 
            this.buttonOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOptions.Enabled = false;
            this.buttonOptions.Location = new System.Drawing.Point(921, 439);
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.Size = new System.Drawing.Size(75, 23);
            this.buttonOptions.TabIndex = 2;
            this.buttonOptions.Text = "Options";
            this.buttonOptions.UseVisualStyleBackColor = true;
            this.buttonOptions.Click += new System.EventHandler(this.buttonOptions_Click);
            // 
            // textBoxUsers
            // 
            this.textBoxUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUsers.Location = new System.Drawing.Point(840, 12);
            this.textBoxUsers.Multiline = true;
            this.textBoxUsers.Name = "textBoxUsers";
            this.textBoxUsers.ReadOnly = true;
            this.textBoxUsers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxUsers.Size = new System.Drawing.Size(156, 421);
            this.textBoxUsers.TabIndex = 4;
            // 
            // timerCheckUpdate
            // 
            this.timerCheckUpdate.Enabled = true;
            this.timerCheckUpdate.Interval = 600000;
            this.timerCheckUpdate.Tick += new System.EventHandler(this.timerCheckUpdate_Tick);
            // 
            // ClientDisplay
            // 
            this.AcceptButton = this.buttonSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 474);
            this.Controls.Add(this.textBoxUsers);
            this.Controls.Add(this.buttonOptions);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxInput);
            this.Controls.Add(this.richTextBoxMessages);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ClientDisplay";
            this.Text = "Chernobyl Relay Chat <Unknown>";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientDisplay_FormClosing);
            this.Load += new System.EventHandler(this.ClientDisplay_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxMessages;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Timer timerGameCheck;
        private System.Windows.Forms.Timer timerGameUpdate;
        private System.Windows.Forms.Button buttonOptions;
        private System.Windows.Forms.TextBox textBoxUsers;
        private System.Windows.Forms.Timer timerCheckUpdate;
    }
}

