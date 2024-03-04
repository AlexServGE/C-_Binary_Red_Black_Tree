using System;
using static TreeSample.BinaryTree;

namespace TreeSample
{
    /// <summary>
    /// Бинарное дерево
    /// </summary>
    public class BinaryTree
    {

        #region Поля

        /// <summary>
        /// Главный элемент (root)
        /// </summary>
        private Node _root;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Создать новое бинарное дерево на основе массива данных
        /// </summary>
        /// <param name="array">Массив данных</param>
        public BinaryTree(IEnumerable<int> array)
        {
            foreach (var e in array)
                Add(e);
        }

        #endregion // Constructors

        #region Отрисовка бинарного дерева (дополнительно)

        /// <summary>
        /// Распечатать на консоль структуру дерева
        /// </summary>
        public void Print()
        {
            Print(_root, string.Empty, null, true);
        }
        
        /// <summary>
        /// Главный метод печати дерева на консоль
        /// </summary>
        /// <param name="node">Элемент дерева</param>
        /// <param name="indent">Отступ</param>
        /// <param name="lastNode">Последний элемент (нет листьев)</param>
        private static void Print(Node node, string indent, bool? isRight, bool lastNode)
        {
            Console.Write(indent);
            if (lastNode)
            {
                Console.Write("└─");
                indent += "  ";
            }
            else
            {
                Console.Write("├─");
                indent += "│ ";
            }

            Console.ForegroundColor = node.Color == Color.Red ? ConsoleColor.Red : ConsoleColor.White;
            Console.WriteLine($"[{(isRight == null ? "Root" : (isRight == true ? "R" : "L"))}] {node.Value}");
            Console.ResetColor();


            int childrenCount = 0;
            if (node.Right != null)
                childrenCount++;
            if (node.Left != null)
                childrenCount++;
            if (node.Right != null)
                Print(node.Right, indent, true, node.Left != null ? false : true);
            if (node.Left != null)
                Print(node.Left, indent, false, true);

            //var children = new List<Node>();
            //if (node.Right != null)
            //    children.Add(node.Right);
            //if (node.Left != null)
            //    children.Add(node.Left);
            
            //for (var i = 0; i < children.Count; i++)
            //    Print(children[i], indent, i == 0, i == children.Count - 1);
        }

        /*public List<int> Print()
        {
            List<int> result = new List<int>();
            List<Node> nodes = new List<Node>();
            nodes.Add(root);
            while (nodes.Count != 0)
            {
                List<Node> children = new List<Node>();
                foreach (var node in nodes)
                {
                    result.Add(node.value);
                    if (node.left != null)
                        children.Add(node.left);
                    if (node.right != null)
                        children.Add(node.right);
                }
                nodes = children;
            }
            return result;
        }*/

        #endregion

        #region Поиск значения элемента в бинарном дереве (v1, поиск в глубину)

        /// <summary>
        /// Поиск значения элемента в бинарном дереве
        /// (поиск в глубину)
        /// </summary>
        /// <param name="value">Искомое значение</param>
        /// <returns>Результат поиска</returns>
        public bool Contains(int value)
        {
            Node node = _root;
            while (node != null)
            {
                if (node.Value == value)
                    return true;

                if (node.Value > value)
                    node = node.Left;
                else
                    node = node.Right;
            }
            return false;
        }

        #endregion

        #region Поиск значения элемента в бинарном дереве (v2, рекурсия)

        /// <summary>
        /// Поиск значения элемента в бинарном дереве
        /// (через рекурсию)
        /// </summary>
        /// <param name="value">Искомое значение</param>
        /// <returns>Результат поиска</returns>
        public bool ContainsV2(int value)
        {
            // Отбрасываем частный случай, когда в дереве нет узлов
            if (_root == null)
                return false;
            // Иначе выполняем рекурсивный поиск по дереву, выбираем левый или правый дочерний
            // узел, в зависимости от искомого значения (больше или меньше)
            return Contains(_root, value);
        }

        private bool Contains(Node node, int value)
        {
            return node.Value == value || Contains(node.Value > value ? node.Left : node.Right, value);
        }

