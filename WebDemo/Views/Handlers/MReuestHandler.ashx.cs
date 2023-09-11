using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Services;
using System.Web.SessionState;

namespace WebDemo.Views.Handlers
{
    /// <summary>
    /// Summary description for MReuestHandler
    /// </summary>
    /// 
    public class MReuestHandler : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            // Load file meta data with FileInfo
            FileInfo fileInfo = new FileInfo(context.Server.MapPath(@"~\Images\indispensable_color_page_001.png"));

            // The byte[] to save the data in
            byte[] data = new byte[fileInfo.Length];

            // Load a filestream and put its content into the byte[]
            using (FileStream fs = fileInfo.OpenRead())
            {
                fs.Read(data, 0, data.Length);
            }

            var ss = context.Session["s"];
            //context.Response.ContentType = "text/json";
            //context.Response.Write(JsonConvert.SerializeObject(new int[,] { { 1, 10 }, { 2, 15 }, { 3, 13 }, { 4, 17 } }));
            context.Response.BinaryWrite(data);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}