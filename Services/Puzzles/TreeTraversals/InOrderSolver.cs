namespace JackBlog.Services;
/**
 * Definition for a binary tree node.
 * public class TreeNode {
 *     public int val;
 *     public TreeNode left;
 *     public TreeNode right;
 *     public TreeNode(int val=0, TreeNode left=null, TreeNode right=null) {
 *         this.val = val;
 *         this.left = left;
 *         this.right = right;
 *     }
 * }
 */
public record TreeNode(int val = 0, TreeNode? left = null, TreeNode? right = null);
public class InOrderSolver : ICodePuzzleSolver<TreeNode, IList<int>>
{
    public IList<int> Solve(TreeNode testCase)
    {
        return new List<int>();
    }
}

