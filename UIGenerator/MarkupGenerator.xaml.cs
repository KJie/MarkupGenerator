using EnvDTE;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using UIGenerator;
using Westwind.RazorHosting;
using ZTn.Json.JsonTreeView;

namespace DST.UIGenerator
{
    /// <summary>
    /// Interaction logic for MarkupGenerator.xaml
    /// </summary>
    public partial class MarkupGenerator : System.Windows.Controls.UserControl
    {
        #region >> Fields

        private System.Timers.Timer jsonValidationTimer;

        #endregion
        /// <summary>
        /// Cached instance of RazorHost
        /// </summary>
        RazorEngine<RazorTemplateBase> Host { get; set; }
        string SwaggerJson = string.Empty;

        public MarkupGenerator()
        {
            InitializeComponent();

            //System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(GetSwaggerJson));
            //thread.Start();
            GetSwaggerJson();
        }

        public void SetupAPIs()
        {
            if (SwaggerJson == string.Empty)
            {
                GetSwaggerJson();
            }

            JToken jToken = JToken.Parse(SwaggerJson);
            //JToken jPathToken = jToken.SelectToken("paths");
            List<string> apiList = new List<string>();

            foreach (KeyValuePair<string, JToken> subObj in (JObject)jToken["paths"])
            {
                apiList.Add(subObj.Key);

            }
            cbxAPIs.ItemsSource = apiList;
            cbxAPIs.SelectedIndex = 1;
        }
        /// <summary>
        /// Get the completely swagger json
        /// </summary>
        public void GetSwaggerJson()
        {
            try
            {
                if (SwaggerJson == string.Empty)
                {
                    DTE dte = UIGeneratorPackage.GetGlobalService(typeof(DTE)) as DTE;
                    Properties swaggerPros = dte.get_Properties("UI Generator Setting", "Swagger Setting");
                    string baseURL = swaggerPros.Item("SwaggerBaseURL").Value.ToString();
                    WebRequest request = WebRequest.Create(baseURL);
                    WebResponse response = request.GetResponse();
                    Stream dataStream = response.GetResponseStream();
                    string strJson = string.Empty;
                    using (StreamReader reader = new StreamReader(dataStream, Encoding.UTF8))
                    {
                        SwaggerJson = reader.ReadToEnd();
                    }
                    response.Close();
                }

                //SetupAPIs();
                JToken jToken = JToken.Parse(SwaggerJson);
                //JToken jPathToken = jToken.SelectToken("paths");
                List<string> apiList = new List<string>();

                foreach (KeyValuePair<string, JToken> subObj in (JObject)jToken["paths"])
                {
                    apiList.Add(subObj.Key);

                }
                cbxAPIs.ItemsSource = apiList;
                cbxAPIs.SelectedIndex = 1;
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Can not get schema from server, please check the swagger docs address.");
            }
        }
        private void btnGo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (SwaggerJson == string.Empty)
                {
                    GetSwaggerJson();
                }

                JToken jBaseToken = JToken.Parse(SwaggerJson);
                string strAPIPath = "paths." + cbxAPIs.SelectedItem.ToString();
                JToken jCurentToken = jBaseToken.SelectToken(strAPIPath);
                jCurentToken = jCurentToken["post"] == null ? jCurentToken["get"]["responses"] : jCurentToken["post"]["responses"];

