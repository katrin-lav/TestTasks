using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetrieveKateCustomerPlugins
{
    public static class SecurityHelper
    {
        public static bool UserIsInTeam(IOrganizationService service, Guid userId, string teamName)
        {
            var fetchXml = $@"
              <fetch>
                <entity name='teammembership'>
                  <filter>
                    <condition attribute='systemuserid' operator='eq' value='{userId}' />
                  </filter>
                  <link-entity name='team' from='teamid' to='teamid'>
                    <filter>
                      <condition attribute='name' operator='eq' value='{teamName}' />
                    </filter>
                  </link-entity>
                </entity>
              </fetch>";

            var result = service.RetrieveMultiple(new FetchExpression(fetchXml));

           return result.Entities.Count > 0;
        }
    }
}
