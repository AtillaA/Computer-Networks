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
            this.start_button = new System.Windows.Forms.Button();
            this.roundbox = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // connect_button
            // 
            this.connect_button.Location = new System.Drawing.Point(43, 324);
            this.connect_button.Name = "connect_button";
            this.connect_button.Size = new System.Drawing.Size(296, 66);
            this.connect_button.TabIndex = 0;
            this.connect_button.Text = "Create Connection";
            this.connect_button.UseVisualStyleBackColor = true;
            this.connect_button.Click += new System.EventHandler(this.connect_button_Click);
            // 
            // port_num
            // 
            this.port_num.Location = new System.Drawing.Point(43, 233);
            this.port_num.Name = "port_num";
            this.port_num.Size = new System.Drawing.Size(296, 38);
            this.port_num.TabIndex = 1;
            this.port_num.TextChanged += new System.EventHandler(this.port_num_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 189);
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
            this.server_close.Location = new System.Drawing.Point(43, 427);
            this.server_close.Name = "server_close";
            this.server_close.Size = new System.Drawing.Size(296, 59);
            this.server_close.TabIndex = 5;
            this.server_close.Text = "Close Server";
            this.server_close.UseVisualStyleBackColor = true;
            this.server_close.Click += new System.EventHandler(this.server_close_Click);
            // 
            // clear_button
            // 
            this.clear_button.Location = new System.Drawing.Point(611, 716);
            this.clear_button.Name = "clear_button";
            this.clear_button.Size = new System.Drawing.Size(271, 59);
            this.clear_button.TabIndex = 9;
            this.clear_button.Text = "Clear";
            this.clear_button.UseVisualStyleBackColor = true;
            this.clear_button.Click += new System.EventHandler(this.clear_button_Click);
            // 
            // start_button
            // 
            this.start_button.Enabled = false;
            this.start_button.Location = new System.Drawing.Point(43, 518);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(296, 96);
            this.start_button.TabIndex = 10;
            this.start_button.Text = "Start";
            this.start_button.UseVisualStyleBackColor = true;
            this.start_button.Click += new System.EventHandler(this.start_button_Click);
            // 
            // roundbox
            // 
            this.roundbox.Location = new System.Drawing.Point(150, 82);
            this.roundbox.Name = "roundbox";
            this.roundbox.Size = new System.Drawing.Size(76, 59);
            this.roundbox.TabIndex = 11;
            this.roundbox.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 32);
            this.label2.TabIndex = 12;
            this.label2.Text = "Round:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1211, 871);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.roundbox);
            this.Controls.Add(this.start_button);
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
        private System.Windows.Forms.Button start_button;
        private System.Windows.Forms.RichTextBox roundbox;
        private System.Windows.Forms.Label label2;
    }
}

