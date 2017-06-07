using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DST.UIGenerator
{
    [CLSCompliant(false), ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("AFB8E9CC-6F25-46C4-A75A-9CFE2694DE7B")]
    public class HTMLTemplateOptions : DialogPage
    {
        private string markupTemplate = "";

        public string MarkupTemplate
        {
            get { return markupTemplate; }
            set { markupTemplate = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override IWin32Window Window
        {
            get
            {
                TemplateUserControl page = new TemplateUserControl();
                page.settings = this;
                page.Initialize();
                return page;
            }
        }


    }
}
