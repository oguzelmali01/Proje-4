using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace Project4
{
    class Node
    {
        public int height;
        public int value;
        public Node left, right;

        public Node(int item)
        {
            value = item;
            height = 1;
            left = right = null;
        }


    }

    class AVLTree
    {
        Node root;

        int Height(Node node)
        {
            if (node == null)
                return 0;
            return node.height;
        }

        int Max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        int getBalance(Node node)
        {
            if (node == null) return 0;
            return Height(node.left) - Height(node.right);

        }

        Node RotateRight(Node y)
        {
            Node x = y.left;
            Node T2 = x.right;

            x.right = y;
            y.left = T2;

            y.height = Max(Height(y.left), Height(y.right)) + 1;
            x.height = Max(Height(x.left), Height(x.right)) + 1;

            return x;
        }

        Node RotateLeft(Node x)
        {
            Node y = x.right;
            Node T2 = y.left;

            y.left = x;
            x.right = T2;

            x.height = Max(Height(x.left), Height(x.right)) + 1;
            y.height = Max(Height(y.left), Height(y.right)) + 1;

            return y;
        }

        Node Insert(Node node, int value)
        {
            if (node == null)
                return new Node(value);

            if (value < node.value)
                node.left = Insert(node.left, value);
            else if (value > node.value)
                node.right = Insert(node.right, value);
            else
                return node;

            node.height = 1 + Max(Height(node.left), Height(node.right));

            int balance = getBalance(node);

            if (balance > 1 && value < node.left.value)
                return RotateRight(node);

            if (balance < -1 && value > node.right.value)
                return RotateLeft(node);

            if (balance > 1 && value > node.left.value)
            {
                node.left = RotateLeft(node.left);
                return RotateRight(node);
            }

            if (balance < -1 && value < node.right.value)
            {
                node.right = RotateRight(node.right);
                return RotateLeft(node);
            }

            return node;
        }

        public void Insert(int value)
        {
            root = Insert(root, value);
        }

        void PreOrder(Node node)
        {
            if (node != null)
            {
                Console.Write(node.value + " ");
                PreOrder(node.left);
                PreOrder(node.right);
            }
        }
        void Print2DUtil(Node root, int space)
        {
            if (root == null)
                return;

            int count = 5;

            space += count;

            Print2DUtil(root.right, space);

            Console.Write("\n");
            for (int i = count; i < space; i++)
                Console.Write(" ");
            Console.Write(root.value + "\n");

            Print2DUtil(root.left, space);
        }

        public void Print2D()
        {
            Print2DUtil(root, 0);
        }
        public void PrintPreOrder()
        {
            PreOrder(root);
        }
    }

    class DijkstraAlgorithm
    {
        private int V = 9;

        private int minDistance(int[] dist, bool[] sptSet)
        {
            int min = int.MaxValue, min_index = -1;

            for (int v = 0; v < V; v++)
                if (sptSet[v] == false && dist[v] <= min)
                {
                    min = dist[v];
                    min_index = v;
                }

            return min_index;
        }

        private void printSolution(int[] dist)
        {
            Console.Write("Vertex \t\t Distance " + "from Source\n");
            for (int i = 0; i < V; i++)
                Console.Write(i + " \t\t " + dist[i] + "\n");
        }

        public void ApplyDijkstra(int[,] graph, int src)
        {
            int[] dist = new int[V];
            bool[] sptSet = new bool[V];

            for (int i = 0; i < V; i++)
            {
                dist[i] = int.MaxValue;
                sptSet[i] = false;
            }

            dist[src] = 0;

            for (int count = 0; count < V - 1; count++)
            {
                int u = minDistance(dist, sptSet);
                sptSet[u] = true;

                for (int v = 0; v < V; v++)
                    if (!sptSet[v] && graph[u, v] != 0 && dist[u] != int.MaxValue && dist[u] + graph[u, v] < dist[v])
                        dist[v] = dist[u] + graph[u, v];
            }

            printSolution(dist);
        }
    }



    class PrimMST
    {
        private int V; // Number of vertices in the graph

        public PrimMST(int v)
        {
            V = v;
        }

        private int MinKey(int[] key, bool[] mstSet)
        {
            int min = int.MaxValue, minIndex = -1;

            for (int v = 0; v < V; v++)
            {
                if (mstSet[v] == false && key[v] < min)
                {
                    min = key[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

        private void PrintMST(int[] parent, int[,] graph)
        {
            Console.WriteLine("Edge \tWeight");
            for (int i = 1; i < V; i++)
            {
                Console.WriteLine($"{parent[i]} - {i}\t{graph[i, parent[i]]}");
            }
        }

        public void PrimMSTAlgorithm(int[,] graph)
        {
            int[] parent = new int[V];
            int[] key = new int[V];
            bool[] mstSet = new bool[V];

            for (int i = 0; i < V; i++)
            {
                key[i] = int.MaxValue;
                mstSet[i] = false;
            }

            key[0] = 0;
            parent[0] = -1;

            for (int count = 0; count < V - 1; count++)
            {
                int u = MinKey(key, mstSet);
                mstSet[u] = true;

                for (int v = 0; v < V; v++)
                {
                    if (graph[u, v] != 0 && mstSet[v] == false && graph[u, v] < key[v])
                    {
                        parent[v] = u;
                        key[v] = graph[u, v];
                    }
                }
            }

            PrintMST(parent, graph);
        }
    }

    class GraphBFS
    {
        private int V; // Number of vertices

        private List<int>[] adj; // Adjacency List

        public GraphBFS(int v)
        {
            V = v;
            adj = new List<int>[V];
            for (int i = 0; i < V; ++i)
            {
                adj[i] = new List<int>();
            }
        }

        public void AddEdge(int v, int w)
        {
            adj[v].Add(w); // Add w to v's list
        }

        public void BFS(int s)
        {
            bool[] visited = new bool[V];

            Queue<int> queue = new Queue<int>();

            visited[s] = true;
            queue.Enqueue(s);

            while (queue.Count != 0)
            {
                s = queue.Dequeue();
                Console.Write(s + " ");

                foreach (int i in adj[s])
                {
                    if (!visited[i])
                    {
                        visited[i] = true;
                        queue.Enqueue(i);
                    }
                }
            }
        }
    }

    public class TrieNode
    {
        public Dictionary<char, TrieNode> Children { get; private set; }
        public bool IsEndOfWord { get; set; }

        public TrieNode()
        {
            Children = new Dictionary<char, TrieNode>();
            IsEndOfWord = false;
        }
    }

    public class Trie
    {
        private readonly TrieNode root;

        public Trie()
        {
            root = new TrieNode();
        }

        public void Insert(string word)
        {
            TrieNode current = root;

            foreach (char c in word)
            {
                if (!current.Children.ContainsKey(c))
                {
                    current.Children[c] = new TrieNode();
                }
                current = current.Children[c];
            }

            current.IsEndOfWord = true;
        }

        public bool Search(string word)
        {
            TrieNode current = root;

            foreach (char c in word)
            {
                if (!current.Children.ContainsKey(c))
                {
                    return false;
                }
                current = current.Children[c];
            }

            return current.IsEndOfWord;
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            AVLTree avlTree = new AVLTree();

            avlTree.Insert(10);
            avlTree.Insert(20);
            avlTree.Insert(30);
            avlTree.Insert(40);
            avlTree.Insert(50);
            avlTree.Insert(25);

            avlTree.Print2D();
            Console.WriteLine();
            avlTree.PrintPreOrder();
            Console.WriteLine();
            Console.WriteLine();



            int[,] graph =
            {
                { 0, 4, 0, 0, 0, 0, 0, 8, 0 },
                { 4, 0, 8, 0, 0, 0, 0, 11, 0 },
                { 0, 8, 0, 7, 0, 4, 0, 0, 2 },
                { 0, 0, 7, 0, 9, 14, 0, 0, 0 },
                { 0, 0, 0, 9, 0, 10, 0, 0, 0 },
                { 0, 0, 4, 14, 10, 0, 2, 0, 0 },
                { 0, 0, 0, 0, 0, 2, 0, 1, 6 },
                { 8, 11, 0, 0, 0, 0, 1, 0, 7 },
                { 0, 0, 2, 0, 0, 0, 6, 7, 0 }

            };
            DijkstraAlgorithm dijkstra = new DijkstraAlgorithm();
            dijkstra.ApplyDijkstra(graph, 0);

            Console.WriteLine();
            Console.WriteLine();

            int[,] graph2 = new int[,] { { 0, 2, 0, 6, 0 },
                { 2, 0, 3, 8, 5 },
                { 0, 3, 0, 0, 7 },
                { 6, 8, 0, 0, 9 },
                { 0, 5, 7, 9, 0 } };

            int vertices = 5;
            PrimMST prim = new PrimMST(vertices);
            // Print the solution
            prim.PrimMSTAlgorithm(graph2);
            Console.WriteLine();
            Console.WriteLine();


            GraphBFS bfs = new GraphBFS(4);
            bfs.AddEdge(0, 1);
            bfs.AddEdge(0, 2);
            bfs.AddEdge(1, 2);
            bfs.AddEdge(2, 0);
            bfs.AddEdge(2, 3);
            bfs.AddEdge(3, 3);

            Console.WriteLine("Bread First Search from vertex 2:");
            bfs.BFS(2);
            Console.WriteLine();
            Console.WriteLine();

            Trie trie = new Trie();
            string[] words = { "apple", "app", "apricot", "banana", "bat", "cat" };

            foreach (var word in words)
            {
                trie.Insert(word);
            }

            Console.WriteLine($"Search for 'apple': {trie.Search("apple")}");
            Console.WriteLine($"Search for 'app': {trie.Search("app")}");
            Console.WriteLine($"Search for 'apricot': {trie.Search("apricot")}");
            Console.WriteLine($"Search for 'banana': {trie.Search("banana")}");
            Console.WriteLine($"Search for 'bat': {trie.Search("bat")}");
            Console.WriteLine($"Search for 'cat': {trie.Search("cat")}");
            Console.WriteLine($"Search for 'dog': {trie.Search("dog")}");
            Console.WriteLine($"Search for 'city': {trie.Search("city")}");
            Console.WriteLine($"Search for 'state': {trie.Search("state")}");
        }

    }
}
