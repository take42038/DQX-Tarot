namespace DQXTarotApp
{
    partial class frmMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbRank = new System.Windows.Forms.ComboBox();
            this.lblRank = new System.Windows.Forms.Label();
            this.lblMonster = new System.Windows.Forms.Label();
            this.cmbMonster = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cmbRank
            // 
            this.cmbRank.Font = new System.Drawing.Font("MS UI Gothic", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbRank.FormattingEnabled = true;
            this.cmbRank.Location = new System.Drawing.Point(359, 92);
            this.cmbRank.Name = "cmbRank";
            this.cmbRank.Size = new System.Drawing.Size(445, 45);
            this.cmbRank.TabIndex = 0;
            this.cmbRank.Text = "ーーーーーーー";
            this.cmbRank.SelectionChangeCommitted += new System.EventHandler(this.cmbRank_SelectionChangeCommitted);
            // 
            // lblRank
            // 
            this.lblRank.AutoSize = true;
            this.lblRank.Font = new System.Drawing.Font("MS UI Gothic", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblRank.Location = new System.Drawing.Point(114, 100);
            this.lblRank.Name = "lblRank";
            this.lblRank.Size = new System.Drawing.Size(171, 37);
            this.lblRank.TabIndex = 1;
            this.lblRank.Text = "ランク選択";
            this.lblRank.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMonster
            // 
            this.lblMonster.AutoSize = true;
            this.lblMonster.Font = new System.Drawing.Font("MS UI Gothic", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMonster.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMonster.Location = new System.Drawing.Point(114, 208);
            this.lblMonster.Name = "lblMonster";
            this.lblMonster.Size = new System.Drawing.Size(231, 37);
            this.lblMonster.TabIndex = 2;
            this.lblMonster.Text = "モンスター選択";
            // 
            // cmbMonster
            // 
            this.cmbMonster.Font = new System.Drawing.Font("MS UI Gothic", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbMonster.FormattingEnabled = true;
            this.cmbMonster.Location = new System.Drawing.Point(359, 200);
            this.cmbMonster.Name = "cmbMonster";
            this.cmbMonster.Size = new System.Drawing.Size(445, 45);
            this.cmbMonster.TabIndex = 3;
            this.cmbMonster.Text = "ランクを選択してください";
            // 
            // frmMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1174, 529);
            this.Controls.Add(this.cmbMonster);
            this.Controls.Add(this.lblMonster);
            this.Controls.Add(this.lblRank);
            this.Controls.Add(this.cmbRank);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "メイン画面";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbRank;
        private System.Windows.Forms.Label lblRank;
        private System.Windows.Forms.Label lblMonster;
        private System.Windows.Forms.ComboBox cmbMonster;
    }
}

