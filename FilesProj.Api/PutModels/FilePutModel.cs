namespace FilesProj.Api.PutModels
{
    public class FilePutModel
    {
        public string Name { get; set; }
        //public string Path { get; set; }
        //public string FullPath { get; set; }
        public string Type { get; set; }
        public decimal Size { get; set; }
        public int? FolderId { get; set; }
        public bool IsDeleted { get; set; } = false;


    }
}
