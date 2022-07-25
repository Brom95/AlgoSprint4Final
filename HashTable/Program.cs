// https://contest.yandex.ru/contest/24414/run-report/69483811/
/*
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
    
*/
using System;
using System.Collections;

class Program
{
    static void Main()
    {
        var commandsCount = int.Parse(Console.ReadLine()!);
        var table = new MyHashTable();
        for (int i = 0; i < commandsCount; i++)
        {
            var input = Console.ReadLine();
            var result = table.ExecuteCommand(input!);
            if (result is not null)
            {
                Console.WriteLine(result);
            }
        }
    }
}

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
        var node = storage[basket]!;
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


        var node = FindOrLast(basket, key);
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
    private string GetFromBasket(int basket, string key)
    {
        if (storage[basket] is null)
            return "None";
        var node = FindOrLast(basket, key);
        return (node.Key == key) ? node.Value : "None";
    }
    private string DeleteFromBasket(int basket, string key)
    {
        if (storage[basket] is null)
            return "None";

        var node = storage[basket]!;
        while (node.Key != key && node.Next?.Key != key && node.Next is not null)
        {
            node = node.Next;
        }
        if (node.Key == key)
        {
            storage[basket] = node.Next;
            return node.Value;
        }
        if (node.Next?.Key == key)
        {
            var result = node.Next.Value;
            node.Next = node.Next?.Next ?? null;
            return result;
        }
        return "None";
    }
    public void Put(string key, string value) => PutToBasket(GetBasket(key), key, value);

    public string Get(string key) => GetFromBasket(GetBasket(key), key);
    public string Delete(string key) => DeleteFromBasket(GetBasket(key), key);

}

public static class HashTableExtension
{
    public static string? ExecuteCommand(this MyHashTable table, string command)
    {
        var commandArr = command.Split();
        switch (commandArr[0])
        {
            case "put":
                table.Put(commandArr[1], commandArr[2]);
                return null;
            case "get":
                return table.Get(commandArr[1]);
            case "delete":
                return table.Delete(commandArr[1]);

        }
        return null;
    }
}
