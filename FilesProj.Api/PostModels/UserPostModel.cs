using System.ComponentModel.DataAnnotations;

namespace FilesProj.Api.PostModels
{

    public class UserPostModel
    {
        
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
