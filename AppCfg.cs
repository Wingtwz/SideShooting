namespace SideShooting
{
    /// <summary>
    /// Contiene la configuración que se usará en el juego
    /// </summary>
    public class AppCfg
    {
        /// <summary>
        /// Determina si la música está habilitada
        /// </summary>
        public bool MusicEnabled { get; set; }
        /// <summary>
        /// Determina si los efectos de sonido están habilitados
        /// </summary>
        public bool SoundEnabled { get; set; }
        /// <summary>
        /// Indica la dirección IP del servidor de juego
        /// </summary>
        public string ServerIP { get; set; }
        /// <summary>
        /// Indica el puerto del servidor de juego
        /// </summary>
        public int Port { get; set; }
    }
}
