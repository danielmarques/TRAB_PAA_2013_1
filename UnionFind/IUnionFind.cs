using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionFind
{
    /// <summary>
    /// Interface para as diversas classes que implementam o UnionFind de forma diferente.
    /// </summary>
    public interface IUnionFind
    {
        /// <summary>
        /// Faz a união de dois conjuntos.
        /// </summary>
        /// <param name="firstSetRepresentative">Identificador (ou índice) do representante do primeiro conjunto que será unido.</param>
        /// <param name="secondSetRepresentative">Identificador (ou índice) do representante do segundo conjunto que será unido.</param>
        void Union(int firstSetRepresentative, int secondSetRepresentative);

        /// <summary>
        /// Retorna o identificador (ou índice) do representante do conjunto ao qual o elemento pertence. 
        /// </summary>
        /// <param name="element">Identificador (ou índice) do elemento</param>
        /// <returns></returns>
        int Find(int element);
    }
}
