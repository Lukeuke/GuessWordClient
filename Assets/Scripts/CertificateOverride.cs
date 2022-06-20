using UnityEngine.Networking;

public class CertificateOverride : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}