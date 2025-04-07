namespace FilesProj.Api.PostModels
{
    public class FolderPostModel
    {
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public int? ParentId { get; set; }
    }
}
