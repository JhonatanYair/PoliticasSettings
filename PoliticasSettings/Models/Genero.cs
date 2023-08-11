using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PoliticasSettings.Models
{
    public partial class Genero
    {
        public Genero()
        {
            Persona = new HashSet<Persona>();
        }

        public int Id { get; set; }
        public string Genero1 { get; set; }

        [JsonIgnore]
        public virtual ICollection<Persona> Persona { get; set; }
    }
}