                string strAPISchema = "{\"description\": \"No Content\"}";
                //has success response
                if (jCurentToken["200"] != null)
                {
                    jCurentToken = jCurentToken["200"]["schema"];
                    strAPISchema = ExtractSchema2(jBaseToken, jCurentToken);
                }
                //txtSchema.Text = JToken.Parse(strAPISchema).ToString();
                byte[] byteArray = Encoding.ASCII.GetBytes(strAPISchema);
                MemoryStream stream = new MemoryStream(byteArray);
                jTokenTree.SetJsonSource(stream);
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Get schema failed.");
            }
        }

        private string ExtractSchema2(JToken jBaseToken, JToken jCurrentToken)
        {
            string strSchema = string.Empty;
            if (jCurrentToken == null)
            {
                return strSchema;
            }
            strSchema = jCurrentToken.ToString();
            if (jCurrentToken["type"] == null)
            {
                if (jCurrentToken["$ref"] != null)
                {
                    string strRefPath = jCurrentToken["$ref"].ToString();
                    strRefPath = strRefPath.Substring(2).Replace("/", "['") + "']";
                    JToken jToken = jBaseToken.SelectToken(strRefPath);
                    string strSubSchema = ExtractSchema2(jBaseToken, jToken);
                    strSchema = strSchema.Replace("\"$ref\": \"" + jCurrentToken["$ref"].ToString() + "\"", strSubSchema.Substring(1, strSubSchema.Length - 2).Trim());
                    return strSchema;
                }
            }
            string strCurentTokenType = jCurrentToken["type"].ToString();
            if (strCurentTokenType == "object" || strCurentTokenType == "array")
            {
                JObject jParentObject = strCurentTokenType == "object" ? (JObject)jCurrentToken["properties"] == null ? (JObject)jCurrentToken["additionalProperties"] : (JObject)jCurrentToken["properties"] : (JObject)jCurrentToken["items"];
                foreach (KeyValuePair<string, JToken> subObj in jParentObject)
                {
                    if (!subObj.Value.HasValues)
                    {
                        continue;
                    }
                    if (subObj.Value["$ref"] != null)
                    {//need to recursive call the ExtractSchema function if it is a reference
                        string strRefPath = subObj.Value["$ref"].ToString();
                        strRefPath = strRefPath.Substring(2).Replace("/", "['") + "']";
                        JToken jToken = jBaseToken.SelectToken(strRefPath);
                        string strSubSchema = ExtractSchema2(jBaseToken, jToken);
                        strSchema = strSchema.Replace("\"$ref\": \"" + subObj.Value["$ref"].ToString() + "\"", strSubSchema.Substring(1, strSubSchema.Length - 2).Trim());
                    }
                    else if (subObj.Value["type"].ToString() == "object")
                    {
                        JToken propertyToken = subObj.Value["properties"] == null ? subObj.Value["additionalProperties"] : subObj.Value["Properties"];

                        if (propertyToken["$ref"] != null)
                        {
                            string strRefPath = propertyToken["$ref"].ToString();
                            strRefPath = strRefPath.Substring(2).Replace("/", "['") + "']";
                            JToken jToken = jBaseToken.SelectToken(strRefPath);
                            string strSubSchema = ExtractSchema2(jBaseToken, jToken);
                            strSchema = strSchema.Replace("\"$ref\": \"" + propertyToken["$ref"].ToString() + "\"", strSubSchema.Substring(1, strSubSchema.Length - 2).Trim());
                        }
                    }
                    else if (subObj.Value["type"].ToString() == "array")
                    {//
                        if (subObj.Value["items"]["$ref"] != null)
                        {
                            string strRefPath = subObj.Value["items"]["$ref"].ToString();
                            strRefPath = strRefPath.Substring(2).Replace("/", "['") + "']";
                            JToken jToken = jBaseToken.SelectToken(strRefPath);
                            string strSubSchema = ExtractSchema2(jBaseToken, jToken);
                            strSchema = strSchema.Replace("\"$ref\": \"" + subObj.Value["items"]["$ref"].ToString() + "\"", strSubSchema.Substring(1, strSubSchema.Length - 2).Trim());
                        }
                    }
                }
            }

            return strSchema;
        }
        /// <summary>
        /// Extract API schema from swagger base schema
        /// </summary>
        /// <param name="jBaseToken"></param>
        /// <param name="strPath"></param>
        /// <returns></returns>
        private string ExtractSchema(JToken jBaseToken, string strPath)
        {
            string strSchema = string.Empty;
            string strRefPath = strPath.Substring(2).Replace("/", "['") + "']";

            JToken jToken = jBaseToken.SelectToken(strRefPath);
            if (jToken == null)
            {
                return strSchema;
            }
            strSchema = jToken.ToString();
            if (jToken["type"] == null)
            {
                System.Windows.Forms.MessageBox.Show("Invalid json schema with nested '$ref'. \r\n" + strSchema);
                return strSchema;
            }

            if (jToken["type"].ToString() == "object")
            {
                foreach (KeyValuePair<string, JToken> subObj in (JObject)jToken["properties"])
                {
                    if (subObj.Value["$ref"] != null)
                    {//no type
                        string strSubRefPath = subObj.Value["$ref"].ToString();
                        string strSubSchema = ExtractSchema(jBaseToken, strSubRefPath);
                        strSchema = strSchema.Replace("\"$ref\": \"" + strSubRefPath + "\"", strSubSchema.Substring(1, strSubSchema.Length - 2).Trim());
                    }
                    else if (subObj.Value["type"].ToString() == "array")
                    {
                        if (subObj.Value["items"]["$ref"] != null)
                        {
                            string strSubRefPath = subObj.Value["items"]["$ref"].ToString();
                            string strSubSchema = ExtractSchema(jBaseToken, strSubRefPath);
                            strSchema = strSchema.Replace("\"$ref\": \"" + strSubRefPath + "\"", strSubSchema.Substring(1, strSubSchema.Length - 2).Trim());
                        }
                    }
                }
            }
            else if (jToken["type"].ToString() == "array")
            {
                foreach (KeyValuePair<string, JToken> subObj in (JObject)jToken["items"])
                {
                    if (subObj.Value["$ref"] != null)
                    {
                        string strSubRefPath = subObj.Value["$ref"].ToString();
                        string strSubSchema = ExtractSchema(jBaseToken, strSubRefPath);
                        strSchema = strSchema.Replace("\"$ref\": \"" + strSubRefPath + "\"", strSubSchema.Substring(1, strSubSchema.Length - 2).Trim());
                    }
                }
            }

            return strSchema;
        }

        private void btnGenerate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var host = new RazorEngine();

            string strSchema = txtSchema.Text;
            if (strSchema == string.Empty || !IsValidJson(strSchema))
            {
                System.Windows.Forms.MessageBox.Show("Please provide invalid Json schema.");
            }

            JToken jToken = JToken.Parse(strSchema);
            CustomContext context = new CustomContext();
            context.Type = jToken["type"].ToString();
            ParseJosonToContext(context, jToken);

            DTE dte = UIGeneratorPackage.GetGlobalService(typeof(DTE)) as DTE;
            Properties templatePros = dte.get_Properties("UI Generator Setting", "HTML Template");

            string strTemplate = templatePros.Item("MarkupTemplate").Value.ToString();

            string result = host.RenderTemplate(
                strTemplate, context, inferModelType: true
                  );

            if (result == null)
            {
                System.Windows.Forms.MessageBox.Show(host.ErrorMessage, "Template Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            txtGenerated.Text = result;
        }

        /// <summary>
        /// Parse the API schema to our customContext type
        /// </summary>
        /// <param name="parentContext"></param>
        /// <param name="jToken"></param>
        private void ParseJosonToContext(CustomContext parentContext, JToken jToken)
        {
            //Object
            if (jToken["type"] != null && jToken["type"].ToString().Equals("object", StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (KeyValuePair<string, JToken> subObj in (JObject)jToken["properties"])
                {
                    CustomContext context = new CustomContext();
                    context.Name = subObj.Key;
                    context.Type = subObj.Value["type"] == null ? "" : subObj.Value["type"].ToString();
                    context.Format = subObj.Value["format"] == null ? "" : subObj.Value["format"].ToString();
                    parentContext.Items.Add(context);

                    if (context.Type.Equals("object", StringComparison.CurrentCultureIgnoreCase))
                    {//the subObj is an object
                        ParseJosonToContext(context, subObj.Value);
                    }
                    else if (context.Type.Equals("array", StringComparison.CurrentCultureIgnoreCase))
                    {//the subObj is an array
                        ParseJosonToContext(context, subObj.Value["items"]);
                    }
                }
            }
        }

        /// <summary>
        /// Routine that returns an instance of the RazorHost hosted
        /// in a separate AppDomain. Checks for existance of the host
        /// and creates only if needed. You need to cache the host
        /// to avoid excessive resource use.
        /// </summary>
        /// <returns></returns>
        private RazorEngine<RazorTemplateBase> CreateHost()
        {

            if (this.Host != null)
                return this.Host;

            // Use Static Methods - no error message if host doesn't load
            this.Host = RazorEngineFactory<RazorTemplateBase>.CreateRazorHost();
            //this.Host = RazorEngineFactory<RazorTemplateBase>.CreateRazorHostInAppDomain();
            if (this.Host == null)
            {
                System.Windows.Forms.MessageBox.Show("Unable to load Razor Template Host",
                                "Razor Hosting", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return this.Host;
        }

        /// <summary>
        /// To check whether the Json is valid
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        private static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                FileName = "Index.html",
                Filter = "HTML Files|*.html"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = (FileStream)saveFileDialog.OpenFile())
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                    {
                        sw.Write(txtGenerated.Text);
                    }
                }
            }
        }

        private void btnText_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in cbxAPIs.Items)
            {
                cbxAPIs.SelectedItem = item;
                try
                {
                    if (SwaggerJson == string.Empty)
                    {
                        GetSwaggerJson();
                    }

                    JToken jBaseToken = JToken.Parse(SwaggerJson);
                    string strAPIPath = "paths." + cbxAPIs.SelectedItem.ToString();
                    JToken jCurentToken = jBaseToken.SelectToken(strAPIPath);
                    jCurentToken = jCurentToken["post"] == null ? jCurentToken["get"]["responses"] : jCurentToken["post"]["responses"];

                    string strAPISchema = "{\"description\": \"No Content\"}";
                    //has success response
                    if (jCurentToken["200"] != null)
                    {
                        jCurentToken = jCurentToken["200"]["schema"];
                        strAPISchema = ExtractSchema2(jBaseToken, jCurentToken);
                    }
                    txtSchema.Text = JToken.Parse(strAPISchema).ToString();
                }
                catch (Exception)
                {
                    txtGenerated.Text = txtGenerated.Text + cbxAPIs.SelectedItem.ToString() + "\r\n";
                }
            }
        }

        private void txtJsonValue_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            StartValidationTimer();
        }

        private void txtJsonValue_LostFocus(object sender, RoutedEventArgs e)
        {
            txtJsonValue.TextChanged -= txtJsonValue_TextChanged;
        }

        private void txtJsonValue_GotFocus(object sender, RoutedEventArgs e)
        {
            txtJsonValue.TextChanged += txtJsonValue_TextChanged;
        }

        private void SetJsonStatus(string text, bool isError)
        {
            //if (InvokeRequired)
            //{
            //    Invoke(new SetJsonStatusDelegate(SetActionStatus), text, isError);
            //    return;
            //}

            txtJsonStatus.Text = text;
            txtJsonStatus.Foreground = isError ? new SolidColorBrush(Colors.OrangeRed) : new SolidColorBrush(Colors.Black);
        }

        private void StartValidationTimer()
        {
            jsonValidationTimer?.Stop();

            jsonValidationTimer = new System.Timers.Timer(250);

            jsonValidationTimer.Elapsed += (o, args) =>
            {
                jsonValidationTimer.Stop();

                jTokenTree.Invoke(new Action(JsonValidationTimerHandler));
            };

            jsonValidationTimer.Start();
        }

        private void JsonValidationTimerHandler()
        {
            try
            {
                jTokenTree.UpdateSelected(txtJsonValue.Text);

                SetJsonStatus("Json format validated.", false);
            }
            catch (JsonReaderException exception)
            {
                SetJsonStatus(
                    $"INVALID Json format at (line {exception.LineNumber}, position {exception.LinePosition})",
                    true);
            }
            catch
            {
                SetJsonStatus("INVALID Json format", true);
            }
        }

        private void jTokenTree_AfterSelect(object sender, AfterSelectEventArgs e)
        {
            // If jsonValueTextBox is focused then it triggers this event in the update process, so don't update it again ! (risk: infinite loop between events).
            if(!txtJsonValue.IsFocused)
            {
                txtJsonValue.Text = e.GetJsonString();
            }
        }
    }
}
