namespace IntranetLoteriaNacional.Data
{
    public class ZonasInfo
    {
        public string Zona { get; set; }
        public int CuestionariosNovedades { get; set; }
    }

    public class SupervisoresInfo
    {
        public string Supervisor { get; set; }
        public string PDS { get; set; }
        public int CuestionariosNovedades { get; set; }
    }

    public class RankingPDSInfo
    {
        public int codigoPDS { get; set; }
        public string nombrePDS { get; set; }
        public int cantidadFormularios { get; set; }
        public decimal porcentajeCumplimiento { get; set; }
    }

    public class RankingCumplimientoPDSInfo
    {
        public int grupo { get; set; }
        public string cantidadPDS { get; set; }
    }
}
