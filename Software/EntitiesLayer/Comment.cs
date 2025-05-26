namespace EntitiesLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        [Key]
        public int ID_Comment { get; set; }

        public DateTime? CommentDate { get; set; }

        [StringLength(1000)]
        public string CommentContent { get; set; }

        public int? UserID_User { get; set; }

        public int? SuggestionID_Suggestion { get; set; }

        public virtual User User { get; set; }
    }
}
