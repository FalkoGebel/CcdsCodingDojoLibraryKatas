using System.Collections;

namespace _02_FindFileDuplicates
{
    public interface IDuplicates
    {
        IEnumerable Filepath { get; }
    }
}