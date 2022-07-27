// https://contest.yandex.ru/contest/24414/run-report/69510548/
/*
-- ПРИНЦИП РАБОТЫ --
Я реализовал хештаблицу на основе массива, используя для разрещения зависимостей метод цепочек. При добавлении элемента индекс корзины вычисляется в методе GetBasket как модуль от хэша строки по модулю размера массива. В случае, если корзина пуста, в нее добавляется голова односвязного списка (Node) в противном же случае происходит проход по односвязному списку либо до достижения хвоста списка, либо до нахождения элемента с идентичным ключом. По итогу происходит либо замена поля Value элемента односвязного списка, либо добавление нового элемента в конец списка. 
Аналогично операции удаления и получения элемента сводятся к операциям удаления и поиска элемента из односвязного списка.

-- ДОКАЗАТЕЛЬСТВО КОРРЕКТНОСТИ --

Согласно https://ru.wikipedia.org/wiki/%D0%A5%D0%B5%D1%88-%D1%82%D0%B0%D0%B1%D0%BB%D0%B8%D1%86%D0%B0 Хеш-табли́ца — это структура данных, реализующая интерфейс ассоциативного массива, а именно, она позволяет хранить пары (ключ, значение) и выполнять три операции: операцию добавления новой пары, операцию поиска и операцию удаления пары по ключу. 

--  ВРЕМЕННАЯ СЛОЖНОСТЬ --
Данная реализация хештаблицы использует метод цепочек для разрешения коллизий 
(https://practicum.yandex.ru/learn/algorithms/courses/7f101a83-9539-4599-b6e8-8645c3f31fad/sprints/49971/topics/618173c7-3c0e-4955-b88b-d7146f9ffe2e/lessons/a151c825-5a76-4ab2-a6f6-1886b1783383/), 
по этому :
    Удаление: 
        в лучшем случае -- О(1)
        в худшем -- О(n)
    Добавление:
        в лучшем случае -- О(1)
        в худшем -- О(n)
    Получение:
        в лучшем случае -- О(1)
        в худшем -- О(n)
Сложность выполнения N  комманд составит в лучшем случае О(N), в худшем O(N^2)

-- ПРОСТРАНСТВЕННАЯ СЛОЖНОСТЬ --
Для хранения ссылок на головы односвязных списков используется массив из 4801 элементов, что потребует O(4801) = O(1) памяти в лучшем случаеи O(4801+k) = O(k), где k это количество добавляемых в таблицу эллементов, в худщем случае. Для буферезированного вывода используется О(N-k), по этому итоговая пространственная сложность составит O(N-k+k) = O(N).
*/
// FIX Тут надо добавить оценку по скорости и памяти отдельно. Также напоминаю что помимо X элементов у нас есть еще N команда. Можно считать что операции выполняются за O(1)
using System;
using System.Text;

int commandsCount = int.Parse(Console.ReadLine()!);
MyHashTable table = new MyHashTable();
var sb = new StringBuilder();
for (int i = 0; i < commandsCount; i++)
{
    string? input = Console.ReadLine();
    string? result = table.ExecuteCommand(input!);
    if (result is not null)
    {
        sb.AppendLine(result);
    }
}
Console.Write(sb.ToString());


public sealed class MyHashTable
{
    private class Node
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public Node? Next { get; set; }
    }
    private const int capacity = 4801;
    private readonly Node?[] storage = new Node[capacity];

    private static int GetBasket(string key)
    {
        int hash = Math.Abs(key.GetHashCode()) % capacity;
        return hash;
    }
    private Node FindOrLast(int basket, string key)
    {
        Node node = storage[basket]!;
        while (node.Key != key && node.Next is not null)
        {
            node = node.Next;
        }
        return node;
    }
    private void PutToBasket(int basket, string key, string value)
    {
        if (storage[basket] is null)
        {

            storage[basket] = new Node { Key = key, Value = value, Next = null };
            return;
        }


        Node node = FindOrLast(basket, key);
        if (node.Key == key)
        {
            node.Value = value;
            return;
        }
        if (node.Next is null)
        {
            node.Next = new Node { Key = key, Value = value, Next = null };
        }

    }
    private int? GetFromBasket(int basket, string key)
    {
        if (storage[basket] is null)
        {
            return null;
        }

        Node node = FindOrLast(basket, key);
        return (node.Key == key) ? int.Parse(node.Value) : null;
    }
    private int? DeleteFromBasket(int basket, string key)
    {
        if (storage[basket] is null)
        {
            return null;
        }

        Node node = storage[basket]!;
        while (node.Key != key && node.Next?.Key != key && node.Next is not null)
        {
            node = node.Next;
        }
        if (node.Key == key)
        {
            storage[basket] = node.Next;
            return int.Parse(node.Value);
        }
        if (node.Next?.Key == key)
        {
            string result = node.Next.Value;
            node.Next = node.Next?.Next ?? null;
            return int.Parse(result);
        }
        return null;
    }
    public void Put(string key, string value)
    {
        PutToBasket(GetBasket(key), key, value);
    }

    public int? Get(string key)
    {
        return GetFromBasket(GetBasket(key), key);
    }

    public int? Delete(string key)
    {
        return DeleteFromBasket(GetBasket(key), key);
    }
}

public static class HashTableExtension
{
    public static string? ExecuteCommand(this MyHashTable table, string command)
    {
        string[] commandArr = command.Split();
        switch (commandArr[0])
        {
            case "put":
                table.Put(commandArr[1], commandArr[2]);
                return null;
            case "get":
                return (table.Get(commandArr[1])?.ToString() ?? "None");
            case "delete":
                return (table.Delete(commandArr[1])?.ToString() ?? "None");
            default:
                break;
        }
        return null;
    }
}
