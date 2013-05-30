using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphs;

namespace Main
{
    public class Program
    {
        static void Main(string[] args)
        {

           ICollection<IEnumerable<int>> inputList;

           var path = @"D:\Downloads\TRAB-PAA-2013-1\graphs_1\graph_a1.in";
           inputList = ReadInputFile(path);
           Graph graph_test = new Graph(inputList);
           Console.Write(graph_test);

        }

        /// <summary>
        /// Função que lê um arquivo de entrada (definido em path) e transcreve seu cointeúdo para uma lista de arrays de inteiros.
        /// O formato do arquivo deve obedecer o padrão: 
        ///     [número de vértices]
        ///     [aresta 1, vértice 1] [aresta 1, vértice 2] [risco aresta 1] ...
        /// </summary>
        /// <param name="path">Caminho para o arquivo de entrada que contêm a representação do grafo (instância de teste).</param>
        /// <returns>
        ///     Retorna uma lista de arrays de inteiros. O primeiro elemento da lista possui um único elememnto (número de vértices do grafo) e os demais possúem 3 elementos (dois vértices e o pedo da aresta)
        /// </returns>
        private static ICollection<IEnumerable<int>> ReadInputFile(string path)

        {
            //Inicializa o leitor de arquivos.
            StreamReader sr = new StreamReader(path);

            //Inicializa a lista genérica que será retornada. Cada posição da lista pode conter um ou mais ítens do tipo inteiro.
            List<IEnumerable<int>> imputList = new List<IEnumerable<int>>();

            //Lê o arquivo até o fim
            while (!sr.EndOfStream)
            {
                //Executa as seguintes operações:
                // (i)   Lê uma linha do arquivo;
                // (ii)  Separa seu conteúdo (utiliza o caractere espaço como separador);
                // (iii) Converte o conteúdo para inteiro;
                // (iv)  Coloca os elemantos em uma posição da lista.
                imputList.Add(sr.ReadLine().Split(' ').Select(c => int.Parse(c)));
            }

            return imputList;
        }
    }
}
