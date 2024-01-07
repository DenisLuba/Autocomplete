using System;
using System.Collections.Generic;

namespace Autocomplete;

public class RightBorderTask
{
    /// <returns>
    /// Возвращает индекс правой границы. 
    /// То есть индекс минимального элемента, который не начинается с prefix и большего prefix.
    /// Если такого нет, то возвращает items.Length
    /// </returns>
    /// <remarks>
    /// Функция НЕ рекурсивная и работает за O(log(items.Length)*L), где L — ограничение сверху на длину фразы
    /// </remarks>

    public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
    {
        if (phrases.Count == 0) return 0;
        left = left <= 0 || left >= phrases.Count ? 0 : left;
        right = right <= 0 || right >= phrases.Count ? phrases.Count - 1 : right;
        while (left < right)
        {
            var middle = left + (right - left) / 2;
            if (string.Compare(phrases[middle], prefix, StringComparison.InvariantCultureIgnoreCase) <= 0
                || phrases[middle].Contains(prefix, StringComparison.InvariantCultureIgnoreCase))
                left = middle + 1;
            else right = middle - 1;
        }

        right = right < 0 ? 0 : right;
        return string.Compare(prefix, phrases[right], StringComparison.InvariantCultureIgnoreCase) >= 0
               || phrases[right].Contains(prefix, StringComparison.InvariantCultureIgnoreCase)
            ? right + 1
            : right;
    }
}