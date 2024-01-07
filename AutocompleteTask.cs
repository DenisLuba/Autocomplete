using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Autocomplete;

internal class AutocompleteTask
{
    /// <returns>
    /// Возвращает первую фразу словаря, начинающуюся с prefix.
    /// </returns>
    /// <remarks>
    /// Эта функция уже реализована, она заработает, 
    /// как только вы выполните задачу в файле LeftBorderTask
    /// </remarks>
    public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
    {
        var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
        if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
            return phrases[index];

        return null;
    }

    /// <returns>
    /// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count) 
    /// элементов словаря, начинающихся с prefix.
    /// </returns>
    /// <remarks>Эта функция должна работать за O(log(n) + count)</remarks>
    public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
    {
        // тут стоит использовать написанный ранее класс LeftBorderTask
        return null;
    }

    /// <returns>
    /// Возвращает количество фраз, начинающихся с заданного префикса
    /// </returns>
    public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
    {
        // тут стоит использовать написанные ранее классы LeftBorderTask и RightBorderTask
        return -1;
    }
}

[TestFixture]
public class AutocompleteTests
{
    [Test]
    public void TopByPrefix_IsEmpty_WhenNoPhrases()
    {
        // ...
        //CollectionAssert.IsEmpty(actualTopWords);
    }

    // ...

    [Test]
    public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
    {
        // ...
        //Assert.AreEqual(expectedCount, actualCount);
    }

    // ...


    [TestCase(new[] { "a", "a" }, "a", 2, 1)]
    [TestCase(new[] { "a", "b" }, "a", 1, 2)]
    [TestCase(new[] { "b", "b" }, "a", 0, 3)]
    [TestCase(new[] { "a", "b", "b", "b" }, "a", 1, 4)]
    [TestCase(new[] { "ab", "b", "b", "b" }, "a", 1, 5)]
    [TestCase(new[] { "ab", "b", "b", "b" }, "ab", 1, 6)]
    [TestCase(new string[0] { }, "a", 0, 7)]
    [TestCase(new string[] { "a", "ab", "abc" }, "aa", 1, 8)]
    [TestCase(new string[] { "ab", "ab", "ab", "ab" }, "a", 4, 9)]
    [TestCase(new string[] { "ab", "ab", "ab", "ab" }, "aa", 0, 10)]

    public void TestCases(string[] arr, string prefix, int expectedResult, int testNumber)
    {
        var phrases = arr.ToList();
        Console.WriteLine($"Test: {testNumber} Expected: {expectedResult}");
        Assert.AreEqual(expectedResult, RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count()));
    }
}