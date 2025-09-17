using System.Collections.Generic;
using UnityEngine;

public static class MyUtils
{
    public static List<int> PickUnique(int n, int count)
    {
        List<int> numbers = new List<int>();
        for (int i = 0; i < n; i++) 
            numbers.Add(i);

        List<int> picked = new List<int>();

        // »Ì±â
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, numbers.Count);
            picked.Add(numbers[index]);
            numbers.RemoveAt(index); 
        }

        return picked;
    }
}
