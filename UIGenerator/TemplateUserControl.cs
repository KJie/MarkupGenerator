using System.Windows.Forms;

namespace DST.UIGenerator
{
    public partial class TemplateUserControl : UserControl
    {
        public TemplateUserControl()
        {
            InitializeComponent();
        }

        internal HTMLTemplateOptions settings;

        public void Initialize()
        {
            rtfTemplate.Text = settings.MarkupTemplate;
        }

        private void rtfDetail_TextChanged(object sender, System.EventArgs e)
        {
            settings.MarkupTemplate = rtfTemplate.Text;
        }
    }
}
