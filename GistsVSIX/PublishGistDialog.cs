using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsoft.VisualStudio.Extensions.Gists
{
    public partial class PublishGistDialog : Form
    {
        public PublishGistDialog()
        {
            InitializeComponent();
        }

        public string Filename => this.FilenameTextBox.Text;
        public string Description => this.DescriptionTextBox.Text;
        public bool IsPublic => this.IsPublicCheckBox.Checked;
    }
}
