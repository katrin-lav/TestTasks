using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetrieveKateCustomerPlugins
{
    public class PostCustomerRetrieveMultiple : PluginBase
    {
        private readonly string teamName;

        public PostCustomerRetrieveMultiple(string unsecureConfiguration, string secureConfiguration)
            : base(typeof(PostCustomerRetrieve))
        {
            teamName = secureConfiguration;
        }


        protected override void ExecuteCdsPlugin(ILocalPluginContext localContext)
        {
            var context = localContext.PluginExecutionContext;
            var tracing = localContext.TracingService;
            var service = localContext.SystemUserService;

            tracing.Trace("PostCustomerRetrieveMultiple plug-in execution started.");

            if (context.MessageName != "RetrieveMultiple" || context.Stage != (int)SdkMessageProcessingStepStage.PostOperation)
                return;

            if (!context.OutputParameters.Contains("BusinessEntityCollection") || !(context.OutputParameters["BusinessEntityCollection"] is EntityCollection customers))
            {
                tracing.Trace("Output 'BusinessEntityCollection' not found or is invalid.");
                return;
            }


            if (string.IsNullOrWhiteSpace(teamName))
            {
                tracing.Trace("Secure configuration does not contain a team name.");
                return;
            }


            if (!SecurityHelper.UserIsInTeam(service, context.InitiatingUserId, teamName)) {
                foreach (var customer in customers.Entities)
                {
                    if (customer.Attributes.Contains("lea_description"))
                    {
                        customer.Attributes.Remove("lea_description");
                    }
                }
                tracing.Trace($"Description (lea_description) has been removed from the output. Reason: the user is not in '{teamName}' team.");
            }

            tracing.Trace("PostCustomerRetrieveMultiple plug-in execution finished.");
        }
    }
}
