using Ascon.Pilot.ClientCore.Search;
using Ascon.Pilot.Common.DataProtection;
using Ascon.Pilot.DataClasses;
using Ascon.Pilot.Server.Api;
using Ascon.Pilot.Server.Api.Contracts;
using System;
using System.Linq;
using System.Net;

namespace RabbitMQ.Consumer.PilotICE
{
    public class SearchService
    {
        private string _PilotAtributeName;
        private string _PilotStartGIUD = "00000001-0001-0001-0001-000000000001";
        private static string _PilotAtributeValue;

        public void StartSearch(string PilotAtributeName, string PilotAtributeValue,
                                RabbitMQConsumerConfig ConfigApp) 
        {
            _PilotAtributeName = PilotAtributeName;
            _PilotAtributeValue = PilotAtributeValue;

            // Connect to demo base pilot-ice_ru
            var credentials = ConnectionCredentials.GetConnectionCredentials( ConfigApp.PilotICE_URL, 
                                                                              ConfigApp.PilotICE_search_user, ConfigApp.PilotICE_search_passwd.ConvertToSecureString());

            var client = new HttpPilotClient(credentials.GetConnectionString(), credentials.GetConnectionProxy());
            // set isCheckedClientVersion to true if you need to check if the versions of the client and server match
            client.Connect(false);
            var resultCallBack = new SearchResultCallBack(ShowResult);
            var serverApi = client.GetServerApi(resultCallBack);
            client.GetAuthenticationApi().Login(credentials.DatabaseName, credentials.Username, credentials.ProtectedPassword, false, 103);
            serverApi.OpenDatabase();
            serverApi.AddSearch(MakeSearchDefinition( ));
            //Console.ReadKey();
           


        }

        private DSearchDefinition MakeSearchDefinition()
        {
            //var id = new Guid("00000000-0000-0000-0000-000000000000");
            //var obj = obj


            var query = QueryBuilderFactory.CreateEmptyQueryBuilder();


            query.Must(ObjectFields.Context.Be(new Guid( _PilotStartGIUD ))); // Основной комплект "Технологии производства"
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
                MaxResults = 5, // Ограничение количества результатов поиска
                SearchKind = SearchKind.Custom,
                SearchString = query.ToString()
            }
            };
        }

        private void ShowResult(DSearchResult res)
        {
            // Use search result here
            if (res.Found == null) Console.WriteLine("Поиск не дал результатов");
            else
            {
                Console.WriteLine($"Результаты поиска в Основном комплекте \"Технологии производства\":\nНайдено объектов: {res.Found.Count()}.");
                //query.Must(ObjectFields.ParentId.Be(parentId));
                Console.WriteLine($"Id's:\n{string.Join("\n", res.Found)}");


            }
        }


    }

    
}
