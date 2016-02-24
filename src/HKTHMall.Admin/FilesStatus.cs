using System;
using System.IO;

namespace HKTHMall.Admin
{
    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string delete_url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_type { get; set; }
    }

    public class FilesStatus
    {
        public const string HandlerPath = "/Upload/";

        public FilesStatus()
        {
        }

        public FilesStatus(FileInfo fileInfo)
        {
            this.SetValues(fileInfo.Name, (int) fileInfo.Length, fileInfo.FullName);
        }

        public FilesStatus(string fileName, int fileLength, string fullPath)
        {
            this.SetValues(fileName, fileLength, fullPath);
        }

        public string group { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int size { get; set; }
        public string progress { get; set; }
        public string url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_url { get; set; }
        public string delete_type { get; set; }
        public string error { get; set; }

        private void SetValues(string fileName, int fileLength, string fullPath)
        {
            this.name = fileName;
            this.type = "image/png";
            this.size = fileLength;
            this.progress = "1.0";
            this.url = HandlerPath + "UploadHandler.ashx?f=" + fileName;
            this.delete_url = HandlerPath + "UploadHandler.ashx?f=" + fileName;
            this.delete_type = "DELETE";

            var ext = Path.GetExtension(fullPath);

            var fileSize = ConvertBytesToMegabytes(new FileInfo(fullPath).Length);
            if (fileSize > 3 || !this.IsImage(ext)) this.thumbnail_url = "/Content/img/generalFile.png";
            else this.thumbnail_url = @"data:image/png;base64," + this.EncodeFile(fullPath);
        }

        private bool IsImage(string ext)
        {
            return ext == ".gif" || ext == ".jpg" || ext == ".png";
        }

        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(File.ReadAllBytes(fileName));
        }

        private static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes/1024f)/1024f;
        }
    }
}