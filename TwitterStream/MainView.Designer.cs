namespace TwitterStream
{
    partial class MainView
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
            this.Refresh = new System.Windows.Forms.Button();
            this.TweetList = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // Refresh
            // 
            this.Refresh.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Refresh.Location = new System.Drawing.Point(0, 459);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(672, 23);
            this.Refresh.TabIndex = 1;
            this.Refresh.Text = "Refresh";
            this.Refresh.UseVisualStyleBackColor = true;
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // TweetList
            // 
            this.TweetList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TweetList.AutoScroll = true;
            this.TweetList.ColumnCount = 1;
            this.TweetList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TweetList.Location = new System.Drawing.Point(13, 13);
            this.TweetList.Name = "TweetList";
            this.TweetList.RowCount = 2;
            this.TweetList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TweetList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TweetList.Size = new System.Drawing.Size(647, 440);
            this.TweetList.TabIndex = 2;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 482);
            this.Controls.Add(this.TweetList);
            this.Controls.Add(this.Refresh);
            this.Name = "MainView";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Refresh;
        private System.Windows.Forms.TableLayoutPanel TweetList;
    }
}

