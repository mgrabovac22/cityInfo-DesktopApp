namespace EntitiesLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Institution")]
    public partial class Institution
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Institution()
        {
            PeriodLocation = new HashSet<PeriodLocation>();
            User = new HashSet<User>();
        }

        [Key]
        public int ID_Institution { get; set; }

        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public short DaysOnDuty { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(1000)]
        public string Address { get; set; }

        [Required]
        [StringLength(1000)]
        public string WorkingHours { get; set; }

        [Required]
        [StringLength(100)]
        public string InstitutionName { get; set; }

        public int? ID_Type { get; set; }

        public virtual InstitutionType InstitutionType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PeriodLocation> PeriodLocation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> User { get; set; }
    }
}
