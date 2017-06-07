#region License
/*
 **************************************************************
 *  Author: Rick Strahl 
 *          � West Wind Technologies, 2010-2012
 *          http://www.west-wind.com/
 * 
 * Created: 10/1/2012
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 **************************************************************  
*/
#endregion

using System;

namespace Westwind.RazorHosting
{

    /// <summary>
    /// Configuration objects that can be passed to templates to pass additional 
    /// information down to the templates from a Host container
    /// </summary>
    [Serializable]
    public class RazorTemplateConfiguration
    {        
        /// <summary>
        /// Use this object to pass configuration data to the template
        /// </summary>
        public object ConfigData;
    }

    /// <summary>
    /// Folder Host specific configuration object
    /// </summary>
    [Serializable]
    public class RazorFolderHostTemplateConfiguration : RazorTemplateConfiguration
    {
        public string TemplatePath = string.Empty;
        public string TemplateRelativePath = string.Empty;
        public string PhysicalPath = string.Empty;
    }
    

}
