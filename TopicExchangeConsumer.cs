using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json;
using System.Security.Principal;
using Ascon.Pilot.DataClasses;
using Ascon.Pilot.DataModifier;
using Ascon.Pilot.Server.Api;
using RabbitMQ.Consumer.PilotICE;
using Ascon.Pilot.Common.DataProtection;
using Ascon.Pilot.SDK;
using Serilog;

namespace MRabbitMQ.Consumer.PilotICE
{
    public static class TopicExchangeConsumer
    {

        public static void Consume(IModel RabbitMQ_channel, RabbitMQConsumerConfig ConfigApp )
        {

            var remoteProvider = new RemoteApiProvider();
            var credentials = ConnectionCredentials.GetConnectionCredentials( ConfigApp.PilotICE_URL,
                                                                               ConfigApp.PilotICE_user,
                                                                               ConfigApp.PilotICE_passwd.ConvertToSecureString());

            remoteProvider.ConnectToPilotServer(credentials);

            Log.Information(string.Format("Connect PilotICE Server  Host: {0}", ConfigApp.PilotICE_URL));


            var backend = remoteProvider.GetBackend();
            var root = backend.GetObject(DObject.RootId);
            var folderType = backend.GetType("projectfolder");
            var modifier = remoteProvider.GetNewModifier();
            var folderBuilder = modifier.CreateObject(Guid.NewGuid(), root.Id, folderType.Id)
                .SetAttribute("name", Guid.NewGuid().ToString());

            var newFolderObject = folderBuilder.GetNewObject();
            var projectType = backend.GetType("project");
            var projectBuilder = modifier.CreateObject(Guid.NewGuid(), newFolderObject.Id, projectType.Id)
                .SetAttribute("project_name", DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"));

            var projectObject = projectBuilder.GetNewObject();
            var sectionType = backend.GetType("main_set");
            var sectionBuilder = modifier.CreateObject(Guid.NewGuid(), projectObject.Id, sectionType.Id)
                .SetAttribute("name", "Section name");

            var storageProvider = remoteProvider.GetStorage();

            var documentInfo = new DocumentInfo(DirectoryHelper.GetLocalFile());
            var sectionObject = sectionBuilder.GetNewObject();
            
            

            




            RabbitMQ_channel.ExchangeDeclare( "mechel_asumi", ExchangeType.Topic);
            RabbitMQ_channel.QueueDeclare("demo-topic-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            
            RabbitMQ_channel.QueueBind("mechel_asumi_queue", "mechel_asumi", "account.*");
            RabbitMQ_channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(RabbitMQ_channel);
            consumer.Received += (sender, e) => {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                //Console.WriteLine(message);

                //var myDeserializedClass = JsonConvert.DeserializeObject<RabbitMQ.Consumer.PilotICE.AsumiDoc>(message);

                new SearchService().StartSearch();
               

               var myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(message);

                Console.WriteLine(myDeserializedClass[0].JsonDogPIR[0].ContractTheme )   ;
                Log.Information(string.Format("Desirilize JSON object : {0}",
                       myDeserializedClass[0].JsonDogPIR[0].NumDocRab));

                var documentType = backend.GetType("me_folder_dog_pir");
                modifier.CreateObject(Guid.NewGuid(), sectionObject.Id, documentType.Id)
                    .SetAttribute("ContractsTip", myDeserializedClass[0].JsonDogPIR[0].ContractsTip)
                    .SetAttribute("NumDocRab", myDeserializedClass[0].JsonDogPIR[0].NumDocRab)
                    .SetAttribute("Num_Zakaz", myDeserializedClass[0].JsonDogPIR[0].Num_Zakaz)
                    .SetAttribute("DateRec", myDeserializedClass[0].JsonDogPIR[0].DateRec)
                    .SetAttribute("NumDocZak", myDeserializedClass[0].JsonDogPIR[0].NumDocZak)
                    .SetAttribute("NumDocMI", myDeserializedClass[0].JsonDogPIR[0].NumDocMI)
                    .SetAttribute("NumDocPlan", myDeserializedClass[0].JsonDogPIR[0].NumDocPlan)
                    //.SetAttribute("me_podobject_name", myDeserializedClass[0].JsonDogPIR[0].me_podobject_name)
                    .SetAttribute("KontragentBaseName", myDeserializedClass[0].JsonDogPIR[0].KontragentBaseName)
               //     .SetAttribute("me_structuraPSD", myDeserializedClass[0].JsonDogPIR[0].me_structuraPSD)

                    .SetAttribute("NumDocObj", myDeserializedClass[0].JsonDogPIR[0].NumDocObj)

                    .SetAttribute("ProektStadiya", myDeserializedClass[0].JsonDogPIR[0].ProektStadiya)
                  //  .SetAttribute("ContractThemeShort", myDeserializedClass[0].JsonDogPIR[0].ContractThemeShort)
                    .SetAttribute("ContractTheme", myDeserializedClass[0].JsonDogPIR[0].ContractTheme)
                  //.SetAttribute("SumContractNoNDS", myDeserializedClass[0].JsonDogPIR[0].SumContractNoNDS)
                    .SetAttribute("NDSProc", myDeserializedClass[0].JsonDogPIR[0].NDSProc)
                    .SetAttribute("NDSContract", myDeserializedClass[0].JsonDogPIR[0].NDSContract)

                    .SetAttribute("DatеStatys", myDeserializedClass[0].JsonDogPIR[0].DatеStatys)



                    .SetAttribute("DateContract", myDeserializedClass[0].JsonDogPIR[0].DateContract)
                    .SetAttribute("DateContractZakL", myDeserializedClass[0].JsonDogPIR[0].DateContractZakL)
                    .SetAttribute("DateContractNach", myDeserializedClass[0].JsonDogPIR[0].DateContractNach)
                    .SetAttribute("DateContractFinish", myDeserializedClass[0].JsonDogPIR[0].DateContractFinish)


                //  .SetAttribute("SumContract", myDeserializedClass[0].JsonDogPIR[0].SumContract)
                //    .SetAttribute("SumAdvanceContract_procent", myDeserializedClass[0].JsonDogPIR[0].SumAdvanceContract_procent)
                //  .SetAttribute("SumAdvanceContract", myDeserializedClass[0].JsonDogPIR[0].SumAdvanceContract)
                //    .SetAttribute("SumContractPrim", myDeserializedClass[0].JsonDogPIR[0].SumContractPrim)

                 //   .SetAttribute("PrimDO", myDeserializedClass[0].JsonDogPIR[0].SumContractPrim)



                    .AddFile(documentInfo, storageProvider);


                if (modifier.AnyChanges())
                {
                    modifier.Apply();
                    Log.Information(string.Format("Insert card to PilotICE  Host: {0}", 
                        myDeserializedClass[0].JsonDogPIR[0].NumDocRab));
                }

            };

            RabbitMQ_channel.BasicConsume("mechel_asumi_queue", true, consumer);
            Console.WriteLine("Consumer started");
            Console.ReadLine();

            



        }
    }
}