        #endregion

        #region Добовление нового элемента

        /// <summary>
        /// Добавление нового узла в бинарное дерево,
        /// проведение автоматической ребалансировки дерева (при необходимости)
        /// </summary>
        /// <param name="value">Значение нового узла</param>
        /// <returns>Результат добавления нового узла в дерево</returns>
        public bool Add(int value)
        {
            if (_root == null)
            {
                // Частный случай, добавление первого, главного узла в дерево.
                // Ребалансировка не требуется т. к. узел всего лишь один.
                _root = new Node();
                // Цвет корневого узла всегда черный
                _root.Color = Color.Black;
                // Установим новое значение корневого узла
                _root.Value = value;
                // Успешное добовление корневого узла в дерево
                return true;
            }
            // Добавление нового узла в дерево
            bool result = AddNode(_root, value);
            // Проведение полной ребалансировки дерево относительно корневого узла
            _root = Rebalance(_root);
            // Цвет корневого узла всегда черный
            _root.Color = Color.Black;
            // Вернем результат добавления нового узла в дерево
            return result;
        }

        /// <summary>
        /// Добавление нового узла в дерево
        /// </summary>
        /// <param name="node">Текущий узел</param>
        /// <param name="value">Значение нового узла</param>
        /// <returns>Результат добавления нового узла в дерево</returns>
        private bool AddNode(Node node, int value)
        {
            // Защита.
            // Мы не можем добавить значение, которое уже имеется в дереве (правило работы бинарного дерева)
            if (node.Value == value)
                return false;
            // В зависимости от значения нового узла, углубляемся в дерево, либо вправо, либо влево
            if (node.Value > value)
            {
                if (node.Left != null)
                {
                    // Если левый дочерний узел существует, углубляемся в него
                    bool result = AddNode(node.Left, value);
                    // Произведем ребалансировку относительно подузла
                    node.Left = Rebalance(node.Left);
                    // вернем результат добавления нового элемента
                    return result;
                }
                else
                {
                    // Если левый дочерний узел НЕ существует
                    node.Left = new Node
                    {
                        Color = Color.Red,
                        Value = value
                    };
                    return true;
                }
            }
            else
            {
                // Если правый дочерний узел существует, углубляемся в него 
                if (node.Right != null)
                {
                    bool result = AddNode(node.Right, value);
                    // Произведем ребалансировку относительно подузла
                    node.Right = Rebalance(node.Right);
                    // вернем результат добавления нового элемента
                    return result;
                }
                else
                {
                    // Если правый дочерний узел НЕ существует
                    node.Right = new Node
                    {
                        Color = Color.Red,
                        Value = value
                    };
                    return true;
                }
            }
        }

        #endregion

        #region Удаление узла дерева по его значению

        /// <summary>
        /// Идея удаления элемента делится на несколько случаев:
        ///     ● У узла нет дочерних узлов;
        ///     ● У узла есть левый дочерних узлов;
        ///     ● У узла есть правый дочерних узлов;
        ///     ● У узла есть оба дочерних узла.
        /// </summary>
        /// <param name="value">Значение удаляемого узла</param>
        /// <returns>Результат удаления узла</returns>
        public bool Remove(int value)
        {
            // Частный случай. Если дерево пустое - ничего не делаем.
            if (_root == null) return false;

            //_root = RemoveNode(_root, value);
            //return _root;
            RemoveNode(_root, value, out bool res);
            return res;
        }

