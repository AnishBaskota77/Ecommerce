using FAMAndIMS.Data.Model.AssetManagementModel.AssetAllocationModel;
using IronBarCode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.ML.OnnxRuntime;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;

namespace FAMAndIMS.Data.Common
{
    public static class GenerateBarCode
    {
        public static string GenerateBarCodeString(IHostingEnvironment env, string folderPath, string barCodeText)
        {
            if (!Directory.Exists(Path.Combine(env.WebRootPath, folderPath)))
                Directory.CreateDirectory(Path.Combine(env.WebRootPath, folderPath));

            // Generate Barcode
            GeneratedBarcode barcode = IronBarCode.BarcodeWriter.CreateBarcode(barCodeText, BarcodeWriterEncoding.Code128);
            barcode.ResizeTo(100, 60);
            barcode.ChangeBarCodeColor(Color.BlueViolet);
            barcode.SetMargins(10);

            // Create file path
            string fileName = "Barcode_" + DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss_ffff") + ".png";
            string filePath = Path.Combine(folderPath, fileName);
            string serverFilePath = Path.Combine(env.WebRootPath, filePath);

            // Save Barcode Image
            barcode.SaveAsPng(serverFilePath);

            return "/" + filePath;
        }
    }
}
