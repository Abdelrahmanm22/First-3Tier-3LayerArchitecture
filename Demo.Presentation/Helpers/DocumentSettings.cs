using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Demo.Presentation.Helpers
{
    public static class DocumentSettings
    {
        //upload
        public static string UplaodFile(IFormFile file,string folderName)
        {
            // 1. Get Located Folder Path
            //string foldePath = Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\"+folderName;
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);
            // 2. Get File Name and make it Unique
            string FileName = $"{Guid.NewGuid()}{file.FileName}";
            // 3. Get Path[Folder Path + FileName]
            string FilePath = Path.Combine(FolderPath, FileName);
            // 4. Save File As Streams
            using var Fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(Fs);
            // 5. Return File Name
            return FileName;
        }
    }
}
