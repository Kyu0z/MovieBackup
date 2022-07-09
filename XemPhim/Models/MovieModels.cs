using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XemPhim.Models
{
    public class MovieManager
    {
        protected readonly DBContext dbContext;

        public MovieManager(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }

    [Table("Movies")]
    public class Movie
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Overview { get; set; }

        public Resource Backdrop { get; set; }

        public Resource Poster { get; set; }

        public DateTime ReleaseAt { get; set; }

        public int VoteAverage { get; set; }

        public int VoteCount { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public ApplicationUser CreatedBy { get; set; }
    }
}