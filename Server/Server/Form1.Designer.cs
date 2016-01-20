namespace Server
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnListen = new System.Windows.Forms.Button();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.lbIp = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(245, 10);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(106, 23);
            this.btnListen.TabIndex = 0;
            this.btnListen.Text = "Start Server";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(82, 12);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(157, 20);
            this.txtIp.TabIndex = 1;
            this.txtIp.Text = "192.168.1.4";
            this.txtIp.TextChanged += new System.EventHandler(this.txtIp_TextChanged);
            // 
            // lbIp
            // 
            this.lbIp.AutoSize = true;
            this.lbIp.Location = new System.Drawing.Point(15, 17);
            this.lbIp.Name = "lbIp";
            this.lbIp.Size = new System.Drawing.Size(61, 13);
            this.lbIp.TabIndex = 2;
            this.lbIp.Text = "IP Address:";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(12, 39);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(344, 180);
            this.txtLog.TabIndex = 3;
            this.txtLog.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 231);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lbIp);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.btnListen);
            this.Name = "Form1";
            this.Text = "Chat Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Label lbIp;
        private System.Windows.Forms.RichTextBox txtLog;
    }
}

