using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    public class PreCustomerCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity target &&
                target.LogicalName == "lea_katecustomer")
            {
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                string firstName = target.GetAttributeValue<string>("lea_firstname") ?? string.Empty;
                string lastName = target.GetAttributeValue<string>("lea_lastname") ?? string.Empty;
                EntityReference companyRef = target.GetAttributeValue<EntityReference>("lea_company");

                string fullName;

                if (companyRef != null)
                {
                    Entity company = service.Retrieve(companyRef.LogicalName, companyRef.Id, new ColumnSet("lea_name"));
                    string companyName = company.GetAttributeValue<string>("lea_name") ?? string.Empty;

                    fullName = $"{firstName} {lastName} ({companyName})";
                }
                else
                {
                    fullName = $"{firstName} {lastName}";
                }

                target["lea_name"] = fullName;
            }
        }
    }
}
