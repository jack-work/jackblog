namespace JackBlog.Services;
public class TrappingRainWaterSolver : ICodePuzzleSolver<int[], int>
{
    public int Solve(int[] height) {
        if (height.Length <= 1) return 0;
        var st = new Stack<Rec>();
        var tallestThusFar = height.First();
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(GetBoundSubset(height)));
        foreach (var h in GetBoundSubset(height)) {
            if (h > tallestThusFar) tallestThusFar = h;
            if (h == 3) break;
            AddToStack(st, new Rec(h, Water: 0, ValleyWidth: 1), (Rec curr, Rec top) => {
                return curr with { Val = curr.Val, Water = (Math.Min(tallestThusFar,curr.Val) - top.Val)*top.ValleyWidth + top.Water, ValleyWidth = top.ValleyWidth + 1 };
            });
        }
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(st, new System.Text.Json.JsonSerializerOptions() { WriteIndented = true }));
        return st.Aggregate(0, (acc, curr) => acc += curr.Water);
    }
    public int[] GetBoundSubset(int[] height) {
        if (height.Length <= 1) return height;
        int curr;
        int next;
        for (curr = 0, next = 1; next < height.Length && height[curr] < height[next]; curr++, next++) { }
        var left = curr;
        for (curr = height.Length-1, next = curr - 1; next >= 0 && height[curr] < height[next]; curr--, next--) { }
        var right = curr;
        return height[left..(right + 1)];
        
    }
    public void AddToStack<T>(Stack<T> stack, T item, Func<T, T, T> op) where T : IVal {
        var curr = item;
        while (stack.Any() && stack.Peek().Val < item.Val)
        {
            var top = stack.Pop();
            curr = op(curr, top);
        }
        stack.Push(curr);
    }
}

public record Rec(int Val, int Water, int ValleyWidth) : IVal;
public interface IVal {
    public int Val { get; }
}
