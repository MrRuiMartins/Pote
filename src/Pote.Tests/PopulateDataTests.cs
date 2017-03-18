using System.IO;
using NUnit.Framework;
using Pote.Tests.Support;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Pote.Tests
{
    [TestFixture]
    public class PopulateDataTests
    {
        [Test]
        public void Create_Sqlite_Database()
        {
            var dbPath = "~/App_Data/db.sqlite".MapAbsolutePath();
            var dbFactory = new OrmLiteConnectionFactory(
                dbPath, SqliteDialect.Provider);

            if (File.Exists(dbPath)) 
                File.Delete(dbPath);

            PoteData.LoadData();

            using (var db = dbFactory.Open())
            {
                db.CreateTables(overwrite:false, tableTypes:PoteFactory.ModelTypes.ToArray());

                db.SaveAll(PoteData.Mediators);
            }
        }
    }
}
