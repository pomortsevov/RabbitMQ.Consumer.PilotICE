using Ascon.Pilot.ClientCore.Search;
using Ascon.Pilot.Common.DataProtection;
using Ascon.Pilot.DataClasses;
using Ascon.Pilot.Server.Api;
using Ascon.Pilot.Server.Api.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace RabbitMQ.Consumer.PilotICE
{
    public class SearchPilotCardService
    {
        private string _PilotAtributeName;
        private string _PilotStartGIUD = "00000001-0001-0001-0001-000000000001";
        private static string _PilotAtributeValue;
        private static int _PilotICE_licence = 103;


        //public bool  DSearchResult localserch;
        //public List<string> FoundPilotICEUID = new List<string>();
        public object FoundPilotICEUID ;

        public object firstId;
        public string FirstID_str;
        // public string Str { get; set; } = "Hello from SecondClass";

        public string fst;

        //DSearchResult res_search;

        private void ShowResult1_Adapted(DSearchResult res)
        {
            SearchResultContainer container = ShowResult1(res); 

            Console.WriteLine(container.FirstID );
            fst= container.FirstID;
        }


        public void StartSearch(string PilotAtributeName, string PilotAtributeValue,
                                RabbitMQConsumerConfig ConfigApp)
        {
            _PilotAtributeName = PilotAtributeName;
            _PilotAtributeValue = PilotAtributeValue;
                
            // Connect to base pilot-ice_ru
            var credentials = ConnectionCredentials.GetConnectionCredentials( ConfigApp.PilotICE_URL, 
                                                                              ConfigApp.PilotICE_search_user, ConfigApp.PilotICE_search_passwd.ConvertToSecureString());

            var client = new HttpPilotClient(credentials.GetConnectionString(), credentials.GetConnectionProxy());
            // set isCheckedClientVersion to true if you need to check if the versions of the client and server match
            client.Connect(false);

            //var resultCallBack = new SearchResultCallBack(ShowResult1);

            var resultCallBack = new SearchResultCallBack(ShowResult1_Adapted);

            Console.WriteLine(fst);


            var serverApi = client.GetServerApi(resultCallBack);
            client.GetAuthenticationApi().Login(credentials.DatabaseName, credentials.Username, credentials.ProtectedPassword, false, _PilotICE_licence);
            serverApi.OpenDatabase();
            serverApi.AddSearch(MakeSearchDefinition( ));

            //res_search = 


        }

        private DSearchDefinition MakeSearchDefinition()
        {
            
            var query = QueryBuilderFactory.CreateEmptyQueryBuilder();


            query.Must(ObjectFields.Context.Be(new Guid( _PilotStartGIUD ))); 
            query.MustNot(ObjectFields.ObjectState.Be(ObjectState.Frozen));
            //
            //query.MustAnyOf(AttributeFields.Integer("sheet_number").BeInRange(1, 3));

            //query.MustAnyOf(AttributeFields.Integer("sheet_number").


            // query.MustAnyOf(AttributeFields.String("name")

            //var builder = QueryBuilder.CreateObjectQueryBuilder();
            query.Must(AttributeFields.String(_PilotAtributeName).Be(_PilotAtributeValue));




            
            return new DSearchDefinition
            {
                Id = Guid.NewGuid(),
                Request =
            {
                MaxResults = 10, // Ограничение количества результатов поиска
                SearchKind = SearchKind.Custom,
                SearchString = query.ToString()
            }
            };
        }

        private SearchResultContainer ShowResult1(DSearchResult res)
        {
            var resultContainer = new SearchResultContainer();

            if (res.Found != null) // Предположим, что Ids - это коллекция
            {
                resultContainer.FirstID = res.Found.First().ToString();
                //Console.WriteLine($"Добавлено ID: {string.Join(", ", res.Ids)}");
            }


            // Use search result here
            if (res.Found == null) Console.WriteLine("Совпрадений по Id_CustContracts нет");
            else
            {
                Console.WriteLine($"Результаты поиска Найдено объектов: {res.Found.Count()}.");
                //query.Must(ObjectFields.ParentId.Be(parentId));
                Console.WriteLine($"Id's:\n{string.Join("\n", res.Found)}");

                    FoundPilotICEUID = res.Found;
                //FoundPilotICEUID.Add( res.Found );
                // localserch = res;
                //Log.Information(string.Format("Найдены UID: {0}", );

                firstId = res.Found.First();

                FirstID_str = res.Found.First().ToString();
                Console.WriteLine($"Найдено объектов: {res.Found.Count()}. Первый ID: {firstId}");


            }
            return resultContainer;
        }


        public string GetFirstID()
        {

        return  FirstID_str;
           

        }

    }

    
}
