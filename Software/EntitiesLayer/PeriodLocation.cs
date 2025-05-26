namespace EntitiesLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PeriodLocation")]
    public partial class PeriodLocation
    {
        [Key]
        public int ID_PeriodLocation { get; set; }

        [Required]
        [StringLength(1000)]
        public string Location { get; set; }

        [Required]
        [StringLength(1000)]
        public string Period { get; set; }

        [StringLength(100)]
        public string Location_area { get; set; }

        public int ID_Institution { get; set; }

        public virtual Institution Institution { get; set; }
    }
}
