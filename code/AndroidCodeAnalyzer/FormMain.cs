using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AndroidCodeAnalyzer
{
    public partial class formMain : Form
    {
        public formMain()
        {
            InitializeComponent();
        }

        private void buttonDownloadFdroid_Click(object sender, EventArgs e)
        {
            FormDownloadFdroid form = new FormDownloadFdroid();
            form.ShowDialog(this);
        }

        private void buttonDownloadRepos_Click(object sender, EventArgs e)
        {
            FormDownloadRepos form = new FormDownloadRepos();
            form.ShowDialog(this);
        }

        private void buttonCommitHistory_Click(object sender, EventArgs e)
        {
            FormCommitHistory form = new FormCommitHistory();
            form.ShowDialog(this);
        }

        private void buttonManifestHistory_Click(object sender, EventArgs e)
        {
            FormManifestHistory form = new FormManifestHistory();
            form.ShowDialog(this);
        }

        private void buttonProcessAuthor_Click(object sender, EventArgs e)
        {
            FormProcessAuthorRating form = new FormProcessAuthorRating();
            form.ShowDialog(this);
        }

        private void buttonProcessPermissions_Click(object sender, EventArgs e)
        {
            FormProcessPermissions form = new FormProcessPermissions();
            form.ShowDialog(this);
        }

    }
}
