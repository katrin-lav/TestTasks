using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    namespace Task1
    {
        public class PreCustomerUpdate : IPlugin
        {
            public void Execute(IServiceProvider serviceProvider)
            {
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                if (context.InputParameters.Contains("Target") &&
                    context.InputParameters["Target"] is Entity target &&
                    target.LogicalName == "lea_katecustomer")
                {
                    var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    var service = serviceFactory.CreateOrganizationService(context.UserId);

                    Entity preImage = null;
                    if (context.PreEntityImages.Contains("PreImage"))
                    {
                        preImage = context.PreEntityImages["PreImage"];
                    }

                    string firstName = target.Contains("lea_firstname")
                        ? target.GetAttributeValue<string>("lea_firstname")
                        : preImage?.GetAttributeValue<string>("lea_firstname") ?? string.Empty;

                    string lastName = target.Contains("lea_lastname")
                        ? target.GetAttributeValue<string>("lea_lastname")
                        : preImage?.GetAttributeValue<string>("lea_lastname") ?? string.Empty;

                    EntityReference companyRef = target.Contains("lea_company")
                        ? target.GetAttributeValue<EntityReference>("lea_company")
                        : preImage?.GetAttributeValue<EntityReference>("lea_company");

                    string fullName;

                    if (companyRef != null)
                    {
                        var company = service.Retrieve(companyRef.LogicalName, companyRef.Id, new ColumnSet("lea_name"));
                        var companyName = company.GetAttributeValue<string>("lea_name") ?? string.Empty;
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

}
