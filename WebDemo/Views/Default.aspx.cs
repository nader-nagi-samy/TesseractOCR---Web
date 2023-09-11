using IronPython.Hosting;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Tesseract;

namespace WebDemo.Views
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


                // Read from file.
                //using (var imageFromFile = new MagickImage(Server.MapPath(@"~\Images\indispensable_color_page_001.jpg")))
                //{
                //    var mMagickFactory = new MagickFactory();
                //    //imageFromFile.Grayscale();
                //    //imageFromFile.AutoThreshold(AutoThresholdMethod.Undefined);
                //    //imageFromFile.ColorType = ColorType.Grayscale;
                //    //imageFromFile.HasAlpha = false;

                //    var objCleaner = new TextCleanerScript<ushort>(factory: mMagickFactory); 
                //    IMagickImage img = objCleaner.Execute(imageFromFile);
                //    img.Despeckle();
                //    //img.Write(AppDomain.CurrentDomain.BaseDirectory + "Images\\Output.jpeg");
                //    img.Write(Server.MapPath(@"~\Images\Alligator_Transparent.png"));
                //}
            }
        }


        #region PageEvents

        protected void SubmitFile_Click(object sender, EventArgs e)
        {
            if (ImageFile.PostedFile != null && ImageFile.PostedFile.ContentLength > 0)
            {
                //
                //Image.FromStream(ImageFile.PostedFile.InputStream);
                //Bitmap.FromStream(ImageFile.PostedFile.InputStream);





                using (var engine = new TesseractEngine(Server.MapPath(@"~/tessdata"), "ara", EngineMode.Default))
                {
                    using (var image = new Bitmap(ImageFile.PostedFile.InputStream))
                    {
                        using (var pix = PixConverter.ToPix(image))
                        {

                            using (var page = engine.Process(pix))
                            {

                                // GetOcrText ....
                                //ResultText.Text = page.GetText();
                                ResultText.InnerText = page.GetText();
                                // Confidence `%`.
                                ConfidenceLabel.Text = string.Format("{0:P}", page.GetMeanConfidence());

                                ExportAudio.Visible = true;

                            }// Dispose


                        } // Dispose


                    } // Dispose


                }// Dispose

                inputPanel.Visible = false;
                resultPanel.Visible = true;
                AudioPanel.Visible = false;
            }
        }

        #endregion

        protected void ReSubmitFile_Click(object sender, EventArgs e)
        {
            inputPanel.Visible = true;
            resultPanel.Visible = false;
            AudioPanel.Visible = false;
        }

        protected void ExportAudio_Click(object sender, EventArgs e)
        {
            var exportAudioTask = Task.Factory.StartNew((Object obj) =>
            {
                var data = obj as string;
                if (data == null) return false;

                var engine = Python.CreateEngine();
                var searchPaths = engine.GetSearchPaths();
                searchPaths.Add(@"C:\Program Files\IronPython 3.4\Lib");
                searchPaths.Add(@"C:\Program Files\IronPython 3.4\Lib\site-packages");

                engine.SetSearchPaths(searchPaths);
                var scope = engine.CreateScope();
                var source = engine.CreateScriptSourceFromFile(Server.MapPath(@"~\example.py"));
                source.Execute(scope);

                // C:\Users\DELL\source\repos\TesseractOCR\WebDemo
                var greetings = scope.GetVariable<Func<object, object, object>>("greetings");
                try
                {
                    //return (string)greetings(data, Server.MapPath(@"~\Audio\hello.mp3"));
                    return greetings(data, Server.MapPath(@"~\Audio\hello.mp3"));

                }
                catch (Exception)
                {

                    return false;
                }

            }, ResultText.InnerText);

            Task.WaitAll(exportAudioTask);

            if ((bool)exportAudioTask.Result)
            {

                inputPanel.Visible = false;
                resultPanel.Visible = false;
                AudioPanel.Visible = true;

                string htmlEncoded = $"<audio autoplay  >\r\n        <source src=\"{Page.ResolveUrl(@"~\Audio\hello.mp3")}\" type=\"audio/mp3\" />\r\n        Your Browser don't support the audio tag\r\n    </audio>";
                AudioControls.Controls.Add(new LiteralControl(htmlEncoded));
            }


            //ExportAudio.Text = exportAudioTask.Result;

        }
    }
}


/*

 
 */