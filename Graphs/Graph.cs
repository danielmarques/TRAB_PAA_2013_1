﻿using System;
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
        /// Maior peso de uma aresta do grafo.
        /// </summary>
        private int maxWeight;

        /// <summary>
        /// A lista de adjacências do grafo utiliza a classe Dictionary e a interface ICollection, ambas implementadas pelo C#.
        /// O Dictionary cria um mapeamento (Chave, Valor) e prove métodos que dão suporte à essa coleção de ítens.
        /// O ICollecion é uma interface genérica que define métodos para manipular coleções de ítens. Para implementar essa interface foi utilizada a classe List do C#.
        /// A Lista de adjacências do grafo é então uma lista (Dictionary) de coleções (ICollection) de vértices (vertex definido em structs (acima)).
        /// </summary>
        private Dictionary<int, ICollection<Vertex>> adjacencyLists;

        /// <summary>
        /// Lista de arestas do grafo
        /// </summary>
        private List<Tuple<int, Edge>> graphEdges;

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
            this.numberOfVertices = inputList.First().First();

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

            //Preenche a lista de arestas do grafo
            graphEdges = ListGraphEdges();
        }


        /// <summary>
        /// Construtor da classe Grafo. Inicializa a lista de adjacências do grafo. Randoniza as arestas do grafo de acordo com o percentual.
        /// </summary>
        /// <param name="inputList">
        ///     Lista de números no formato das instâsncias de teste:
        ///     [número de vértices]
        ///     [aresta 1, vértice 1] [aresta 1, vértice 2] [risco aresta 1] ...
        ///     Obs: Não pode ter arestas repetidas
        /// </param>
        /// <param name="percentual">Número de 1 até 100 que indica a densidade das arestas no grafo resultante.</param>param>
        public Graph(ICollection<IEnumerable<int>> inputList, int percentual)
        {
            this.adjacencyLists = new Dictionary<int, ICollection<Vertex>>();
            Random rd = new Random();

            //Cria uma nova lista contendo apenas as arestas (Pula a primeira linha do arquivo).
            var listOfEdges = inputList.Skip(1);

            this.numberOfEdges = 0;

            //Constroi a lista de adjacências do grafo.
            //Para cada aresta do grafo, coloca a aresta na lista de adjacências (para os dois vértices).
            foreach (var edge in listOfEdges)
            {
                if (rd.Next(1, 100) <= percentual)
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
                    //Calcula o número de arestas do grafo.
                    this.numberOfEdges++;
                }
            }

            //Calcula o número de vértices do grafo.
            this.numberOfVertices = adjacencyLists.Count;

            //Preenche a lista de arestas do grafo
            graphEdges = ListGraphEdges();
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
        }

        /// <summary>
        /// Retorna todas as arestas do grafo, pareadas aos seus respectivos pesos, em uma lista de tuplas.
        /// </summary>
        /// <returns></returns>
        private List<Tuple<int, Edge>> ListGraphEdges()
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

            //Verifica qual é o tipo de implementação do kruskal foi escolhida e executa as ações condizentes       
            switch (implementationType)
            {
                case KruskalType.LinkedListUFHeapSort:

                    //Union Find implementado com listas encadeadas
                    unionFind = new UnionFindLL(this.numberOfVertices);

                    //Faz o sorting com o HeapSort (in place)
                    Sorting.Heap<Edge>.HeapSort(ref graphEdges);

                    break;

                case KruskalType.TreeUFHeapSort:

                    //Union Find implementado com arvores
                    unionFind = new UnionFindT(this.numberOfVertices);

                    //Faz o sorting com o HeapSort (in place)
                    Sorting.Heap<Edge>.HeapSort(ref graphEdges);

                    break;

                case KruskalType.LinkedListUFCountingSort:

                    //Union Find implementado com listas encadeadas
                    unionFind = new UnionFindLL(this.numberOfVertices);

                    //Faz o sorting com o CountingSort
                    graphEdges = Sorting.Sorting<Edge>.CountingSort(graphEdges, this.maxWeight);

                    break;

                case KruskalType.TreeUFCountingSort:

                    //Union Find implementado com arvores
                    unionFind = new UnionFindT(this.numberOfVertices);

                    //Faz o sorting com o CountingSort
                    graphEdges = Sorting.Sorting<Edge>.CountingSort(graphEdges, this.maxWeight);

                    break;

                default:

                    throw new ArgumentException("Tipo de Kruskal não especificado.");
            }

            //Percorre a lista ordenada de Arestas. 
            //Termina o looping quando todos os vértices já tiverem sido colocados na arvore geradora mínima.
            int i = 1;
            int j = 0;
            while (i < this.numberOfVertices)
            {
                //Decobre os conjuntos aos quais os vértices pertencem
                int set1 = unionFind.Find(graphEdges[j].Item2.vertexTo);
                int set2 = unionFind.Find(graphEdges[j].Item2.vertexFrom);

                //Verifica se os vértices pertencem ao mesmo grupo (forma ciclo)
                if (set1 != set2)
                {
                    //Soma o risco da aresta ao risco total da arvore geradora mínima
                    minimumSpaningTreeCost += graphEdges[j].Item1;
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
            //Declaração de variáveis auxiliares que serão usadas globalmente
            int initialVertex = 1;
            int minimumSpaningTreeCost = 0;
            Edge edge = new Edge();

            //Verifica qual é o tipo de implementação do Prim foi escolhida e executa as ações condizentes       
            switch (implementationType)
            {
                case PrimType.PQEdge:
                    //Prim com fila de prioridades sobre as arestas

                    //Vetor boolean com n + 1 posições que controla quais vértices já foram explorados. O vetor é inicializado com todas as posições em false.
                    bool[] explored = Enumerable.Repeat(false, numberOfVertices + 1).ToArray();

                    //Declaração de variável que representa o heap
                    var heap = new Heap<Edge>();

                    //Marca o vértice inicial como explorado
                    explored[initialVertex] = true;

                    //Coloca no heap as arestas incidentes sobre o vértice inicial
                    foreach (var vertex in adjacencyLists[initialVertex])
                    {
                        if (explored[vertex.key] == false)
                        {
                            edge.vertexFrom = initialVertex;
                            edge.vertexTo = vertex.key;
                            heap.HeapAdd(vertex.weight, edge);
                        }
                    }

                    //Enquanto o heap não estiver vazio
                    while (heap.HeapSize() > 0 )
                    {
                        //Extrai a menor aresta do heap. A aresta vem encapsulada com peso e índice.
                        var extractedEdge = heap.HeapExtractMin();
                        int edgeWeight = extractedEdge.Item1;
                        edge = extractedEdge.Item3;
                        
                        //Verifica qual vértice ainda não foi explorado
                        int unexploredVertex = 0;
                        
                        if (explored[edge.vertexFrom] == false)                            
                            unexploredVertex = edge.vertexFrom;
                        
                        else if (explored[edge.vertexTo] == false)
                            unexploredVertex = edge.vertexTo;

                        //Se os dois vértices já foram explorados não faz nada
                        if (unexploredVertex != 0)
                        {
                            minimumSpaningTreeCost += edgeWeight;
                            explored[unexploredVertex] = true;

                            //Adiciona as arestas na vizinhaca do vértice no heap
                            foreach (var vertex in adjacencyLists[unexploredVertex])
                            {
                                //Se o vértice de destino já foi explorado antão não coloca a aresta no heap
                                if (explored[vertex.key] != true)
                                {
                                    edge.vertexFrom = unexploredVertex;
                                    edge.vertexTo = vertex.key;
                                    heap.HeapAdd(vertex.weight, edge);
                                }
                            }
                        }
                    }                  

                    break;

                case PrimType.PQVertex:

                    //Prim com fila de prioridades sobre os vértices

                    //Inicializa a distância de todos os vértices em infinito, salvo o primeiro, que é igual a zero.
                    //Tupla: Item1 - Distância do vértice para o grupo (prioridade no heap); Item2 - Índice do vértice; Item3 - Conteúdo do vértice.
                    //A lista tem esse formato para que possa se passada como parâmetro para o heap
                    var initialDistances = Enumerable.Range(1, numberOfVertices).Select(a => new Tuple<int, int, int>(int.MaxValue, a, a)).ToList();
                    initialDistances[0] = new Tuple<int, int, int>(0, 1, 1);

                    //Cria o heap com as distancias iniciais para os vértices
                    var distHeap = new Heap<int>(initialDistances);

                    while (distHeap.HeapSize() > 0)
                    {
                        var extractedVertex = distHeap.HeapExtractMin();

                        //Incrementa o custa da arvore geradora mínima com o valor mínimo extraido do heap
                        minimumSpaningTreeCost += extractedVertex.Item1;

                        //Para cada vértice adjacente aquele que foi retirado do heap
                        foreach (var vertex in adjacencyLists[extractedVertex.Item2])
                        {
                            int vertexKey = distHeap.HeapGetKey(vertex.key);
                            //Verifica se sua distância do grupo dimunuiu
                            if ((vertexKey > 0) && (vertexKey > vertex.weight))
                            {
                                distHeap.HeapChangeKey(vertex.weight, vertex.key);
                            }
                        }
                    }

                    break;
              
                default:

                    throw new ArgumentException("Tipo de Prim não especificado.");
            }
            

            return minimumSpaningTreeCost;
        }

        #endregion
    }
}
