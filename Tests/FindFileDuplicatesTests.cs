using _02_FindFileDuplicates;
using FluentAssertions;

namespace Tests
{
    [TestClass]
    public class FindFileDuplicatesTests
    {
        private static string _folderpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/FindFileDuplicatesKata/";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Directory.CreateDirectory(_folderpath);
            Directory.CreateDirectory(_folderpath + "Subfolder1");
            Directory.CreateDirectory(_folderpath + "Subfolder2");
            Directory.CreateDirectory(_folderpath + "Subfolder3");
            File.WriteAllText(_folderpath + "Subfolder1/EmptyFile1.txt", string.Empty);
            File.WriteAllText(_folderpath + "Subfolder1/EmptyFile2.txt", string.Empty);
            File.WriteAllText(_folderpath + "Subfolder1/EmptyFile3.txt", string.Empty);
            File.WriteAllText(_folderpath + "Subfolder1/TextFile1.txt", "This is a one liner.");
            File.WriteAllText(_folderpath + "Subfolder1/TextFile2.txt", "This is a two liner.\nReally!");
            File.WriteAllText(_folderpath + "Subfolder1/TextFile3b.txt", "This is a three liner.\nReally!\nI swear.");


            File.WriteAllText(_folderpath + "Subfolder2/EmptyFile1.txt", string.Empty);
            File.WriteAllText(_folderpath + "Subfolder2/emptyfile3.txt", string.Empty);
            File.WriteAllText(_folderpath + "Subfolder2/TextFile1.txt", "This is a one liner.");
            File.WriteAllText(_folderpath + "Subfolder2/TextFile2.txt", "This is a two liner.\nReally!");
            File.WriteAllText(_folderpath + "Subfolder2/TextFile4b.txt", "This is a 2? Byte file.");
            File.WriteAllText(_folderpath + "Subfolder2/LargeFile.txt", "This is a very large file…a really large file...large than the other files in the pot.\nI swear.");


            File.WriteAllText(_folderpath + "Subfolder3/TextFile1.txt", "This is a one liner.");
            File.WriteAllText(_folderpath + "Subfolder3/TextFile3.txt", "This is a three liner.\nReally!\nI swear.");
            File.WriteAllText(_folderpath + "Subfolder3/TextFile3b.txt", "This is a three liner.\nReally!\nI swear.");
            File.WriteAllText(_folderpath + "Subfolder3/TextFile4.txt", "This is a 23 Byte file.");
            File.WriteAllText(_folderpath + "Subfolder3/TextFile4b.txt", "This is a 2? Byte file.");
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(@"C:\invalid_folder\")]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_compile_candidates_with_invalid_folderpath(string folderpath)
        {
            FindFileDuplicatesLogic sut = new();
            sut.CompileCandidates(folderpath);
        }

        [TestMethod]
        public void Test_get_files_from_folderpath()
        {
            List<string> files = FindFileDuplicatesLogic.GetFileForPaths(_folderpath);
            files.Count().Should().Be(17);
        }

        [TestMethod]
        public void Test_compile_candidates_default()
        {
            FindFileDuplicatesLogic sut = new();
            IEnumerable<string> candidates = (IEnumerable<string>)sut.CompileCandidates(_folderpath);

            // 2 + 2 + 3 + 2 + 2 + 2
            candidates.Count().Should().Be(13);
        }

        [TestMethod]
        public void Test_compile_candidates_size()
        {
            FindFileDuplicatesLogic sut = new();
            IEnumerable<string> candidates = (IEnumerable<string>)sut.CompileCandidates(_folderpath, CompareModes.Size);

            // 5 + 3 + 2 + 3 + 3
            candidates.Count().Should().Be(16);
        }

        [DataTestMethod]
        [DataRow("", "d41d8cd98f00b204e9800998ecf8427e")]
        [DataRow("Franz jagt im komplett verwahrlosten Taxi quer durch Bayern", "a3cca2b2aa1e3b5b3b5aad99a8529074")]
        [DataRow("Frank jagt im komplett verwahrlosten Taxi quer durch Bayern", "7e716d0e702df0505fc72e2b89467910")]
        public void Test_Md5(string input, string md5)
        {
            FindFileDuplicatesLogic.GetMd5(input).Should().Be(md5);
        }


        [TestMethod]
        public void Test_check_candidates()
        {
            FindFileDuplicatesLogic sut = new();
            IEnumerable<string> candidates_default = (IEnumerable<string>)sut.CompileCandidates(_folderpath);
            IEnumerable<string> results_default = (IEnumerable<string>)sut.CheckCandidates(candidates_default);
            results_default.Count().Should().Be(13);

            IEnumerable<string> candidates_size = (IEnumerable<string>)sut.CompileCandidates(_folderpath, CompareModes.Size);
            IEnumerable<string> results_size = (IEnumerable<string>)sut.CheckCandidates(candidates_size);
            results_size.Count().Should().Be(15);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Directory.Delete(_folderpath, true);
        }
    }
}