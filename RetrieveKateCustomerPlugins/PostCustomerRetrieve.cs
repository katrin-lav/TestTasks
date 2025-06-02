using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace RetrieveKateCustomerPlugins
{
  

    public class PostCustomerRetrieve : PluginBase
    {

        private readonly string teamName;

        public PostCustomerRetrieve(string unsecureConfiguration, string secureConfiguration)
            : base(typeof(PostCustomerRetrieve))
        {
            teamName = secureConfiguration;
        }


        protected override void ExecuteCdsPlugin(ILocalPluginContext localContext)
        {
            var context = localContext.PluginExecutionContext;
            var tracing = localContext.TracingService;
            var service = localContext.SystemUserService;

            tracing.Trace("PostCustomerRetrieve plug-in execution started.");

            if (context.MessageName != "Retrieve" || context.Stage != (int)SdkMessageProcessingStepStage.PostOperation)
                return;

            if (!context.OutputParameters.Contains("BusinessEntity") || !(context.OutputParameters["BusinessEntity"] is Entity entity))
            {
                tracing.Trace("Output 'BusinessEntity' not found or is invalid.");
                return;
            }
                

            if (string.IsNullOrWhiteSpace(teamName))
            {
                tracing.Trace("Secure configuration does not contain a team name.");
                return;
            }

            var hasAccess = SecurityHelper.UserIsInTeam(service, context.InitiatingUserId, teamName);

            if (!hasAccess && entity.Attributes.Contains("lea_description"))
            {
                entity["lea_description"] = null;
                tracing.Trace($"Description (lea_description) has been removed from the output. Reason: the user is not in '{teamName}' team.");
            }

            tracing.Trace("PostCustomerRetrieve plug-in execution finished.");
        }
    }

}
