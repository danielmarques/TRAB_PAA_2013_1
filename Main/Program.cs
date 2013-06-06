using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphs;
using Sorting;


namespace Main
{
    public class Program
    {
        #region Properties

        /// <summary>
        /// Buffer que guarda os resultados/desempenho dos algorítmos.
        /// </summary>
        private static StringBuilder outputBuffer = new StringBuilder("Arquivo de Entrada;Algoritmo;Custo Total AGM;Número de Vértices;Número de Arestas;Tempo de Execução (milisegundos)\n");

        /// <summary>
        /// Caminho relativo para a pasta default onde ficam os arquivos de entrada
        /// </summary>
        private static string inputPath = Path.Combine(Environment.CurrentDirectory, @"Inputs\");

        /// <summary>
        /// Caminho relativo para a pasta default onde são escritos os arquivos de saída
        /// </summary>
        private static string outputPath = Path.Combine(Environment.CurrentDirectory, @"Outputs\OUTPUT");

        #endregion

        /// <summary>
        /// Função principal do programa
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Verifica na pasta Inputs quais são os arquivos de entrada (instâncias de teste) 
            var inputFiles = Directory.EnumerateFiles(inputPath, "*.in", SearchOption.AllDirectories);

            var timer = Stopwatch.StartNew();

            //Para cada arquivo de entrada na pasta Inputs
            foreach (var inputFile in inputFiles)
            {
                //Inicialização do Grafo
                var listFromInputFile = Program.ReadInputFile(inputFile);
                var inputFileName = Directory.GetParent(inputFile).Name + "\\" + Path.GetFileName(inputFile);
                Graph graph = new Graph(listFromInputFile);

                Console.WriteLine("Processando: {0}\n", inputFileName);
                
                //Execução do Kruskal

                foreach (KruskalType krustalType in Enum.GetValues(typeof(KruskalType)) )
                {
                    timer.Restart();
                    int result = graph.Kruskal( krustalType );
                    timer.Stop();
                    WriteOutputBuffer(inputFileName, "Kruskal" + Enum.GetName(typeof(KruskalType),krustalType) , result, graph.NumberOfVertices, graph.NumberOfEdges, timer.Elapsed.TotalMilliseconds);                     
                }                
            }

            //Escreve os resultados no arquivo
            WriteOutputFile();
        }

        #region Methods

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

        /// <summary>
        /// Guarda os resultados na propriedade outputBuffer. No fim do programa o método WriteOutputFile é chamado para escrever este conteúdo em um arquivo.
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="algorithmName"></param>
        /// <param name="algorithmResult"></param>
        /// <param name="numberOfVertex"></param>
        /// <param name="numberOfEdges"></param>
        /// <param name="timeElapsed"></param>
        private static void WriteOutputBuffer(string inputName, string algorithmName, int algorithmResult, int numberOfVertex, int numberOfEdges, double timeElapsed)
        {

            outputBuffer.AppendFormat("{0};{1};{2};{3};{4};{5}\n", inputName, algorithmName, algorithmResult, numberOfVertex, numberOfEdges, timeElapsed);

        }

        /// <summary>
        /// Escreve o conteúdo da propriedade outputBuffer em um arquivo.
        /// </summary>
        private static void WriteOutputFile()
        {

            StreamWriter sr = new StreamWriter(File.OpenWrite(String.Format("{0}{1}.csv" , outputPath, DateTime.Now.ToString("yyyyMMddhhmmss") ) ), Encoding.UTF8);
            
            sr.Write(outputBuffer);
            sr.Close();
        }

        #endregion

        #region DEBUG TEST EXPORT

#if DEBUG    

        public static ICollection<IEnumerable<int>> ExportedReadInputFile(string path)
        {
            return ReadInputFile(path);
        }
        
#endif

        #endregion
    }
}
