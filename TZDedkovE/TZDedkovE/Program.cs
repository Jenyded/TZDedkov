using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskExam
{
    internal class TaskSolver
    {
        public static void Main(string[] args)
        {
            TestGenerateWordsFromWord();
            TestMaxLengthTwoChar();
            TestGetPreviousMaxDigital();
            TestSearchQueenOrHorse();
            TestCalculateMaxCoins();

            Console.WriteLine("All Test completed!");
        }

        #region задание 1) Слова из слова
        /// задание 1) Слова из слова
        public static List<string> GenerateWordsFromWord(string word, List<string> wordDictionary)
        {
            var words = new List<string>();
            foreach (var listWord in wordDictionary)
            {
                var wordChars = word.ToCharArray().ToList();
                List<char> listWordChars = listWord.ToCharArray().ToList();
                var contains = true;
                for (int i = 0; i < listWordChars.Count(); i++)
                {
                    if (!wordChars.Contains(listWordChars[i]))
                    {
                        contains = false;
                        break;
                    }
                    wordChars.Remove(listWordChars[i]);
                }
                if (contains) words.Add(listWord);
            }
            var res = words.OrderBy(x => x).ToList();
            return res;
        }
        #endregion

        #region задание 2) Два уникальных символа
        /// задание 2) Два уникальных символа
        public static int MaxLengthTwoChar(string input)
        {
            var counts = new Dictionary<char, int>();
            var lastChar = '\0';
            foreach (var c in input)
            {
                if (counts.ContainsKey(c))
                {
                    counts[c]++;
                }
                else
                {
                    counts[c] = 1;
                }

                if (lastChar.Equals(c))
                {
                    counts[c] = Int32.MinValue;
                }
                lastChar = c;
            }
            
            var sortedCounts = counts.Where(c => c.Value > 0).OrderByDescending(c => c.Value);
            var res = sortedCounts.Count();
            
            if (res >= 2)
            {
                byte i = 0;
                res = 0;
                foreach (var keyValuePair in sortedCounts)
                {
                    if (i >= 2) break;
                    res += keyValuePair.Value;
                    i++;
                }
            }
            else
            {
                return 0;
            }
            return res;
        }
        #endregion

        #region задание 3) Предыдущее число
        /// задание 3) Предыдущее число
        public static long GetPreviousMaxDigital(long value)
        {
            string numStr = value.ToString();
            int lenghtStr = numStr.Length;
            for (int i = lenghtStr - 1; i > 0; i--)
            {
                if (numStr[i - 1] > numStr[i])
                {
                    char[] numArray = numStr.ToCharArray();
                    char previousNumber = '0';  // находим следующую наименьшую цифру в числе
                    int prevIndex = -1;
                    for (int j = i; j < lenghtStr; j++)
                    {
                        if (numArray[j] > previousNumber && numArray[j] < numStr[i - 1])
                        {
                            previousNumber = numArray[j];
                            prevIndex = j;
                        }
                    }

                    if (prevIndex == -1)
                    {
                        return -1;
                    }

                    numArray[prevIndex] = numStr[i - 1]; //Меняем местами цифру со следующей наименьшей 
                    numArray[i - 1] = previousNumber;

                    ReverseComparer reverse = new ReverseComparer();
                    Array.Sort(numArray, i, lenghtStr - i, reverse); //Сортируем цифры после следующей наименьшей цифры

                    string prevNum = new string(numArray);

                    if (prevNum[0] == '0') // Проверка, начинается ли новое число с нуля
                    {
                        return -1;
                    }
                    return int.Parse(prevNum);
                }
            }
            return -1;
        }
        #endregion

        #region задание 4) Конь и Королева
        /// задание 4) Конь и Королева
        public static List<int> SearchQueenOrHorse(char[][] gridMap)
        {
            int startRow = -1;
            int startColumn = -1;

            int endRow = -1;
            int endColumn = -1;

            for (int row = 0; row < gridMap.Length; row++)
            {
                for (int column = 0; column < gridMap[row].Length; column++)
                {
                    if (gridMap[row][column] == 's')
                    {
                        startRow = row;
                        startColumn = column;
                    }

                    if (gridMap[row][column] == 'e')
                    {
                        endRow = row;
                        endColumn = column;
                    }
                }
            }

            if (startRow < 0 || startColumn < 0 || endRow < 0 || endColumn < 0)
            {
                return new List<int> { -1, -1 };
            }
            return FindShortestPath(gridMap, startRow, startColumn, endRow, endColumn);
        }

        static List<int> FindShortestPath(char[][] matrix, int start_row, int start_column, int end_row, int end_column)
        {
            List<int> moves = new List<int>(); // массив для хранения ходов коня и королевы
            bool[,] visited = new bool[matrix.Length, matrix[0].Length]; // массив для отметки посещенных клеток

            // Нахождение ходов коня
            int knightMoves = FindKnightMoves(matrix, start_row, start_column, end_row, end_column, visited, new int[] { 2, 1, -1, -2, -2, -1, 1, 2 });
            moves.Add(knightMoves);

            visited = new bool[matrix.Length, matrix[0].Length]; // сбрасываем массив для отметки посещенных клеток

            // Нахождение ходов королевы
            int queenMoves = FindQueenMoves(matrix, start_row, start_column, end_row, end_column, visited, new int[] { 0,  1, //  0  1 - сдвиг вправо
                                                                                                                       0, -1, //  0 -1 - сдвиг влево
                                                                                                                      -1,  0, // -1  0 - сдвиг вверх
                                                                                                                       1,  0, //  1  0 - сдвиг вниз    
                                                                                                                       1,  1, //  1  1 - сдвиг по диагонали вниз вправо
                                                                                                                       1, -1, //  1  1 - сдвиг по диагонали вниз влево
                                                                                                                      -1,  1, //  1  1 - сдвиг по диагонали вверх вправо
                                                                                                                      -1, -1  // -1 -1 - сдвиг по диагонали вверх влево
                                                                                                                      });
            moves.Add(queenMoves);

            return moves;
        }

        static int FindKnightMoves(char[][] matrix, int start_row, int start_column, int end_row, int end_column, bool[,] visited, int[] dx)
        {
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(new Node(start_row, start_column, 0));

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                // Проверяем координаты клетки на соответствие конечной клетке
                if (currentNode.Row == end_row && currentNode.Column == end_column)
                    return currentNode.Moves;

                // Ищем все возможные ходы
                for (int i = 0; i < dx.Length - 1; i += 2)
                {
                    int newRow = currentNode.Row + dx[i];
                    int newColumn = currentNode.Column + dx[i + 1];

                    if (newRow >= 0 && newRow < matrix.Length && newColumn >= 0 && newColumn < matrix[0].Length && matrix[newRow][newColumn] != 'x' && !visited[newRow, newColumn])
                    {
                        visited[newRow, newColumn] = true;
                        queue.Enqueue(new Node(newRow, newColumn, currentNode.Moves + 1));
                    }
                }
            }
            return -1;
        }

        static Node QueenMoves(Node node, int end_row, int end_column, char[][] matrix, bool[,] visited, int step_row, int step_column)
        {
            // Проверяем координаты клетки на соответствие конечной клетке
            if (node.Row == end_row && node.Column == end_column)
                return node;

            Node tempNode = node;

            int new_row = node.Row + step_row;
            int new_column = node.Column + step_column;

            if (new_row >= 0 && new_row < matrix.Length && new_column >= 0 && new_column < matrix[0].Length && matrix[new_row][new_column] != 'x' && !visited[new_row, new_column])
            {
                visited[new_row, new_column] = true;
                tempNode = QueenMoves(new Node(new_row, new_column, node.Moves), end_row, end_column, matrix, visited, step_row, step_column);
            }
            return tempNode;
        }

        static int FindQueenMoves(char[][] matrix, int start_row, int start_column, int end_row, int end_column, bool[,] visited, int[] dx)
        {
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(new Node(start_row, start_column, 0));
            visited[start_row, start_column] = true;

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                // Проверяем координаты клетки на соответствие конечной клетке
                if (currentNode.Row == end_row && currentNode.Column == end_column)
                    return currentNode.Moves;

                // Ищем все возможные ходы
                for (int i = 0; i < dx.Length - 1; i += 2)
                {
                    Node tempNode = currentNode;
                    tempNode = QueenMoves(tempNode, end_row, end_column, matrix, visited, dx[i], dx[i + 1]);
                    if (tempNode != currentNode)
                    {
                        queue.Enqueue(new Node(tempNode.Row, tempNode.Column, currentNode.Moves + 1));
                    }
                }
            }
            return -1;
        }
        #endregion

        #region задание 5) Жадина
        /// задание 5) Жадина
        public static long CalculateMaxCoins(int[][] mapData, int idStart, int idFinish)
        {
            var maxValue = -1;
            for (var i = 0; i < mapData.Length; i++)
                for (var j = 0; j < mapData[i].Length - 1; j++)
                {
                    if (maxValue < mapData[i][j])
                    {
                        maxValue = mapData[i][j];
                    }
                }

            var roads = new bool[maxValue + 1, maxValue + 1];
            var coins = new int[maxValue + 1, maxValue + 1];
            for (var i = 0; i < mapData.Length; i++)
            {
                var X = mapData[i][0];
                var Y = mapData[i][1];
                roads[X, Y] = true;
                coins[X, Y] = mapData[i][2];
            }

            return FindMaxCoins(maxValue, idStart, idFinish, roads, coins);
        }
        
        // NumberOfCities - количество городов
        // start - начальная точка
        // end - конечная точка
        // roads - матрица смежности, где roads[i,j] = true если между городами i и j существует дорога, иначе false
        // coins - матрица с количеством монет на каждой дороге, где coins[i,j] - количество монет на дороге между городами i и j
        public static int FindMaxCoins(int NumberOfCities, int start, int end, bool[,] roads, int[,] coins)
        {
            int[] dist = Enumerable.Repeat(-1, NumberOfCities + 1).ToArray();
            dist[start] = 0;
            var queue = new PriorityQueue<int, int>();
            queue.Enqueue(0, start);
            while (queue.Count > 0)
            {
                queue.TryDequeue(out var currentDist, out var currentNode);

                if (dist[currentNode] < currentDist)
                    continue;

                for (int neighbor = 0; neighbor < NumberOfCities + 1; neighbor++)
                {
                    if (!roads[currentNode, neighbor])
                        continue;

                    if (dist[neighbor] == -1 || dist[neighbor] < dist[currentNode] + coins[currentNode, neighbor])
                    {
                        dist[neighbor] = dist[currentNode] + coins[currentNode, neighbor];
                        queue.Enqueue(dist[neighbor], neighbor);
                    }
                }
            }
            return dist[end] != -1 ? dist[end] : -1;
        }
        #endregion

        /// Тесты (можно/нужно добавлять свои тесты) 

        private static void TestGenerateWordsFromWord()
        {
            var wordsList = new List<string>
            {
                "кот", "ток", "око", "мимо", "гром", "ром", "мама",
                "рог", "морг", "огр", "мор", "порог", "бра", "раб", "зубр"
            };

            AssertSequenceEqual(GenerateWordsFromWord("арбуз", wordsList), new List<string> { "бра", "зубр", "раб" });
            AssertSequenceEqual(GenerateWordsFromWord("лист", wordsList), new List<string>());
            AssertSequenceEqual(GenerateWordsFromWord("маг", wordsList), new List<string>());
            AssertSequenceEqual(GenerateWordsFromWord("погром", wordsList), new List<string> { "гром", "мор", "морг", "огр", "порог", "рог", "ром" });
        }

        private static void TestMaxLengthTwoChar()
        {
            AssertEqual(MaxLengthTwoChar("beabeeab"), 5);
            AssertEqual(MaxLengthTwoChar("а"), 0);
            AssertEqual(MaxLengthTwoChar("ab"), 2);
            AssertEqual(MaxLengthTwoChar("abababaabababecececececececececeee"), 15);            
        }

        private static void TestGetPreviousMaxDigital()
        {
            AssertEqual(GetPreviousMaxDigital(21), 12l);
            AssertEqual(GetPreviousMaxDigital(531), 513l);
            AssertEqual(GetPreviousMaxDigital(1027), -1l);
            AssertEqual(GetPreviousMaxDigital(2071), 2017l);
            AssertEqual(GetPreviousMaxDigital(207034), 204730l);
            AssertEqual(GetPreviousMaxDigital(135), -1l);
        }

        private static void TestSearchQueenOrHorse()
        {
            char[][] gridA =
            {
                new[] {'s', '#', '#', '#', '#', '#'},
                new[] {'#', 'x', 'x', 'x', 'x', '#'},
                new[] {'#', '#', '#', '#', 'x', '#'},
                new[] {'#', '#', '#', '#', 'x', '#'},
                new[] {'#', '#', '#', '#', '#', 'e'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridA), new[] { 3, 2 });

            char[][] gridB =
            {
                new[] {'s', '#', '#', '#', '#', 'x'},
                new[] {'#', 'x', 'x', 'x', 'x', '#'},
                new[] {'#', 'x', '#', '#', 'x', '#'},
                new[] {'#', '#', '#', '#', 'x', '#'},
                new[] {'x', '#', '#', '#', '#', 'e'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridB), new[] { -1, 3 });

            char[][] gridC =
            {
                new[] {'s', '#', '#', '#', '#', 'x'},
                new[] {'x', 'x', 'x', 'x', 'x', 'x'},
                new[] {'#', '#', '#', '#', 'x', '#'},
                new[] {'#', '#', '#', 'e', 'x', '#'},
                new[] {'x', '#', '#', '#', '#', '#'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridC), new[] { 2, -1 });

            char[][] gridD =
            {
                new[] {'e', '#'},
                new[] {'x', 's'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridD), new[] { -1, 1 });

            char[][] gridE =
            {
                new[] {'e', '#'},
                new[] {'x', 'x'},
                new[] {'#', 's'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridE), new[] { 1, -1 });

            char[][] gridF =
            {
                new[] {'x', '#', '#', 'x'},
                new[] {'#', 'x', 'x', '#'},
                new[] {'#', 'x', '#', 'x'},
                new[] {'e', 'x', 'x', 's'},
                new[] {'#', 'x', 'x', '#'},
                new[] {'x', '#', '#', 'x'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridF), new[] { -1, 5 });
        }

        private static void TestCalculateMaxCoins()
        {
            var mapA = new[]
            {
                new []{0, 1, 1},
                new []{0, 2, 4},
                new []{0, 3, 3},
                new []{1, 3, 10},
                new []{2, 3, 6},
            };

            AssertEqual(CalculateMaxCoins(mapA, 0, 3), 11l);

            var mapB = new[]
            {
                new []{0, 1, 1},
                new []{1, 2, 53},
                new []{2, 3, 5},
                new []{5, 4, 10}
            };

            AssertEqual(CalculateMaxCoins(mapB, 0, 5), -1l);

            var mapC = new[]
            {
                new []{0, 1, 1},
                new []{0, 3, 2},
                new []{0, 5, 10},
                new []{1, 2, 3},
                new []{2, 3, 2},
                new []{2, 4, 7},
                new []{3, 5, 3},
                new []{4, 5, 8}
            };

            AssertEqual(CalculateMaxCoins(mapC, 0, 5), 19l);
        }

        /// Тестирующая система, лучше не трогать этот код

        private static void Assert(bool value)
        {
            if (value)
            {
                return;
            }

            throw new Exception("Assertion failed");
        }

        private static void AssertEqual(object value, object expectedValue)
        {
            if (value.Equals(expectedValue))
            {
                return;
            }

            throw new Exception($"Assertion failed expected = {expectedValue} actual = {value}");
        }

        private static void AssertSequenceEqual<T>(IEnumerable<T> value, IEnumerable<T> expectedValue)
        {
            if (ReferenceEquals(value, expectedValue))
            {
                return;
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (expectedValue is null)
            {
                throw new ArgumentNullException(nameof(expectedValue));
            }

            var valueList = value.ToList();
            var expectedValueList = expectedValue.ToList();

            if (valueList.Count != expectedValueList.Count)
            {
                throw new Exception($"Assertion failed expected count = {expectedValueList.Count} actual count = {valueList.Count}");
            }

            for (var i = 0; i < valueList.Count; i++)
            {
                if (!valueList[i].Equals(expectedValueList[i]))
                {
                    throw new Exception($"Assertion failed expected value at {i} = {expectedValueList[i]} actual = {valueList[i]}");
                }
            }
        }
    }

    #region В рамках задания 3) Предидущее число
    
    public class ReverseComparer : IComparer<char>
    {
        public int Compare(char x, char y)
        {
           return y.CompareTo(x); // сравнение Х и Y в обратном порядке
        }
    }
    #endregion

    #region В рамках задания 4) Конь и Королева

    class Node
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Moves { get; set; }
        public Node(int row, int column, int moves)
        {
            Row = row;
            Column = column;
            Moves = moves;
        }
    }
    #endregion
}
