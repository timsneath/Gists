using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Microsoft.VisualStudio.Extensions.Gists
{
    /// <summary>
    /// Interaction logic for PublishGistDialog.xaml
    /// </summary>
    public partial class PublishGistDialog : Window
    {
        public PublishGistDialog()
        {
            InitializeComponent();
        }


        public string Filename { get { return this.FilenameTextBox.Text; } set { this.FilenameTextBox.Text = value; } }
        public string Description => this.DescriptionTextBox.Text;
        public bool IsPublic => (bool)this.IsPublicCheckBox.IsChecked;
        public bool PublishOnlySelection => this.CodeToIncludeComboBox.SelectedIndex == 0;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
