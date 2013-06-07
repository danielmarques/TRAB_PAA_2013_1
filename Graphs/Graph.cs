using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sorting;
using System.Diagnostics;
using UnionFind;

namespace Graphs
{

    #region Enuns

    /// <summary>
    /// Identificadores para os tipos de implementação do algoritmo de Kruskal e Prim
    /// </summary>
    public enum KruskalType
    {
        LinkedListUFHeapSort,       //UnionFind com listas encadeadas e sorting com HeapSort
        TreeUFHeapSort,             //UnionFind com arvores e sorting com HeapSort
        LinkedListUFCountingSort,   //UnionFind com listas encadeadas e sorting com CountingSort
        TreeUFCountingSort          //UnionFind com arvores e sorting com CountingSort
    }
    public enum PrimType
    {
        PQEdge,                     //PQEdge Fila de Prioridades sobre as arestas
        PQVertex                    //PQVertex Fila de Prioridades com ChangeKey sobre os vértices
    }
    #endregion

    /// <summary>
    /// Classe que prove diversas operações com grafos.
    /// </summary>
    public class Graph
    {
        #region Structs
        /// <summary>
        /// Estrutura de dados que representará um vértice na lista de adjacências do grafo. 
        /// Cada vértice posicionado na adjacência de outro tem um identificador (key) e um peso (weight).
        /// O peso refer-se à aresta que liga os dois vértices.
        /// </summary>
        protected struct Vertex
        {
            public int key;
            public int weight;

            public Vertex(int key, int weight)
            {
                this.key = key;
                this.weight = weight;
            }
        }

        /// <summary>
        /// Estrutura de dados que representa uma aresta do grafo (sem peso).
        /// </summary>
        protected struct Edge
        {
            public int vertexFrom { get; set; }
            public int vertexTo { get; set; }
            
        }
        #endregion

        #region Properties

        /// <summary>
        /// Número de Vértices do grafo.
        /// </summary>
        private int numberOfVertices;
        public int NumberOfVertices
        {
            get { return numberOfVertices; }
        }
        
        /// <summary>
        /// Número de arestas do grafo.
        /// </summary>
        private int numberOfEdges;
        public int NumberOfEdges
        {
            get { return numberOfEdges; }            
        }

        /// <summary>
        /// Maior e menor peso de uma aresta do grafo.
        /// </summary>
        private int maxWeight;
        private int minWeight;

        /// <summary>
       /// A lista de adjacências do grafo utiliza a classe Dictionary e a interface ICollection, ambas implementadas pelo C#.
       /// O Dictionary cria um mapeamento (Chave, Valor) e prove métodos que dão suporte à essa coleção de ítens.
       /// O ICollecion é uma interface genérica que define métodos para manipular coleções de ítens. Para implementar essa interface foi utilizada a classe List do C#.
       /// A Lista de adjacências do grafo é então uma lista (Dictionary) de coleções (ICollection) de vértices (vertex definido em structs (acima)).
       /// </summary>
        private Dictionary<int, ICollection<Vertex>> adjacencyLists;
                
        #endregion

        #region Methods

