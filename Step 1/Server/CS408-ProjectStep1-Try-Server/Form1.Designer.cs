namespace CS408_ProjectStep1_Try_Server
{
    partial class Form1
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
            this.connect_button = new System.Windows.Forms.Button();
            this.port_num = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.result_box = new System.Windows.Forms.RichTextBox();
            this.server_close = new System.Windows.Forms.Button();
            this.clear_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // connect_button
            // 
            this.connect_button.Location = new System.Drawing.Point(43, 305);
            this.connect_button.Name = "connect_button";
            this.connect_button.Size = new System.Drawing.Size(296, 66);
            this.connect_button.TabIndex = 0;
            this.connect_button.Text = "Create Connection";
            this.connect_button.UseVisualStyleBackColor = true;
            this.connect_button.Click += new System.EventHandler(this.connect_button_Click);
            // 
            // port_num
            // 
            this.port_num.Location = new System.Drawing.Point(43, 217);
            this.port_num.Name = "port_num";
            this.port_num.Size = new System.Drawing.Size(296, 38);
            this.port_num.TabIndex = 1;
            this.port_num.TextChanged += new System.EventHandler(this.port_num_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 32);
            this.label1.TabIndex = 3;
            this.label1.Text = "Port:";
            // 
            // result_box
            // 
            this.result_box.Location = new System.Drawing.Point(370, 52);
            this.result_box.Name = "result_box";
            this.result_box.Size = new System.Drawing.Size(761, 627);
            this.result_box.TabIndex = 4;
            this.result_box.Text = "";
            this.result_box.TextChanged += new System.EventHandler(this.result_box_TextChanged);
            // 
            // server_close
            // 
            this.server_close.Location = new System.Drawing.Point(43, 428);
            this.server_close.Name = "server_close";
            this.server_close.Size = new System.Drawing.Size(296, 59);
            this.server_close.TabIndex = 5;
            this.server_close.Text = "Close Server";
            this.server_close.UseVisualStyleBackColor = true;
            this.server_close.Click += new System.EventHandler(this.server_close_Click);
            // 
            // clear_button
            // 
            this.clear_button.Location = new System.Drawing.Point(634, 711);
            this.clear_button.Name = "clear_button";
            this.clear_button.Size = new System.Drawing.Size(218, 59);
            this.clear_button.TabIndex = 9;
            this.clear_button.Text = "Clear";
            this.clear_button.UseVisualStyleBackColor = true;
            this.clear_button.Click += new System.EventHandler(this.clear_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1156, 800);
            this.Controls.Add(this.clear_button);
            this.Controls.Add(this.server_close);
            this.Controls.Add(this.result_box);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.port_num);
            this.Controls.Add(this.connect_button);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connect_button;
        private System.Windows.Forms.TextBox port_num;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox result_box;
        private System.Windows.Forms.Button server_close;
        private System.Windows.Forms.Button clear_button;
    }
}

