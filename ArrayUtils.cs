namespace TreeSample;

public static class ArrayUtils
{
    private static readonly Random Random = new Random();
    
    public static IEnumerable<int> GetArray(int elementsCount)
    {
        if (elementsCount is <= 0 or > 199)
            throw new Exception("Кол-во элементов должно быть в диапазоне от 1 до 199.");
        HashSet<int> hashSet = new();
        while(hashSet.Count != elementsCount)
            hashSet.Add(Random.Next(-99, 100));
        return hashSet.ToArray();
    }

    public static IEnumerable<int> GetArrayV2()
    {
        return new int[] { 1, 3, 4, 5, 6, 7, 8 };
        //return new int[]{ 5, 3, 15, 1, 8, 24 };
        //return new int[] { 8, 1, 3, 4, 5, 6, 7, 9, 10, 11, 12, 13 };
    }

    public static IEnumerable<int> GetArray() => GetArray(Random.Next(10, 21));
}