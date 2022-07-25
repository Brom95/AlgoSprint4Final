using System;
using System.Collections;

class Program
{
    static void Main(string[] args)
    {
        var commandsCount = int.Parse(Console.ReadLine());
        var table = new MyHashTable();
        for (int i = 0; i < commandsCount; i++)
        {
            var input = Console.ReadLine();
            var result = table.ExecuteCommand(input);
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
        public string Key { get; set; }
        public string Value { get; set; }
        public Node? Next { get; set; }
    }
    private const int capacity = Int32.MaxValue / 2;
    private Node?[] storage = new Node[capacity];

    private int GetBasket(string key)
    {
        int hash = key.GetHashCode();
        return ((hash > 0) ? hash : capacity + hash) % capacity;
    }
    private Node FindOrLast(int basket, string key)
    {
        var node = storage[basket];
        while (node.Key != key || node.Next is not null)
        {
            node = node.Next;
        }
        return node;
    }
    private void PutToBasket(int basket, string key, string value)
    {
        if (storage[basket] is not null)
        {
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
            return;
        }
        storage[basket] = new Node { Key = key, Value = value, Next = null };
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

        var node = storage[basket];
        while (node.Key != key && node.Next?.Key != key && node.Next is not null)
        {
            node = node.Next;
        }
        if (node.Key == key)
        {
            storage[basket] = null;
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
