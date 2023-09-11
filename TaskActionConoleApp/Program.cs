using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using OpenCvSharp;
using PythonWrapper;


namespace TaskActionConoleApp
{
    internal class Program
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Main()
        {
            string inputFileName = $@"C:\Users\DELL\Desktop\m-img.jpg";
            string outputFileName = $@"C:\Users\DELL\Desktop\Rect";
            using (var large = Cv2.ImRead(inputFileName))
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
                rgb.SaveImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"rgb-{idx}.jpg"));




            }
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey();
        }


        //static void Main(string[] args)
        //{
        //    // Compile script
        //    //PythonScript script = new PythonScript(@"C:\Users\DELL\source\repos\TesseractOCR\TaskActionConoleApp\example.py");

        //    //// Call top-level function
        //    //string returnValue = script.CallFunction<string>("get_string", "World");
        //    //Console.WriteLine(returnValue);

        //    //// Create Python object
        //    //PythonClass pyClass = script.GetClass("ExampleClass");
        //    //PythonObject pyObj = pyClass.Instantiate(5);

        //    //// Get property
        //    //int number = pyObj.GetProperty<int>("number");
        //    //Console.WriteLine("Number property is {0}", number);

        //    //// Get return value as Python object
        //    //PythonObject pyObj2 = pyObj.CallMethod("get_object");
        //    //int number2 = pyObj2.GetProperty<int>("number");
        //    //Console.WriteLine("Number property for 2nd object is {0}", number2);

        //    var engine = Python.CreateEngine();
        //    var searchPaths = engine.GetSearchPaths();
        //    searchPaths.Add(@"C:\myProject\packages\DynamicLanguageRuntime.1.1.2");
        //    searchPaths.Add(@"C:\Program Files\IronPython 3.4\Lib");
        //    searchPaths.Add(@"C:\Program Files\IronPython 3.4\Lib\site-packages");

        //    engine.SetSearchPaths(searchPaths);
        //    var scope = engine.CreateScope();
        //    var source = engine.CreateScriptSourceFromFile(@"C:\Users\DELL\source\repos\TesseractOCR\TaskActionConoleApp\example.py");
        //    source.Execute(scope);

        //    var greetings = scope.GetVariable<Func<object, object, object>>("greetings");
        //    Console.WriteLine(greetings("هذا النص هو مثال لنص يمكن أن يستبدل في نفس المساحة، لقد تم توليد هذا النص من مولد النص العربى، حيث يمكنك أن تولد مثل هذا النص أو العديد من النصوص الأخرى إضافة إلى زيادة عدد الحروف التى يولدها التطبيق.\r\nإذا كنت تحتاج إلى عدد أكبر من الفقرات يتيح لك مولد النص العربى زيادة عدد الفقرات كما تريد، النص لن يبدو مقسما ولا يحوي أخطاء لغوية، مولد النص العربى مفيد لمصممي المواقع على وجه الخصوص، حيث يحتاج العميل فى كثير من الأحيان أن يطلع على صورة حقيقية لتصميم الموقع.\r\nومن هنا وجب على المصمم أن يضع نصوصا مؤقتة على التصميم ليظهر للعميل الشكل كاملاً،دور مولد النص العربى أن يوفر على المصمم عناء البحث عن نص بديل لا علاقة له بالموضوع الذى يتحدث عنه التصميم فيظهر بشكل لا يليق.", @"C:\Users\DELL\Desktop\hello.mp3"));

        //}
 
    }









    //            public static void Main()
    //        {
    //            /*
    //             cv2.threshold(cv2.GaussianBlur(img, (5, 5), 0), 0, 255, cv2.THRESH_BINARY + cv2.THRESH_OTSU)[1]

    //cv2.threshold(cv2.bilateralFilter(img, 5, 75, 75), 0, 255, cv2.THRESH_BINARY + cv2.THRESH_OTSU)[1]

    //cv2.threshold(cv2.medianBlur(img, 3), 0, 255, cv2.THRESH_BINARY + cv2.THRESH_OTSU)[1]

    //cv2.adaptiveThreshold(cv2.GaussianBlur(img, (5, 5), 0), 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY, 31, 2)

    //cv2.adaptiveThreshold(cv2.bilateralFilter(img, 9, 75, 75), 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY, 31, 2)

    //cv2.adaptiveThreshold(cv2.medianBlur(img, 3), 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY, 31, 2)


    //             */

    //            string inputFileName = $@"C:\Users\DELL\Desktop\WhatsApp Image 2023-08-20 at 18.54.25.jpg";
    //            string outputFileName = $@"C:\Users\DELL\Desktop\Rect";
    //            using (var image = Cv2.ImRead(inputFileName))
    //            {
    //                var gray = image.CvtColor(ColorConversionCodes.BGR2GRAY);

    //                var thresh1 = gray.Threshold(0, 255, ThresholdTypes.Otsu | ThresholdTypes.BinaryInv);
    //                var rectkernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(18, 18));

    //                var dilation = thresh1.Dilate(rectkernel, iterations: 1);
    //                // cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_NONE

    //                dilation.FindContours(out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchy, RetrievalModes.External, method: ContourApproximationModes.ApproxNone);
    //                var x = 0;
    //                foreach (var contour in contours)
    //                {
    //                    OpenCvSharp.Rect rect = Cv2.BoundingRect(contour);
    //                    // Drawing a rectangle on copied image
    //                    image.Rectangle(rect, new Scalar(0, 255, 0), 2);

    //                    // Cropping the text block for giving input to OCR
    //                    //var cropped = im2[y: y + h, x: x + w]
    //                    var cropped = image[rect];
    //                    cropped.SaveImage($@"C:\Users\DELL\Desktop\Rect\p-{x++}.png");
    //                }


    //            }

    //            using (var image = Cv2.ImRead(@"C:\Users\DELL\Desktop\Rect\p-0.png"))
    //            {
    //                var gray = image.CvtColor(ColorConversionCodes.BGR2GRAY)
    //               .Resize(OpenCvSharp.Size.Zero, fx: 1.5, fy: 1.5, interpolation: InterpolationFlags.LinearExact);
    //                gray.SaveImage(@"C:\Users\DELL\Desktop\Rect\p-0-s.png");
    //            }


    //            using (var image = Cv2.ImRead(@"C:\Users\DELL\Desktop\Rect\p-1.png"))
    //            {
    //                var gray = image.CvtColor(ColorConversionCodes.BGR2GRAY)
    //               .Resize(OpenCvSharp.Size.Zero, fx: 1.2, fy: 1.2, interpolation: InterpolationFlags.LinearExact);
    //                gray.SaveImage(@"C:\Users\DELL\Desktop\Rect\p-1-s.png");
    //            }

    //            using (var image = Cv2.ImRead(@"C:\Users\DELL\Desktop\Rect\p-2.png"))
    //            {
    //                var gray = image.CvtColor(ColorConversionCodes.BGR2GRAY)
    //               .Resize(OpenCvSharp.Size.Zero, fx: 1.5, fy: 1.5, interpolation: InterpolationFlags.LinearExact);
    //                gray.SaveImage(@"C:\Users\DELL\Desktop\Rect\p-2-s.png");
    //            }

    //            Console.ReadKey();
    //        }

    //}
}


