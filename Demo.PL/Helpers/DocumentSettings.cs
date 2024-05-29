using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helpers
{
    public static class DocumentSettings
    {
        //Upload

        public static string UploadFile(IFormFile file,string FolderName)
        {
            // 1. Get Located Folder Path ->ده Images الي هو عندي ال

            //string FolderPath ="F:\\ASP.NET Route Academy\\05 MVC\\Session 05\\Demo\\Demo MVC Project Solution\\Demo.PL\\wwwroot\\Files\\Images\\";

            /* << F:\\ASP.NET Route Academy\\05 MVC\\Session 05\\Demo\\Demo MVC Project Solution\\Demo.PL\\ >> بس ده في مشكلة ان الجزء د خاص بيا مش بكل الناس
             * فهنشيلة ونبدلة بي -> Directory.GetCurrentDirectory()
             */

            //string FolderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\" + FolderName; //فمحتاجين نشوف حاجه تانيه Concat بس دي فيها 

            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);

            // 2. Get File Name And Make It Unique

            string FileName = $"{file.FileName}";

            // 3. Get File Path[Folder Path + File Name]

            string FilePath = Path.Combine(FolderPath, FileName);

            // 4. Save File As Streams

            using var FileStream = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(FileStream);
            // 5. Return File Name
            return FileName;
        }

        //Delete
        public static void DeleteFile(string FileName ,string FolderName)
        {
            // 1. Get File Path
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName,FileName);

            // 2. Check if File Exists Or Not موجود ولا لا

            if (File.Exists(FilePath))
            {
                //If Exists Remove It
                File.Delete(FilePath);
            }
        }
    }
}
