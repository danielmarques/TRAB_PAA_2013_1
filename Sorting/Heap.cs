using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
    public class Heap<TValue>
    {
        private int heapLength;
        
        /// <summary>
        /// Recebe uma lista de prioridades reordena a mesma na forma de Heap Max in place.
        /// </summary>
        /// <param name="priorityQueue">Lista de prioridades</param>
        public Heap(ref List<Tuple<int, TValue>> priorityQueue)
        {
          
            //Recupera o tamanho da lista
            int heapLenght = priorityQueue.Count;

            //Executa o heapfyDown para cada elemento a partir da metade da lista para o primeiro
            //Cria o heap em O(n)
            for (int i = (int) Math.Ceiling( heapLenght / 2.0 ) ; i >= 0; i--)
            {
                heapfyDown(ref priorityQueue, i);
            }            
        }

        /// <summary>
        /// Compara a prioridade de um elemento do Heap Max com seus filhos e troca posições quando necessário.
        /// Faz isso recusrsivamente até que o elemento esteja na posição correta no Heap Min, ou seja, sua prioridade é menor do que a dos filhos.
        /// </summary>
        /// <param name="priorityQueue">Lista de prioridades de onde o elemento faz parte</param>
        /// <param name="elementPosition">Posição do elemento na lista de prioridades</param>
        /// <returns></returns>
        private Boolean heapfyDown(ref List<Tuple<int, TValue>> priorityQueue, int elementPosition)
        {
            //Verifica se realmente existe um elemento aquela posição na lista
            if ((priorityQueue.Count - 1) < elementPosition)
            {
                return false;
            }

            //A prioridade do elemento atual recebe o valor armazenado na fila de prioridades para a posição indicada
            //Item1 = Prioridade, Item2 = valor ou conteúdo do elemento em si (que é genérico - TValue)
            int currentElementPriority = priorityQueue[elementPosition].Item1;

            //Inicializa e tenta copiar a prioridade do primeiro filho
            int childPriority1 = -1;
            if ((priorityQueue.Count - 1) >= ((2 * elementPosition) + 1))
            {
                childPriority1 = priorityQueue[(2 * elementPosition) + 1].Item1;
            }

            //Inicializa e tenta copiar a prioridade do segundo filho
            int childPriority2 = -1;
            if ((priorityQueue.Count - 1) >= ((2 * elementPosition) + 2))
            {
                childPriority2 = priorityQueue[(2 * elementPosition) + 2].Item1;
            }

            //Inicialização de variáveis auxiliares para lidar com o filho de menor prioridade
            int minChildPriority = -1;
            int minChildPosition = -1;

            //Verifica e o elemento tem os dois filhos
            if ((childPriority1 > -1) && (childPriority2 > -1))
            {
                //Verifica qual é o menor filho
                //Guarda a prioridade e posição do menor filho
                if (childPriority1 < childPriority2)
                {
                    minChildPriority = childPriority1;
                    minChildPosition = (2 * elementPosition) + 1;
                }
                else
                {
                    minChildPriority = childPriority2;
                    minChildPosition = (2 * elementPosition) + 2;
                }
            } 
            //Se possui apenas o primeiro filho então ele é o menor filho
            else if (childPriority1 > -1)
            {
                    minChildPriority = childPriority1;
                    minChildPosition = (2 * elementPosition) + 1;
            }
            //Se possui apenas o segundo filho então ele é o menor filho
            else if (childPriority2 > -1)
            {
                minChildPriority = childPriority2;
                minChildPosition = (2 * elementPosition) + 2;
            } 

            //Verifica se a prioridade do elemento é maior que a do filho de menor prioridade
            if ((currentElementPriority > minChildPriority)&&(minChildPriority!=-1))
            {
                //Se for troca o elemento com o filho de menor prioridade e chama o heapfyDown para a nova posição
                swapElements(ref priorityQueue, elementPosition, minChildPosition);
                heapfyDown(ref priorityQueue, minChildPosition);
            }

            return true;
        }

        /// <summary>
        /// Troca dois elementos de posição em uma lista de prioridades
        /// </summary>
        /// <param name="priorityQueue">Lista de prioridades</param>
        /// <param name="elementPosition1">Posição do primeiro elemento</param>
        /// <param name="elementPosition2">Posição do segundo elemento</param>
        /// <returns></returns>
        private Boolean swapElements(ref List<Tuple<int, TValue>> priorityQueue, int elementPosition1, int elementPosition2)
        {
            //Verifica se realmente existem elementos aquelas posições da lista
            if (((priorityQueue.Count - 1) < elementPosition1) || ((priorityQueue.Count - 1) < elementPosition2))
            {
                return false;
            }

            var tempElement = priorityQueue[elementPosition1];
            priorityQueue[elementPosition1] = priorityQueue[elementPosition2];
            priorityQueue[elementPosition2] = tempElement;

            return true;
        }

        /// <summary>
        /// Retorna o elemento raíz do heap
        /// </summary>
        /// <param name="priorityQueue">Lista de prioridades de onde a raíz será extraída.</param>
        /// <returns></returns>
        public Tuple<int, TValue> extractRoot(ref List<Tuple<int, TValue>> priorityQueue)
        {
            //Verifica se existe um elemento raíz
            if (priorityQueue.Count == 0)
            {
                return null;
            }

            //Guarda o elemento raíz
            var root = priorityQueue.First();

            //Troca a raíz com o último elemento
            swapElements(ref priorityQueue, priorityQueue.Count - 1, 0);

            //Exclui o último elemento da lista de prioridades
            priorityQueue.RemoveAt(priorityQueue.Count - 1);

            //Restaura o Heap Min
            heapfyDown(ref priorityQueue, 0);

            return root;
        }

    }
}
