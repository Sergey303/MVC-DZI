using System;
using System.Web.Mvc;
using Fogid.DocumentStorage;

namespace WebDZCassetesDemo
{
    [RoutePrefix("dzi")]
    public class DziController : Controller
    {
        [Route("{cassetteName}/{dirName}/{fileName}_files/{level}/{x}_{y}.jpg")]
        public ActionResult GetDZIJpeg(string cassetteName, string dirName, string fileName, string level, string x, string y)
        {
            return File(DStorage.GetFileFromSarc(cassetteName, dirName, fileName,
                        string.Format("{0}_files/{1}/{2}_{3}.jpg", fileName, level, x, y)), "image/jpg");
        }



        [Route("{cassetteName}/{dirName}/{fileName}")]
        public ActionResult DziXml(string cassetteName, string dirName, string fileName)
        {
            return File(DStorage.GetFileFromSarc(cassetteName, dirName, fileName, fileName+".xml"), "text/xml");
        }

        public string GetDZIXml(string id)
        {
            //XNamespace dzn = "http://schemas.microsoft.com/deepzoom/2009";
            //return new XElement(dzn + "Image", new XAttribute(XNamespace.Xmlns + "dzn", dzn)  , new XAttribute("ServerFormat", "Default")).ToString();

            return
                "<?xml version='1.0' encoding='utf-8'?><Image TileSize='256' Overlap='1' Format='jpg' xmlns='http://schemas.microsoft.com/deepzoom/2009'>"
                + GetDZISizeWidthHeight(id) + "</Image>";
        }

        private string GetDZISizeWidthHeight(string id)
        {
            //PaCell readImageCell = dzImgaseCells[id];//ReadImageCell(id,true);
            //var lastLevel = readImageCell.Root.Elements().Last();
            //var x = lastLevel.Elements().ToArray();
            //int width = (x.Count() - 1) * 256;
            //var y = x.Last().Elements().ToArray();
            //int height = (y.Count() - 1) * 256;
            //   Sarc.
            //var img = Image.FromStream(new MemoryStream(((object[])y.Last().Get()).Cast<byte>().ToArray()));
            ////   readImageCell.Close();
            //width += img.Width;
            //height += img.Height;

            var sizeWidthHeight = String.Format("<Size Width='{0}' Height='{1}' />", 1000, 1000);
            return sizeWidthHeight;
        }

        [Route("c/{dzcid}")]
        public ActionResult DzcXml(string dzcid)
        {
          

            //lock (StoreModel.Model)
            //{
            //    var absoluteUri = ControllerContext.HttpContext.Request.Url;
            //    var currentUrl = absoluteUri.AbsoluteUri.Substring(0, absoluteUri.AbsoluteUri.Length - absoluteUri.AbsolutePath.Length);
            //    return Content(StoreModel.Model.Store.GetDZC(dzcid.Split('_').Select(int.Parse), currentUrl), "text/xml");
            //}
            return new EmptyResult();

        }
        
        [Route("dziview/{dzid:int}")]
        public ActionResult ViewDeepZoomImage(int dzid)
        {
       return new EmptyResult();
        }


        //[Route("dzcview/{*dzcid}")]
        //public ActionResult ViewDeepZoomCollection(string dzcid)
        //{
        //    return View("DeepZoomCollection", (object)dzcid);
        //}
        [Route("dzcview/{dzcid:int}")]
        public ActionResult ViewDeepZoomCollection(int dzcid)
        {
            return new EmptyResult();

        }
    }
}
