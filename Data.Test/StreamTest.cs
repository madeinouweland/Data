using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;

namespace Data.Test
{
    [TestClass]
    public class StreamTest
    {
        [TestMethod]
        public async Task ReturnNullForNotExistingFile()
        {
            var streamer = new Streamer();
            var stream = await streamer.StreamForReadAsync("notexist");
            Assert.IsNull(stream);
        }

        [TestMethod]
        public async Task ReturnEmptyListForNonexistingDocument()
        {
            var database = new Database();
            var all = await database.GetAll<NonExistingDocument>();
            Assert.IsTrue(all.Count == 0);
        }

        [TestMethod]
        public async Task SaveEmployees()
        {
            var database = new Database();

            var employee = new Employee { Name = "Vera", Age = 40 };
            var saved = await database.Save<Employee>(employee);
            Assert.IsTrue(saved.Id == 1);

            var employee2 = new Employee { Name = "Chuck", Age = 42 };
            var saved2 = await database.Save<Employee>(employee2);
            Assert.IsTrue(saved2.Id == 2);

            // save again
            var saved3 = await database.Save<Employee>(employee2);
            Assert.IsTrue(saved3.Id == 2);
        }
    }
}
