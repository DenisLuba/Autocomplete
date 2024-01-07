using System;
using System.Collections.Generic;

namespace Autocomplete;

// Внимание!
// Есть одна распространенная ловушка при сравнении строк: строки можно сравнивать по-разному:
// с учетом регистра, без учета, зависеть от кодировки и т.п.
// В файле словаря все слова отсортированы методом StringComparison.InvariantCultureIgnoreCase.
// Во всех функциях сравнения строк в C# можно передать способ сравнения.
public class LeftBorderTask
{
    /// <returns>
    /// Возвращает индекс левой границы.
    /// То есть индекс максимальной фразы, которая не начинается с prefix и меньшая prefix.
    /// Если такой нет, то возвращает -1
    /// </returns>
    /// <remarks>
    /// Функция рекурсивная и работает за O(log(items.Length)*L), где L — ограничение сверху на длину фразы
    /// </remarks>

    public static int GetLeftBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
    {
        if (phrases.Count == 0) return -1;
        left = left <= 0 || left >= phrases.Count ? 0 : left;
        right = right <= 0 || right >= phrases.Count ? phrases.Count - 1 : right;
        return BinarySearchLeftBorder(phrases, prefix, left, right);
    }

    public static int BinarySearchLeftBorder(IReadOnlyList<string> phrases, string prefix, int left, int right)
    {
        if (left >= right)
            return string.Compare(phrases[left], prefix, StringComparison.InvariantCultureIgnoreCase) < 0
                ? left
                : left - 1;
        var middle = left + (right - left) / 2;
        return string.Compare(phrases[middle], prefix, StringComparison.InvariantCultureIgnoreCase) < 0
            ? BinarySearchLeftBorder(phrases, prefix, middle + 1, right)
            : BinarySearchLeftBorder(phrases, prefix, left, middle - 1);
    }
}