        /// <summary>
        /// Construtor da classe Grafo. Inicializa a lista de adjacências do grafo.
        /// </summary>
        /// <param name="inputList">
        ///     Lista de números no formato das instâsncias de teste:
        ///     [número de vértices]
        ///     [aresta 1, vértice 1] [aresta 1, vértice 2] [risco aresta 1] ...
        ///     Obs: Não pode ter arestas repetidas
        /// </param>
        public Graph(ICollection<IEnumerable<int>> inputList)
        {
            this.adjacencyLists = new Dictionary<int, ICollection<Vertex>>();

            //Cria uma nova lista contendo apenas as arestas (Pula a primeira linha do arquivo).
            var listOfEdges = inputList.Skip(1);

            //Calcula o número de vértices do grafo (Primeira linha do arquivo).
            this.numberOfVertices =  inputList.First().First();
            
            //Calcula o número de arestas do grafo. Cada linha do arquivo será contada como uma aresta única.
            this.numberOfEdges = inputList.Count - 1;
            
            //Constroi a lista de adjacências do grafo.
            //Para cada aresta do grafo, coloca a aresta na lista de adjacências (para os dois vértices).
            foreach (var edge in listOfEdges)
            {
                //Edge: 0 - Vértice de saída, 1 - Vértice de chegada, 2 - Peso da aresta
                //Como o grafo é não direcionado, a aresta aparece na adjacencia dos dois vértices
                this.SetEdge(edge.ElementAt(0), edge.ElementAt(1), edge.ElementAt(2));

                //Teste para não adicionar duas vezes arestas de um vértice para ele mesmo
                if (edge.ElementAt(0) != edge.ElementAt(1))
                {
                    this.SetEdge(edge.ElementAt(1), edge.ElementAt(0), edge.ElementAt(2));
                }
                

                //Determina o maior peso entre todas as arestas do grafo
                this.maxWeight = Math.Max(this.maxWeight, edge.ElementAt(2));
            }
        }

        /// <summary>
        /// Adiciona uma nova aresta à lista de adjacências do grafo.
        /// </summary>
        /// <param name="fromVertex">Vértice de partida da aresta.</param>
        /// <param name="toVertex">Vértice de chagada da aresta.</param>
        /// <param name="edgeWeight">Peso da aresta.</param>
        private void SetEdge(int fromVertex, int toVertex, int edgeWeight)
        {
            Vertex v = new Vertex(toVertex, edgeWeight);

            //Verifica se o vértice de saída já existe na lista de adjacências.
            //Se existe então simplesmente coloca mais um vértice (toVertex) em sua adjacência.
            //Senão cria uma posição para o vértice de saída com o vértice de chegada em sua adjacência.
            if (adjacencyLists.ContainsKey(fromVertex))
            {
                adjacencyLists[fromVertex].Add(v);
            }
            else
            {
                var adj = new List<Vertex>();
                adj.Add(v);
                adjacencyLists.Add(fromVertex, adj);
            }

            ////Verifica se o vértice de chegada já existe na lista de adjacências. Se não existir, cria uma posição para o mesmo. 
            //if (!adjacencyLists.ContainsKey(toVertex))
            //{
            //    var adj = new List<Vertex>();
            //    adjacencyLists.Add(toVertex, adj);
            //}
        }

        /// <summary>
        /// Retorna todas as arestas do grafo, pareadas aos seus respectivos pesos, em uma lista de tuplas.
        /// </summary>
        /// <returns></returns>
        private List<Tuple<int,Edge>> ListGraphEdges()
        {
            List<Tuple<int, Edge>> listOfEdges = new List<Tuple<int, Edge>>();

            //Itera por cada linha da lista de adjacências do grafo. Ou seja, a cada passada considera toda a vizinhança de um vértice do grafo.
            foreach (var adjacenyList in adjacencyLists)
            {
                //Verticaliza de uma única vez todos os vizinhos do vértice, formata cada aresta com seu peso e adiciona a lista de arestas
                listOfEdges.AddRange(adjacenyList.Value.Where(c => c.key >= adjacenyList.Key).Select(l => new Tuple<int, Edge>(item1: l.weight, item2: new Edge() { vertexFrom = adjacenyList.Key, vertexTo = l.key })));
            }
       
            return listOfEdges;
            
        }

        #endregion

        #region kruskal

