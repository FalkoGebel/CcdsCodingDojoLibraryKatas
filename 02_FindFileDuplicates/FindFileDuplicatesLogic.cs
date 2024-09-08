using System.Collections;

namespace _02_FindFileDuplicates
{
    public class FindFileDuplicatesLogic : ICheckForDuplicates
    {
        public static List<string> GetFileForPaths(string folderpath)
            => Directory.GetFiles(folderpath, "*", SearchOption.AllDirectories)
                        .Select(f => (new FileInfo(f)).FullName)
                        .ToList();

        public static string GetMd5(string input)
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = System.Security.Cryptography.MD5.HashData(inputBytes);

            return Convert.ToHexString(hashBytes).ToLower();
        }

        public IEnumerable CheckCandidates(IEnumerable candidates)
        {
            Dictionary<string, string> candidatesWithMd5 = [];

            foreach (string c in (IEnumerable<string>)candidates)
            {
                candidatesWithMd5[c] = GetMd5(string.Concat(File.ReadAllBytes(c)));
            }

            return candidatesWithMd5.Where(c => candidatesWithMd5.Count(cc => c.Value == cc.Value) > 1)
                                    .Select(c => c.Key);
        }

        public IEnumerable CompileCandidates(string folderpath) => CompileCandidates(folderpath, CompareModes.SizeAndName);

        public IEnumerable CompileCandidates(string folderpath, CompareModes mode)
        {
            if (string.IsNullOrEmpty(folderpath) || !Directory.Exists(folderpath))
                throw new ArgumentException("Missing or invalid folderpath");

            Duplicates duplicates = new();
            List<string> files = GetFileForPaths(folderpath);
            List<FileInfo> fileInfos = [];

            foreach (string file in files)
                fileInfos.Add(new FileInfo(file));

            foreach (FileInfo fileInfo in fileInfos)
            {
                switch (mode)
                {
                    case CompareModes.SizeAndName:
                        if (fileInfos.Count(fi => fileInfo.Name.ToLower() == fi.Name.ToLower() && fileInfo.Length == fi.Length) > 1)
                            duplicates.Add(fileInfo.Name, fileInfo.Length, fileInfo.FullName);
                        break;

                    case CompareModes.Size:
                        if (fileInfos.Count(fi => fileInfo.Length == fi.Length) > 1)
                            duplicates.Add(fileInfo.Name, fileInfo.Length, fileInfo.FullName);
                        break;

                    default: throw new ArgumentException($"Compare mode \"{mode}\" not supported");
                }
            }

            return duplicates.Filepath;
        }
    }
}