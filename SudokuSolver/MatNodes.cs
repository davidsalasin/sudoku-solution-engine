namespace SudokuSolver;

// Class MatNode: Used to create a sparse double linked matrix for DLX (Algorithm X, dancing links) algorithm.
class MatNode
{
    // Property: string Tag.
    // Tags MatNode (self) with string for identification.
    public string Tag { get; set; }
    // Property: int ColumnLength. 
    // ** Only applied and used by Top MatNode columns in sparse matrixes to keep track of each column's MatNode count.
    public int ColumnLength { get; set; }
    // Property: MatNode TopNode.
    // References Top MatNode of the column in the sparse matrix.
    public MatNode TopNode { get; set; }
    // Property: MatNode UpNode.
    // References Above MatNode in the sparse matrix.
    public MatNode UpNode { get; set; }
    // Property: MatNode DownNode.
    // References Below MatNode in the sparse matrix.
    public MatNode DownNode { get; set; }
    // Property: MatNode RightNode.
    // References Right MatNode in the sparse matrix.
    public MatNode RightNode { get; set; }
    // Property: MatNode LeftNode.
    // References Left MatNode in the sparse matrix.
    public MatNode LeftNode { get; set; }

    // Class Constructor.
    // RECEIVES: string tag.
    // CONSTRUCTS -> MatNode with received tag, with all MatNode properties references pointing at self.
    public MatNode(string tag)
    {
        this.Tag = tag;

        UpNode = this;
        DownNode = this;
        RightNode = this;
        LeftNode = this;
        TopNode = this;
    }
    // Class Constructor.
    // RECEIVES: string tag, MatNode upNode, MatNode downNode, MatNode rightNode, MatNode leftNode, MatNode topNode.
    // CONSTRUCTS -> MatNode with received tag, with each property reference connected to each received reference (in order, respectively).
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

