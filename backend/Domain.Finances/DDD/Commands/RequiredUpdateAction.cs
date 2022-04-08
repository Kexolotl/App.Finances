using System.ComponentModel.DataAnnotations;

namespace Domain.Finances.DDD.Commands
{
    public class RequiredUpdateAction<T>
    {
        [Required]
        public T Value { get; set; }
    }
}
