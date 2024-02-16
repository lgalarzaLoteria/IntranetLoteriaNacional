using IntranetLoteriaNacional.Pages.StoreCheck;
using IntranetLoteriaNacional.Shared.Constants;
using static LoteriaNacionalDominio.StoreCheckDTO;
using System.Net.Http;
using Blazored.LocalStorage;
using LoteriaNacionalDominio;
using Newtonsoft.Json;
using System.Net;
using System.Security.Policy;


namespace IntranetLoteriaNacional.Data
{
    public class StoreCheckService
    {
        private string EndPointStr { get; set; } = null!;
        ResumenGerencialZonasDTO resumen;
        ResumenGerencialZonasDTO[] Data;
        RankingPDSInfo[] DataRankingPDS;

        public List<ZonasInfo> Zonas;
        public List<SupervisoresInfo> Supervisores;
        
        public List<ZonasInfo> RecuperarDataResumenZonas()
        {
            try
            {
                EndPointStr = "http://jbg15pp03/APILoteriaNacional/api/" + EndPoints.obtenerResumenGerencialZonas;
     
                var request = (HttpWebRequest)WebRequest.Create(EndPointStr);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return new List<ZonasInfo>();
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            var bobyRespuesta = JsonConvert.DeserializeObject<RespuestaDTO>(responseBody)!;
                            Data = JsonConvert.DeserializeObject<ResumenGerencialZonasDTO[]>(bobyRespuesta.Body)!;
                            Zonas = new List<ZonasInfo>();

                            foreach (var item in Data)
                            {
                                Zonas.Add(new ZonasInfo { Zona = item.nombreZona, CuestionariosNovedades = item.CuestionariosNovedades });
                            }
                        }
                    }
                }
                return Zonas;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public List<SupervisoresInfo> RecuperarDataResumenSupervisores(string zona)
        {
            try
            {
                EndPointStr = "http://jbg15pp03/APILoteriaNacional/api/" + EndPoints.obtenerResumenGerencialZonas;

                var request = (HttpWebRequest)WebRequest.Create(EndPointStr);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return new List<SupervisoresInfo>();
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            var bobyRespuesta = JsonConvert.DeserializeObject<RespuestaDTO>(responseBody)!;
                            Data = JsonConvert.DeserializeObject<ResumenGerencialZonasDTO[]>(bobyRespuesta.Body)!;
                            Supervisores = new List<SupervisoresInfo>();

                            foreach (var item in Data.Where(dt=>dt.nombreZona.ToUpper().Trim()==zona.ToUpper().Trim()))
                            {
                                Supervisores.Add(new SupervisoresInfo { Supervisor = item.nombreSupervisor, PDS = item.nombrePDS , CuestionariosNovedades = item.CuestionariosNovedades });
                            }
                        }
                    }
                }
                return Supervisores;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public List<RankingPDSInfo> RecuperarRankingPDS()
        {
            try
            {
                EndPointStr = "http://jbg15pp03/APILoteriaNacional/api/" + EndPoints.obtenerRankingPDS;

                var request = (HttpWebRequest)WebRequest.Create(EndPointStr);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return new List<RankingPDSInfo>();
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            var bobyRespuesta = JsonConvert.DeserializeObject<RespuestaDTO>(responseBody)!;
                            DataRankingPDS = JsonConvert.DeserializeObject<RankingPDSInfo[]>(bobyRespuesta.Body)!;
                            
                        }
                    }
                }
                return DataRankingPDS.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
