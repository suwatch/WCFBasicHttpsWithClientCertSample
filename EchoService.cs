using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace WCFBasicHttpSample
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class EchoService : IEcho
    {
        public string Echo(string message)
        {
            var properties = OperationContext.Current.RequestContext.RequestMessage.Properties;
            var clientCert = (X509Certificate2)properties["ClientCert"];
            return $"{message.ToUpper()} from ClientCertThumbprint {clientCert.Thumbprint}";
        }

        public Stream DownloadStream()
        {
            byte[] buffer = new byte[1024];
            Random rand = new Random();
            rand.NextBytes(buffer);

            MemoryStream stream = new MemoryStream(buffer);
            return stream;
        }

        public void UploadStream(Stream stream)
        {
            int readResult;
            int bytesRead = 0;
            byte[] buffer = new byte[1000];
            do
            {
                readResult = stream.Read(buffer, 0, buffer.Length);
                bytesRead += readResult;
            }
            while (readResult != 0);

            stream.Close();
        }
    }
}
