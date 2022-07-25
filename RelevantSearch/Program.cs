using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Program
{
    static void Main()
    {
        var documentsCount = int.Parse(Console.ReadLine()!);
        var storage = new Dictionary<string, Dictionary<int, int>>();
        for (int i = 0; i < documentsCount; i++)
        {
            var input = Console.ReadLine()!.Split();
            foreach (var item in input)
            {
                if (!storage.ContainsKey(item))
                    storage[item] = new Dictionary<int, int>();
                if (!storage[item].ContainsKey(i))
                    storage[item][i] = 0;
                storage[item][i]++;
            }
        }
        var requestsCount = int.Parse(Console.ReadLine()!);
        for (int i = 0; i < requestsCount; i++)
        {
            var indexScoreDict = new Dictionary<int, int>();

            var inputWords = new HashSet<string>(Console.ReadLine()!.Split()); // This constructor is an O(n) operation, where n is the number of elements in the collection parameter
            foreach (var item in inputWords)
            {
                if (storage.ContainsKey(item)) // O(1)
                {
                    foreach (var storageItem in storage[item])
                    {
                        if (!indexScoreDict.TryAdd(storageItem.Key, storageItem.Value))
                        {
                            indexScoreDict[storageItem.Key] += storageItem.Value;
                        }
                    }

                }
            }

            Console.WriteLine(string.Join(" ", indexScoreDict.OrderByDescending(i => i.Value).ThenBy(i => i.Key).Take(5).Select(i => i.Key + 1)));
        }
    }
}