        /// <summary>
        /// Algoritmo de Kruskal com diversas implementações.
        /// </summary>
        /// <param name="implementationType">Tipo de implemantação que será usada para executar o algoritmo</param>
        /// <returns></returns>
        public int Kruskal(KruskalType implementationType)
        {
            //Declaração de variáveis auxiliares
            int minimumSpaningTreeCost = 0;

            //IUnionFind é uma interface, sua implementação será escolhida no swich abaixo
            IUnionFind unionFind = null;            

            //Cria uma lista de arestas a partir da lista de adjacencias do grafo
            List<Tuple<int, Edge>> edges = ListGraphEdges();

            //Verifica qual é o tipo de implementação do kruskal foi escolhida e executa as ações condizentes       
            switch (implementationType)
            {
                case KruskalType.LinkedListUFHeapSort:

                    //Union Find implementado com listas encadeadas
                    unionFind = new UnionFindLL(this.numberOfVertices);

                    //Faz o sorting com o HeapSort (in place)
                    Sorting.Sorting<Edge>.HeapSort(ref edges);

                    break;
                  
                case KruskalType.TreeUFHeapSort:

                    //Union Find implementado com arvores
                    unionFind = new UnionFindT(this.numberOfVertices);

                    //Faz o sorting com o HeapSort (in place)
                    Sorting.Sorting<Edge>.HeapSort(ref edges);

                    break;

                case KruskalType.LinkedListUFCountingSort:

                    //Union Find implementado com listas encadeadas
                    unionFind = new UnionFindLL(this.numberOfVertices);

                    //Faz o sorting com o CountingSort
                    edges = Sorting.Sorting<Edge>.CountingSort(edges, this.maxWeight);

                    break;

                case KruskalType.TreeUFCountingSort:

                    //Union Find implementado com arvores
                    unionFind = new UnionFindT(this.numberOfVertices);

                    //Faz o sorting com o CountingSort
                    edges = Sorting.Sorting<Edge>.CountingSort(edges, this.maxWeight);

                    break;

                default:

                    throw new ArgumentException("Tipo de Kruskal não especificado.");
            }

            //Percorre a lista ordenada de Arestas. Para quando todos os vértices já tiverem sido colocados na arvore geradora mínima.
            int i = 1;
            int j = 0;
            while (i < this.numberOfVertices)
            {
                //Decobre os conjuntos aos quais os vértices pertencem
                int set1 = unionFind.Find(edges[j].Item2.vertexTo);
                int set2 = unionFind.Find(edges[j].Item2.vertexFrom);

                //Verifica se os vértices pertencem ao mesmo grupo (forma ciclo)
                if ( set1 != set2 )
                {
                    //Soma o risco da aresta ao risco total da arvore geradora mínima
                    minimumSpaningTreeCost += edges[j].Item1;
                    //Une os conjuntos nos quais estão os vértices da aresta que foi adicionada na árvore geradora mínima
                    unionFind.Union(set1, set2);
                    i++;
                }

                j++;                
            }

            return minimumSpaningTreeCost;
        }

        #endregion

        #region Prim

        /// <summary>
        /// Algoritmo de Prim com as duas implementações.
        /// </summary>
        /// <param name="implementationType">Tipo de implemantação que será usada para executar o algoritmo</param>
        /// <returns></returns>
        public int Prim(PrimType implementationType)
        {
             //Declaração de variáveis auxiliares
            int minimumSpaningTreeCost = 0;       

            //Cria uma lista de arestas a partir da lista de adjacencias do grafo
            List<Tuple<int, Edge>> edges = ListGraphEdges();

            //Reordena a lista de arestas na forma de Heap Min (in place)
            Sorting.Sorting<Edge>.HeapfyMin(ref edges);

            //Verifica qual é o tipo de implementação do Prim foi escolhida e executa as ações condizentes       
            switch (implementationType)
            {
                case PrimType.PQEdge:

                    //Devolve o elemento mínimo do Heap retirando do mesmo a aresta de menor prioridade
                    var minEdge = Sorting.Sorting<Edge>.HeapExtractMin(edges);

                    break;

                //case PrimType.PQVertex:

                    //Faz o sorting com o HeapSort (in place)
                    //Sorting.Sorting<Edge>.HeapSortMin(ref edges); ?

                  //  break;
              
                default:

                    throw new ArgumentException("Tipo de Prim não especificado.");
            }

            //INSERIR PRIM

            }

            return minimumSpaningTreeCost;
        }

        #endregion
    }    
}
