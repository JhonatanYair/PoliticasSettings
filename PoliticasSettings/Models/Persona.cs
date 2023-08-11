using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PoliticasSettings.Models
{
    public partial class Persona
    {
        public Persona()
        {
            Hijo = new HashSet<Hijo>();
            Padre = new HashSet<Padre>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public int IdGenero { get; set; }
        public int? Edad { get; set; }

        public virtual Genero? IdGeneroNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<Hijo> Hijo { get; set; }
        [JsonIgnore]
        public virtual ICollection<Padre> Padre { get; set; }
    }
}