        /// <summary>
        /// Удаление узла из дерева
        /// </summary>
        /// <param name="node">Текущий узел</param>
        /// <param name="value">Значение удаляемого узла</param>
        /// <returns>Результат удаления узла</returns>
        private Node RemoveNode(Node node, int value, out bool res)
        {
            res = true;
            // если нашли нужный элемент, начинаем процедуру удаления
            if (node.Value == value)
            {
                // обработка самого простого случая, вместо узла возвращается null
                if (node.Left == null && node.Right == null)
                {
                    return null;
                }

                // обработка двух случаев, с только одним из поддеревьев
                if (node.Left == null)
                {
                    return node.Right;
                }

                if (node.Right == null)
                {
                    return node.Left;
                }

                // если у ноды есть оба потомка
                var minNodeInRightSubtree = FindMinElement(node.Right);
                // заменили текущий элемент минимальным из правого поддерева
                node.Value = minNodeInRightSubtree.Value;

                // ищем в правом поддереве минимальный элемент,
                // значение которого уже вставлено на место текущего
                node.Right = RemoveNode(node.Right, minNodeInRightSubtree.Value, out res);
                return node;
            }

            // попадаем сюда, если элемент не был найден,
            // просто проваливаемся в дерево глубже и глубже

            // производится рекурсивный вызов этой же функции,
            // при этом если элемент не будет найден,
            // то алгоритм просто будет возвращать существующую ссылку на поддерево,
            // которая присвоится в ту же позицию
            if (node.Value > value)
            {
                if (node.Left == null)
                {
                    res = false;
                    //console.warn(elementNotFoundMessage);
                    return node;
                }
                // проваливаемся в левое поддерево,
                // после рекурсивной отработки функции _deleteNode
                // будет возвращен текущий элемент,
                // который в предыдущем вызове будет присвоен
                node.Left = RemoveNode(node.Left, value, out res);

                // присваивание на рекурсивный уровень выше,
                // может быть как в левое поддерево,так и в правое,
                // на текущем уровне мы не знаем какое поддерево обрабатываем
                return node;
            }

            // аналогичная обработка для правого поддерева
            else
            //if (node.Value < value)
            {
                if (node.Right == null)
                {
                    res = false;
                    //console.warn(elementNotFoundMessage);
                    return node;
                }
                node.Right = RemoveNode(node.Right, value, out res);
                return node;
            }
        }

        /// <summary>
        /// Найти минимальное значение дочернего узла
        /// относительно текущего родителя
        /// </summary>
        /// <param name="node">Родительский узел</param>
        /// <returns>Узел с минимальным значением</returns>
        private Node FindMinElement(Node node)
        {
            // У текущего узла  нет дочерних узлов, возвращаем текущий узел,
            // частный случай
            if (node.Left == null) return node;
            // Рекурсивным способом пытаемся найти минимальное значение узла, углубляясь
            // в левые подузлы
            return FindMinElement(node.Left);
        }

        #endregion

        #region Балансировка бинарного красно-черного дерева (в левостороннее красно-черное дерево)

        /// <summary>
        /// Сбалансированное дерево - это частный случай бинарного дерева, у которого выполняются
        /// =======================
        /// следующее требование:
        /// для любого узла дерева высота его правого поддерева отличается от высоты его
        /// левого поддерева не более чем на единицу.
        /// 
        /// Красно-черное дерево имеет следующие критерии:
        /// ====================
        ///     ● Каждая нода имеет цвет(красный или черный) и имеет не более 2х потомков
        ///     ● Корень дерева всегда черный
        ///     ● Новая нода всегда красная
        ///     ● У краной ноды все дети черного цвета
        /// 
        /// Левостороннее красно-черное дерево - частный случай красно-черного дерева
        /// ==================================
        /// 
        ///     ● Красные ноды могут быть только левым ребенком
        ///     
        /// Чтобы данные условия выполнялись, после добавления элемента в дерево необходимо
        /// произвести балансировку, благодаря которой все критерии выше станут валидными.
        /// Для балансировки существует 3 операции – [1] левый малый поворот,
        /// [2] правый малый поворот и [3] смена цвета
        /// 
        /// Балансировка бинарного красно-черного дерева
        /// 
        /// </summary>
        /// <param name="node">Родительский узел</param>
        /// <returns></returns>
        private Node Rebalance(Node node)
        {
            // Сохраним текущий родительский узел
            Node result = node;
            // Признак того, что необходимо выполнить ребалансировку подузла
            bool needRebalance;
            do
            {
                // Изначально ребалансировка подузла не требуется
                needRebalance = false;
                // Если оба дочерних узла существуют и цвета дочерних узлов не соответствуют критениям красно-черного дерева
                if (result.Right != null && result.Right.Color == Color.Red &&
                    (result.Left == null || result.Left.Color == Color.Black)) 
                {
                    needRebalance = true; // Необходима ребалансировка (правосторонний поворот)
                    result = RightSwap(result);
                }
                // Если дочерний левый узел существует и он красный, а также его левый дочерний узел
                // тоже является красным, необходимо произвести ребалансировку (левосторонний поворот)
                if (result.Left != null && result.Left.Color == Color.Red &&
                    result.Left.Left != null && result.Left.Left.Color == Color.Red)
                {
                    needRebalance = true;
                    result = LeftSwap(result);
                }
                // Если после ребалансировки оба дочерних узла оказались красными
                if (result.Left != null && result.Left.Color == Color.Red &&
                    result.Right != null && result.Right.Color == Color.Red)
                {
                    needRebalance = true;
                    // Необходимо произвести смену цвета родительского и дочерних узлов
                    ColorSwap(result);
                }
            } while (needRebalance); // Цикл работает до тех пор пока требуется ребалансировка
            return result; // Возвращаем новый родительский узел после проведения ребалансировки
        }

