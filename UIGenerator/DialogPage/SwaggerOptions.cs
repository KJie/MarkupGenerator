using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace DST.UIGenerator
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public class SwaggerOptions : DialogPage
    {
        private string baseURL = "";

        [Category("Swagger Setting")]
        [DisplayName("Base URL")]
        public string SwaggerBaseURL
        {
            get { return baseURL; }
            set { baseURL = value; }
        }
    }
}
