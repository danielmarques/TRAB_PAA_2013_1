using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
    public class Sorting<TValue>
    {

        public static void HeapSort(ref List<Tuple<int, TValue>> priorityQueue)
        {

            Heapfy(ref priorityQueue);

            int size = priorityQueue.Count;

            for (int i = size-1; i >= 0; i--)
            {
                swapElements(ref priorityQueue, 0, i);
                size--;
                heapfyDown(ref priorityQueue, size, 0);
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
                heapfyDown(ref priorityQueue, heapLenght, i);
            }
        }

        /// <summary>
        /// Compara a prioridade de um elemento do Heap Max com seus filhos e troca posições quando necessário.
        /// Faz isso recusrsivamente até que o elemento esteja na posição correta no Heap Min, ou seja, sua prioridade é menor do que a dos filhos.
        /// </summary>
        /// <param name="priorityQueue">Lista de prioridades de onde o elemento faz parte</param>
        /// <param name="elementPosition">Posição do elemento na lista de prioridades</param>
        /// <returns></returns>
        private static void heapfyDown(ref List<Tuple<int, TValue>> priorityQueue, int listSize, int elementPosition)
        {


            int leftChild = elementPosition * 2 + 1;
            int rightChild = elementPosition * 2 + 2;
            int bigger = 0;

            if (leftChild < listSize && priorityQueue[leftChild].Item1 > priorityQueue[elementPosition].Item1)
                bigger = leftChild;
            else
                bigger = elementPosition;

            if (rightChild < listSize && priorityQueue[rightChild].Item1 > priorityQueue[bigger].Item1)
                bigger = rightChild;


            if (bigger != elementPosition)
            {
                swapElements(ref priorityQueue, bigger, elementPosition);
                heapfyDown(ref priorityQueue, listSize, bigger);
            }


        }

        /// <summary>
        /// Troca dois elementos de posição em uma lista de prioridades
        /// </summary>
        /// <param name="priorityQueue">Lista de prioridades</param>
        /// <param name="elementPosition1">Posição do primeiro elemento</param>
        /// <param name="elementPosition2">Posição do segundo elemento</param>
        /// <returns></returns>
        private static void swapElements(ref List<Tuple<int, TValue>> priorityQueue, int elementPosition1, int elementPosition2)
        {

            var tempElement = priorityQueue[elementPosition1];
            priorityQueue[elementPosition1] = priorityQueue[elementPosition2];
            priorityQueue[elementPosition2] = tempElement;

        }

    }
}
