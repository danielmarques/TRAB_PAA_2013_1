using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
    /// <summary>
    /// Classe que prove as operações de ordenação.
    /// </summary>
    /// <typeparam name="TValue">Tipo genérico dos elementos que compõem a lista de prioridades</typeparam>
    public class Sorting<TValue>
    {
        #region Heap
        /// <summary>
        /// Ordena o vetor de entrada usando o heap sort.
        /// </summary>
        /// <param name="priorityQueue">Lista de elementos com suas prioridades</param>
        public static void HeapSort(ref List<Tuple<int, TValue>> priorityQueue)
        {
            //Coloca o vetor na ordem heap (Max).
            Heapfy(ref priorityQueue);

            //Recupera o tamanho do vetor
            int size = priorityQueue.Count;

            //Sucessivamente coloca o maior elemento no final do vetor
            for (int i = size-1; i >= 0; i--)
            {
                //Troca o primeiro elemento com o último
                SwapElements(ref priorityQueue, 0, i);
                //Diminui o tamenho do vetor
                size--;
                //Restaura a propriedade do heap
                HeapfyDown(ref priorityQueue, size, 0);
            }
        }

        /// <summary>
        /// Recebe uma lista de prioridades reordena a mesma na forma de Heap Max in place.
        /// </summary>
        /// <param name="priorityQueue">Lista de prioridades</param>
        private static void Heapfy(ref List<Tuple<int, TValue>> priorityQueue)
        {
            //Recupera o tamanho da lista
            int heapLenght = priorityQueue.Count;

            //Executa o heapfyDown para cada elemento a partir da metade da lista para o primeiro
            //Cria o heap em O(n)
            for (int i = (int)Math.Ceiling(heapLenght / 2.0); i >= 0; i--)
            {
                HeapfyDown(ref priorityQueue, heapLenght, i);
            }
        }

        /// <summary>
        /// Compara a prioridade de um elemento do Heap Max com seus filhos e troca posições quando necessário.
        /// Faz isso recusrsivamente até que o elemento esteja na posição correta no Heap Min, ou seja, sua prioridade é menor do que a dos filhos.
        /// </summary>
        /// <param name="priorityQueue">Lista de prioridades de onde o elemento faz parte</param>
        /// <param name="heapSize">Tamanho do heap</param>
        /// <param name="elementPosition">Posição do elemento na lista de prioridades</param>
        /// <returns></returns>
        private static void HeapfyDown(ref List<Tuple<int, TValue>> priorityQueue, int heapSize, int elementPosition)
        {

            //Variaveis auxiliares para representar os filhos
            int leftChild = elementPosition * 2 + 1;
            int rightChild = elementPosition * 2 + 2;
            //Variavel que será usada para guardar o maior entre elemento atual, filho da direita e filho da esquerda
            int bigger = 0;

            //Verifica se o filho da esquerda é válido e se é maior do que o elemento atual
            if (leftChild < heapSize && priorityQueue[leftChild].Item1 > priorityQueue[elementPosition].Item1)
                bigger = leftChild;
            else
                bigger = elementPosition;

            //Verifica se o filho da direita é válido e se é maior do que o maior calculado no passo anterior
            if (rightChild < heapSize && priorityQueue[rightChild].Item1 > priorityQueue[bigger].Item1)
                bigger = rightChild;

            //Se o elemento não é maior do que o maior dos filhos
            if (bigger != elementPosition)
            {
                //Troca os elementos para manter a propriedade do heap entre eles
                SwapElements(ref priorityQueue, bigger, elementPosition);
                //Mantem a porpriedade do heap no nívelm ais baixo
                HeapfyDown(ref priorityQueue, heapSize, bigger);
            }


        }

        /// <summary>
        /// Troca dois elementos de posição em uma lista de prioridades
        /// </summary>
        /// <param name="priorityQueue">Lista de prioridades</param>
        /// <param name="elementPosition1">Posição do primeiro elemento</param>
        /// <param name="elementPosition2">Posição do segundo elemento</param>
        /// <returns></returns>
        private static void SwapElements(ref List<Tuple<int, TValue>> priorityQueue, int elementPosition1, int elementPosition2)
        {

            var tempElement = priorityQueue[elementPosition1];
            priorityQueue[elementPosition1] = priorityQueue[elementPosition2];
            priorityQueue[elementPosition2] = tempElement;

        }

        #endregion

        #region Counting

        /// <summary>
        /// Recebe uma lista de prioridades e retorna a mesma ordenada.
        /// </summary>
        /// <param name="priorityQueue">Lista de prioridades no formato (prioridade, elemento).</param>
        /// <param name="range">Máxima prioridade de um elemento na lista.</param>
        /// <returns></returns>
        public static List<Tuple<int, TValue>> CountingSort(List<Tuple<int, TValue>> priorityQueue, int range)
        {
            //Vetor auxiliar que será retornado ao final
            List<Tuple<int, TValue>> orderedList = new List<Tuple<int,TValue>>();

            //Legenda:
            //priorityQueue.item1 = Prioridade do elemento na lista
            //priorityQueue.item2 = Valor ou conteúdo do elemento na lista

            //Histograma do Counting Sort que será usado para guardar listas de elementos com o mesmo peso
            List<int>[] histogram = new List<int>[range+1];

            //Percorre a lista de prioridades e preenche o histograma
            for (int i = 0; i < priorityQueue.Count; i++)
			{
                //O histograma (usando a prioridade do elemento como posição) guarda (na lista daquela prioridade) o índice (posição) do elemento na lista de prioridades
                if (histogram[priorityQueue[i].Item1] == null)
                {
                    histogram[priorityQueue[i].Item1] = new List<int>();
                }

                histogram[priorityQueue[i].Item1].Add(i);
			}

            //Monsta a lista ordenada a partir da lista de entrada e do histograma
            foreach (var priority in histogram)
            {
                if (priority == null)
                    continue;

                foreach (var element in priority)
                {
                    //Coloca na lista ordenada o proximo elemento indicado pelo histograma
                    orderedList.Add(priorityQueue[element]);
                }
            }

            return orderedList;
        }

        #endregion
    }
}
