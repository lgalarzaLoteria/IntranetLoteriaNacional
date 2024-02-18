namespace IntranetLoteriaNacional.Shared.Constants
{
    public class EndPoints
    {
        #region EndPoints Seguridad
        public static string validarUsuario = "Seguridad/Login";
        public static string obtieneMenuUsuario = "Seguridad/ObtieneMenuUsuario";
        public static string verificarPermisos = "Seguridad/MantenimientoObtienePermiso";
        #endregion

        #region EndPoints Store Check
        public static string obtenerNovedades = "Mantenimiento/MantenimientoObtenerNovedades";
        public static string grabarNovedades = "Mantenimiento/MantenimientoGrabarNovedades";
        public static string listarNovedades = "Mantenimiento/ObtenerNovedades";
        public static string obtenerSecciones = "Mantenimiento/MantenimientoObtenerSecciones";
        public static string grabarSecciones = "Mantenimiento/MantenimientoGrabarSecciones";
        public static string listarSecciones = "Mantenimiento/ObtenerSecciones";
        public static string listarSeccionesFormulario = "Mantenimiento/ObtenerSeccionesFormulario";
        public static string obtenerPreguntas = "Mantenimiento/MantenimientoObtenerPreguntas";
        public static string grabarPreguntas = "Mantenimiento/MantenimientoGrabarPreguntas";
        public static string cargarPreguntasSeccion = "Mantenimiento/ObtenerPreguntas";
        public static string grabarCuestionario = "StoreCheck/LlenarFormulario";
        public static string obtenerCuestionarioIngresado = "StoreCheck/ConsultarFormularioIngresado";
        public static string obtieneZonasPorSupervisor = "StoreCheck/ObtieneZonasPorSupervisor";
        public static string consultarFormulariosporPOS = "StoreCheck/ConsultarFormulariosporPOS";
        public static string ConsultarFormulariosRevisadosporPOS = "StoreCheck/ConsultarFormulariosRevisadosporPOS";
        public static string consultarFormularioIngresado = "StoreCheck/ConsultarFormularioIngresado";
        public static string obtenerResumenGerencialZonas = "StoreCheck/ObtieneResumenGerencialZonas";
        public static string consultarFormularioConNovedades = "StoreCheck/ConsultarFormularioConNovedades";
        public static string revisarCuestionario = "StoreCheck/RevisarFormulario";
        public static string obtenerRankingPDS = "StoreCheck/ObtieneRankingPDS";
        public static string obtenerPDSPorRangoCumplimiento = "StoreCheck/ObtienePDSPorRangoCumplimiento";
        public static string obtenerCalificacioCustionariosporPDS = "StoreCheck/ObtieneCalificacioCustionariosporPDS";

        #endregion

        #region EndPoints Bitácora Soporte Batch
        public static string obtenerAplicaciones = "Mantenimiento/MantenimientoObtenerAplicaciones";
        public static string grabarAplicaciones = "Mantenimiento/MantenimientoGrabarAplicaciones";
        public static string listarAplicaciones = "Mantenimiento/ObtenerAplicaciones";
        #endregion
    }
}
