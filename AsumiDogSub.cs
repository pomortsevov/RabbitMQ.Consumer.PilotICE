using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer.PilotICE
{
    public class JsonDogSUB
    {
        public int id_SubContracts { get; set; }
        public int Id_SubContrOsn { get; set; }
        public int Id_CustContracts { get; set; }
        public int Id_CustContrOsn { get; set; }
        public string DateRec { get; set; }
        public string User { get; set; }
        public string GIP { get; set; }
        public string OtvIspolnitel_1 { get; set; }
        public string OtvIspolnitel_2 { get; set; }
        public string OtvIspolnitel_3 { get; set; }
        public string KontragentBaseName_SUB { get; set; }
        public int NumDocMI { get; set; }
        public string ObjectName { get; set; }
        public string ProektStadiya { get; set; }
        public string ContractNumber_SUB { get; set; }
        public object ContractTheme_SUB { get; set; }
        public object ContractThemeShort_SUB { get; set; }
        public string StatysContracts_SUB { get; set; }
        public object DatStatys_SUB { get; set; }
        public object NalOriginala_SUB { get; set; }
        public string DateContract_SUB { get; set; }
        public string DateContractSrok { get; set; }
        public string DateContractZakL_SUB { get; set; }
        public string DateNachPoDog { get; set; }
        public string DateKonPoDog { get; set; }
        public string DateContractFinish_SUB { get; set; }
        public int SumContractNoNDFL_tmp { get; set; }
        public int SumContractNoNDS { get; set; }
        public int NDSProc { get; set; }
        public int NDSContract { get; set; }
        public int SumContract { get; set; }
        public int SumContractDS { get; set; }
        public string Valuta { get; set; }
        public int SumAdvanceContract { get; set; }
        public object SumContractPrim { get; set; }
        public object PrimDO { get; set; }
        public object ContractDocLink_SUB { get; set; }
    }

    public class RootJSONSub
    {
        public List<JsonDogSUB> JsonDogSUB { get; set; }
    }


}
