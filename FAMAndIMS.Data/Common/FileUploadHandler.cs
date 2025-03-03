using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Common
{
    public class FileUploadHandler
    {
        public static async Task<string> UploadFile(IHostingEnvironment hostEnv, string folderPath, IFormFile file)
        {
            if (!Directory.Exists("" + hostEnv.WebRootPath + "\\" + folderPath))
                Directory.CreateDirectory("" + hostEnv.WebRootPath + "\\" + folderPath);

            if (file == null) return "/" + folderPath;

            var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
            var fileExtension = System.IO.Path.GetExtension(file.FileName);
            folderPath += fileNameWithoutExtension + "_" + DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss_ffff") + fileExtension;

            var serverFolder = Path.Combine(hostEnv.WebRootPath, folderPath);
            await using (var fileStream = new FileStream(serverFolder, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
                fileStream.Flush();
            }
            return "/" + folderPath;
        }
    }
}
