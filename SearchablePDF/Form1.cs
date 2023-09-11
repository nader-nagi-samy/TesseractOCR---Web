﻿using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Tesseract;
using static System.Net.Mime.MediaTypeNames;

namespace SearchablePDF
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }


        private void ConvertButton_Click(object sender, EventArgs e)
        {
            var docuentFileName = FilePathLabel.Text;
            PageConfidenceListBox.Items.Clear();
            if (Path.GetExtension(docuentFileName).ToLower().Equals(".pdf"))
            {
                try
                {

                    GhostscriptPngDevice dev = new GhostscriptPngDevice(GhostscriptPngDeviceType.Png16m);
                    dev.GraphicsAlphaBits = GhostscriptImageDeviceAlphaBits.V_4;
                    dev.TextAlphaBits = GhostscriptImageDeviceAlphaBits.V_4;
                    dev.InputFiles.Add(docuentFileName);
                    dev.DownScaleFactor = 2;
                    dev.CustomSwitches.Add("-dDOINTERPOLATE"); // custom parameter
                    dev.CustomSwitches.Add("-r500*500");
                    dev.OutputPath = $@"{Path.GetDirectoryName(docuentFileName)}\indispensable_color_page_%03d.png";
                    dev.Process();


                    foreach (var filePath in Directory.GetFiles(Path.GetDirectoryName(docuentFileName),
                        "*.png", SearchOption.AllDirectories).ToList())
                    {
                        using (var engine = new TesseractEngine($@"{AppDomain.CurrentDomain.BaseDirectory}\tessdata\", "ara", EngineMode.Default))
                        {
                            using (var pix = Pix.LoadFromFile(filePath))
                            {
                                using (Page recognizedPage = engine.Process(pix))
                                {
                                    string recognizedText = recognizedPage.GetText();
                                    PageConfidenceListBox.Items.Add($"Mean confidence: {recognizedPage.GetMeanConfidence():P}");
                                    using (var writer = new StreamWriter($@"{Path.GetDirectoryName(filePath)}\{Path.GetFileNameWithoutExtension(filePath)}.txt"))
                                    {
                                        writer.Write(recognizedText.Normalize());
                                    }
                                }

                            }


                        }


                        Console.WriteLine(filePath);
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            if (SelectPdfFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = SelectPdfFileDialog.FileName;
                FilePathLabel.Text = filePath;
            }
            else
            {
                FilePathLabel.Text = string.Empty;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}


/*
  //for (int i = 0; i < pdf.PageCount; ++i)
                        //{
                        //    if (documentText.Length > 0)
                        //        documentText.Append("\r\n\r\n");

                        //    PdfPage page = pdf.Pages[i];
                        //    string searchableText = page.GetText();

                        //    // Simple check if the page contains searchable text.
                        //    // We do not need to perform OCR in that case.
                        //    if (!string.IsNullOrEmpty(searchableText.Trim()))
                        //    {
                        //        documentText.Append(searchableText);
                        //        PageConfidenceListBox.Items.Add($"Text Page #{i}");
                        //        continue;
                        //    }

                        //    // This page is not searchable.
                        //    // Save the page as a high-resolution image
                        //    PdfDrawOptions options = PdfDrawOptions.Create();
                        //    options.BackgroundColor = new PdfRgbColor(255, 255, 255);
                        //    options.HorizontalResolution = 300;
                        //    options.VerticalResolution = 300;

                        //    string pageImage = $"page_{i}.png";
                        //    page.Save(pageImage, options);

                        //    // Perform OCR
                        //    using (Pix img = Pix.LoadFromFile(pageImage))
                        //    {
                        //        using (Page recognizedPage = engine.Process(img))
                        //        {

                        //            PageConfidenceListBox.Items.Add($"Mean confidence for page #{i}: {recognizedPage.GetMeanConfidence():P}");
                        //            string recognizedText = recognizedPage.GetText();
                        //            documentText.Append(recognizedText);
                        //        }
                        //    }

                        //    File.Delete(pageImage);

                        //    using (var writer = new StreamWriter($@"{Path.GetDirectoryName(docuentFileName)}\page_{i}.txt"))
                        //        writer.Write(documentText.ToString());
                        //}
 */