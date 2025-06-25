using Ascon.Pilot.ClientCore.Search;
using Ascon.Pilot.Common.DataProtection;
using Ascon.Pilot.DataClasses;
using Ascon.Pilot.Server.Api;
using Ascon.Pilot.Server.Api.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
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

        public int r;

        //DSearchResult res_search;

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
            
            var resultCallBack = new SearchResultCallBack(ShowResult1);
           
           

            var serverApi = client.GetServerApi(resultCallBack);
            client.GetAuthenticationApi().Login(credentials.DatabaseName, credentials.Username, credentials.ProtectedPassword, false, _PilotICE_licence);
            serverApi.OpenDatabase();
            serverApi.AddSearch(MakeSearchDefinition( ));

            //res_search = 


        }

        private DSearchDefinition MakeSearchDefinition()
        {
            //var id = new Guid("00000000-0000-0000-0000-000000000000");
           


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

        private void ShowResult1(DSearchResult res)
        {
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

               

            }

        }


        private void res()
        {



        }

    }

    
}
