# Koko Eating Bananas

## Problem Description

Koko loves to eat bananas. There are `n` piles of bananas, the `i`-th pile has `piles[i]` bananas. The guards have gone and will come back in `h` hours.

Koko can decide her bananas-per-hour eating speed of `k`. Each hour, she chooses some pile of bananas and eats `k` bananas from that pile. If the pile has less than `k` bananas, she eats all of them instead and won't eat any more bananas during this hour.

Koko likes to eat slowly but still wants to finish eating all the bananas before the guards return.

Return the minimum integer `k` such that she can eat all the bananas within `h` hours.

## Example 1:

```
Input: piles = [3,6,7,11], h = 8
Output: 4
```

## Example 2:

```
Input: piles = [30,11,23,4,20], h = 5
Output: 30
```

## Example 3:

```
Input: piles = [30,11,23,4,20], h = 6
Output: 23
```

## Constraints:

- `1 <= piles.length <= 10^4`
- `piles.length <= h <= 10^9`
- `1 <= piles[i] <= 10^9`

## Approach

This problem can be solved using binary search. The key insight is to search for the minimum eating speed `k` that allows Koko to finish all bananas within `h` hours.

1. Define a search space for `k` from 1 to the maximum number of bananas in any pile.
2. For each potential value of `k`, calculate how many hours it would take Koko to eat all bananas.
3. Use binary search to find the minimum value of `k` that satisfies the constraint.

## Complexity Analysis

- Time Complexity: O(n log m) where n is the number of piles and m is the maximum number of bananas in any pile.
- Space Complexity: O(1) as we only use a constant amount of extra space.

