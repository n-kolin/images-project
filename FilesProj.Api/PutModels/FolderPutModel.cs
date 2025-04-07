namespace FilesProj.Api.PutModels
{
    public class FolderPutModel
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool IsDeleted { get; set; }

    }
}
