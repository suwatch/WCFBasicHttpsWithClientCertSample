using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WCFBasicHttpSample
{
    public class X509AuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            var properties = operationContext.RequestContext.RequestMessage.Properties;
            var request = (HttpRequestMessageProperty)properties[HttpRequestMessageProperty.Name];
            var values = request.Headers.GetValues("X-ARR-ClientCert");
            if (values == null || values.Length != 1)
            {
                return false;
            }

            var clientCert = new X509Certificate2(Convert.FromBase64String(values[0]));
            if (!clientCert.Verify())
            {
                return false;
            }

            properties.Add("ClientCert", clientCert);
            return true;
        }
    }
}