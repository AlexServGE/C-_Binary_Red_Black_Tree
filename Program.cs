namespace TreeSample
{
    internal abstract class Program
    {
        private static void Main()
        {
            BinaryTree binaryTree = new BinaryTree(ArrayUtils.GetArrayV2());
            
            while (true)
            {
                Console.WriteLine("Работа с бинарным деревом");
                Console.WriteLine("===============================\n");
                binaryTree.Print();
                Console.WriteLine("\n===============================");
                Console.WriteLine("0. Завершение работы приложения");
                Console.WriteLine("1. Сгенерировать новое дерево");
                Console.WriteLine("2. Добавить новый узел");
                Console.WriteLine("3. Найти узел по значению");
                Console.WriteLine("4. Удалить узел по значению");
                Console.Write("Выберите пункт меню: ");

                if (int.TryParse(Console.ReadLine(), out int no))
                {
                    switch (no)
                    {
                        case 0:
                            Console.WriteLine("Завершение работы приложения.");
                            return;
                        case 1:
                            binaryTree = new BinaryTree(ArrayUtils.GetArray());
                            break;
                        case 2:
                            Console.WriteLine(binaryTree.Add(ProcessNumber()) ? "Узел успешно добавлен." :
                                "Невозможно добавить новый узел в дерево. Возможно, узел по подобным значением уже присутствует в дереве.");
                            break;
                        case 3:
                            Console.WriteLine(binaryTree.Contains(ProcessNumber()) ? "Узел успешно найден." :
                                "Узел не найден.");
                            break;
                        case 4:
                            Console.WriteLine(binaryTree.Remove(ProcessNumber()) ? "Узел успешно удален." :
                                "Узел не найден.");
                            break;
                        default:
                            Console.WriteLine("Укажите корректный пункт меню.");
                            break;
                    }
                }
                else
                    Console.WriteLine("Укажите корректный пункт меню.");
            }
        }

        private static int ProcessNumber()
        {
            while (true)
            {
                Console.Write("Укажите число: ");
                if (int.TryParse(Console.ReadLine(), out int number))
                    return number;
                else
                    Console.WriteLine("Укажите корректное целое число.");
            }
        }
    }
}