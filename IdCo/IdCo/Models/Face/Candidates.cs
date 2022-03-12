using System;

namespace IdCo.Models.Face
{
    public class Candidates
    {
        /// <summary>
        /// Identificador de la persona candidata.
        /// </summary>
        public Guid PersonId { get; set; }
        /// <summary>
        /// Nivel de confianza en el candidato [0, 1].
        /// </summary>
        public double Confidence { get; set; }
    }
}
