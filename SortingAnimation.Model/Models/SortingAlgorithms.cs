using System.Collections.Generic;
using System.Linq;

namespace SortingAnimation.Model.Models
{
    /// <summary>
    /// Класс функций сортировки
    /// </summary>
    public static class SortingAlgoritgms
    {
        public static LinkedList<int[]> BubbleSort(int[] array, bool isAscending)
        {
            var clonedArray = (int[])array.Clone();

            var linkedList = new LinkedList<int[]>();

            linkedList.AddFirst((int[])clonedArray.Clone());

            for (var j = 0; j <= clonedArray.Length - 2; j++)
            {
                for (var i = 0; i <= clonedArray.Length - 2; i++)
                {
                    if (isAscending && clonedArray[i + 1] < clonedArray[i] ||
                        !isAscending && clonedArray[i + 1] > clonedArray[i])
                    {
                        (clonedArray[i + 1], clonedArray[i]) = (clonedArray[i], clonedArray[i + 1]);

                        linkedList.AddLast((int[])clonedArray.Clone());
                    }
                }
            }

            return linkedList;
        }

        public static LinkedList<int[]> CycleSort(int[] array, bool isAscending)
        {
            var clonedArray = (int[])array.Clone();

            var linkedList = new LinkedList<int[]>();

            linkedList.AddFirst((int[])clonedArray.Clone());

            for (var start = 0; start < clonedArray.Length; start++)
            {
                var item = clonedArray[start];
                var position = start;

                do
                {
                    var positionTo = clonedArray.Where((first, second) =>
                        second != start && first < item && isAscending ||
                        second != start && first > item && !isAscending).Count();

                    if (position != positionTo)
                    {
                        while (position != positionTo && item == clonedArray[positionTo])
                        {
                            positionTo++;
                        }

                        (clonedArray[positionTo], item) = (item, clonedArray[positionTo]);

                        position = positionTo;

                        linkedList.AddLast((int[])clonedArray.Clone());
                    }
                } while (start != position);
            }

            return linkedList;
        }

        public static LinkedList<int[]> GnomeSort(int[] array, bool isAscending)
        {
            var clonedArray = (int[])array.Clone();

            var linkedList = new LinkedList<int[]>();

            linkedList.AddFirst((int[])clonedArray.Clone());

            var position = 1;

            while (position < clonedArray.Length)
            {
                if (clonedArray[position].CompareTo(clonedArray[position - 1]) >= 0 && isAscending ||
                    clonedArray[position].CompareTo(clonedArray[position - 1]) <= 0 && !isAscending)
                {
                    position++;
                }
                else
                {
                    (clonedArray[position], clonedArray[position - 1]) = (clonedArray[position - 1], clonedArray[position]);

                    linkedList.AddLast((int[])clonedArray.Clone());

                    if (position > 1)
                    {
                        position--;
                    }
                }
            }

            return linkedList;
        }

        public static LinkedList<int[]> HeapSort(int[] array, bool isAscending)
        {
            var clonedArray = (int[])array.Clone();

            var linkedList = new LinkedList<int[]>();

            linkedList.AddFirst((int[])clonedArray.Clone());

            void HeapSortAdjust(int[] arrayHeapSort, int i, int m)
            {
                var temp = arrayHeapSort[i];
                var j = i * 2 + 1;
                while (j <= m)
                {
                    if (j < m)
                    {
                        if (arrayHeapSort[j] < arrayHeapSort[j + 1] && isAscending ||
                            arrayHeapSort[j] > arrayHeapSort[j + 1] && !isAscending)
                        {
                            j++;
                        }
                    }

                    if (temp < arrayHeapSort[j] && isAscending || temp > arrayHeapSort[j] && !isAscending)
                    {
                        arrayHeapSort[i] = arrayHeapSort[j];
                        i = j;
                        j = 2 * i + 1;

                        linkedList.AddLast((int[])arrayHeapSort.Clone());
                    }
                    else
                    {
                        j = m + 1;
                    }
                }

                arrayHeapSort[i] = temp;
            }

            for (var i = (clonedArray.Length - 1) / 2; i >= 0; i--)
            {
                HeapSortAdjust(clonedArray, i, clonedArray.Length - 1);
            }

            for (var i = clonedArray.Length - 1; i >= 1; i--)
            {
                (clonedArray[0], clonedArray[i]) = (clonedArray[i], clonedArray[0]);

                linkedList.AddLast((int[])clonedArray.Clone());

                HeapSortAdjust(clonedArray, 0, i - 1);
            }

            return linkedList;
        }

