

namespace BlogService.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime ModifyAt { get; set; } = DateTime.Now;
    }
}