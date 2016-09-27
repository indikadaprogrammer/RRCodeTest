using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRCodeTest
{
    public class AccessDataStore
    {

        #region "Private Methods"

        private string connectionString = "";

        #endregion


        #region "Constructors"

        public AccessDataStore(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #endregion

        #region "Helper Methods"

        /// <summary>
        /// Returns the connection string that can be used to access database using EntityFramework.
        /// </summary>
        /// <returns></returns>
        public string GetEntityConnectionString()
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder(this.connectionString);

            EntityConnectionStringBuilder ecsb = new EntityConnectionStringBuilder();
            ecsb.Provider = "System.Data.SqlClient";
            ecsb.ProviderConnectionString = scsb.ToString();
            ecsb.Metadata = "res://*/RRCodeTestDB.csdl|res://*/RRCodeTestDB.ssdl|res://*/RRCodeTestDB.msl";
            return ecsb.ToString();
        }

        /// <summary>
        /// Retrieves records of type Entity with matching type by calling stored procedure "GetEntitiesByType". 
        /// </summary>
        /// <param name="Type">Type filed value used to filter records</param>
        /// <param name="db">DbContext for the datasource</param>
        /// <returns>
        /// Returns  a list of Entity objects that matches the criteria.  
        /// </returns>
        private List<Entity> GetEntitiesByTypeUsingDBContext(string Type, DbContext db)
        {
            List<Entity> entityList = null;
            try
            {
                SqlParameter typeParameter = new SqlParameter("@Type", Type);
                //NOTE : Although following two lines do work as expected, it can not be tested using mock frameworks
                //       since SqlQuery is not marked as virtual. 
                //List<Entity> entityList = db.Database.SqlQuery<Entity>("GetEntitiesByType @type", typeParameter).ToList();
                //return entityList;
                entityList = db.Set<Entity>().SqlQuery("GetEntitiesByType @Type", typeParameter).AsNoTracking().ToList();
            }
            catch (Exception e)
            {
                //Instead of writing to console, we may write the stack trace to a file
                Console.WriteLine("Error while calling stored procedure GetEntitiesByTypeUsingDBContext. " + e.Message);
                throw e;
            }
            return entityList;
        }

        #endregion


        #region "Public Methods"

        /// <summary>
        /// Retrieves records of type Entity with matching type by calling stored procedure "GetEntitiesByType".
        /// </summary>
        /// <param name="Type">Type filed value used to filter records</param>
        /// <returns>
        /// Returns  a list of Entity objects that matches the criteria.  
        /// </returns>
        public List<Entity> GetEntitiesByType(string Type)
        {
            List<Entity> entityList = null;
            try
            {
                using (RRCodeTestDBEntities db = new RRCodeTestDBEntities(GetEntityConnectionString())) 
                {
                    entityList = GetEntitiesByTypeUsingDBContext(Type, db);
                }
            }
            catch (Exception e)
            {
                //Instead of writing to console, we may write the stack trace to a file
                Console.WriteLine("Error while calling stored procedure GetEntitiesByType. " + e.Message);
                throw e;
            }
            return entityList;
        }
        
        /// <summary>
        /// This is the answer to question 9, which shows how to get the same results by executing 
        /// direct query rather than using a stored procedure.
        /// </summary>
        /// <param name="Type">Type filed value used to filter records</param>
        /// <returns>
        /// Returns  a list of Entity objects that matches the criteria.  
        /// </returns>
        public List<Entity> GetEntitiesByTypeWithoutUsingStoredProcedure(string Type)
        {
            List<Entity> entityList = null;
            try
            {
                using (RRCodeTestDBEntities db = new RRCodeTestDBEntities(GetEntityConnectionString()))
                {
                    var query = from e in db.Entities
                                 where  e.Type == Type
                                 select e;
                    entityList = query.ToList<Entity>();
                }
            }
            catch (Exception e)
            {
                //Instead of writing to console, we may write the stack trace to a file
                Console.WriteLine("Error while calling stored procedure GetEntitiesByTypeWithoutUsingStoredProcedure. " + e.Message);
                throw e;
            }
            return entityList;
        }


        #endregion

    }
}
