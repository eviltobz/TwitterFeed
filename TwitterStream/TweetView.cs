using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitterFeed;

namespace TwitterStream
{
    public partial class TweetView : UserControl
    {
        public TweetView(Tweet source)
        {
            InitializeComponent();
            Display.Resize += Display_Resize;
            Display.Text = source.ToString();
        }

        void Display_Resize(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Print("resizey");
            this.Height = Display.Height;
            this.Width = Display.Width;
        }
    }
}
