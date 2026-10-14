using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T> // ensures T implements IHeapItem
{
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void AddItem(T _item)
    {
        _item.HeapIndex = currentItemCount;
        ietms[currentItemCount] = _item;
        SortUp(_item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--; // Reduce count of items in heap

        items[0] = items[currentItemCount]; // Move last item to top
        items[0].HeapIndex = 0; // Update heap index as now first
        SortDown(items[0]); // Gp down graph to find where this item now belongs

        return firstItem;
    }

    public void UpdateItem(T _item)
    {
        SortUp(_item); // e.g. f cost changed when new path found to item so should be moved up graph
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(T _item)
    {
        return Equals(items[_item.HeapIndex], _item); // Check if item in heap at searched for item's heap index matches item being searched for
    }

    void SortDown(T _item)
    {
        while (true)
        {
            int childIndexLeft = (_item.HeapIndex * 2) + 1;// Maths expained for heap index calculatons in https://www.youtube.com/watch?v=3Dw5d7PlcTM
            int childIndexRight = (_item.HeapIndex * 2) + 2;
            int swapIndex = 0;

            // Check object has any children (check left)
            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                // Check if has child to swap with to thr right
                if (childIndexRight < currentItemCount)
                {
                    // Check priorities of children to determine which way to swap
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)  // If left child has lower priority, should swap right instead
                    {
                        swapIndex = childIndexRight;
                    }
                }

                // Found child for swap check, now check if swap needed
                if (items.CompareTo(items[swapIndex]) < 0)
                    Swap(_item, items[swapIndex]);
                else // Has higher priority than child so should remain 
                    return;
            }

            // Has no children
            else
                return; // No swap as no children
        }
    }

    void SortUp(T _item)
    {
        int parentIndex = (_item.HeapIndex - 1) / 2; // Maths expained for heap index calculatons in https://www.youtube.com/watch?v=3Dw5d7PlcTM

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0) // Higher priority returns 1, same returns 0, lower returns - 1
            {
                Swap(items, parentItem);
            }
            else // Not higher priority than parent
                break; // Stop going up loop checking for swaps
        }
    }

    void Swap(T _itemA, T _itemB)
    {
        // Swap items in array
        items[_itemA.HeapIndex] = _itemB;
        items[_itemB.HeapIndex] = _itemA;

        // Swap heap indexes
        int itemAIndex = _itemA.HeapIndex;
        _itemA.HeapIndex = _itemB.HeapIndex;
        _itemB.HeapIndex = itemAIndex;

    }
}

public interface IHeapItem<T>: IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
