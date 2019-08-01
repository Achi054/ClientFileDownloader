using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FileDownload.Models;
using Ionic.Zip;
using Ionic.Zlib;

namespace FileDownload.Helpers
{
    public class AttachmentHelper
    {
        ICollection<FileContent> attachments = new List<FileContent>();
        ICollection<FileDetails> fileDetails = new List<FileDetails>();
        string attachmentPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..\\Attachments"));
        ZipFile zipfile;

        public AttachmentHelper()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            zipfile = new ZipFile(Encoding.UTF8);
            SetFileContent();
            SetFileDetails();
        }

        public IEnumerable<FileDetails> GetAttchaments()
        {
            return fileDetails.ToList();
        }

        public AttachmentModel DownloadAttchments(IEnumerable<int> attachmentIds)
        {
            var missingFiles = new List<FileDetails>();
            if (attachmentIds.Any(x => x == 0))
                missingFiles.Add(fileDetails.First());
            var validAttachments = fileDetails.Where(x => attachmentIds.Contains(x.Id));
            missingFiles.AddRange(validAttachments.Where(x => !attachmentIds.Contains(x.Id) && x.Id != 0));

            var attachmentFiles = attachments.Where(x => attachmentIds.Contains(x.Id));

            return new AttachmentModel { Attachments = attachmentFiles, MissingAttachments = missingFiles };
        }

        public AttachmentModel DownloadAttchmentsAsZip(IEnumerable<int> attachmentIds)
        {
            var missingFiles = new List<FileDetails>();
            if (attachmentIds.Any(x => x == 0))
                missingFiles.Add(fileDetails.First());
            var validAttachments = fileDetails.Where(x => attachmentIds.Contains(x.Id));
            missingFiles.AddRange(fileDetails.Where(x => !attachmentIds.Contains(x.Id) && x.Id != 0));

            var attachmentFiles = attachments.Where(x => attachmentIds.Contains(x.Id));

            MemoryStream outputStream = new MemoryStream();
            zipfile.CompressionLevel = CompressionLevel.None;
            foreach (var file in attachmentFiles)
            {
                zipfile.AddEntry(file.Name, file.Content);
            }
            zipfile.Save(outputStream);
            outputStream.Position = 0;

            var zipFileContent = new FileContent(1, "application/zip", "Attachments", Convert.ToBase64String(outputStream.ToArray()));
            return new AttachmentModel { Attachments = new List<FileContent> { zipFileContent }, MissingAttachments = missingFiles };
        }

        private void SetFileDetails()
        {
            fileDetails.Add(new FileDetails { Id = 0, Name = "nonexistingfile.txt" });
            foreach (var file in attachments)
            {
                fileDetails.Add(new FileDetails { Id = file.Id, Name = file.Name });
            }
        }

        private void SetFileContent()
        {
            var counter = 1;
            string[] filePaths = Directory.GetFiles(attachmentPath);
            foreach (var file in filePaths)
            {
                Byte[] bytes = System.IO.File.ReadAllBytes(file);

                var fileExtension = Path.GetExtension(file);
                attachments.Add(new FileContent
                    (counter++, $"application/{FileExtension(fileExtension.Substring(1, fileExtension.Length - 1))}",
                    Path.GetFileName(file), Convert.ToBase64String(bytes)));
            }
        }

        private string FileExtension(string extension)
        {
            switch (extension)
            {
                case "txt":
                    return "text";
                case "jfif":
                    return "jpeg";
                default:
                    return "pdf";
            }
        }
    }
}
