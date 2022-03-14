using System;

namespace IdCo.Models.Face
{
    public class Face
    {
        /// <summary>
        /// Identificador de la persona.
        /// </summary>
        public Guid PersonId { get; set; }
        /// <summary>
        /// Identifador del rostro.
        /// </summary>
        public Guid PersistedFaceId { get; set; }
        /// <summary>
        /// Dimensiones del rectangulo que indica la posición de la cara detectada
        /// </summary>
        public FaceRectangle FaceRectangle { get; set; }
        /// <summary>
        /// Candidatos identificados para un rostro.
        /// </summary>
        public Candidates[] Candidates { get; set; }
    }
}
