using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Laugh.code;

public class NonRepeatingRandomSet<T>
{
    private List<T> _upcoming;
    private int _index = 0;
    public NonRepeatingRandomSet(IEnumerable<T> data)
    {
        _upcoming = new List<T>(data);
        Shuffle(_upcoming);
    }


    public T GetRandom()
    {
        if (_index >= _upcoming.Count)
        {
            Shuffle(_upcoming);
            _index = 0;
        }
        return _upcoming[_index++];
    }
    
    private static void Shuffle(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Shared.Next(n + 1);  
            (list[k], list[n]) = (list[n], list[k]);
        }  
    }
}