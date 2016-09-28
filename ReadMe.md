## How to Execute Unit Tests
- Executing unit tests does not require an actual database.
- Open the test solution is visual studio and 
  run any of the unit tests found in following files :
  - UnitTestAccessDataStore.cs


## How to Execute Integration Tests
- Executing integration tests requires an actual database.
- Create a data base in a SQL Server.
- Create table "Entity" by executing RRCodeTest\sql\CreateTables.sql.
- Create stored procedure [dbo.GetEntitiesByType] by executing RRCodeTest\sql\CreateStoredProcs.sql.
- Update the connection string in RRCodeTestAutomatedTests\TestInputs\Common\configuration.json so that test cases point to the database.
- Execute Integration Tests found in IntegrationTests.cs.

## View Automated Test Code Coverage
- Automated Test Code Coverage can be viewed by opening
  TestCoverage/index.htm


## Answers to Optional Questions

#### 5. Modify the library such that its configuration source could also be a webservice call. The webservice does not exist yet.

Configuration of the library is done using a class that implements IConfigurator interface. One concrete implementation is JsonBasedConfigurator which reads a JSON file to get the connection string. To enable library to be configured using a web service :

1. Create a new class (e.g. WebServiceBasedConfigurator ) which implements the IConfigurator interface. 
2. Within the GetConnectionString method of the above class, implement the logic to call the Web Service and 
   retrieve the connection string.
3. When using the library, instantiate a ConfigurationFactory object using an instance of the above class created in step1.

#### 6. The library should be resilient to the “cloud” environment. Add //todo: items at specific sections of code that you feel could be refactored, listing what kind of improvements could be made.

It is not clear what is meant by "cloud". However, if you want the library to work against a 
Azure SQL instance, it should work without modifications to the code, assuming that the user configures the connection string appropriately. Connection string for an Azure SQL instance 
is in the following format.

<pre>
Server=tcp:hostname.database.windows.net,1433;Database=RRCodeTestDB;User ID=username@hostname;Password=your_password_here;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;
</pre>


#### 7.	Briefly outline what strategies you may employ in the future as your entity graph and relationships become more complex.

As the entity graph and relationships changes, you may be required to update the Entity Data Model to reflect those changes. This can be done by following steps.

1. Double click EntityDataModel (edmx file) to open the EDM.
2. Right click and select "Update Model From Database...".
3. In the "Update Wizard" select the database objects to be Added/Refreshed/Deleted and click "Finish".
4. If you require more customized entities, associations that does not exist in the Database, 
   you may add them directly to EDM.
5. Save the edmx file.
6. If the changes to the edmx causes existing code to break (i.e. since entities that were present before were deleted), fix the code accordingly. 

#### 8.	A customer is complaining about slow response; the issue been traced back to fragmented sql server index, that your library maintains. Please discuss your strategies at diagnosing the problem, and suggest a solution.

If the problem has traced back to fragmented indexes, following would be the corrective actions.

1. Verify the fragmentation level of the Index
   To get the statistics on the index you may use the SQL Server Management Studio(SSMS) or query sys.dm_db_index_physical_stats table. Based on the 
   value of avg_fragment_size_in_percent, you have two options.

	- If 5 < avg_fragment_size_in_percent < 30
	  	Reorganize the Index (See step2 below)

	- If 30 < avg_gragment_size_in_precent
		Rebuild the Index (See step3 below)

2. Reorganize the index
	Use ALTER INDEX REORGANIZE command or use SSMS.
3. Rebuild the index
    Use ALTER INDEX REBUILD command or use SSMS.
 
Reference : [https://msdn.microsoft.com/en-us/library/ms189858.aspx](https://msdn.microsoft.com/en-us/library/ms189858.aspx "Reorganize and Rebuild Indexes")

#### 9.	If you did not have to use a stored procedure to retrieve the entities, how would you approach the problem differently? 

Rather than using a stored procedures, you may use query 
capabilities provided by Entity Framework to get the same 
results. See the AccessDataStore.GetEntitiesByTypeWithoutUsingStoredProcedure method for the implementation.

