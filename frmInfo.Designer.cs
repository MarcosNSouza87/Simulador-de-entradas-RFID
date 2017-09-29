namespace Prj_Simulador_B01
{
    partial class FrmInfo
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
            this.pnl_gearDt = new System.Windows.Forms.GroupBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.log_gif = new System.Windows.Forms.PictureBox();
            this.label32 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnl_gearDt.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.log_gif)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_gearDt
            // 
            this.pnl_gearDt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl_gearDt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(38)))));
            this.pnl_gearDt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnl_gearDt.Controls.Add(this.panel7);
            this.pnl_gearDt.Controls.Add(this.label3);
            this.pnl_gearDt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnl_gearDt.ForeColor = System.Drawing.Color.White;
            this.pnl_gearDt.Location = new System.Drawing.Point(1, 4);
            this.pnl_gearDt.Name = "pnl_gearDt";
            this.pnl_gearDt.Size = new System.Drawing.Size(198, 184);
            this.pnl_gearDt.TabIndex = 58;
            this.pnl_gearDt.TabStop = false;
            this.pnl_gearDt.Text = "Informações sobre";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.panel7.Controls.Add(this.log_gif);
            this.panel7.Controls.Add(this.label32);
            this.panel7.Location = new System.Drawing.Point(-6, 13);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(217, 50);
            this.panel7.TabIndex = 21;
            // 
            // log_gif
            // 
            this.log_gif.BackColor = System.Drawing.Color.White;
            this.log_gif.Image = global::Prj_Simulador_B01.Properties.Resources.icon_simulador;
            this.log_gif.Location = new System.Drawing.Point(151, 3);
            this.log_gif.Name = "log_gif";
            this.log_gif.Size = new System.Drawing.Size(47, 44);
            this.log_gif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.log_gif.TabIndex = 71;
            this.log_gif.TabStop = false;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label32.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label32.Location = new System.Drawing.Point(12, 11);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(99, 20);
            this.label32.TabIndex = 29;
            this.label32.Text = "SimuRFINet";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label3.Location = new System.Drawing.Point(6, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 91);
            this.label3.TabIndex = 60;
            this.label3.Text = "\r\nDesenvolvedores:\r\nMarcos Nunes de Souza\r\nGitHub - MarcosNSouza87\r\n\r\nSteffano Fe" +
    "sta\r\n\r\n";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 13);
            this.label2.TabIndex = 59;
            this.label2.Text = "Versão 1.002  de  23 Maio de 2017  ";
            // 
            // FrmInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(202, 215);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pnl_gearDt);
            this.ForeColor = System.Drawing.Color.PowderBlue;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sobre";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnl_gearDt.ResumeLayout(false);
            this.pnl_gearDt.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.log_gif)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox pnl_gearDt;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox log_gif;
    }
}

