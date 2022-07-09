using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XemPhim.Models
{
    public class ResourceManager
    {
        protected readonly DBContext dbContext;

        public ResourceManager(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }

    [Table("Resources")]
    public class Resource
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public ApplicationUser CreatedBy { get; set; }
    }
}