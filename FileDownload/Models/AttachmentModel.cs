using System.Collections.Generic;

namespace FileDownload.Models
{
    public class AttachmentModel
    {
        public AttachmentModel()
        {
            MissingAttachments = new HashSet<FileDetails>();
        }
        public IEnumerable<FileDetails> MissingAttachments { get; set; }
        public IEnumerable<FileContent> Attachments { get; set; }
    }
}
