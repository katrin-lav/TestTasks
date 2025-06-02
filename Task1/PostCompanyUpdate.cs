using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Task1
{
    public class PostCompanyUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity target &&
                target.LogicalName == "lea_katecompany")
            {
                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var service = serviceFactory.CreateOrganizationService(context.UserId);

                var companyId = target.Id;

                var companyName = target.GetAttributeValue<string>("lea_name");

                var query = new QueryExpression("lea_katecustomer")
                {
                    ColumnSet = new ColumnSet("lea_firstname", "lea_lastname"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("lea_company", ConditionOperator.Equal, companyId)
                        }
                    }
                };

                var customers = service.RetrieveMultiple(query);

                foreach (var customer in customers.Entities)
                {
                    string firstName = customer.GetAttributeValue<string>("lea_firstname") ?? string.Empty;
                    string lastName = customer.GetAttributeValue<string>("lea_lastname") ?? string.Empty;

                    string fullName = $"{firstName} {lastName} ({companyName})";

                    Entity updateCustomer = new Entity("lea_katecustomer", customer.Id)
                    {
                        ["lea_name"] = fullName
                    };

                    service.Update(updateCustomer);
                }
            }
        }
    }
}

