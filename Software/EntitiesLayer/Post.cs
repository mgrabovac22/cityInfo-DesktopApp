namespace EntitiesLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Post")]
    public partial class Post
    {
        [Key]
        public int ID_Post { get; set; }

        [Required]
        [StringLength(255)]
        public string PostName { get; set; }

        [Required]
        [StringLength(1000)]
        public string PostContent { get; set; }

        public int ID_User { get; set; }

        public short? isUrgent { get; set; }

        public DateTime? DateCreated { get; set; }

        public virtual User User { get; set; }
    }
}
