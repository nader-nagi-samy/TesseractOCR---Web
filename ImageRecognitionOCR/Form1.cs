using OpenCvSharp;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Tesseract;

namespace ImageRecognitionOCR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            if (SelectImageFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = SelectImageFileDialog.FileName;
                FilePathLabel.Text = filePath;
            }
            else
            {
                FilePathLabel.Text = string.Empty;
            }
        }

        private void ConvertButton_Click(object sender, EventArgs e)
        {

            try
            {
                using (var engine = new TesseractEngine($@"{AppDomain.CurrentDomain.BaseDirectory}\tessdata\", "ara", EngineMode.Default))
                {


                    using (var large = Cv2.ImRead(FilePathLabel.Text))
                    {
                        Mat rgb = new Mat(), small = new Mat(), grad = new Mat(), bw = new Mat(), connected = new Mat();




                        // downsample and use it for processing
                        Cv2.Resize(large, rgb, OpenCvSharp.Size.Zero, fx: 1.2, fy: 1.2, interpolation: InterpolationFlags.Cubic);
                        //Cv2.CopyTo(large, rgb);



                        Cv2.CvtColor(rgb, small, ColorConversionCodes.BGR2GRAY);

                        // morphological gradient
                        var morphKernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(3, 3));
                        Cv2.MorphologyEx(small, grad, MorphTypes.Gradient, morphKernel);

                        // binarize
                        Cv2.Threshold(grad, bw, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                        // connect horizontally oriented regions
                        morphKernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(9, 1));
                        Cv2.MorphologyEx(bw, connected, MorphTypes.Close, morphKernel);


                        // find contours
                        var mask = new Mat(Mat.Zeros(bw.Size(), MatType.CV_8UC1), Range.All);
                        Cv2.FindContours(connected, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchy, RetrievalModes.External,
                            ContourApproximationModes.ApproxNone, new OpenCvSharp.Point(0, 0));

                        // filter contours
                        var idx = 0;
                        foreach (var hierarchyItem in hierarchy)
                        {
                            idx = hierarchyItem.Next;
                            if (idx < 0)
                                break;
                            OpenCvSharp.Rect rect = Cv2.BoundingRect(contours[idx]);
                            var maskROI = new Mat(mask, rect);
                            maskROI.SetTo(new Scalar(0, 0, 0));

                            //// fill the contour
                            Cv2.DrawContours(mask, contours, idx, Scalar.White, -1, LineTypes.AntiAlias);

                            // ratio of non-zero pixels in the filled region
                            double r = (double)Cv2.CountNonZero(maskROI) / (rect.Width * rect.Height);
                            if (r > .45 /* assume at least 45% of the area is filled if it contains text */
                                 &&
                            (rect.Height > 8 && rect.Width > 8) /* constraints on region size */
                            /* these two conditions alone are not very robust. better to use something 
                            like the number of significant peaks in a horizontal projection as a third condition */
                            )
                            {
                                Cv2.Rectangle(rgb, rect, Scalar.White, 0);
                            }
                            var contrast = 1.0;
                            var brightness = 17;
                            brightness += (int)(Math.Round(255 * (1 - contrast) / 2));
                            Cv2.AddWeighted(rgb[rect], contrast, rgb[rect], 0, brightness, rgb[rect]);
                            //rgb[rect].SaveImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"rgb-{idx}.jpg"));
                        }
                        //rgb.SaveImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"rgb-{idx}.jpg"));
                        using (var image = new Bitmap(new MemoryStream(rgb.ToBytes())))
                        {

                            using (var pix = PixConverter.ToPix(image)) {
                            
                           
                                using (var page = engine.Process(pix))
                                {
                                    var text = page.GetText();
                                    ConfidenceLabel.Text = string.Format("Mean confidence: {0:P}", page.GetMeanConfidence());
                                    ExportedRichText.Text = text;
                                }
                            }
                        }

                    }

                }
            }
            catch (Exception ee)
            {
                Trace.TraceError(ee.ToString());
                Console.WriteLine("Unexpected Error: " + ee.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(ee.ToString());
            }
            //Console.Write("Press any key to continue . . . ");
            //Console.ReadKey(true);
        }
    }
}