using System.Collections;

namespace _02_FindFileDuplicates
{
    public interface IDuplicates<T> where T : class
    {
        IEnumerable Filepath { get; }
    }
}