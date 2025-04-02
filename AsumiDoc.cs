using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer.PilotICE
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class JsonDogPIR
    {
        public int Id_CustContracts { get; set; }
        public object ID_Byudzhet_Plan_Doc { get; set; }
        public string ContractsTip { get; set; }
        public string NumDocRab { get; set; }
        public string Num_Zakaz { get; set; }
        public string DateRec { get; set; }
        public string User { get; set; }
        public string GIP { get; set; }
        public string OtvIspolnitel_1 { get; set; }
        public string OtvIspolnitel_2 { get; set; }
        public string OtvIspolnitel_3 { get; set; }
        public string KontragentBaseName { get; set; }
        public string NumDocZak { get; set; }
        public int NumDocMI { get; set; }
        public int NumDocPlan { get; set; }
        public string ObjectName { get; set; }
        public int NumDocObj { get; set; }
        public string ProektStadiya { get; set; }
        public string ContractTheme { get; set; }
        public object ContractThemeShort { get; set; }
        public string StatysContracts { get; set; }
        public string DatеStatys { get; set; }
        public string NalOriginala { get; set; }
        public string DateContract { get; set; }
        public string DateContractZakL { get; set; }
        public string DateContractNach { get; set; }
        public string DateContractFinish { get; set; }
        public string SumContractNoNDS { get; set; }
        public int NDSProc { get; set; }
        public string NDSContract { get; set; }
        public string SumContract { get; set; }
        public string Valuta { get; set; }
        public float SumAdvanceContract { get; set; }
        public object SumContractPrim { get; set; }
        public string PrimDO { get; set; }
        public string KontragentAbrName { get; set; }
        public int id_Objects { get; set; }
    }

    public class Root
    {
        public List<JsonDogPIR> JsonDogPIR { get; set; }
    }



}
