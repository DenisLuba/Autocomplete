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

        return "";
    }

    /// <returns>
    /// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count) 
    /// элементов словаря, начинающихся с prefix.
    /// </returns>
    /// <remarks>Эта функция работает за O(log(n) + count)</remarks>
    public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
    {
        if (phrases.Count == 0) return Array.Empty<string>();
        var leftIndex = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
        var countByPrefix = GetCountByPrefix(phrases, prefix);
        if (countByPrefix == 0) return Array.Empty<string>();
        count = countByPrefix < count ? countByPrefix : count;
        var result = new string[count];
        for (int i = leftIndex, j = 0; i < leftIndex + count; i++, j++)
            result[j] = phrases[i];

        return result;
    }

    /// <returns>
    /// Возвращает количество фраз, начинающихся с заданного префикса
    /// </returns>
    public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
    {
        if (phrases.Count == 0) return 0;
        var leftIndex = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
        var rightIndex = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count) - 1;

        if (leftIndex < phrases.Count
            && phrases[leftIndex].StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase)
            && leftIndex <= rightIndex)
            return rightIndex - leftIndex + 1;

        return 0;
    }
}

[TestFixture]
public class AutocompleteTests
{
    [TestCase(new string[] { "aa", "ab", "ac", "bc" }, "a", 2, new string[] { "aa", "ab" })]

    [TestCase(new string[] { }, "ab", 6, new string[0])]
    [TestCase(new[] { "aa", "ab", "bb", "bb", "c" }, "b", 6, new[] { "bb", "bb" })]
    [TestCase(new[] { "aa", "ab", "bb", "bb", "bb" }, "a", 10, new[] { "aa", "ab" })]
    [TestCase(new[] { "aa", "ab", "bb", "bb", "bb", "c" }, "c", 6, new[] { "c" })]
    [TestCase(new[] { "aa", "ab", "bb", "bb", "bb", "c" }, "", 3, new[] { "aa", "ab", "bb" })]
    public void TopByPrefix_IsEmpty_WhenNoPhrases(IReadOnlyList<string> phrases, string prefix, int count, string[] expected)
    {
        var actualTopWords = AutocompleteTask.GetTopByPrefix(phrases, prefix, count);
        Assert.AreEqual(expected, actualTopWords);
        //CollectionAssert.IsEmpty(actualTopWords);
    }

    // ...

    [TestCase(new string[] { }, "ab", 0)]
    [TestCase(new[] { "aa", "ab", "bb", "bb", "bb", "bb", "bb", "bb", "bb", "bb", "c" }, "b", 8)]
    [TestCase(new[] { "aa", "ab", "bb", "bb", "bb", "bb", "bb", "bb", "bb", "bb", "c" }, "a", 2)]
    [TestCase(new[] { "aa", "ab", "bb", "bb", "bb", "bb", "bb", "bb", "bb", "bb", "c" }, "c", 1)]
    [TestCase(new[] { "aa", "ab", "bb", "bb", "bb", "bb", "bb", "bb", "bb", "bb", "c" }, "", 11)]
    public void CountByPrefix_IsTotalCount_WhenEmptyPrefix(IReadOnlyList<string> phrases, string prefix, int expected)
    {
        var actualCount = AutocompleteTask.GetCountByPrefix(phrases, prefix);
        Assert.AreEqual(expected, actualCount);
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