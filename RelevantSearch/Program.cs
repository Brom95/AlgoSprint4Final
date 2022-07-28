// https://contest.yandex.ru/contest/24414/run-report/69515501/


/* 
Заполнение словаря выполняется за O(n) где n -- количество слов во всех документах 
Поиск релевантных документов в худшем случае за О(nk) где n -- количество слов в запросе, k -- количество документов. 
*/
// FIX Не учитывается кол-во запросов. Нет четкого разделения на оценку скорости и памяти, задача оформлена совсем не по шаблону.
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
            var indexScoreArray = new int[documentsCount];

            var inputWords = new HashSet<string>(Console.ReadLine()!.Split()); // This constructor is an O(n) operation, where n is the number of elements in the collection parameter
            foreach (var item in inputWords)
            {
                if (storage.ContainsKey(item)) // O(1)
                {
                    foreach (var storageItem in storage[item])
                    {

                        indexScoreArray[storageItem.Key] += storageItem.Value;

                    }

                }
            }

            var maxIndexes = new List<int>();
            for (var j = 0; j < 5; j++)
            {
                int maxValue = -5;
                int? maxIndex = null;
                for (var k = 0; k < documentsCount; k++)
                {
                    if (indexScoreArray[k] > maxValue && indexScoreArray[k] > 0)
                    {
                        maxValue = indexScoreArray[k];
                        maxIndex = k;
                    }
                }
                if (maxIndex is not null)
                {
                    maxIndexes.Add((int)maxIndex + 1);
                    indexScoreArray[(int)maxIndex] = 0;
                    continue;
                }
                break;

            }
            Console.WriteLine(string.Join(" ", maxIndexes));
        }
    }
}
