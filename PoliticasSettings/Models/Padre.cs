using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PoliticasSettings.Models
{
    public partial class Padre
    {
        public Padre()
        {
            Hijo = new HashSet<Hijo>();
        }

        public int Id { get; set; }
        public string? Ocupacion { get; set; }
        public int? IsWork { get; set; }
        public int IdPersona { get; set; }

        public virtual Persona IdPersonaNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<Hijo> Hijo { get; set; }
    }
}
