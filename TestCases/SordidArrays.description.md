In the **Sordid Arrays** puzzle, you are given two sorted arrays of numbers. Your task is to find the median value when both arrays are combined into one sorted array. However, you must solve this in **O(log(n+m))** time complexity, where n and m are the lengths of the two arrays.

The median is the middle value of a set when arranged in order:
- If the combined array has an odd number of elements, the median is the middle element.
- If the combined array has an even number of elements, the median is the average of the two middle elements.

For example, if you have arrays `[1, 3]` and `[2, 4]`, the combined sorted array would be `[1, 2, 3, 4]`, and the median would be `(2 + 3) / 2 = 2.5`.

This puzzle tests your understanding of:
- Binary search
- Edge cases
- Efficient array manipulation
