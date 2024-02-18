﻿using IntranetLoteriaNacional.Pages.StoreCheck;
using IntranetLoteriaNacional.Shared.Constants;
using static LoteriaNacionalDominio.StoreCheckDTO;
using System.Net.Http;
using Blazored.LocalStorage;
using LoteriaNacionalDominio;
using Newtonsoft.Json;
using System.Net;
using System.Security.Policy;
using RestSharp;
using IntranetLoteriaNacional.Pages;


namespace IntranetLoteriaNacional.Data
{
    public class StoreCheckService
    {
        private string EndPointStr { get; set; } = null!;
        ResumenGerencialZonasDTO resumen;
        ResumenGerencialZonasDTO[] Data;
        RankingCumplimientoPDSInfo[] DataRankingPDS;

        public List<ZonasInfo> Zonas;
        public List<SupervisoresInfo> Supervisores;
        public List<CalificacionCuestionariosPDSDTO>? pdsPorRango;

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
        public List<RankingCumplimientoPDSInfo> RecuperarRankingPDS()
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
                        if (strReader == null) return new List<RankingCumplimientoPDSInfo>();
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            var bobyRespuesta = JsonConvert.DeserializeObject<RespuestaDTO>(responseBody)!;
                            DataRankingPDS = JsonConvert.DeserializeObject<RankingCumplimientoPDSInfo[]>(bobyRespuesta.Body)!;
                            
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

        public List<CalificacionCuestionariosPDSDTO> RecuperarPDSPorRangoCumplimiento(string rangoCumplimientoPDS)
        {
            try
            {
                //EndPointStr = "http://jbg15pp03/APILoteriaNacional/api/StoreCheck/";
                RankingCumplimientoPDSDTO datoConsultar = new RankingCumplimientoPDSDTO();
                datoConsultar.grupo = int.Parse(rangoCumplimientoPDS);
                datoConsultar.cantidadPDS = "0";
                var jsonEnviar = JsonConvert.SerializeObject(datoConsultar);
                var apiCliente = new RestClient("http://jbg15pp03/APILoteriaNacional/api/StoreCheck/");
                var request = new RestRequest("ObtienePDSPorRangoCumplimiento");
                request.Method = Method.Post;
                request.AddHeader("Accept", "application/json");
                request.AddParameter("application/json", jsonEnviar, ParameterType.RequestBody);
                var response = apiCliente.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var rawResponse = response.Content;
                    var jsonResult = JsonConvert.DeserializeObject(rawResponse).ToString();
                    var returnType = JsonConvert.DeserializeObject<RespuestaDTO>(jsonResult);
                    //pdsPorRango
                    if (returnType is not null && returnType.CodigoError == 0 && returnType.Body != "[]")
                    {
                        var ret = JsonConvert.DeserializeObject<CalificacionCuestionariosPDSDTO[]>(returnType.Body)!;
                        pdsPorRango = ret.ToList();
                    }
                }

             return pdsPorRango;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