        /// <summary>
        /// [1] Левосторонний поворот (малый левый поворот)
        /// leftSwap.png
        /// </summary>
        /// <param name="node">Родительский узел</param>
        /// <returns>Новый родительский узел</returns>
        private Node LeftSwap(Node node)
        {
            // Сохраним указатель на левого ребенка
            Node leftChild = node.Left;
            // Сохраним указатель на правый узел левого ребенка
            Node betweenChild = leftChild.Right;
            // Осуществим левосторонний поворт
            leftChild.Right = node;
            node.Left = betweenChild;
            // Смена цвета бывшего левого ребенка
            leftChild.Color = node.Color;
            // Смена цвета бывшего родителя
            node.Color = Color.Red;
            // вернем нового родителя (бывшего левого ребенка)
            return leftChild;
        }

        /// <summary>
        /// [2] Правосторонний поворот (малый правый поворот)
        /// rightSwap.png
        /// </summary>
        /// <param name="node">Родительский узел</param>
        /// <returns>Новый родительский узел</returns>
        private Node RightSwap(Node node)
        {
            // Сохраним указатель на правого ребенка
            Node rightChild = node.Right;
            // Сохраним указатель на левый узел правого ребенка
            Node betweenChild = rightChild.Left;
            // Осуществим правосторонний поворт
            rightChild.Left = node;
            node.Right = betweenChild;
            // Смена цвета бывшего правого ребенка
            rightChild.Color = node.Color;
            // Смена цвета бывшего родителя
            node.Color = Color.Red;
            // вернем нового родителя (бывшего правого ребенка)
            return rightChild;
        }

        /// <summary>
        /// [3] Смена цвета, необходимо поменять цвет родительского узла и детей.
        /// Родитель был черный, дети - красные, родитель стал красным, дети - черными.
        /// swap.png
        /// </summary>
        /// <param name="node">Родительский узел</param>
        private void ColorSwap(Node node)
        {
            // Дети становятся черными
            node.Right.Color = Color.Black;
            node.Left.Color = Color.Black;
            // Родительс тановится красным
            node.Color = Color.Red;
        }

        #endregion

        #region Вспомогательные структуры

        /// <summary>
        /// Узел
        /// </summary>
        public class Node
        {
            /// <summary>
            /// Значение
            /// </summary>
            public int Value  { get; set; }
            
            /// <summary>
            /// Цвет узла
            /// </summary>
            public Color Color { get; set; }
            
            /// <summary>
            /// Указатель на левого ребенка
            /// </summary>
            public Node Left  { get; set; }

            /// <summary>
            /// Указатель на правого ребенка
            /// </summary>
            public Node Right  { get; set; }
        }

        /// <summary>
        /// Цвет узла
        /// </summary>
        public enum Color
        {
            /// <summary>
            /// Красный
            /// </summary>
            Red,
            /// <summary>
            /// Черный
            /// </summary>
            Black
        }

        #endregion

    }
}