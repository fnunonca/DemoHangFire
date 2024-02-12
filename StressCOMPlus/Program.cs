using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Collections.Concurrent;

class Program
{
    static ConcurrentQueue<string> responseQueue = new ConcurrentQueue<string>();
    static async Task Main(string[] args)
    {
        string endpoint = "https://testwsinterno.izipay.pe/authorization/authorizationws3/api/v1/authorize";
        int totalRequests = 7000; // Total de peticiones
        int requestsPerBatch = 30; // Peticiones por segundo

        var fileWriteTask = Task.Run(() => WriteResponsesToFile());


        using (var client = new HttpClient())
        {
            for (int i = 0; i < totalRequests / requestsPerBatch; i++)
            {
                var tasks = new List<Task>();

                for (int j = 0; j < requestsPerBatch; j++)
                {
                    tasks.Add(SendRequestAsync(client, endpoint));
                }

                // Ejecutar todas las tareas (peticiones) en paralelo
                await Task.WhenAll(tasks);

                // Esperar 1 segundo antes de enviar el siguiente lote de peticiones
                //await Task.Delay(1000);
            }
        }

        // Asegúrate de que todas las respuestas hayan sido escritas en el archivo
        while (!responseQueue.IsEmpty)
        {
            await Task.Delay(100); // Espera un tiempo breve
        }
    }

    static async Task SendRequestAsync(HttpClient client, string endpoint)
    {
        string guid = Guid.NewGuid().ToString("N").Substring(0, 15);

        var requestContent = CreateRequestContent(guid);
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json")
        };

        request.Headers.Add("transactionId", guid);

        try
        {
            var response = await client.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();
            responseQueue.Enqueue(responseBody); // Sólo encola la respuesta
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }


    static void WriteResponsesToFile()
    {
        while (!responseQueue.IsEmpty)
        {
            if (responseQueue.TryDequeue(out string response))
            {
                File.AppendAllText("responses.txt", response + Environment.NewLine);
            }
        }
    }

    static string CreateRequestContent(string guid)
    {
        // Plantilla del cuerpo de la solicitud en formato JSON
        string jsonTemplate = @"
        {{
            ""action"": ""pay"",
            ""card"": {{
                ""Pan"": ""4890680100026131"",
                ""BrandId"": 14,
                ""ExpirationDate"": ""2602"",
                ""cvc"": ""047"",
                ""cvcPresent"": ""SI""
            }},
            ""order"": {{
                ""ProcessTypeId"": ""AT"",
                ""Amount"": ""1.00"",
                ""Currency"": ""PEN"",
                ""CurrencySymbol"": ""S/."",
                ""OrderNumber"": ""{0}"",
                ""Installments"": ""02"",
                ""Deferred"": ""0"",
                ""PayMethodId"": ""1"",
                ""ChannelId"": ""4"",
                ""TransactionOrigin"": ""PV"",
                ""wallet"": {{ ""Code"": """", ""Name"": """" }},
                ""DatetimeTerminalTransaction"": ""2024-01-18 10:16:54.583""
            }},
            ""billing"": {{
                ""DocumentType"": ""DNI"",
                ""Document"": ""77822284"",
                ""FirstName"": ""Junior"",
                ""LastName"": ""Vega"",
                ""Email"": ""juniorvegareyes12@gmail.com"",
                ""PhoneNumber"": ""931988302"",
                ""Street"": ""calle el demo sdk"",
                ""City"": ""lima"",
                ""Country"": ""PE"",
                ""State"": ""lima"",
                ""PostalCode"": ""00001""
            }},
            ""shipping"": {{
                ""FirstName"": ""Darwin"",
                ""LastName"": ""Paniagua"",
                ""Email"": ""demo@izipay.pe"",
                ""PhoneNumber"": ""931988302"",
                ""Street"": ""calle el demo sdk"",
                ""City"": ""lima"",
                ""Country"": ""PE"",
                ""State"": ""lima"",
                ""PostalCode"": ""00001""
            }},
            ""merchantInfo"": {{
                ""Code"": ""4001834"",
                ""Name"": ""FPCULQI*VINILOS"",
                ""Type"": ""5331"",
                ""ConfirmationEmail"": ""0"",
                ""FacilitatorCode"": """",
                ""Acquirer"": 1
            }},
            ""authentication"": {{
                ""izipayAS400"": {{ ""idLogMPI"": """", ""auth3DSUcaf"": ""3"", ""auth3DSAAV"": """" }},
                ""cybersourceInfo"": {{
                    ""auth3dsStatus"": """",
                    ""auth3DSAAV"": """",
                    ""auth3dsUcafIndicator"": """",
                    ""auth3dsSli"": """",
                    ""auth3dsS42"": """",
                    ""auth3dsSecureIndicator"": """",
                    ""auth3dsTransactionId"": """",
                    ""auth3dsXid"": """"
                }}
            }},
            ""antifraud"": {{
                ""ClientIp"": null,
                ""DeviceFingerprintId"": ""27192495-2f60-49a3-bd9a-b2a56a00fe52"",
                ""CybersourceInfo"": {{
                    ""IdScoring"": """",
                    ""ScoringCode"": """",
                    ""ScoringMessage"": """"
                }}
            }},
            ""token"": {{
                ""CardToken"": """",
                ""BuyerToken"": """",
                ""Cryptogram"": """",
                ""ExpirationMonthToken"": ""03"",
                ""ExpirationYearToken"": ""25"",
                ""tokenInfo"": {{
                    ""CardTokenId"": """",
                    ""BuyerTokenId"": """",
                    ""buyerCardID"": """",
                    ""CardAlias"": """"
                }},
                ""CustomerToken"": null,
                ""IdentifierToken"": null,
                ""PaymentToken"": null
            }},
            ""server"": {{
                ""public"": {{
                    ""Name"": ""SVDESMW355"",
                    ""WebSiteNode"": ""/LM/W3SVC/4/ROOT/payments"",
                    ""EndPointUrl"": ""testapicontroller.izipay.pe/v1/Form/PaymentV2"",
                    ""EntryDatetimeApiPublic"": ""2024-01-18 10:16:54.374""
                }},
                ""controller"": {{
                    ""Name"": ""SVDESMW355"",
                    ""WebSiteNode"": ""SVDESMW355"",
                    ""EndPointUrl"": ""SVDESMW355"",
                    ""EntryDatetimeApiController"": null
                }},
                ""business"": {{
                    ""Name"": ""SVDESMW355"",
                    ""WebSiteNode"": """",
                    ""EndPointUrl"": """",
                    ""EntryDatetimeApiBusiness"": ""2024-01-18 10:16:54.593"",
                    ""ExitDatetimeApiBusiness"": null
                }},
                ""internal"": {{
                    ""Name"": null,
                    ""WebSiteNode"": null,
                    ""EndPointUrl"": null,
                    ""EntryDatetimeApiInternal"": null,
                    ""ExitDatetimeApiInternal"": null
                }}
            }},
            ""device"": {{
                ""OperativeSystem"": ""android"",
                ""SOVersion"": ""1.3.0"",
                ""SDKVersion"": ""12""
            }}
        }}";

        // Reemplaza {0} en la plantilla con el GUID generado
        return string.Format(jsonTemplate, guid);
    }

}
