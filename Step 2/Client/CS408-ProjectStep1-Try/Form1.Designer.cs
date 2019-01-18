namespace CS408_ProjectStep1_Try
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
            this.send_button = new System.Windows.Forms.Button();
            this.ip_box = new System.Windows.Forms.TextBox();
            this.name_box = new System.Windows.Forms.TextBox();
            this.port_box = new System.Windows.Forms.TextBox();
            this.result_box = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.send_line_box = new System.Windows.Forms.TextBox();
            this.clear_button = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.disconnect_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // connect_button
            // 
            this.connect_button.Location = new System.Drawing.Point(38, 620);
            this.connect_button.Name = "connect_button";
            this.connect_button.Size = new System.Drawing.Size(201, 51);
            this.connect_button.TabIndex = 0;
            this.connect_button.Text = "Connect";
            this.connect_button.UseVisualStyleBackColor = true;
            this.connect_button.Click += new System.EventHandler(this.connect_button_Click);
            // 
            // send_button
            // 
            this.send_button.Location = new System.Drawing.Point(1367, 786);
            this.send_button.Name = "send_button";
            this.send_button.Size = new System.Drawing.Size(158, 51);
            this.send_button.TabIndex = 1;
            this.send_button.Text = "Send";
            this.send_button.UseVisualStyleBackColor = true;
            this.send_button.Click += new System.EventHandler(this.send_button_Click);
            // 
            // ip_box
            // 
            this.ip_box.Location = new System.Drawing.Point(38, 171);
            this.ip_box.Name = "ip_box";
            this.ip_box.Size = new System.Drawing.Size(427, 38);
            this.ip_box.TabIndex = 2;
            this.ip_box.TextChanged += new System.EventHandler(this.ip_box_TextChanged);
            // 
            // name_box
            // 
            this.name_box.Location = new System.Drawing.Point(38, 549);
            this.name_box.Name = "name_box";
            this.name_box.Size = new System.Drawing.Size(427, 38);
            this.name_box.TabIndex = 3;
            this.name_box.TextChanged += new System.EventHandler(this.name_box_TextChanged);
            // 
            // port_box
            // 
            this.port_box.Location = new System.Drawing.Point(38, 345);
            this.port_box.Name = "port_box";
            this.port_box.Size = new System.Drawing.Size(427, 38);
            this.port_box.TabIndex = 4;
            this.port_box.TextChanged += new System.EventHandler(this.port_box_TextChanged);
            // 
            // result_box
            // 
            this.result_box.Location = new System.Drawing.Point(493, 114);
            this.result_box.Name = "result_box";
            this.result_box.Size = new System.Drawing.Size(1032, 657);
            this.result_box.TabIndex = 5;
            this.result_box.Text = "";
            this.result_box.TextChanged += new System.EventHandler(this.result_box_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 310);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 32);
            this.label1.TabIndex = 6;
            this.label1.Text = "Enter Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 32);
            this.label2.TabIndex = 7;
            this.label2.Text = "Enter IP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 514);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(165, 32);
            this.label3.TabIndex = 8;
            this.label3.Text = "Your Name:";
            // 
            // send_line_box
            // 
            this.send_line_box.Location = new System.Drawing.Point(493, 786);
            this.send_line_box.Multiline = true;
            this.send_line_box.Name = "send_line_box";
            this.send_line_box.Size = new System.Drawing.Size(868, 51);
            this.send_line_box.TabIndex = 9;
            this.send_line_box.TextChanged += new System.EventHandler(this.send_line_box_TextChanged);
            // 
            // clear_button
            // 
            this.clear_button.Location = new System.Drawing.Point(931, 854);
            this.clear_button.Name = "clear_button";
            this.clear_button.Size = new System.Drawing.Size(228, 54);
            this.clear_button.TabIndex = 10;
            this.clear_button.Text = "Clear";
            this.clear_button.UseVisualStyleBackColor = true;
            this.clear_button.Click += new System.EventHandler(this.clear_button_Click);
            // 
            // disconnect_button
            // 
            this.disconnect_button.Location = new System.Drawing.Point(264, 620);
            this.disconnect_button.Name = "disconnect_button";
            this.disconnect_button.Size = new System.Drawing.Size(201, 51);
            this.disconnect_button.TabIndex = 11;
            this.disconnect_button.Text = "Disconnect";
            this.disconnect_button.UseVisualStyleBackColor = true;
            this.disconnect_button.Click += new System.EventHandler(this.disconnect_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1634, 965);
            this.Controls.Add(this.disconnect_button);
            this.Controls.Add(this.clear_button);
            this.Controls.Add(this.send_line_box);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.result_box);
            this.Controls.Add(this.port_box);
            this.Controls.Add(this.name_box);
            this.Controls.Add(this.ip_box);
            this.Controls.Add(this.send_button);
            this.Controls.Add(this.connect_button);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connect_button;
        private System.Windows.Forms.Button send_button;
        private System.Windows.Forms.TextBox ip_box;
        private System.Windows.Forms.TextBox name_box;
        private System.Windows.Forms.TextBox port_box;
        private System.Windows.Forms.RichTextBox result_box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox send_line_box;
        private System.Windows.Forms.Button clear_button;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button disconnect_button;
    }
}

