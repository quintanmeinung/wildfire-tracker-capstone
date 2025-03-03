using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace project_wildfire_web.Models;

[Table("FireData")]
    public partial class FireDatum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // 🔥 Ensures FireId is auto-incremented
        [Column("FireId")]
        public int FireId { get; set; }

        [Column("Location")]
        public Geometry Location { get; set; } = null!;

        [Column("RadiativePower")]
        public decimal RadiativePower { get; set; }

        [Column("Polygon")]
        public Geometry Polygon { get; set; } = null!;

        public int? WeatherId { get; set; }

        public virtual WeatherDatum? Weather { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
