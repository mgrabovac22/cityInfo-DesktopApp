namespace EntitiesLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Suggestion")]
    public partial class Suggestion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Suggestion()
        {
            Comment = new HashSet<Comment>();
            Like = new HashSet<Like>();
        }

        [Key]
        public int ID_Suggestion { get; set; }

        [Required]
        [StringLength(255)]
        public string SuggestionName { get; set; }

        [Required]
        [StringLength(1000)]
        public string SuggestionContent { get; set; }

        public int? SuggestionLikes { get; set; }

        public DateTime SuggestionDateCreated { get; set; }

        public int UserID_User { get; set; }

        public int? CommentID_Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Like> Like { get; set; }

        public virtual User User { get; set; }
    }
}
