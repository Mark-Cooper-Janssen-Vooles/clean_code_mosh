using CleanCode.LongMethods;
using System;
using System.Web;

namespace FooFoo
{
    public partial class Download : System.Web.UI.Page
    {
        private readonly TableReader _tableReader = new TableReader();
        private readonly DataFileToCsvMapper _dataFileToCsvMapper = new DataFileToCsvMapper();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearResponse();
            SetCacheability();
            WriteContentResponse(GetCSV());
        }

        private byte[] GetCSV()
        {
            //_tableReader.GetDataTable();

            System.IO.MemoryStream ms = _dataFileToCsvMapper.Map(_tableReader.GetDataTable());
            byte[] byteArray = ms.ToArray();
            ms.Flush();
            ms.Close();
            return byteArray;
        }

        private void WriteContentResponse(byte[] byteArray)
        {
            Response.Charset = System.Text.UTF8Encoding.UTF8.WebName;
            Response.ContentEncoding = System.Text.UTF8Encoding.UTF8;
            Response.ContentType = "text/comma-separated-values";
            Response.AddHeader("Content-Disposition", "attachment; filename=FooFoo.csv");
            Response.AddHeader("Content-Length", byteArray.Length.ToString());
            Response.BinaryWrite(byteArray);
        }

        private void SetCacheability()
        {
            Response.Cache.SetCacheability(HttpCacheability.Private);
            Response.CacheControl = "private";
            Response.AppendHeader("Pragma", "cache");
            Response.AppendHeader("Expires", "60");
        }

        private void ClearResponse()
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Cookies.Clear();
        }
    }
}