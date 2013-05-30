﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{     

    public class Graph
    {
        /// <summary>
        /// Estrutura de dados que representará um vértice na lista de adjacências do grafo. 
        /// Cada vértice posicionado na adjacência de outro tem um identificador (key) e um peso (weight).
        /// O peso refer-se à aresta que liga os dois vértices.
        /// </summary>
        public struct Vertex
        {
            public int key;
            public int weight;

            public Vertex(int key, int weight)
            {
                this.key = key;
                this.weight = weight;
            }
        }      

        #region Properties

        /// <summary>
        /// Número de Vértices do grafo.
        /// </summary>
        private int numberOfVertexes;
        public int NumberOfVertexes
        {
            get { return numberOfVertexes; }
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
       /// A lista de adjacências do grafo utiliza a classe Dictionary e a interface ICollection, ambas implementadas pelo C#.
       /// O Dictionary cria um mapeamento (Chave, Valor) e prove métodos que dão suporte à essa coleção de ítens.
       /// O ICollecion é uma interface genérica que define métodos para manipular coleções de ítens. Para implementar essa interface foi utilizada a classe List do C#.
       /// A Lista de adjacências do grafo é então uma lista (Dictionary) de coleções (ICollection) de vértices (vertex).
       /// </summary>
        private Dictionary<int, ICollection<Vertex>> adjacencyLists;

        /// <summary>
        /// Lista de Arestas do grafo
        /// </summary>
        private ICollection<IEnumerable<int>> edgeList;

        public ICollection<IEnumerable<int>> EdgeList
        {
            get { return edgeList ; }
            
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Construtor da classe Grafo. Inicializa a lista de adjacências do grafo.
        /// </summary>
        /// <param name="inputList">
        ///     Lista de números no formato das instâsncias de teste:
        ///     [número de vértices]
        ///     [aresta 1, vértice 1] [aresta 1, vértice 2] [risco aresta 1] ...
        /// </param>
        public Graph(ICollection<IEnumerable<int>> inputList)
        {
            this.adjacencyLists = new Dictionary<int, ICollection<Vertex>>();

            //Cria uma nova lista contendo apenas as arestas.
            var listOfEdges = inputList.Skip(1);

            //Calcula o número de vértices do grafo
            this.numberOfVertexes =  inputList.First().First();
            
            //Calcula o número de arestas do grafo
            this.numberOfEdges = inputList.Count - 1;

            
            //Constrou a lista de adjacências do grafo.
            //Para cada aresta do grafo, coloca a aresta na lista de adjacências.
            foreach (var edge in listOfEdges)
            {
                //Edge: 0 - Vértice de saída, 1 - Vértice de chegada, 2 - Peso da aresta
                this.setEdge(edge.ElementAt(0), edge.ElementAt(1), edge.ElementAt(2));

            }           
        }

        /// <summary>
        /// Método que adiciona uma nova aresta à lista de adjacências do grafo.
        /// </summary>
        /// <param name="fromVertex">Vértice de partida da aresta.</param>
        /// <param name="toVertex">Vértice de chagada da aresta.</param>
        /// <param name="edgeWeight">Peso da aresta.</param>
        public void setEdge(int fromVertex, int toVertex, int edgeWeight)
        {
            Vertex v = new Vertex(toVertex, edgeWeight);

            //Verifica se o vértice de chegada já existe na lista de adjacências. Se não existir, cria uma posição para o mesmo. 
            if (!adjacencyLists.ContainsKey(toVertex))
            {
                var adj = new List<Vertex>();
                adjacencyLists.Add(toVertex, adj);
            }

            //Verifica se o vértice de saída já existe na lista de adjacências.
            //Se existe então simplesmente coloca mais um vértice em sua adjacência.
            //Senão cria uma posição para o vértice de partida com o vértice de chegada em sua adjacência.
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

        #endregion
    }    
}
