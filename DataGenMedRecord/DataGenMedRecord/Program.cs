using System.Text;

var random = new Random();
var sb = new StringBuilder();

for (int i = 0; i < 100; i++)
{
    string contractId = $"H{random.Next(1000, 9999)}";
    string recordId = $"CLM{random.Next(100, 999)}";
    string status = random.Next(0, 2) == 0 ? "Accepted" : "Rejected";
    string errorCode = status == "Rejected" ? $"E{random.Next(100, 999)}" : "";

    if (status == "Accepted")
    {
        sb.AppendLine($"1*{contractId}*{recordId}*{status}**");
    }
    else
    {
        sb.AppendLine($"1*{contractId}*{recordId}*{status}*{errorCode}*");
    }
}

File.WriteAllText("276_277hcClaimStatusReqRes20242707.edi", sb.ToString());
Console.WriteLine("Generated 1000 samples and saved to 276_277hcClaimStatusReqRes20242707.edi");
