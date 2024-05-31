
namespace FileService
{
    public class AddFileDto(Guid? id, IFormFile file, string address = "")
    {
        public Guid Id { get; set; } = id ?? Guid.NewGuid();
        public IFormFile File { get; set; } = file;
        public string Address { get; set; } = address;
    }
}