/*
 using System;
using System.Threading.Tasks;

//public class ContinuationTwo
//{
//   public static void Main()
//   {
//      var displayData = Task.Factory.StartNew(() => {
//                                                 Random rnd = new Random();
//                                                 int[] values = new int[100];
//                                                 for (int ctr = 0; ctr <= values.GetUpperBound(0); ctr++)
//                                                    values[ctr] = rnd.Next();

//                                                 return values;
//                                              } ).
//                        ContinueWith((x) => {
//                                        int n = x.Result.Length;
//                                        long sum = 0;
//                                        double mean;

//                                        for (int ctr = 0; ctr <= x.Result.GetUpperBound(0); ctr++)
//                                           sum += x.Result[ctr];

//                                        mean = sum / (double) n;
//                                        return Tuple.Create(n, sum, mean);
//                                     } ).
//                        ContinueWith((x) => {
//                                        return String.Format("N={0:N0}, Total = {1:N0}, Mean = {2:N2}",
//                                                             x.Result.Item1, x.Result.Item2,
//                                                             x.Result.Item3);
//                                     } );
//      Console.WriteLine(displayData.Result);
//   }
//}
// The example displays output similar to the following:
//    N=100, Total = 110,081,653,682, Mean = 1,100,816,536.82
 
 */
