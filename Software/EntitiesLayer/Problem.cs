namespace EntitiesLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Problem")]
    public partial class Problem
    {
        [Key]
        public int ID_Problem { get; set; }

        [Required]
        [StringLength(255)]
        public string ProblemName { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public short? Solved { get; set; }

        public DateTime ReportDate { get; set; }

        public DateTime DateOccured { get; set; }

        public int ID_ProblemCategory { get; set; }

        public int ID_User { get; set; }

        [StringLength(1000)]
        public string ProblemReply { get; set; }

        public int? ID_Employee { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public virtual User User { get; set; }

        public virtual ProblemCategory ProblemCategory { get; set; }

        public virtual User User1 { get; set; }
    }
}
