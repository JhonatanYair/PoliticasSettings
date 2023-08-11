using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PoliticasSettings.Models
{
    public partial class Hijo
    {
        public int Id { get; set; }
        public string? Carrera { get; set; }
        public int? IsStudy { get; set; }
        public int IdPersona { get; set; }
        public int IdPadre { get; set; }

        public virtual Padre IdPadreNavigation { get; set; }
        public virtual Persona IdPersonaNavigation { get; set; }
    }
}
