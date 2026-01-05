namespace SudokuSolver;

/// <summary>
/// Class used to create a sparse double linked matrix for DLX (Algorithm X, dancing links) algorithm.
/// </summary>
class MatNode
{
    /// <summary>
    /// Tags MatNode (self) with string for identification.
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    /// Only applied and used by Top MatNode columns in sparse matrices to keep track of each column's MatNode count.
    /// </summary>
    public int ColumnLength { get; set; }

    /// <summary>
    /// References Top MatNode of the column in the sparse matrix.
    /// </summary>
    public MatNode TopNode { get; set; }

    /// <summary>
    /// References Above MatNode in the sparse matrix.
    /// </summary>
    public MatNode UpNode { get; set; }

    /// <summary>
    /// References Below MatNode in the sparse matrix.
    /// </summary>
    public MatNode DownNode { get; set; }

    /// <summary>
    /// References Right MatNode in the sparse matrix.
    /// </summary>
    public MatNode RightNode { get; set; }

    /// <summary>
    /// References Left MatNode in the sparse matrix.
    /// </summary>
    public MatNode LeftNode { get; set; }

    /// <summary>
    /// Constructs a MatNode with the received tag, with all MatNode properties references pointing at self.
    /// </summary>
    /// <param name="tag">Tag string for identification.</param>
    public MatNode(string tag)
    {
        this.Tag = tag;

        UpNode = this;
        DownNode = this;
        RightNode = this;
        LeftNode = this;
        TopNode = this;
    }
    /// <summary>
    /// Constructs a MatNode with the received tag, with each property reference connected to each received reference (in order, respectively).
    /// </summary>
    /// <param name="value">Tag string for identification.</param>
    /// <param name="upNode">Reference to the node above.</param>
    /// <param name="downNode">Reference to the node below.</param>
    /// <param name="rightNode">Reference to the node to the right.</param>
    /// <param name="leftNode">Reference to the node to the left.</param>
    /// <param name="topNode">Reference to the top node of the column.</param>
    public MatNode(string value, MatNode upNode, MatNode downNode, MatNode rightNode, MatNode leftNode, MatNode topNode)
    {
        this.Tag = value;

        // Connects received references to self by the opposite directions.
        UpNode = upNode;
        upNode.DownNode = this;

        DownNode = downNode;
        downNode.UpNode = this;

        RightNode = rightNode;
        rightNode.LeftNode = this;

        LeftNode = leftNode;
        leftNode.RightNode = this;

        // Connects to Top MatNode column in the sparse matrix, and raises its MatNode column count.
        TopNode = topNode;
        TopNode.ColumnLength++;
    }
}

