using BarcodeLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;


namespace iFinco.UI.Util
{
    public class BarCodeGenerator
    {
        static string barcodeFolder = "/barcods";
        public static string GetBarCode(string barCodeTxt)
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(barcodeFolder)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(barcodeFolder));
            }
            Barcode barCode = new Barcode();
            int Width = 250;
            int Height = 80;
            barCode.Alignment = AlignmentPositions.CENTER;
            TYPE type = TYPE.CODE128;
            barCode.IncludeLabel = true;
            barCode.LabelPosition = LabelPositions.BOTTOMCENTER;

            string filePath = barcodeFolder + "/" + FileManager.GetUniqueName("barcod.png");
            Image img = barCode.Encode(type, barCodeTxt, Color.Black, Color.White, Width, Height);
            img.Save(HttpContext.Current.Server.MapPath(filePath));
            return filePath;
        }
    }
}