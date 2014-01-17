using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitterStream
{
    public partial class MainView : Form
    {
        public MainView()
        {
            InitializeComponent();
        }

        private async void Refresh_Click(object sender, EventArgs e)
        {
            var tweets = await new TwitterFeed.Api().GetHomeTimelineAsync();
            TweetList.Controls.Clear();
            TweetList.RowCount = 0;
            TweetList.RowStyles.Clear();
            //View2.Controls.Clear();
            foreach (var tweet in tweets)
            {
                var view = new TweetView(tweet);
                //View2.Controls.Add(view);
                view.Dock = DockStyle.Top;
                view.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                AddRow(view);
            }
            System.Diagnostics.Debug.Print("Tweetlist rowcount = " + TweetList.RowCount.ToString());
        }
        private void AddRow(Control value)
        {
            int rowIndex = AddTableRow();
            TweetList.Controls.Add(value, 0, rowIndex);
        }

        private int AddTableRow()
        {
            int index = TweetList.RowCount++;
            RowStyle style = new RowStyle(SizeType.AutoSize);
            TweetList.RowStyles.Add(style);
            return index;
        }
    }
}
