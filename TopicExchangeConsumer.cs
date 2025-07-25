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
using Ascon.Pilot.ClientCore.Search;
using Ascon.Pilot.Server.Api;
using System.Diagnostics.Metrics;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Net.Mime;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.CompilerServices;
using log4net.ObjectRenderer;
using log4net;
//ing QuerySearchSample;




namespace MRabbitMQ.Consumer.PilotICE
{
    public static class TopicExchangeConsumer
    {
        //private static Guid _new_guid;
        private static Guid _guid2;
        private static List<RootJSONPir> myDeserializedClass;
      
        private static void SearchPilotICECardAtribute (RabbitMQConsumerConfig ConfigApp, string SearchAtrib)
        {
            //var credentials2 = ConnectionCredentials.GetConnectionCredentials(ConfigApp.PilotICE_URL,
            //                                                                   ConfigApp.PilotICE_user,
            //                                                                   ConfigApp.PilotICE_passwd.ConvertToSecureString());
            new SearchPilotCardService().StartSearch("Id_CustContracts", SearchAtrib, ConfigApp)    ;
           
            //SearchPilotCardService.
            //Console.WriteLine($"Найдено объектов: {0}. Первый ID: {firstId}");
        }


        public static void PilotICE_ModifyCard_PIR(RemoteApiProvider PilotICE_RemoteProvider,
                                               string PilotICE_ID, string PilotICE_Cart_ParentType,
                                               string PilotICE_Cart_InsertedType,
                                               List<RootJSONPir> JSON_PIR)
        {


          
            var backend = PilotICE_RemoteProvider.GetBackend();

            var _guid = new Guid(PilotICE_ID);
            var folderType_parent = backend.GetType(PilotICE_Cart_ParentType);



            var PIRType = backend.GetType(PilotICE_Cart_InsertedType);


            var modifier_dog = PilotICE_RemoteProvider.GetNewModifier();

            var id = Guid.NewGuid();

            var builder = modifier_dog.EditObject(_guid)
                                .SetAttribute("NumDocRab", "privet2")
                                .SetAttribute("Type_Export_Card", "Изменение документа")
                                .SetAttribute("Date_Instert_Card", DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"))
                                .SetAttribute("ContractTheme", JSON_PIR[0].JsonDogPIR[0].ContractTheme)
                                .SetAttribute("GIP", "search")
                                
                                ;

            modifier_dog.Apply();

            Log.Information(string.Format("Modify card  PilotICE Id_CustContracts: {0}",
                            myDeserializedClass[0].JsonDogPIR[0].Id_CustContracts));


        }



        public static void PilotICE_InsertNewCard_PIR (RemoteApiProvider PilotICE_RemoteProvider, 
                                                string PilotICE_Parent_ID, string PilotICE_Cart_ParentType,
                                                string PilotICE_Cart_InsertedType,
                                                List<RootJSONPir> JSON_PIR)
        {

            var backend = PilotICE_RemoteProvider.GetBackend();

            var patent_id = new Guid(PilotICE_Parent_ID);
            var folderType_parent = backend.GetType(PilotICE_Cart_ParentType);

            

            var PIRType = backend.GetType( PilotICE_Cart_InsertedType );


            var modifier_dog = PilotICE_RemoteProvider.GetNewModifier();

            var id = Guid.NewGuid();

            var builder = modifier_dog.CreateObject(id, patent_id, PIRType.Id)

                  .SetAttribute("Id_CustContracts", JSON_PIR[0].JsonDogPIR[0].Id_CustContracts)
                  .SetAttribute("Id_CustContrOsn", JSON_PIR[0].JsonDogPIR[0].Id_CustContrOsn)
                  .SetAttribute("Date_Instert_Card", DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"))
                  .SetAttribute("Type_Export_Card", "Новый документ")

                  .SetAttribute("ContractTheme", JSON_PIR[0].JsonDogPIR[0].ContractTheme)
                  .SetAttribute("ContractThemeShort", JSON_PIR[0].JsonDogPIR[0].ContractTheme)
                  .SetAttribute("NumDoc_Byud_sGod", JSON_PIR[0].JsonDogPIR[0].NumDoc_Byud_sGod)
                  .SetAttribute("ID_Byudzhet_Plan_Doc", JSON_PIR[0].JsonDogPIR[0].ID_Byudzhet_Plan_Doc)
                  .SetAttribute("ContractsTip", JSON_PIR[0].JsonDogPIR[0].ContractsTip)
                  .SetAttribute("NumDocRab", JSON_PIR[0].JsonDogPIR[0].Id_CustContrOsn)
                  .SetAttribute("Num_Zakaz", JSON_PIR[0].JsonDogPIR[0].Num_Zakaz)
                  .SetAttribute("DateRec", JSON_PIR[0].JsonDogPIR[0].DateRec)
                  .SetAttribute("User", JSON_PIR[0].JsonDogPIR[0].User)

                  ;



            modifier_dog.Apply();

            Log.Information(string.Format("Insert card to PilotICE  Id_CustContracts: {0}",
                             myDeserializedClass[0].JsonDogPIR[0].Id_CustContracts));

        }

        public static void Consume(IModel RabbitMQ_channel, RabbitMQConsumerConfig ConfigApp )
        {
            //string dbPassword = Environment.GetEnvironmentVariable("PILOTICE_PASSWORD");

            string envVarName = "PILOTICE_PASSWORD";

            string envValue;

            do
            {
                
                
                envValue = Environment.GetEnvironmentVariable( envVarName );

                if (string.IsNullOrEmpty(envValue))
                {
                    Console.WriteLine($"Переменная окружения '{envVarName}' пустая или не существует. Ожидание...");
                    Thread.Sleep(5000); 
                }

            } while (string.IsNullOrEmpty(envValue));

   


            var remoteProvider = new RemoteApiProvider();
            var credentials = ConnectionCredentials.GetConnectionCredentials( ConfigApp.PilotICE_URL,
                                                                               ConfigApp.PilotICE_user,
                                                                               envValue.ConvertToSecureString());

            


            remoteProvider.ConnectToPilotServer(credentials);

            Log.Information(string.Format("Connect PilotICE Server  Host: {0}", ConfigApp.PilotICE_URL));

            //SearchPilotICECardAtribute(ConfigApp);

            // PilotICE_InsertCard_PIR(remoteProvider, "8213bcaa-d8b8-4555-be20-0063100d7a8c", "me_folder_dog_pir_all", "me_folder_dog_pir");


            //PilotICE_InsertCard_PIR(remoteProvider, "e9ca9e59-d778-47d4-b557-a6223b830d35");




            //var backend = remoteProvider.GetBackend();

            //var folderType_parent = backend.GetType("me_folder_dog_pir_all");

            //var patent_id = new Guid("095ef9bd-a55a-421a-8b60-2570c920104b");

            //var PIRType = backend.GetType("me_folder_dog_pir");


            //var modifier_dog = remoteProvider.GetNewModifier();

            //var id = Guid.NewGuid();

            //var builder = modifier_dog.CreateObject(id, patent_id, PIRType.Id)

            //      .SetAttribute("Id_CustContracts", "2774")
            //      .SetAttribute("NumDocRab", "1263");


            //modifier_dog.Apply();




            //var DOPbackend = remoteProvider.GetBackend();

            //var folderType_parentDOP = backend.GetType("me_folder_dog_pir");

            //var DOPpatent_id = new Guid("095ef9bd-a55a-421a-8b60-2570c920104b");

            //var DOPPIRType = backend.GetType("me_folder_dog_pir");


            //var DOPmodifier_dog = remoteProvider.GetNewModifier();

            //var DOPid = Guid.NewGuid();

            //var DOPbuilder = DOPmodifier_dog.CreateObject(id, patent_id, PIRType.Id)

            //      .SetAttribute("Id_CustContracts", "2774")
            //      .SetAttribute("NumDocRab", "1263");


            //DOPmodifier_dog.Apply();







            //var backend = remoteProvider.GetBackend();
            //var root = backend.GetObject(DObject.RootId);
            //var folderType = backend.GetType("projectfolder");
            //var modifier = remoteProvider.GetNewModifier();
            //var folderBuilder = modifier.CreateObject(Guid.NewGuid(), root.Id, folderType.Id)
            //    .SetAttribute("name", Guid.NewGuid().ToString());

            //var newFolderObject = folderBuilder.GetNewObject();
            //var projectType = backend.GetType("project");
            //var projectBuilder = modifier.CreateObject(Guid.NewGuid(), newFolderObject.Id, projectType.Id)
            //    .SetAttribute("project_name", DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"));

            //var projectObject = projectBuilder.GetNewObject();
            //var sectionType = backend.GetType("main_set");
            //var sectionBuilder = modifier.CreateObject(Guid.NewGuid(), projectObject.Id, sectionType.Id)
            //    .SetAttribute("name", "Section name");

            //var storageProvider = remoteProvider.GetStorage();

            //var documentInfo = new DocumentInfo(DirectoryHelper.GetLocalFile());
            //var sectionObject = sectionBuilder.GetNewObject();








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
                var RabbitMQMessage = Encoding.UTF8.GetString(body);

                

                JArray rootArray = JArray.Parse( RabbitMQMessage );

                // Берём первый элемент и получаем все его свойства-массивы
                var JSONarraysInFirstItem = rootArray[0]
                    .Children<JProperty>()
                    .Where(p => p.Value is JArray)
                    .ToList();

                foreach (var arrayProp in JSONarraysInFirstItem)
                {
                    //Console.WriteLine($"Имя JSON массива: {arrayProp.Name}");
                    JArray array = (JArray)arrayProp.Value;

                     switch (arrayProp.Name)
                    {
                        case "JsonDogPIR":
                            //Console.WriteLine($"Имя массива: {arrayProp.Name}");

                            myDeserializedClass = JsonConvert.DeserializeObject<List<RootJSONPir>>(RabbitMQMessage);

                            Console.WriteLine(myDeserializedClass[0].JsonDogPIR[0].ContractTheme);
                            Log.Information(string.Format("Desirilize JSON  Id_CustContracts: {0}  Id_CustContracts: {0}",
                                                 myDeserializedClass[0].JsonDogPIR[0].Id_CustContracts,
                                                 myDeserializedClass[0].JsonDogPIR[0].Id_CustContracts)
                                                 );

                            string Search_Id_CustContracts = myDeserializedClass[0].JsonDogPIR[0].Id_CustContracts;
                            string Search_Id_CustContractsOsn = myDeserializedClass[0].JsonDogPIR[0].Id_CustContrOsn;
                            
                            if (Search_Id_CustContracts == Search_Id_CustContractsOsn )
                            {
                                SearchPilotICECardAtribute(ConfigApp, Search_Id_CustContracts.ToString());

                                PilotICE_InsertNewCard_PIR(remoteProvider,
                                    "e9ca9e59-d778-47d4-b557-a6223b830d35", "me_folder_dog_pir_all", 
                                    "me_folder_dog_pir", myDeserializedClass);

                                PilotICE_ModifyCard_PIR(remoteProvider,
                                    "8213bcaa-d8b8-4555-be20-0063100d7a8c", "me_folder_dog_pir_all",
                                    "me_folder_dog_pir", myDeserializedClass);



                            }
                            else
                            {

                                SearchPilotICECardAtribute(ConfigApp, Search_Id_CustContracts.ToString());

                                PilotICE_InsertNewCard_PIR(remoteProvider,
                                    "095ef9bd-a55a-421a-8b60-2570c920104b", "me_folder_dog_pir_all",
                                    "me_folder_dog_pir", myDeserializedClass);


                            }


                            




                            break;
                        case "JsonDogSUB":
                            Console.WriteLine($"Имя массива: {arrayProp.Name}");
                            break;
                        default:
                            Console.WriteLine("Неизвестный массив");
                            break;
                    }

                    //foreach (JToken item in array)
                    //{


                    //    Console.WriteLine($"id_SubContracts: {item["id_SubContracts"]}");
                    //}
                }




               




               //  var documentType = backend.GetType("me_folder_dog_pir");
               // _new_guid = Guid.NewGuid();
               
                
               // modifier.CreateObject( _new_guid, sectionObject.Id, documentType.Id)
               //     .SetAttribute("Id_CustContracts", myDeserializedClass[0].JsonDogPIR[0].Id_CustContracts)
               //  //   .SetAttribute("Id_CustContrOsn", myDeserializedClass[0].JsonDogPIR[0].Id_CustContrOsn)    
               //     .SetAttribute("ContractsTip", myDeserializedClass[0].JsonDogPIR[0].ContractsTip)
               //     .SetAttribute("NumDocRab", myDeserializedClass[0].JsonDogPIR[0].NumDocRab)
               //     .SetAttribute("Num_Zakaz", myDeserializedClass[0].JsonDogPIR[0].Num_Zakaz)
               //     .SetAttribute("DateRec", myDeserializedClass[0].JsonDogPIR[0].DateRec)
               //     .SetAttribute("NumDocZak", myDeserializedClass[0].JsonDogPIR[0].NumDocZak)
               //     .SetAttribute("NumDocMI", myDeserializedClass[0].JsonDogPIR[0].NumDocMI)
               //     .SetAttribute("NumDocPlan", myDeserializedClass[0].JsonDogPIR[0].NumDocPlan)
               //     //.SetAttribute("me_podobject_name", myDeserializedClass[0].JsonDogPIR[0].me_podobject_name)
               //     .SetAttribute("KontragentBaseName", myDeserializedClass[0].JsonDogPIR[0].KontragentBaseName)
               ////     .SetAttribute("me_structuraPSD", myDeserializedClass[0].JsonDogPIR[0].me_structuraPSD)

               //     .SetAttribute("NumDocObj", myDeserializedClass[0].JsonDogPIR[0].NumDocObj)

               //     .SetAttribute("ProektStadiya", myDeserializedClass[0].JsonDogPIR[0].ProektStadiya)
               //   //  .SetAttribute("ContractThemeShort", myDeserializedClass[0].JsonDogPIR[0].ContractThemeShort)
               //     .SetAttribute("ContractTheme", myDeserializedClass[0].JsonDogPIR[0].ContractTheme)
               //   //.SetAttribute("SumContractNoNDS", myDeserializedClass[0].JsonDogPIR[0].SumContractNoNDS)
               //     .SetAttribute("NDSProc", myDeserializedClass[0].JsonDogPIR[0].NDSProc)
               //     .SetAttribute("NDSContract", myDeserializedClass[0].JsonDogPIR[0].NDSContract)

               //     .SetAttribute("DatеStatys", myDeserializedClass[0].JsonDogPIR[0].DatеStatys)



               //     .SetAttribute("DateContract", myDeserializedClass[0].JsonDogPIR[0].DateContract)
               //     .SetAttribute("DateContractZakL", myDeserializedClass[0].JsonDogPIR[0].DateContractZakL)
               //     .SetAttribute("DateContractNach", myDeserializedClass[0].JsonDogPIR[0].DateContractNach)
               //     .SetAttribute("DateContractFinish", myDeserializedClass[0].JsonDogPIR[0].DateContractFinish)


               // //  .SetAttribute("SumContract", myDeserializedClass[0].JsonDogPIR[0].SumContract)
               // //    .SetAttribute("SumAdvanceContract_procent", myDeserializedClass[0].JsonDogPIR[0].SumAdvanceContract_procent)
               // //  .SetAttribute("SumAdvanceContract", myDeserializedClass[0].JsonDogPIR[0].SumAdvanceContract)
               // //    .SetAttribute("SumContractPrim", myDeserializedClass[0].JsonDogPIR[0].SumContractPrim)

               //  //   .SetAttribute("PrimDO", myDeserializedClass[0].JsonDogPIR[0].SumContractPrim)



               //     .AddFile(documentInfo, storageProvider);

               // //modifier.EditObject(_new_guid).SetAttribute("NumDocMI","privet");



               // if (modifier.AnyChanges())
               // {
               //     modifier.Apply();
               //     Log.Information(string.Format("Insert card to PilotICE  Id_CustContracts: {0}", 
               //         myDeserializedClass[0].JsonDogPIR[0].Id_CustContracts));
               // }



                //_guid2 = new Guid("29b5a867-39bf-4e96-a7f3-f6448fac8853");
                //modifier.EditObject(_guid2).SetAttribute("NumDocMI", "privet");
                //modifier.Apply();


            };

            RabbitMQ_channel.BasicConsume("mechel_asumi_queue", true, consumer);
            Console.WriteLine("Consumer started");
            Console.ReadLine();


           


        }

       
    }
}
