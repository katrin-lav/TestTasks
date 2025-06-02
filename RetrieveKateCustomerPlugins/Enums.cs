using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetrieveKateCustomerPlugins
{
    public enum SdkMessageProcessingStepStage
    {
        PreValidation = 10,
        PreOperation = 20,
        PostOperation = 40
    }
}
