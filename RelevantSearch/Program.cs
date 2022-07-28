// https://contest.yandex.ru/contest/24414/run-report/69515501/
/*
-- ПРИНЦИП РАБОТЫ --
Я реализовал релевантный поиск по документам на основе хэш таблицы, где в качестве ключа выступает слово, а в качестве значения хештаблица, где ключами выступают id документа, а значением -- сколько раз слово представлено в данном документе. Для определения релевантных документов запрос разбивается на отдельные слова и считается сумма вхождений каждого из слов запроса в документы. Пользователю возвращаются не более 5 id документов с наибольшей суммой.

-- ДОКАЗАТЕЛЬСТВО КОРРЕКТНОСТИ --
Из описания алгоритма видно, что представленная хештаблица позволяет определять документы, в которых представлено то или иное слово, и в каких количествах. Это в свою очередь позволяет получить представление о релевантности того или иного документа для приведенного запроса.

-- ВРЕМЕННАЯ СЛОЖНОСТЬ -- 
Заполнение словаря выполняется за O(N) где N -- количество слов во всех документах 
Поиск релевантных документов в худшем случае за О(nk) где n -- количество слов в запросе, k -- количество документов. Поиск пяти максимальных элементов осуществляется за O(5k) = O(k). При выполнении M запросов сложность составит O(M(nk+k)) = O(Mk(n+1)) = O(Mnk), таким образом итоговая сложность программы составит O(N+Mnk).
-- ПРОСТРАНСТВЕННАЯ СЛОЖНОСТЬ --
В худшем случае, если все слова содержатся во всех документах, то для хранения хештаблицы потребуется O(Nk) памяти, где N -- количество слов во всех документах,  а k -- количество документов.
Для вычисления релевантных документов для каждого запроса используется O(n +k+5) = O(n+k) памяти, таким образом общее потребление пасяти O(Nk+n+k) = O(k(N+1)+n) = O(kN+n).
*/

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
