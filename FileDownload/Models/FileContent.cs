namespace FileDownload.Models
{
    public class FileContent
    {
        public FileContent(int id, string mimeType, string name, string content)
        {
            Id = id;
            MimeType = mimeType;
            Name = name;
            Content = content;
        }
        public int Id { get; set; }
        public string MimeType { get; set; }
        public string Name { get; set; }
        public string Base64 { get { return ";base64"; } }
        public string Content { get; set; }
    }
}
