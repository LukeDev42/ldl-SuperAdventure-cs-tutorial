namespace SuperAdventure
{
    partial class TradingScreen
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
            this.lblMyInventory = new System.Windows.Forms.Label();
            this.lblVendorInventory = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMyInventory
            // 
            this.lblMyInventory.AutoSize = true;
            this.lblMyInventory.Location = new System.Drawing.Point(99, 13);
            this.lblMyInventory.Name = "lblMyInventory";
            this.lblMyInventory.Size = new System.Drawing.Size(98, 20);
            this.lblMyInventory.TabIndex = 0;
            this.lblMyInventory.Text = "My Inventory";
            // 
            // lblVendorInventory
            // 
            this.lblVendorInventory.AutoSize = true;
            this.lblVendorInventory.Location = new System.Drawing.Point(349, 13);
            this.lblVendorInventory.Name = "lblVendorInventory";
            this.lblVendorInventory.Size = new System.Drawing.Size(141, 20);
            this.lblVendorInventory.TabIndex = 1;
            this.lblVendorInventory.Text = "Vendor\'s Inventory";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(441, 274);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // TradingScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 293);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblVendorInventory);
            this.Controls.Add(this.lblMyInventory);
            this.Name = "TradingScreen";
            this.Text = "Trade";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMyInventory;
        private System.Windows.Forms.Label lblVendorInventory;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvMyItems;
        private System.Windows.Forms.DataGridView dgvVendorItems;
    }
}