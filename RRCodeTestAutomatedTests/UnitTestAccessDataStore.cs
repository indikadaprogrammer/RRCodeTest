using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RRCodeTest;
using System.Collections.Generic;
using Moq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Data.Entity.Core;

namespace RRCodeTestAutomatedTests
{
    [TestClass]
    public class UnitTestAccessDataStore
    {

        #region "Helper Methods"

        /// <summary>
        ///  Invokes private method GetEntitiesByTypeUsingDBContext and returns the result.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        private List<Entity> InvokeGetEntitiesByTypeUsingDBContext(string query, DbContext dbContext)
        {
            ConfigurationFactory factory = new ConfigurationFactory(new JsonBasedConfigurator());

            AccessDataStore dataStore = new AccessDataStore(factory.GetConnectionString());
            PrivateObject obj = new PrivateObject(dataStore);
            var returnValue = obj.Invoke("GetEntitiesByTypeUsingDBContext", new object[] {query, dbContext} );
            return returnValue as List<Entity>;
        }

        #endregion

        #region "Unit Tests"

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Test_001_GetEntitiesByTypeUsingDBContextThrowsAnException()
        {
            //Copy configuration.json
            UnitTestHelper.CopyFile(Path.Combine(UnitTestHelper.TestInputsFolder, "Common"), Directory.GetCurrentDirectory(), "configuration.json", true);

            var setMock = new Mock<DbSet<Entity>>();
            setMock.Setup(m => m.SqlQuery(It.IsAny<string>(), It.IsAny<object[]>())).Throws(new Exception("Dummy DB Exception"));
            
            var mockContext = new Mock<DbContext>();
            mockContext.Setup(c => c.Set<Entity>()).Returns(setMock.Object);

            //This call should throw an exception
            List<Entity> queryResult = InvokeGetEntitiesByTypeUsingDBContext("DummyType", mockContext.Object); 
        }


        /// <summary>
        /// Make sure GetEntitiesByTypeUsingDBContext method executes sucessfully against the mock DbContext.
        /// </summary>
        [TestMethod]
        public void Test_002_GetEntitiesByTypeUsingDBContextExecutesSuccessfully()
        {
            //Copy configuration.json
            UnitTestHelper.CopyFile(Path.Combine(UnitTestHelper.TestInputsFolder, "Common"), Directory.GetCurrentDirectory(), "configuration.json", true);
            //Build Mock object 
            List<Entity> dummyResults = new List<Entity>() { new Entity{ Id = 1, Type = "Type1", Content = "Content1", Created = new DateTime(2016,9,20, 8, 30, 0)}, 
                                                             new Entity{ Id = 2, Type = "Type2", Content = "Content2", Created = new DateTime(2016,9,25, 15, 30, 0)}};

            //Can not use mocking with DbContext.Database.SqlQuery because it is not marked as virtual.
            //Instead use DbContext.Set.SqlQuery.
            //Reference : http://stackoverflow.com/questions/26014969/how-to-moq-entity-framework-sqlquery-calls
            var setMock = new Mock<DbSet<Entity>>();
            setMock.Setup(m => m.SqlQuery(It.IsAny<string>(), It.IsAny<object[]>())).Returns<string, object[]>((sql, param) =>
            {
                var sqlQueryMock = new Mock<DbSqlQuery<Entity>>();
                sqlQueryMock.Setup(m => m.AsNoTracking()).Returns(sqlQueryMock.Object);
                sqlQueryMock.Setup(m => m.GetEnumerator()).Returns(dummyResults.GetEnumerator());
                return sqlQueryMock.Object;
            });
            
            var mockContext = new Mock<DbContext>();
            mockContext.Setup(c => c.Set<Entity>()).Returns(setMock.Object);

            List<Entity> queryResult = InvokeGetEntitiesByTypeUsingDBContext("DummyType", mockContext.Object); 
            Assert.AreEqual(2, queryResult.Count, "Expected Count : 2 Actual Count : " + queryResult.Count); 
        }

        [TestMethod]
        [ExpectedException(typeof(EntityException))]
        public void Test_003_GetEntitiesByTypeThrowsAnException()
        {
            //Create a dataStore with a empty connection string. So that subsequeny calls will throw an exception
            AccessDataStore dataStore = new AccessDataStore("");
            //This should throw an exception
            dataStore.GetEntitiesByType("DummyType");
        }




        #endregion


    }
}
