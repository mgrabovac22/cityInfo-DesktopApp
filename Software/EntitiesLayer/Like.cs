namespace EntitiesLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Like")]
    public partial class Like
    {
        [Key]
        public int ID_Like { get; set; }

        public int UserID_User { get; set; }

        public int SuggestionID_Suggestion { get; set; }

        public virtual User User { get; set; }

        public virtual Suggestion Suggestion { get; set; }
    }
}
