
namespace FcsResultsRequest
{
    partial class FcsResultsRequest
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
            this.txtSdgId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtSdgId
            // 
            this.txtSdgId.Location = new System.Drawing.Point(87, 88);
            this.txtSdgId.Name = "txtSdgId";
            this.txtSdgId.Size = new System.Drawing.Size(100, 20);
            this.txtSdgId.TabIndex = 1;
            this.txtSdgId.TextChanged += new System.EventHandler(this.txtSdgId_TextChanged);
            this.txtSdgId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSdgId_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(102, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "sdg_id הכנס";
            // 
            // FcsResultsRequest
            // 
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSdgId);
            this.Name = "FcsResultsRequest";
            this.Size = new System.Drawing.Size(302, 192);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSdgId;
        private System.Windows.Forms.Label label1;

    }
}
