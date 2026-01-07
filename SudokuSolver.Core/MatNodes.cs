namespace SudokuSolver.Core;

/// <summary>
/// Represents a node in a sparse doubly-linked matrix for the DLX (Dancing Links) algorithm.
/// </summary>
internal class MatNode
{
    public string Tag { get; set; }
    public int ColumnLength { get; set; }
    public MatNode TopNode { get; set; }
    public MatNode UpNode { get; set; }
    public MatNode DownNode { get; set; }
    public MatNode RightNode { get; set; }
    public MatNode LeftNode { get; set; }

    /// <summary>
    /// Creates an isolated node with all links pointing to itself.
    /// </summary>
    public MatNode(string tag)
    {
        Tag = tag;
        UpNode = this;
        DownNode = this;
        RightNode = this;
        LeftNode = this;
        TopNode = this;
    }

    /// <summary>
    /// Creates a node and links it into the sparse matrix structure.
    /// </summary>
    public MatNode(string tag, MatNode upNode, MatNode downNode, MatNode rightNode, MatNode leftNode, MatNode topNode)
    {
        Tag = tag;

        UpNode = upNode;
        upNode.DownNode = this;

        DownNode = downNode;
        downNode.UpNode = this;

        RightNode = rightNode;
        rightNode.LeftNode = this;

        LeftNode = leftNode;
        leftNode.RightNode = this;

        TopNode = topNode;
        topNode.ColumnLength++;
    }
}

