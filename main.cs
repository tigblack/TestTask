using System;
using System.IO;

class ListNode
{
    public ListNode Prev;
    public ListNode Next;
    public ListNode Rand; // произвольный элемент внутри списка
    public string Data;
}

class ListRand
{
    public ListNode Head;
    public ListNode Tail;
    public int Count;

    public void Serialize(FileStream s)
    {
        using (var writer = new BinaryWriter(s))
        {
            var node = Head;
            while (node != null)
            {
                writer.Write(node.Data);
                writer.Write(CountNode(node.Rand));
                node = node.Next;
            }

            node = Head;
            while (node != null)
            {
                writer.Write(GetNodeIndex(node.Rand));
                node = node.Next;
            }
        }
    }

    public void Deserialize(FileStream s)
    {
        using (var reader = new BinaryReader(s))
        {
            var nodeDict = new Dictionary<int, ListNode>();
            var nodeCount = reader.BaseStream.Length / (sizeof(int) * 2 + sizeof(char) * 256);

            for (int i = 0; i < nodeCount; i++)
            {
                var data = reader.ReadString();
                var randCount = reader.ReadInt32();
                var listNode = new ListNode { Data = data };
                nodeDict[i] = listNode;

                if (Head == null)
                {
                    Head = listNode;
                    Tail = listNode;
                }
                else
                {
                    Tail.Next = listNode;
                    listNode.Prev = Tail;
                    Tail = listNode;
                }

                for (int j = 0; j < randCount; j++)
                {
                    var randIndex = reader.ReadInt32();
                    listNode.Rand = nodeDict[randIndex];
                }
            }
        }
    }

    private int CountNode(ListNode node)
    {
        var count = 0;
        var temp = Head;

        while (temp != null)
        {
            if (temp == node)
            {
                return count;
            }

            count++;
            temp = temp.Next;
        }

        return -1;
    }

    private int GetNodeIndex(ListNode node)
    {
        var index = 0;
        var temp = Head;

        while (temp != null)
        {
            if (temp == node)
            {
                return index;
            }

            index++;
            temp = temp.Next;
        }

        return -1;
    }
}
