using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Microsoft.VisualStudio.Extensions.Clipper
{
    /// <summary>
    /// Interaction logic for AuthDialog.xaml
    /// </summary>
    public partial class AuthDialog : Window
    {
        public AuthDialog()
        {
            InitializeComponent();
        }

        public string authCode; 

        private void webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (e.Uri == null)
            {
                authCode = String.Empty;
                DialogResult = false;
                this.Close();
            }

            if (e.Uri.AbsoluteUri.Contains("code="))
            {
                authCode = Regex.Split(e.Uri.AbsoluteUri, "code=")[1];
                DialogResult = true;
                this.Close();
            }
        }
    }
}
