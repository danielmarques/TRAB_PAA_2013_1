using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionFind
{
    /// <summary>
    /// Classe implementa operções de união e busca (identificar conjunto) em conjuntos disjuntos. 
    /// Usa a representação por lista encadeada e union by rank
    /// </summary>
    public class UnionFindLL : IUnionFind
    {
        #region Properties

        /// <summary>
        /// Cada posição deste vetor guarda informações relativas ao elemento de mesmo índice identificador.
        /// Valores possíveis:
        ///     - Valores negativos: Significa que o elemento (cujo valor é a posição no vetor) é o representante de um conjunto. O módulo do valor é o número de elementos do conjunto.
        ///     - Valores positivos: Valor do índice (ou posição no vetor) do representante do conjunto daquele elemento.
        /// </summary>
        private List<int> setsFrameworks;

        /// <summary>
        /// Vetor que guarda para cada elemento quem é o próximo elemento na sua lista encadeada.
        /// </summary>
        private List<int> nextElement;

        /// <summary>
        /// Vetor que guarda o valor a posição da cauda de cada lista encadeada.
        /// Este valor é guardado na posição realtiva ao representante da lista.
        /// </summary>
        private List<int> setsTails;

        #endregion
        
        #region Constructor

        /// <summary>
        /// O Union Find (UF) considera que cada elemento possui um índice identificador. Este indice deve ser um número inteiro positivo de 1 a n.
        /// Aqui usaremos o UF para manipular n vértices de um grafo, portanto inicialmente existiram tantos conjuntos quanto vértices (ou seja n).
        /// Além disso, neste caso usaremos o próprio valor dos vértices como índice.
        /// </summary>
        /// <param name="numberOfSets"></param>
        public UnionFindLL(int numberOfSets)
        {
            //Inicializa o vetor auxiliar do Union Fint para a quantidade inicial de conjuntos.
            //A posição zero não será usada, por tanto o vetor deve ter uma posição a mais.
            //Inicialmente, cada elemento é representante de seu próprio conjunto (por isso recebe o valor -1).
            this.setsFrameworks = Enumerable.Repeat(-1, numberOfSets + 1).ToList();

            //Inicializa o vetor auxiliar de proximo elemento da lista encadeada.
            //Inicialmente os elementos não tem um próximo (recebem -1).
            this.nextElement = Enumerable.Repeat(-1, numberOfSets + 1).ToList();

            //Inicializa a lista de caldas das listas encadeadas
            //Inicailamente cada elemento é a clada de sua propria lista
            this.setsTails = Enumerable.Range(0, numberOfSets + 1).ToList();
        }

        #endregion
       
        #region Methods

        /// <summary>
        /// Une dois conjuntos.
        /// </summary>
        /// <param name="firstSetRepresentative">Representante do primeiro conjunto.</param>
        /// <param name="secondSetRepresentative">Representante do segundo conjunto.</param>
        public void Union(int firstSetRepresentative, int secondSetRepresentative)
        {
            int smallerSetRepresentative;
            int biggerSetRepresentative;

            //Verifica se os conjuntos são iguais
            if (firstSetRepresentative == secondSetRepresentative)
            {
                throw new ArgumentException("Os conjuntos são iguais.");
            }

            //Verifica se os argumentos realmente se referem à representantes de conjuntos
            if ((setsFrameworks[firstSetRepresentative]>=0)||(setsFrameworks[secondSetRepresentative]>=0))
            {
                throw new ArgumentException("Os argumentos não são representantes de conjuntos.");                
            }

            //Determina que é o menor e o maior conjunto
            if (Math.Abs(setsFrameworks[firstSetRepresentative]) < Math.Abs(setsFrameworks[secondSetRepresentative]))
            {
                smallerSetRepresentative = firstSetRepresentative;
                biggerSetRepresentative = secondSetRepresentative;
            }
            else
	        {
                smallerSetRepresentative = secondSetRepresentative;
                biggerSetRepresentative = firstSetRepresentative;
	        }

            //A cauda do conjunto maior aponta para o ex-representante do conjunto menor
            nextElement[setsTails[biggerSetRepresentative]] = smallerSetRepresentative;

            //A cauda do conjunto maior passa a ser a cauda do conjunto menor
            setsTails[biggerSetRepresentative] = setsTails[smallerSetRepresentative];

            //Os elementos do conjunto menor passam a apontar para o representante do conjunto maior
            //O looping começa a partir do ex-representante do conjunto menor e passa de um elemento para o outro com a ajuda do vetor nextElement
            //Quando o valor do índice ficar negativo significa que chegou na cauda do conjunto
            int i = smallerSetRepresentative;
            while (i > 0)
            {
                //O elemento aponta para o novo representante
                setsFrameworks[i] = biggerSetRepresentative;

                //Incrementa a quantidade de elementos do conjunto
                setsFrameworks[biggerSetRepresentative]--;

                //Passa para o proximo elemento
                i = nextElement[i];
            }

        }

        /// <summary>
        /// Retorna o índice (ou posição) do representande do conjunto do qual o elemento faz parte.
        /// </summary>
        /// <param name="element">Índice (ou posição) do elemento cujo conjunto desejamos saber.</param>
        /// <returns></returns>
        public int Find(int element)
        {
            //O vetor setsFrameworks guarda para cada elemento (na posição do vetor denotada pelo índice do elemento) o valor (ou índice) do representante do conjunto daquele elemento.
            if (setsFrameworks[element] > 0)
            {
                //Se o valor é positivo então é o valor do representante do conjunto
                return setsFrameworks[element];
            }
            else
            {
                //Se o valor é negativo então o elemento é o representante de seu próprio conjunto
                return element;
            }    
        }

        #endregion
    }

    /// <summary>
    /// Classe implementa operções de união e busca (identificar conjunto) em conjuntos disjuntos. 
    /// Usa a representação por arvore (Path comnpression)
    /// </summary>
    public class UnionFindT : IUnionFind
    {
        #region Properties

        /// <summary>
        /// Vetor que guarda, para cada elemento, quem é seu pai na arvore onde o mesmo se encontra.
        /// Uma raiz de arvore te um valor negativo (de módulo igual a altura da arvore). Consideraremos que uma arvore com um único elemento tem altura 1.
        /// </summary>
        private List<int> father;

        #endregion

        #region Constructor

        /// <summary>
        /// Construtor da classe UnionFindT.
        /// O Union Find (UF) considera que cada elemento possui um índice identificador. Este indice deve ser um número inteiro positivo de 1 a n.
        /// Aqui usaremos o UF para manipular n vértices de um grafo, portanto inicialmente existiram tantos conjuntos quanto vértices (ou seja n).
        /// Além disso, neste caso usaremos o próprio valor dos vértices como índice.
        /// </summary>
        /// <param name="numberOfSets">Número de conjuntos que serão criados</param>
        public UnionFindT(int numberOfSets)
        {
            /// Inicializa o vetor auxiliar father para cada elemento.
            /// Na inicialização cada elemento constitui um conjunto (e será a raíz de sua propria arvore).
            /// Para indicar que o elemento é a raiz da arvora, coloca-se o pai do mesmo como sendo -1.
            /// A posição zero não será usada, por tanto o vetor deve ter uma posição a mais.
            this.father = Enumerable.Repeat(-1, numberOfSets + 1).ToList();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Une dois conjuntos.
        /// </summary>
        /// <param name="firstSetRepresentative">Índice (ou posição) do representante do primeiro grupo.</param>
        /// <param name="secondSetRepresentative">Índice (ou posição) do representante do segundo grupo.</param>
        public void Union(int firstSetRepresentative, int secondSetRepresentative)
        {
            //Verifica se os conjuntos são iguais
            if (firstSetRepresentative == secondSetRepresentative)
            {
                throw new ArgumentException("Os conjuntos são iguais.");
            }

            //Verifica se os argumentos realmente se referem à representantes de conjuntos
            if ((father[firstSetRepresentative] >= 0) || (father[secondSetRepresentative] >= 0))
            {
                throw new ArgumentException("Os argumentos não são representantes de conjuntos.");
            }

            //Variáveis auxiliares para armazenar as raízes das arvores
            int smallTreeRoot;
            int bigTreeRoot;

            //Recupera as alturas das arvores
            //Note que o tamanho das arvores é gusrdado no vetor father, na posição da raíz, com sinal negativo.
            int firstTreeHeight = Math.Abs(father[firstSetRepresentative]);
            int secondTreeHeight = Math.Abs(father[secondSetRepresentative]);

            //Como as estruturas usadas para representar os conjuntos são arvores, para unir os memos fazemos a raíz da arvore menor apontar para raíz da arvore maior.
            //Com isso o tamanho da arvore resultante é sempre igual ao da arvore maior.

            //Compara as alturas das árvores
            if (firstTreeHeight > secondTreeHeight)
            {
                bigTreeRoot = firstSetRepresentative;
                smallTreeRoot = secondSetRepresentative;
            }
            else
            {
                bigTreeRoot = secondSetRepresentative;
                smallTreeRoot = firstSetRepresentative;
            }

            //Se as árvores na verdade tinham o mesmo tamanho, então a árvore resultante  terá o tamanho acrescido em 1.
            if (firstTreeHeight == secondTreeHeight)
            {
                //Note que o tamanho das arvores é gusrdado no vetor father, na posição da raíz, com sinal negativo.
                father[bigTreeRoot]--;
            }

            //A raiz da arvore menor aponta para raíz da arvore maior.
            father[smallTreeRoot] = bigTreeRoot;

        }

        /// <summary>
        /// Retorna o índice (ou posição) do representante do conjunto do qual o elemento faz parte.
        /// </summary>
        /// <param name="element">Índice (ou posição) do elemento cujo conjunto desejamos saber.</param>
        /// <returns></returns>
        public int Find(int element)
        {
            //Os conjuntos são arvores onde o representante do conjunto é a raiz da arvore.
            //As raizes não possuel um pai (o valor do pai é menor do que zero).
            //Verifica se o pai do elemento atual é a raíz da arvore.
            if (father[element] < 0)
            {
                //É a raiz da arvore (representante do conjunto).
                return element;
            }
            else
            {             
                //Busca recusivamente pela raiz da arvore
                int root = Find(father[element]);

                //Path Compression
                father[element] = root;

                //Repassa a referência à raiz até sair da recurssão.
                return root;
            }
        }

        #endregion
    }
}
