# SolutionTestTasks

## Task1
Apply Formatting for Name of Customer  

formats:  
<pre lang="markdown">if Company not empty:  
  {Customer.Firstname} {Customer.Lastname} ({CompanyId.Name})  
else  
  {Customer.Firstname} {Customer.Lastname}</pre>
 
plugins (use Pre Images):  
- PreCustomerCreate
triggers on "Firstname", "Lastname" "CompanyId"
populate new "Name" value
- PreCustomerUpdate
triggers on "Firstname", "Lastname", "CompanyId"
populate new "Name" value
- PostCompanyUpdate
triggers on "Name"
populate new "Name" value for linked Customers


## RetrieveKateCustomerPlugins

RETRIEVE/RETRIEVE MULTIPLE PLUGINS <br>

entity: Custom entity Customer <br>
field: description <br>          
- create "Access Private Notes" team
- create Post Customer Retrieve Multiple plugin        
- create Post Customer Retrieve plugin <br>

if current user not in the "Access Private Notes" team plugins must hide Customer.desctiption data  <br>
- Retrieve Multiple parameters: input: "Query"; output: "BusinessEntityCollection"        
- Retrieve parameters: input: "Id"; output: "BusinessEntity"   