        public static LinkedList<int[]> InsertionSort(int[] array, bool isAscending)
        {
            var linkedList = new LinkedList<int[]>();

            var clonedArray = (int[])array.Clone();

            linkedList.AddFirst((int[])clonedArray.Clone());

            for (var i = 1; i < clonedArray.Length; i++)
            {
                var value = clonedArray[i];
                var j = i - 1;
                var isDone = false;
                do
                {
                    if (clonedArray[j] > value && isAscending || clonedArray[j] < value && !isAscending)
                    {
                        clonedArray[j + 1] = clonedArray[j];
                        j--;

                        if (j < 0)
                        {
                            isDone = true;
                        }

                        linkedList.AddLast((int[])clonedArray.Clone());
                    }
                    else
                    {
                        isDone = true;
                    }
                } while (!isDone);

                clonedArray[j + 1] = value;

                linkedList.AddLast((int[])clonedArray.Clone());
            }

            return linkedList;
        }

        public static LinkedList<int[]> QuickSort(int[] array, bool isAscending)
        {
            var linkedList = new LinkedList<int[]>();

            var clonedArray = (int[])array.Clone();

            linkedList.AddFirst((int[])clonedArray.Clone());

            void QuickSortAdjust(int[] arrayQuickSort, int left, int right)
            {
                var i = left;
                var j = right;

                double pivotValue = (left + right) / 2;

                var x = arrayQuickSort[int.Parse(pivotValue.ToString())];

                while (i <= j)
                {
                    while (arrayQuickSort[i] < x && isAscending || arrayQuickSort[i] > x && !isAscending)
                    {
                        i++;
                    }

                    while (x < arrayQuickSort[j] && isAscending || x > arrayQuickSort[j] && !isAscending)
                    {
                        j--;
                    }

                    if (i <= j)
                    {
                        var temp = arrayQuickSort[i];
                        arrayQuickSort[i] = arrayQuickSort[j];
                        i++;
                        arrayQuickSort[j] = temp;
                        j--;
                        linkedList.AddLast((int[])arrayQuickSort.Clone());
                    }
                }

                if (left < j)
                {
                    QuickSortAdjust(arrayQuickSort, left, j);
                }

                if (i < right)
                {
                    QuickSortAdjust(arrayQuickSort, i, right);
                }
            }

            QuickSortAdjust(clonedArray, 0, clonedArray.Length - 1);

            return linkedList;
        }

        public static LinkedList<int[]> MergeSort(int[] array, bool isAscending)
        {
            var linkedList = new LinkedList<int[]>();

            var clonedArray = (int[])array.Clone();

            linkedList.AddFirst((int[])clonedArray.Clone());

            void MergeSortAdjust(int[] arrayMergeSort, int low, int height)
            {
                if (low >= height)
                {
                    return;
                }

                var mid = (low + height) / 2;

                MergeSortAdjust(arrayMergeSort, low, mid);
                MergeSortAdjust(arrayMergeSort, mid + 1, height);

                var endLow = mid;
                var startHigh = mid + 1;
                while (low <= endLow && startHigh <= height)
                {
                    if (arrayMergeSort[low] < arrayMergeSort[startHigh] && isAscending ||
                        arrayMergeSort[low] > arrayMergeSort[startHigh] && !isAscending)
                    {
                        low++;
                    }
                    else
                    {
                        var temp = arrayMergeSort[startHigh];
                        for (var k = startHigh - 1; k >= low; k--)
                        {
                            arrayMergeSort[k + 1] = arrayMergeSort[k];

                            linkedList.AddLast((int[])arrayMergeSort.Clone());
                        }

                        arrayMergeSort[low] = temp;

                        linkedList.AddLast((int[])arrayMergeSort.Clone());

                        low++;
                        endLow++;
                        startHigh++;
                    }
                }
            }

            MergeSortAdjust(clonedArray, 0, clonedArray.Length - 1);

            return linkedList;
        }
    }
}