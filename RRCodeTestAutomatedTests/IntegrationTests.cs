using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RRCodeTest;
using System.Collections.Generic;
using System.IO;
using System.Data.Entity;

namespace RRCodeTestAutomatedTests
{
    [TestClass]
    public class IntegrationTests
    {
        #region "Helper Methods"

        /// <summary>
        /// Helper method to insert records to the table.
        /// </summary>
        /// <param name="entityConnectionString">Connection string to the database</param>
        /// <param name="Type">Value of the Type column</param>
        /// <param name="count">Number of records to be inserted</param>
        private void AddTestRecords(string entityConnectionString, string Type, int count)
        {
            using (RRCodeTestDBEntities db = new RRCodeTestDBEntities(entityConnectionString))
            {
                for (int i = 0; i < count; i++)
                {
                    db.Entities.Add(new Entity() { Type = Type, Content = "AudoAdd " + Type + " " + i, Created = DateTime.Now });
                }
                db.SaveChanges();
            }
        }


        #endregion

        #region "Integration Tests"

        [TestMethod]
        public void Test_101_GetEntitiesByType_ZeroRecords()
        {
            //Copy configuration.json
            UnitTestHelper.CopyFile(Path.Combine(UnitTestHelper.TestInputsFolder, "Common"), Directory.GetCurrentDirectory(), "configuration.json", true);
            ConfigurationFactory factory = new ConfigurationFactory(new JsonBasedConfigurator());
            AccessDataStore dataStore = new AccessDataStore(factory.GetConnectionString());
            
            //Add Records to the database
            string Type = "NonExistingType";
            int numberOfRecords = 0;
            
            List<Entity> selectedEntities = dataStore.GetEntitiesByType(Type);
            Assert.AreEqual(numberOfRecords, selectedEntities.Count, "Expected Count : " + numberOfRecords + " Actual Count : " + selectedEntities.Count);
        }


        [TestMethod]
        public void Test_102_GetEntitiesByType_MultipleRecords()
        {
            //Copy configuration.json
            UnitTestHelper.CopyFile(Path.Combine(UnitTestHelper.TestInputsFolder, "Common"), Directory.GetCurrentDirectory(), "configuration.json", true);
            ConfigurationFactory factory = new ConfigurationFactory(new JsonBasedConfigurator());
            AccessDataStore dataStore = new AccessDataStore(factory.GetConnectionString());
            
            //Add Records to the database
            string Type = "Type" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int numberOfRecords = 3;
            AddTestRecords(dataStore.GetEntityConnectionString(), Type, numberOfRecords); 
            
            List<Entity> selectedEntities = dataStore.GetEntitiesByType(Type);
            Assert.AreEqual(numberOfRecords, selectedEntities.Count, "Expected Count : " + numberOfRecords + " Actual Count : " + selectedEntities.Count);
        }
        
        [TestMethod]
        public void Test_103_GetEntitiesByTypeWithoutUsingStoredProcedure()
        {
            //Copy configuration.json
            UnitTestHelper.CopyFile(Path.Combine(UnitTestHelper.TestInputsFolder, "Common"), Directory.GetCurrentDirectory(), "configuration.json", true);
            ConfigurationFactory factory = new ConfigurationFactory(new JsonBasedConfigurator());
            AccessDataStore dataStore = new AccessDataStore(factory.GetConnectionString());
            
            //Add Records to the database
            string Type = "Type" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int numberOfRecords = 3;
            AddTestRecords(dataStore.GetEntityConnectionString(), Type, numberOfRecords); 
            
            List<Entity> selectedEntities = dataStore.GetEntitiesByTypeWithoutUsingStoredProcedure(Type);
            Assert.AreEqual(numberOfRecords, selectedEntities.Count, "Expected Count : " + numberOfRecords + " Actual Count : " + selectedEntities.Count);
        }
    }

	#endregion

}
