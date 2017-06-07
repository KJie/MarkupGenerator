namespace DST.UIGenerator
{
    partial class TemplateUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabTemplate = new System.Windows.Forms.TabControl();
            this.tabDetail = new System.Windows.Forms.TabPage();
            this.rtfTemplate = new System.Windows.Forms.RichTextBox();
            this.tabTemplate.SuspendLayout();
            this.tabDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabTemplate
            // 
            this.tabTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabTemplate.Controls.Add(this.tabDetail);
            this.tabTemplate.Location = new System.Drawing.Point(0, 0);
            this.tabTemplate.Name = "tabTemplate";
            this.tabTemplate.SelectedIndex = 0;
            this.tabTemplate.Size = new System.Drawing.Size(379, 388);
            this.tabTemplate.TabIndex = 0;
            // 
            // tabDetail
            // 
            this.tabDetail.Controls.Add(this.rtfTemplate);
            this.tabDetail.Location = new System.Drawing.Point(4, 22);
            this.tabDetail.Name = "tabDetail";
            this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabDetail.Size = new System.Drawing.Size(371, 362);
            this.tabDetail.TabIndex = 0;
            this.tabDetail.Text = "Template";
            this.tabDetail.UseVisualStyleBackColor = true;
            // 
            // rtfTemplate
            // 
            this.rtfTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfTemplate.Location = new System.Drawing.Point(0, 0);
            this.rtfTemplate.Name = "rtfTemplate";
            this.rtfTemplate.Size = new System.Drawing.Size(371, 362);
            this.rtfTemplate.TabIndex = 0;
            this.rtfTemplate.Text = "";
            this.rtfTemplate.TextChanged += new System.EventHandler(this.rtfDetail_TextChanged);
            // 
            // TemplateUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabTemplate);
            this.Name = "TemplateUserControl";
            this.Size = new System.Drawing.Size(379, 388);
            this.tabTemplate.ResumeLayout(false);
            this.tabDetail.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabTemplate;
        private System.Windows.Forms.TabPage tabDetail;
        private System.Windows.Forms.RichTextBox rtfTemplate;
    }
}
