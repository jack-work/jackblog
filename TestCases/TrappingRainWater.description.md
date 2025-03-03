The **Trapping Rainwater** problem asks you to calculate how much water can be trapped between bars of varying heights.

Given an array of non-negative integers representing the elevation profile of a terrain where each bar's width is 1 unit, compute how much water can be trapped after it rains.

For example, given the input array `[0,1,0,2,1,0,1,3,2,1,2,1]`, the correct answer is **6 units** of water.

To solve this problem, you need to analyze each position and determine how much water it can hold. The amount of water at any position depends on:
- The minimum of the maximum heights to its left
- The maximum heights to its right
- Minus its own height

This problem tests your ability to:
- Work with arrays
- Recognize patterns
- Optimize for time and space complexity

[Problem on LeetCode](https://leetcode.com/problems/trapping-rain-water/)
